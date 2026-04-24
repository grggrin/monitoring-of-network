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
    public partial class Form_Ignored : Form
    {
        public Form_Ignored()
        {
            InitializeComponent();
            LoadIgnored();
        }

        private void button_addIgnored_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox_IP.Text))
                {
                    MessageBox.Show("Введите IP адрес!");
                    return;
                }

                if (!System.Net.IPAddress.TryParse(textBox_IP.Text, out _))
                {
                    MessageBox.Show("Неверный формат IP!");
                    return;
                }

                string dbPath = Path.Combine(Application.StartupPath, "server.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string createTable = @"CREATE TABLE IF NOT EXISTS Ignored (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    ip TEXT UNIQUE
                                  )";

                    using (SQLiteCommand cmd = new SQLiteCommand(createTable, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    if (string.IsNullOrWhiteSpace(textBoxName.Text))
                    {
                        MessageBox.Show("Введите имя устройства!");
                        return;
                    }
                    string query = "INSERT INTO Ignored (ip, name) VALUES (@ip, @name)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ip", textBox_IP.Text);
                        command.Parameters.AddWithValue("@name", textBoxName.Text);
                        command.ExecuteNonQuery();
                    }
                }
                LoadIgnored();
                MessageBox.Show("Игнорируемое устройство добавлено", "Сообщение");
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Такой IP уже существует!", "Ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void LoadIgnored()
        {
            string dbPath = Path.Combine(Application.StartupPath, "server.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, ip, name FROM Ignored";

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridViewIgnored.DataSource = table;
                }
            }
            dataGridViewIgnored.Columns["id"].HeaderText = "ID";
            dataGridViewIgnored.Columns["ip"].HeaderText = "IP-адрес";
            dataGridViewIgnored.Columns["name"].HeaderText = "Имя устройства";
        }

        private void Form_Ignored_Load(object sender, EventArgs e)
        {

        }

        private void button_deleteIgnored_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewIgnored.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите строку!");
                    return;
                }

                var result = MessageBox.Show("Удалить игнорируемое устройство?", "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes)
                    return;

                int id = Convert.ToInt32(
                    dataGridViewIgnored.SelectedRows[0].Cells["id"].Value
                );

                string dbPath = Path.Combine(Application.StartupPath, "server.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Ignored WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Игнорируемое устройство удалено");
                LoadIgnored(); // обновляем таблицу
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
