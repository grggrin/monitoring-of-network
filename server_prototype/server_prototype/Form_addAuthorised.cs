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
    public partial class Form_addAuthorised : Form
    {
        public Form_addAuthorised()
        {
            InitializeComponent();
            LoadAuthorised();
        }

        private void button_addAuthorised_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("Кнопка нажата");
            try
            {
                if (string.IsNullOrWhiteSpace(textBox_ipAuthorised.Text))
                {
                    MessageBox.Show("Введите IP адрес!");
                    return;
                }

                if (!System.Net.IPAddress.TryParse(textBox_ipAuthorised.Text, out _))
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
                    string createTable = @"CREATE TABLE IF NOT EXISTS Authorised (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    ip TEXT UNIQUE
                                  )";

                    using (SQLiteCommand cmd = new SQLiteCommand(createTable, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // вставка
                    string query = "INSERT INTO Authorised (ip) VALUES (@ip)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ip", textBox_ipAuthorised.Text);
                        command.ExecuteNonQuery();
                    }
                }
                LoadAuthorised();
                MessageBox.Show("Авторизированное устройство добавлено", "Сообщение");
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

        private void button_deleteAuthorised_Click(object sender, EventArgs e)
        {


            try
            {
                if (dataGridViewAuthorised.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите строку!");
                    return;
                }

                var result = MessageBox.Show("Удалить авторизированное устройство?", "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes)
                    return;

                int id = Convert.ToInt32(
                    dataGridViewAuthorised.SelectedRows[0].Cells["id"].Value
                );

                string dbPath = Path.Combine(Application.StartupPath, "server.db");
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Authorised WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Авторизированное устройство удалено");

                LoadAuthorised(); // обновляем таблицу
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

       
        private void LoadAuthorised()
        {
            string dbPath = Path.Combine(Application.StartupPath, "server.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, ip FROM Authorised";

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridViewAuthorised.DataSource = table;
                }
            }

            dataGridViewAuthorised.Columns["id"].HeaderText = "ID";
            dataGridViewAuthorised.Columns["ip"].HeaderText = "IP-адрес";
        }
    }
}
