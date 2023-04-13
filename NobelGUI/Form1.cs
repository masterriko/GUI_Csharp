using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace NobelGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            String tekst = letnica.Text;
            int leto;
            List<string> selectedItems = new List<string>();
            if (!int.TryParse(tekst, out leto))
            {
                label3.BackColor = Color.Red;
                label3.Text = "Neveljaven vnos!";
            }
            else if (2023 < leto || leto < 1900)
            {
                label3.BackColor = Color.Red;
                label3.Text = "Neveljavna letnica!";
            }
            else
            {
                foreach (var item in checkedListBox1.Items)
                {
                    if (checkedListBox1.GetItemChecked(checkedListBox1.Items.IndexOf(item)))
                    {
                        selectedItems.Add(item.ToString());
                    }
                }
                selectedItems.Add(leto.ToString());
                label3.BackColor = Color.Azure;
                label3.Text = "Iščem";
                listBox1.Items.Clear();
                await Task.Run(async () =>
                {
                    using (var conn = CreateConnection())
                    {
                        await ReadDataAsync(conn, selectedItems);
                    }
                });
            }

        }
        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source= nobelDB.sqlite; Version = 3; New = False; Compress = True; "); ;
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }
        async Task ReadDataAsync(SQLiteConnection conn, List<String> selectedItems)
        {
            using (var cmd = conn.CreateCommand())
            {
                string query = "SELECT winner FROM nobel WHERE YourColumn IN (";
                query += string.Join(",", selectedItems.Select(item => $"'{item}'"));
                query += ")";
                cmd.CommandText = query;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string myreader = reader.GetString(0);
                        listBox1.Items.Add(myreader);
                    }
                }
            }
            label3.BackColor = Color.Green;
            label3.Text = "Opravljeno";
        }
    }
}
