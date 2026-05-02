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
using System.Xml.Linq;
using System.Collections.Concurrent;

namespace server_prototype
{
    public partial class Form_monitoring : Form
    {
        class DeviceInfo
        {
            public string IP { get; set; }
            public string MAC { get; set; }
            public string Name { get; set; }

            public HostStatus Status { get; set; }

            public DateTime LastSeen { get; set; }

            public bool SeenByARP { get; set; }
            public bool SeenByPing { get; set; }
            public bool SeenByAgent { get; set; }
            public bool SeenByTCP { get; set; }
        }

        enum HostStatus
        {
            Offline,
            Silent,
            Online
        }
        [System.Runtime.InteropServices.DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int destIp, int srcIp, byte[] macAddr, ref int phyAddrLen);
        string GetNameByIp(string ip)
        {
            using (var conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                string query = @"
                SELECT name FROM Agents WHERE ip = @ip
                UNION
                SELECT name FROM Authorised WHERE ip = @ip
                UNION
                SELECT name FROM Ignored WHERE ip = @ip
                LIMIT 1";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ip", ip);

                    var result = cmd.ExecuteScalar();

                    return result?.ToString() ?? "Unknown";
                }
            }
        }
        string GetAgentUrl(string ip)
        {
            return $"http://{ip}:5050/getinfo";
        }
        string dbPath = "Data Source=server.db;";
        CancellationTokenSource _cts;
        private static HttpListener _listener; // статический, общий для всех экземпляров
        HashSet<string> _authorized = new HashSet<string>();
        HashSet<string> _ignored = new HashSet<string>();
        List<string> _agents = new List<string>();
        string configPath = System.IO.Path.Combine(Application.StartupPath, "interval.txt");
        //Dictionary<string, DeviceInfo> _devices = new Dictionary<string, DeviceInfo>();
        ConcurrentDictionary<string, DeviceInfo> _devices = new ConcurrentDictionary<string, DeviceInfo>();
        public Form_monitoring()
        {
            InitializeComponent();
            InitTables();
            InitDatabase();
            LoadListsFromDB();
            StartAgentReceiver();
            textBoxInterval.Text = "60"; // значение по умолчани
            gridAgents.CellClick += GridAgents_CellClick;
        }

        // ================= UI INIT =================
        void InitTables()
        {
            gridDevices.Columns.Add("name", "Имя");
            gridDevices.Columns.Add("ip", "IP");
            gridDevices.Columns.Add("status", "Статус");
            gridDevices.Columns.Add("type", "Тип");

            gridDevices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            gridAgents.Columns.Add("time", "Время");
            gridAgents.Columns.Add("name", "Имя");
            gridAgents.Columns.Add("ip", "IP");
            gridAgents.Columns.Add("log", "Log");
            gridAgents.Columns.Add("warning", "Warning");

            gridAgents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        void StartAgentReceiver()
        {
            // Если listener уже запущен, не делаем ничего
            if (_listener != null && _listener.IsListening)
                return;

            // Создаём один статический listener
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://+:5050/agentdata/");

            try
            {
                _listener.Start();
                Log("HTTP сервер для агентов запущен (порт 5050)");
            }
            catch (HttpListenerException ex)
            {
                Log("Ошибка запуска HTTP сервера: " + ex.Message);
                return;
            }

            // Запуск обработчика в отдельной задаче
            Task.Run(() =>
            {
                while (_listener != null && _listener.IsListening)
                {
                    try
                    {
                        var ctx = _listener.GetContext();

                        string ip = ctx.Request.RemoteEndPoint.Address.ToString();

                        // Проверка агента
                        if (!_agents.Contains(ip))
                        {
                            Log($"Попытка несанкционированного доступа от {ip}");
                            ctx.Response.StatusCode = 403;
                            ctx.Response.Close();
                            continue;
                        }

                        // Опционально: проверка API-ключа
                        string apiKey = ctx.Request.Headers["X-API-KEY"];
                        if (apiKey != "secret123")
                        {
                            Log($"Неверный API-ключ от {ip}");
                            ctx.Response.StatusCode = 401;
                            ctx.Response.Close();
                            continue;
                        }

                        string data;
                        using (var reader = new StreamReader(ctx.Request.InputStream))
                        {
                            data = reader.ReadToEnd();
                        }

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
                        break;
                    }
                }
            });
        }

        string ExtractWarning(string text)
        {
            var lines = text.Split('\n');

            var warningLines = lines
                .Where(l => l.Contains("WARNING"))
                .ToList();

            return string.Join("\n", warningLines);
        }

        // ================= КНОПКИ =================
        private void buttonStart_Click(object sender, EventArgs e)
        {
            int interval = int.Parse(textBoxInterval.Text);
            _cts = new CancellationTokenSource();
           // Task.Run(() => MonitorLoop(interval, _cts.Token));
            Task.Run(async () => await MonitorLoop(interval, _cts.Token));
            Log("Мониторинг запущен");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (_cts == null) return;

            _cts.Cancel();
            Log("Мониторинг остановлен");
            // доп. защита
            _cts.Dispose();
            _cts = null;
        }

        // ================= ОСНОВНОЙ ЦИКЛ =================


        /* async Task MonitorLoop(int interval, CancellationToken token)
         {
             try
             {
                 while (!token.IsCancellationRequested)
                 {
                     LoadListsFromDB();
                     ClearDevices();
                     MonitorAuthorizedDevices();
                     PollAgents();
                     DiscoverDevices(token);


                     await Task.Delay(100, token);
                 }
             }
             catch (OperationCanceledException)
             {
                 Log("Мониторинг остановлен");
             }
         }*/
        async Task MonitorLoop(int interval, CancellationToken token)
        {
            try
            {
                int elapsed = 0;

                while (!token.IsCancellationRequested)
                {
                    LoadListsFromDB();
                    ClearDevices();

                    MonitorAuthorizedDevices();
                    PollAgents();
                    DiscoverDevices(token);

                    //  вместо долгого Delay — маленькие шаги
                    elapsed = 0;

                    while (elapsed < interval * 1000)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        await Task.Delay(100, token); // быстрый отклик
                        elapsed += 100;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Log("Мониторинг остановлен");
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
        /*void DiscoverDevices(CancellationToken token)
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
                new ParallelOptions
               {
                MaxDegreeOfParallelism = 50,
                CancellationToken = token
                },
                    current =>
                    {
                        if (token.IsCancellationRequested)
                            return;

                        string ipStr = FromUInt((uint)current);
                        string name = GetNameByIp(ipStr);

                        if (_ignored.Contains(ipStr)) return;

                        var status = GetHostStatus(ipStr);

                        if (status != HostStatus.Offline && !_authorized.Contains(ipStr))
                        {
                            AddDevice(ipStr, status.ToString(), "Unauthorized");
                        }
                    });


            /*Parallel.For((long)network + 1, (long)broadcast,
                new ParallelOptions { MaxDegreeOfParallelism = 50 },
                current =>
                {
                    string ipStr = FromUInt((uint)current);
                    string name = GetNameByIp(ipStr);
                    if (_ignored.Contains(ipStr)) return;

                    var status = GetHostStatus(ipStr);

                    if (status != HostStatus.Offline && !_authorized.Contains(ipStr))
                    {
                        AddDevice(ipStr, status.ToString(), "Unauthorized");
                    }

                    
                });
        }*/

        void DiscoverDevices(CancellationToken token)
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
            new ParallelOptions
            {
                MaxDegreeOfParallelism = 100,
                CancellationToken = token
            },
            current =>
            {
                token.ThrowIfCancellationRequested(); 

                string ipStr = FromUInt((uint)current);

                if (_ignored.Contains(ipStr)) return;

                var status = GetHostStatus(ipStr);

                if (status != HostStatus.Offline && !_authorized.Contains(ipStr))
                {
                    AddDevice(ipStr, status.ToString(), "Unauthorized");
                }
            });
        }
        void StopAgentReceiver()
        {
            try
            {
                _listener?.Stop();
                _listener?.Close();
                _listener = null;

                Log("HTTP сервер остановлен");
            }
            catch (Exception ex)
            {
                Log("Ошибка остановки listener: " + ex.Message);
            }
        }
        void MonitorAuthorizedDevices()
        {

            foreach (string ip in _authorized)
            {
                if (_cts.Token.IsCancellationRequested)
                    return;

                if (_ignored.Contains(ip)) continue;

                var status = GetHostStatus(ip);
                string name = GetNameByIp(ip);

                AddDevice(ip, status.ToString(), "Authorized");
                LogMonitoring(ip, status.ToString(), name);
            }


            /* foreach (string ip in _authorized)
             {
                 /*if (_ignored.Contains(ip)) continue;

                 /*bool alive = IsHostAlive(ip);
                 string status = alive ? "Online" : "Offline";

            var status = GetHostStatus(ip);
                string name = GetNameByIp(ip);
                //string StrStatus = status.ToString();
                AddDevice(ip, status.ToString(), "Authorized");
                LogMonitoring(ip, status.ToString(), name);
            }*/
        }

       

        void PollAgents()
        {
            foreach (string ip in _agents)
            {
                if (_cts.Token.IsCancellationRequested)
                    return;
                try
                {
                    string url = GetAgentUrl(ip);

                    using (var wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        string report = wc.DownloadString(url);

                        // извлекаем warning
                        string warning = ExtractWarning(report);

                        // сохраняем ПОЛНЫЙ отчёт
                        AddAgentLog(ip, report, warning);
                        LogAgent(ip, report, warning);
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
        /*bool DeviceExists(string ip)
        {
            return gridDevices.Rows
                .Cast<DataGridViewRow>()
                .Any(r => r.Cells[1].Value?.ToString() == ip);
        }*/
        bool DeviceExists(string ip)
        {
            foreach (DataGridViewRow row in gridDevices.Rows)
            {
                if (row.Cells[1].Value?.ToString() == ip)
                    return true;
            }
            return false;
        }
        void AddDevice(string ip, string status, string type)
        {
            if (gridDevices.InvokeRequired)
            {
                gridDevices.Invoke(new Action<string, string, string>(AddDevice), ip, status, type);
                return;
            }
            if (gridDevices.Columns.Count == 0)
                return;

            if (DeviceExists(ip))
                return;
            string name = GetNameByIp(ip);
            int rowIndex = gridDevices.Rows.Add(name, ip, status, type);
            var row = gridDevices.Rows[rowIndex];

            if (type == "Unauthorized")
                row.DefaultCellStyle.BackColor = Color.LightCoral;
            else if (status == "Offline")
                row.DefaultCellStyle.BackColor = Color.LightGray;
            else if (status == "Silent")
                row.DefaultCellStyle.BackColor = Color.Khaki;
            else
                row.DefaultCellStyle.BackColor = Color.LightGreen;
            if (gridDevices.Rows.Count > 0)
                gridDevices.FirstDisplayedScrollingRowIndex = gridDevices.Rows.Count - 1;
        }

        /*void AddAgentLog(string ip, string log, string warning)
        {
            if (gridAgents.InvokeRequired)
            {
                gridAgents.Invoke(new Action<string, string, string>(AddAgentLog), ip, log, warning);
                return;
            }

            string name = GetNameByIp(ip);

            gridAgents.Rows.Add(
                DateTime.Now.ToString("HH:mm:ss"),
                name,
                ip,
                log,
                warning);
        }*/
        void AddAgentLog(string ip, string log, string warning)
        {
            if (gridAgents.InvokeRequired)
            {
                gridAgents.Invoke(new Action<string, string, string>(AddAgentLog), ip, log, warning);
                return;
            }

            if (gridAgents.Columns.Count == 0)
                return; // защита от  ошибки

            string name = GetNameByIp(ip);

            gridAgents.Rows.Add(
                DateTime.Now.ToString("HH:mm:ss"),
                name,
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
      

        void LogMonitoring(string ip, string status,string name)
        {
            
                using (var conn = new SQLiteConnection(dbPath))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand(
                        "INSERT INTO Monitoring_log (time, ip, name, status) VALUES (@time,@ip,@name,@status)", conn))
                    {
                        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        cmd.Parameters.AddWithValue("@ip", ip);
                        cmd.Parameters.AddWithValue("@name", name);
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
                string name = GetNameByIp(ip);
                using (var cmd = new SQLiteCommand(
                      "INSERT INTO Agent_log (time, ip, name, log, warning) VALUES (@time,@ip,@name,@log,@warning)", conn))
                {
                   
                    cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@ip", ip);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@log", log);
                    cmd.Parameters.AddWithValue("@warning", warning);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        void UpdateDevice(string ip, Action<DeviceInfo> update)
        {
            var dev = _devices.GetOrAdd(ip, _ => new DeviceInfo
            {
                IP = ip,
                LastSeen = DateTime.MinValue
            });

            update(dev);
            dev.LastSeen = DateTime.Now;
        }

        // ================= УТИЛИТЫ =================
        /* bool IsHostAlive(string ip)
         {
             try
             {
                 using (var ping = new Ping())
                 {
                     return ping.Send(ip, 300).Status == IPStatus.Success;
                 }
             }
             catch { return false; }
         }*/
        bool IsHostAliveARP(string ip)
        {
            try
            {
                IPAddress destIP = IPAddress.Parse(ip);
                byte[] macAddr = new byte[6];
                int macAddrLen = macAddr.Length;

                byte[] addressBytes = destIP.GetAddressBytes();
  
                int intAddress = (int)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(addressBytes, 0));
                int result = SendARP(intAddress, 0, macAddr, ref macAddrLen);

                return result == 0; // 0 = SUCCESS
            }
            catch
            {
                return false;
            }
        }
        HostStatus GetHostStatus(string ip)
        {
            bool agent = IsAgentAlive(ip);
            bool arp = IsHostAliveARP(ip);
            bool ping = false;
            bool tcp = false;

            try
            {
                using (var pingReq = new Ping())
                    ping = pingReq.Send(ip, 120).Status == IPStatus.Success;
            }
            catch { }

            int[] ports = { 135, 445, 80 };

            foreach (var port in ports)
            {
                if (IsPortOpen(ip, port, 120))
                {
                    tcp = true;
                    break;
                }
            }

            // 🔥 обновляем модель
            UpdateDevice(ip, d =>
            {
                d.SeenByAgent = agent;
                d.SeenByARP = arp;
                d.SeenByPing = ping;
                d.SeenByTCP = tcp;
            });

            // 🔥 логика статуса
            if (agent)
                return HostStatus.Online;

            if (ping || tcp)
                return HostStatus.Online;

            if (arp)
                return HostStatus.Silent;

            // 🔥 главное улучшение:
            if (_devices.TryGetValue(ip, out var dev))
            {
                if ((DateTime.Now - dev.LastSeen).TotalMinutes < 5)
                    return HostStatus.Silent;
            }

            return HostStatus.Offline;
        }
        /*bool IsHostAlive(string ip)
        {
            // 1. ARP 
            if (IsHostAliveARP(ip))
                return true;

            // 2. Ping
            try
            {
                using (var ping = new Ping())
                {
                    if (ping.Send(ip, 200).Status == IPStatus.Success)
                        return true;
                }
            }
            catch { }

            // 3. TCP
            int[] ports = { 80, 135, 445, 5050 };

            foreach (var port in ports)
            {
                if (IsPortOpen(ip, port))
                    return true;
            }

            // 4. Агент
            if (IsAgentAlive(ip))
                return true;

            return false;
        }*/
        bool IsPortOpen(string ip, int port, int timeout = 300)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(ip, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(timeout);
                    return success && client.Connected;
                }
            }
            catch
            {
                return false;
            }
        }

        bool IsAgentAlive(string ip)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    wc.DownloadString($"http://{ip}:5050/getinfo");
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /*uint ToUInt(IPAddress ip)
        {
            byte[] b = ip.GetAddressBytes();
            return (uint)(b[0] << 24 | b[1] << 16 | b[2] << 8 | b[3]);
        }*/
        uint ToUInt(IPAddress ip)
        {
            byte[] b = ip.GetAddressBytes();
            return ((uint)b[0] << 24) |
                   ((uint)b[1] << 16) |
                   ((uint)b[2] << 8) |
                   b[3];
        }

        string FromUInt(uint ip)
        {
            return $"{(ip >> 24) & 255}.{(ip >> 16) & 255}.{(ip >> 8) & 255}.{ip & 255}";
        }


        private void GridAgents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = gridAgents.Rows[e.RowIndex];

            string textToShow = "";
            string title = "";

            if (e.ColumnIndex == 3) // Log
            {
                textToShow = row.Cells[3].Value?.ToString();
                title = "Полный лог";
            }
            else if (e.ColumnIndex == 4) // Warning
            {
                textToShow = row.Cells[4].Value?.ToString();
                title = "Предупреждения";
            }
            else
            {
                return; // остальные колонки игнорируем
            }

            if (string.IsNullOrWhiteSpace(textToShow))
                textToShow = "Нет данных";

            FormLogViewer form = new FormLogViewer(textToShow);
            form.Text = title; // заголовок окна
            form.ShowDialog();
        }

        private void buttonStop_Click_1(object sender, EventArgs e)
        {
            if (_cts == null)
                return;

            _cts.Cancel();
            Log("Мониторинг остановлен");
            StopAgentReceiver();
            // НЕ уничтожаем сразу, пока поток может ещё использовать токен
            // только обнуляем ссылку после остановки логики
           //_cts = null;
        }
    }
}


