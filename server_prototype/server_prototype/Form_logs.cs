using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server_prototype
{
    public partial class Form_logs : Form
    {
        string dbPath = "Data Source=server.db;";
        public Form_logs()
        {
            InitializeComponent();

            comboBoxFilter.Items.AddRange(new string[]
            {
                "All",
                "Online",
                "Offline",
                "Unauthorized"
            });
            comboBoxFilter.SelectedIndex = 0;
            comboBoxFilter.SelectedIndexChanged += comboBoxFilter_SelectedIndexChanged;

            LoadLogs("", "All", dateTimeFrom.Value, dateTimeTo.Value);
            gridLogs.RowPrePaint += gridLogs_RowPrePaint;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            dateTimeFrom.ValueChanged += dateTimeFrom_ValueChanged;
            dateTimeTo.ValueChanged += dateTimeTo_ValueChanged;

            this.Shown += (s, e) => ApplyFilter();
         
        }

        /*void LoadLogs()
        {
            using (var conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                string query = "SELECT time, ip, status FROM Monitoring_log ORDER BY time DESC";

                var adapter = new SQLiteDataAdapter(query, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                gridLogs.DataSource = table;
            }
        }*/
        DataTable _table;

        void LoadLogs(string search, string status, DateTime from, DateTime to)
        {
            using (var conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                var query = new StringBuilder();
                query.Append(@"
            SELECT time, ip, status
            FROM Monitoring_log
            WHERE 1=1
        ");

                var cmd = new SQLiteCommand();
                cmd.Connection = conn;

                if (!string.Equals(status, "All", StringComparison.OrdinalIgnoreCase))
                {
                    query.Append(" AND LOWER(status) = LOWER(@status)");
                    cmd.Parameters.AddWithValue("@status", status);
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query.Append(" AND (ip LIKE @search OR status LIKE @search OR time LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                }

                query.Append(" AND time BETWEEN @from AND @to");

                cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                query.Append(" ORDER BY time DESC");

                cmd.CommandText = query.ToString();

                var adapter = new SQLiteDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                gridLogs.DataSource = table;
            }
        }


        /*void ApplyFilter()
        {
            string search = textBoxSearch.Text.Trim().ToLower();
            string filter = comboBoxFilter.SelectedItem.ToString().ToLower();

            DateTime from = dateTimeFrom.Value;
            DateTime to = dateTimeTo.Value;

            foreach (DataGridViewRow row in gridLogs.Rows)
            {
                if (row.IsNewRow) continue;

                string timeStr = row.Cells["time"].Value?.ToString() ?? "";
                string ip = row.Cells["ip"].Value?.ToString().ToLower() ?? "";
                string status = row.Cells["status"].Value?.ToString().ToLower() ?? "";

                bool visible = true;

                //  ПОИСК
                if (!string.IsNullOrEmpty(search))
                {
                    visible =
                        timeStr.ToLower().Contains(search) ||
                        ip.Contains(search) ||
                        status.Contains(search);
                }

                //  ФИЛЬТР ПО СТАТУСУ
                if (filter != "all")
                {
                    visible = visible && status == filter;
                }

                //  ФИЛЬТР ПО ВРЕМЕНИ
                if (DateTime.TryParse(timeStr, out DateTime rowTime))
                {
                    if (rowTime < from || rowTime > to)
                        visible = false;
                }

                //  ПРИМЕНЯЕМ В КОНЦЕ
                row.Visible = visible;
            }
        }*/


        /* void ApplyFilter()
         {
             LoadLogs(
                 textBoxSearch.Text.Trim(),
                 comboBoxFilter.Text,   
                 dateTimeFrom.Value,
                 dateTimeTo.Value
             );
         }*/

        void ApplyFilter()
        {
            string status = comboBoxFilter.Text;

            if (string.IsNullOrWhiteSpace(status))
                status = "All";

            LoadLogs(
                textBoxSearch.Text.Trim(),
                status,
                dateTimeFrom.Value,
                dateTimeTo.Value
            );
        }




        //подсветка
        private void gridLogs_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = gridLogs.Rows[e.RowIndex];

            string status = row.Cells["status"].Value?.ToString();

            if (string.IsNullOrEmpty(status)) return;

            status = status.ToLower();

            if (status == "unauthorized")
                row.DefaultCellStyle.BackColor = Color.LightCoral;
            else if (status == "offline")
                row.DefaultCellStyle.BackColor = Color.LightGray;
            else if (status == "online")
                row.DefaultCellStyle.BackColor = Color.LightGreen;
        }
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void dateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void dateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
          
        }
    }

}
