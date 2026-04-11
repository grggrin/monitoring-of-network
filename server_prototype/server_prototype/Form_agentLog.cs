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
            dataGridViewLogs.CellClick += DataGridViewLogs_CellClick;
            dataGridViewLogs.RowPrePaint += dataGridViewLogs_RowPrePaint;
        }



        /* private void LoadLogs()
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
         }*/

        void LoadLogs(string search = "", string ip = "", DateTime? from = null, DateTime? to = null)
        {
            string dbPath = Path.Combine(Application.StartupPath, "server.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var query = new StringBuilder();
                query.Append("SELECT time, ip, log, warning FROM Agent_log WHERE 1=1 ");

                var cmd = new SQLiteCommand();
                cmd.Connection = connection;

                // 🔍 поиск по тексту
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query.Append("AND (log LIKE @search OR warning LIKE @search) ");
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                }

                // 🌐 фильтр по IP
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    query.Append("AND ip = @ip ");
                    cmd.Parameters.AddWithValue("@ip", ip);
                }

                // ⏱ фильтр по дате
                if (from.HasValue)
                {
                    query.Append("AND time >= @from ");
                    cmd.Parameters.AddWithValue("@from", from.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (to.HasValue)
                {
                    query.Append("AND time <= @to ");
                    cmd.Parameters.AddWithValue("@to", to.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                // 🔽 сортировка
                query.Append("ORDER BY time DESC");

                cmd.CommandText = query.ToString();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridViewLogs.DataSource = table;
                }
            }
        }
        private void DataGridViewLogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridViewLogs.Rows[e.RowIndex];

            string textToShow = "";
            string title = "";

            if (e.ColumnIndex == dataGridViewLogs.Columns["log"].Index)
            {
                textToShow = row.Cells["log"].Value?.ToString();
                title = "Полный лог";
            }
            else if (e.ColumnIndex == dataGridViewLogs.Columns["warning"].Index)
            {
                textToShow = row.Cells["warning"].Value?.ToString();
                title = "Предупреждения";
            }
            else
            {
                return; // остальные колонки не обрабатываем
            }

            if (string.IsNullOrWhiteSpace(textToShow))
                textToShow = "Нет данных";

            FormLogViewer form = new FormLogViewer(textToShow);
            form.Text = title;
            form.ShowDialog();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            LoadLogs( textBoxSearch.Text,    textBoxIP.Text,  dateFrom.Value, dateTo.Value);
        }
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            LoadLogs(
                textBoxSearch.Text,
                textBoxIP.Text,
                dateFrom.Value,
                dateTo.Value
            );
        }
        private void dataGridViewLogs_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dataGridViewLogs.Rows[e.RowIndex];

            string warning = row.Cells["warning"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(warning))
            {
                row.DefaultCellStyle.BackColor = Color.LightPink;
            }
        }
    }
}
