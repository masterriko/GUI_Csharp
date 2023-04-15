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
using System.Xml;

namespace NobelGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IDictionary<string, string> translation = new Dictionary<string, string>() { { "medicina", "Medicine" }, { "mir", "Peace" }, { "kemija", "Chemistry" }, { "literatura", "Literature" } , { "fizika", "Physics" } , { "ekonomija", "Economics" } };
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
                        selectedItems.Add(translation[item.ToString()]);
                    }
                }
                label3.BackColor = Color.Azure;
                label3.Text = "Iščem";
                listBox1.Items.Clear();
                SQLiteConnection sqlite_conn;
                sqlite_conn = CreateConnection();
                ReadData(sqlite_conn, selectedItems, tekst);
            }

        }
        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source= nobelDB.db; Version = 3; New = True; Compress = True; ");
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
        private void ReadData(SQLiteConnection conn, List<String> selectedItems, string leto)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            string query = $"SELECT winner FROM nobel WHERE yr= {leto} AND subject IN (";
            query += string.Join(",", selectedItems.Select(item => $"'{item}'"));
            query += ")";
            sqlite_cmd.CommandText = query;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                listBox1.Items.Add(myreader);
            }
            conn.Close();
            label3.BackColor = Color.Green;
            label3.Text = "Opravljeno";
        }
    }
}
