using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Data.SQLite;
//using Microsoft.Data.Sqlite;
using System.Data.SQLite;

using System.IO;

namespace server_prototype
{
    public partial class Form_addAgents : Form
    {
        public Form_addAgents()
        {
            InitializeComponent();
            LoadAgents();

            //MessageBox.Show("Это точно Form2");
        }

        /*private void button_addAgent_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Кнопка нажата");

            try
            {
                if (string.IsNullOrWhiteSpace(textBox_ipAgent.Text))
                {
                    MessageBox.Show("Введите IP адрес!");
                    return;
                }

                if (!System.Net.IPAddress.TryParse(textBox_ipAgent.Text, out _))
                {
                    MessageBox.Show("Неверный формат IP!");
                    return;
                }

                string dbPath = Path.Combine(Application.StartupPath, "server.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // создаём таблицу если нет
                    string createTable = @"CREATE TABLE IF NOT EXISTS Agents (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    ip TEXT
                                  )";

                    using (SQLiteCommand cmd = new SQLiteCommand(createTable, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // вставка
                    string query = "INSERT INTO Agents (ip) VALUES (@ip)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ip", textBox_ipAgent.Text);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Агент добавлен", "Сообщение");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        } */


        private void button_addAgent_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("Кнопка нажата");
            try
            {
                if (string.IsNullOrWhiteSpace(textBox_ipAgent.Text))
                {
                    MessageBox.Show("Введите IP адрес!");
                    return;
                }

                if (!System.Net.IPAddress.TryParse(textBox_ipAgent.Text, out _))
                {
                    MessageBox.Show("Неверный формат IP!");
                    return;
                }

                string dbPath = Path.Combine(Application.StartupPath, "server.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // таблица с UNIQUE
                    string createTable = @"CREATE TABLE IF NOT EXISTS Agents (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    ip TEXT UNIQUE
                                  )";

                    using (SQLiteCommand cmd = new SQLiteCommand(createTable, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // вставка
                    string query = "INSERT INTO Agents (ip) VALUES (@ip)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ip", textBox_ipAgent.Text);
                        command.ExecuteNonQuery();
                    }
                }
                LoadAgents();
                MessageBox.Show("Агент добавлен", "Сообщение");
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





        private void button_deleteAgents_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewAgents.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите строку!");
                    return;
                }

                var result = MessageBox.Show("Удалить агента?", "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes)
                    return;

                int id = Convert.ToInt32(
                    dataGridViewAgents.SelectedRows[0].Cells["id"].Value
                );

                string dbPath = Path.Combine(Application.StartupPath, "server.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Agents WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Агент удалён");

                LoadAgents(); // обновляем таблицу
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void LoadAgents()
        {
            string dbPath = Path.Combine(Application.StartupPath, "server.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, ip FROM Agents";

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridViewAgents.DataSource = table;
                }
            }

            dataGridViewAgents.Columns["id"].HeaderText = "ID";
            dataGridViewAgents.Columns["ip"].HeaderText = "IP-адрес";
        }
    }
}
