using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server_prototype
{
    public partial class Form_agentLog : Form
    {
        public Form_agentLog()
        {
            InitializeComponent();
            LoadLogs();
        }



        private void LoadLogs()
        {
            string dbPath = Path.Combine(Application.StartupPath, "server.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT time, ip, log, warning FROM Agent_log";

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridViewLogs.DataSource = table;
                }
            }
            dataGridViewLogs.Columns["time"].HeaderText = "Время";
            dataGridViewLogs.Columns["ip"].HeaderText = "IP-адрес";
            dataGridViewLogs.Columns["log"].HeaderText = "Лог";
            dataGridViewLogs.Columns["warning"].HeaderText = "Предупреждение";
        }
    }
}
