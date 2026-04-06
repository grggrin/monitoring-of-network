using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;


namespace server_prototype
{
    public partial class Form_monitoring : Form
    {
        string GetAgentUrl(string ip)
        {
            return $"http://{ip}:5050/getinfo";
        }


        string dbPath = "Data Source=server.db;";
        CancellationTokenSource _cts;

        HashSet<string> _authorized = new HashSet<string>();
        HashSet<string> _ignored = new HashSet<string>();
        List<string> _agents = new List<string>();
        string configPath = System.IO.Path.Combine(Application.StartupPath, "interval.txt");

        public Form_monitoring()
        {
            InitializeComponent();
            InitTables();
            InitDatabase();
            StartAgentReceiver();
            textBoxInterval.Text = "60"; // значение по умолчани
        }

        // ================= UI INIT =================
        void InitTables()
        {
            gridDevices.Columns.Add("ip", "IP");
            gridDevices.Columns.Add("status", "Статус");
            gridDevices.Columns.Add("type", "Тип");

            gridDevices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridAgents.Columns.Add("time", "Время");
            gridAgents.Columns.Add("ip", "IP");
            gridAgents.Columns.Add("log", "Log");
            gridAgents.Columns.Add("warning", "Warning");

            gridAgents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        void StartAgentReceiver()
        {
            Task.Run(() =>
            {
                HttpListener listener = new HttpListener();

                listener.Prefixes.Add("http://+:5050/agentdata/");
                listener.Start();
                Log("HTTP сервер для агентов запущен (порт 5050)");

                while (true)
                {
                    try
                    {
                        var ctx = listener.GetContext();

                        string data = "";

                        using (var reader = new StreamReader(ctx.Request.InputStream))
                        {
                            data = reader.ReadToEnd();
                        }

                        string ip = ctx.Request.RemoteEndPoint.Address.ToString();

                        // логируем
                        // AddAgentLog(ip, data, "");
                        // LogAgent(ip, data, "");

                        string warning = ExtractWarning(data);

                        AddAgentLog(ip, data, warning);
                        LogAgent(ip, data, warning);


                        Log($"Данные от агента {ip}");

                        ctx.Response.StatusCode = 200;
                        ctx.Response.Close();
                    }
                    catch (Exception ex)
                    {
                        Log("Ошибка HTTP сервера: " + ex.Message);
                    }
                }
            });
        }

        string ExtractWarning(string text)
        {
            var lines = text.Split('\n');

            var warningLine = lines
                .FirstOrDefault(l => l.Contains("WARNING:"));

            return warningLine ?? "";
        }

        // ================= КНОПКИ =================
        private void buttonStart_Click(object sender, EventArgs e)
        {
            int interval = int.Parse(textBoxInterval.Text);

            _cts = new CancellationTokenSource();

            Task.Run(() => MonitorLoop(interval, _cts.Token));

            Log("Мониторинг запущен");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
            Log("Мониторинг остановлен");
        }

        // ================= ОСНОВНОЙ ЦИКЛ =================
        void MonitorLoop(int interval, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                LoadListsFromDB();

                ClearDevices();

                DiscoverDevices();
                MonitorAuthorizedDevices();
                PollAgents();

                Thread.Sleep(interval * 1000);
            }
        }

        // ================= БАЗА =================
        void InitDatabase()
        {
            using (var conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                string sql = @"
                CREATE TABLE IF NOT EXISTS Agents (ip TEXT);
                CREATE TABLE IF NOT EXISTS Authorised (ip TEXT);
                CREATE TABLE IF NOT EXISTS Ignored (ip TEXT);

                CREATE TABLE IF NOT EXISTS Monitoring_log (
                    time TEXT,
                    ip TEXT,
                    status TEXT
                );

                CREATE TABLE IF NOT EXISTS Agent_log (
                    time TEXT,
                    ip TEXT,
                    log TEXT,
                    warning TEXT
                );";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        void LoadListsFromDB()
        {
            _authorized = LoadSet("Authorised");
            _ignored = LoadSet("Ignored");
            _agents = LoadList("Agents");
        }

        HashSet<string> LoadSet(string table)
        {
            var set = new HashSet<string>();

            using (var conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT ip FROM " + table, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        set.Add(reader.GetString(0));
                }
            }

            return set;
        }

        List<string> LoadList(string table)
        {
            var list = new List<string>();

            using (var conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT ip FROM " + table, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(reader.GetString(0));
                }
            }

            return list;
        }

        // ================= СКАНИРОВАНИЕ =================
        void DiscoverDevices()
        {
            Log("Сканирование сети...");

            var iface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(i => i.OperationalStatus == OperationalStatus.Up &&
                                     i.GetIPProperties().GatewayAddresses.Count > 0);

            if (iface == null) return;

            var ipInfo = iface.GetIPProperties().UnicastAddresses
                .FirstOrDefault(a => a.Address.AddressFamily == AddressFamily.InterNetwork);

            if (ipInfo == null) return;

            uint ip = ToUInt(ipInfo.Address);
            uint mask = ToUInt(ipInfo.IPv4Mask);
            uint network = ip & mask;
            uint broadcast = network | ~mask;

            Parallel.For((long)network + 1, (long)broadcast,
                new ParallelOptions { MaxDegreeOfParallelism = 50 },
                current =>
                {
                    string ipStr = FromUInt((uint)current);

                    if (_ignored.Contains(ipStr)) return;

                    if (IsHostAlive(ipStr) && !_authorized.Contains(ipStr))
                    {
                        AddDevice(ipStr, "Online", "Unauthorized");
                        Log(" Неавторизованный: " + ipStr);
                        LogMonitoring(ipStr, "Unauthorized");
                    }
                });
        }

        void MonitorAuthorizedDevices()
        {
            foreach (string ip in _authorized)
            {
                if (_ignored.Contains(ip)) continue;

                bool alive = IsHostAlive(ip);
                string status = alive ? "Online" : "Offline";

                AddDevice(ip, status, "Authorized");
                LogMonitoring(ip, status);
            }
        }

        // ================= АГЕНТЫ =================
        /*void PollAgents()
        {
            foreach (string ip in _agents)
            {
                try
                {
                    string url = "http://" + ip + ":5050/getinfo";

                    using (var wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        string result = wc.DownloadString(url);

                        AddAgentLog(ip, result, "");
                        LogAgent(ip, result, "");
                    }
                }
                catch (Exception ex)
                {
                    AddAgentLog(ip, "", ex.Message);
                    LogAgent(ip, "", ex.Message);
                }
            }
        } */
        void PollAgents()
        {
            foreach (string ip in _agents)
            {
                if (_cts.Token.IsCancellationRequested)
                    return;
                
                try
                {
                    //string url = "http://" + ip + ":5050/getinfo";
                    string url = GetAgentUrl(ip);
                    using (var wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        string report = wc.DownloadString(url);

                        //  парсим
                        double cpu = ParseValue(report, "CPU Load:");
                        double ram = ParseValue(report, "RAM Usage:");
                        double net = ParseValue(report, "Network Usage:");
                        double diskC = ParseDiskUsage(report, "C:\\");
                        // лог = можно оставить как есть (или укоротить)
                        string log = $"CPU: {cpu}% | RAM: {ram}% | NET: {net}%";

                      

                        AddAgentLog(ip, log, "");
                        LogAgent(ip, log, "");

                        //if (!string.IsNullOrEmpty(warning))
                         //   Log($"⚠️ Агент {ip}: {warning}");
                    }
                }
                catch (Exception ex)
                {
                    AddAgentLog(ip, "", ex.Message);
                    LogAgent(ip, "", ex.Message);

                    Log($"Ошибка агента {ip}: {ex.Message}");
                }
            }
        }
        double ParseValue(string text, string key)
        {
            try
            {
                var line = text.Split('\n')
                               .FirstOrDefault(x => x.Contains(key));

                if (line == null) return 0;

                // пример: CPU Load: 1,17 %
                string value = line.Split(':')[1]
                                   .Replace("%", "")
                                   .Replace(",", ".")
                                   .Trim();

                return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }
        double ParseDiskUsage(string text, string disk)
        {
            try
            {
                var line = text.Split('\n')
                               .FirstOrDefault(x => x.Contains(disk) && x.Contains("Used"));

                if (line == null) return 0;

                // (37,25%)
                int start = line.IndexOf('(');
                int end = line.IndexOf('%');

                string value = line.Substring(start + 1, end - start - 1)
                                   .Replace(",", ".");

                return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        // ================= UI =================
        void AddDevice(string ip, string status, string type)
        {
            if (gridDevices.InvokeRequired)
            {
                gridDevices.Invoke(new Action<string, string, string>(AddDevice), ip, status, type);
                return;
            }

           /* int rowIndex = gridDevices.Rows.Add(ip, status, type);
            var row = gridDevices.Rows[rowIndex];
            
            if (type == "Unauthorized")
                row.DefaultCellStyle.BackColor = Color.LightCoral;
            else if (status == "Offline")
                row.DefaultCellStyle.BackColor = Color.LightGray;
            else
                row.DefaultCellStyle.BackColor = Color.LightGreen;*/

            if (gridDevices.Columns.Count == 0)
                return;

            int rowIndex = gridDevices.Rows.Add(ip, status, type);
            var row = gridDevices.Rows[rowIndex];

            if (type == "Unauthorized")
                row.DefaultCellStyle.BackColor = Color.LightCoral;
            else if (status == "Offline")
                row.DefaultCellStyle.BackColor = Color.LightGray;
            else
                row.DefaultCellStyle.BackColor = Color.LightGreen;

            if (gridDevices.Rows.Count > 0)
                gridDevices.FirstDisplayedScrollingRowIndex = gridDevices.Rows.Count - 1;
        }

        void AddAgentLog(string ip, string log, string warning)
        {
            if (gridAgents.InvokeRequired)
            {
                gridAgents.Invoke(new Action<string, string, string>(AddAgentLog), ip, log, warning);
                return;
            }

            gridAgents.Rows.Add(
                DateTime.Now.ToString("HH:mm:ss"),
                ip,
                log,
                warning);
        }

        void Log(string msg)
        {
            if (listBoxLog.InvokeRequired)
            {
                listBoxLog.Invoke(new Action<string>(Log), msg);
                return;
            }

            listBoxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {msg}");

            // автоскролл вниз
            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
        }

        void ClearDevices()
        {
            if (gridDevices.InvokeRequired)
            {
                gridDevices.Invoke(new Action(ClearDevices));
                return;
            }

            gridDevices.Rows.Clear();
        }

        // ================= ЛОГ В БД =================
        /* void LogMonitoring(string ip, string status)
         {
             using (var conn = new SQLiteConnection(dbPath))
             {
                 conn.Open();

                 using (var cmd = new SQLiteCommand(
                     "INSERT INTO Monitoring_log VALUES (@time,@ip,@status)", conn))
                 {
                     cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                     cmd.Parameters.AddWithValue("@ip", ip);
                     cmd.Parameters.AddWithValue("@status", status);

                     cmd.ExecuteNonQuery();
                 }
             }
         }  */

        void LogMonitoring(string ip, string status)
        {
            
                using (var conn = new SQLiteConnection(dbPath))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(
                        "INSERT INTO Monitoring_log (time, ip, status) VALUES (@time,@ip,@status)", conn))
                    {
                        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        cmd.Parameters.AddWithValue("@ip", ip);
                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteNonQuery();
                    }
                }
            
        }


        void LogAgent(string ip, string log, string warning)
        {
            using (var conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                //using (var cmd = new SQLiteCommand(
                 //  "INSERT INTO Agent_log VALUES (@time,@ip,@log,@warning)", conn))
                using (var cmd = new SQLiteCommand(
                      "INSERT INTO Agent_log (time, ip, log, warning) VALUES (@time,@ip,@log,@warning)", conn))
                {
                   
                    cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@ip", ip);
                    cmd.Parameters.AddWithValue("@log", log);
                    cmd.Parameters.AddWithValue("@warning", warning);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================= УТИЛИТЫ =================
        bool IsHostAlive(string ip)
        {
            try
            {
                using (var ping = new Ping())
                {
                    return ping.Send(ip, 300).Status == IPStatus.Success;
                }
            }
            catch { return false; }
        }

        uint ToUInt(IPAddress ip)
        {
            byte[] b = ip.GetAddressBytes();
            return (uint)(b[0] << 24 | b[1] << 16 | b[2] << 8 | b[3]);
        }

        string FromUInt(uint ip)
        {
            return $"{(ip >> 24) & 255}.{(ip >> 16) & 255}.{(ip >> 8) & 255}.{ip & 255}";
        }

        private void buttonStop_Click_1(object sender, EventArgs e)
        {
            _cts?.Cancel();
            Log("Мониторинг остановлен");
        }
    }
}

