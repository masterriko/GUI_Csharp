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
using System.Data.Entity;

namespace filmiGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source= filmi.sqlite; Version = 3; New = True; Compress = True; ");
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

        private async void button1_Click(object sender, EventArgs e)
        {

            String tekst = textBox1.Text;
            int leto;
            if (!int.TryParse(tekst, out leto))
            {
                label2.BackColor = Color.Red;
                label2.Text = "Neveljaven vnos!";
            }
            else if ( 2023 < leto || leto < 1900)
            {
                label2.BackColor = Color.Red;
                label2.Text = "Neveljavna letnica!";
            }
            else
            {
                label2.BackColor = Color.Azure;
                label2.Text = "Iščem";
                listBox1.Items.Clear();
                await Task.Run(async () =>
                {
                    using (var conn = CreateConnection())
                    {
                        await ReadDataAsync(conn, leto);
                    }
                });
            }
        }
        async Task ReadDataAsync(SQLiteConnection conn, int leto)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT naslov FROM filmi WHERE leto = {leto}";
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string myreader = reader.GetString(0);
                        listBox1.Items.Add(myreader);
                    }
                }
            }
            label2.BackColor = Color.Green;
            label2.Text = "Opravljeno";
        }
    }
}
