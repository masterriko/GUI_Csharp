using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NobelPostGres
{
    public partial class Form1 : Form
    {
        private const string povNiz = @"Server= baza.fmf.uni-lj.si; 
        User Id= student11;
        Password= student;
        Database= nobel2012;";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlConnection povezava = new NpgsqlConnection(povNiz);
            IDictionary<string, string> translation = new Dictionary<string, string>() { { "medicina", "Medicine" }, { "mir", "Peace" }, { "kemija", "Chemistry" }, { "literatura", "Literature" }, { "fizika", "Physics" }, { "ekonomija", "Economics" } };
            String tekst = textBox1.Text;
            int leto;
            List<string> selectedItems = new List<string>();
            if (!int.TryParse(tekst, out leto))
            {
                label1.BackColor = Color.Red;
                label1.Text = "Neveljaven vnos!";
            }
            else if (2023 < leto || leto < 1900)
            {
                label1.BackColor = Color.Red;
                label1.Text = "Neveljavna letnica!";
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
                label1.BackColor = Color.Azure;
                label1.Text = "Iščem";
                listBox1.Items.Clear();

                povezava.Open();
                string query = $"SELECT winner FROM nobel WHERE yr= {leto} AND subject IN (";
                query += string.Join(",", selectedItems.Select(item => $"'{item}'"));
                query += ")";
                NpgsqlCommand ukaz = new NpgsqlCommand(query, povezava);
                NpgsqlDataReader rez = ukaz.ExecuteReader();
                while (rez.Read())
                {
                    string vrstica = "";
                    for (int i = 0; i < rez.VisibleFieldCount; i++)
                    {
                        vrstica += rez[i].ToString() + " ";
                    }
                    listBox1.Items.Add(vrstica);
                }
                povezava.Close();
            }
            label1.BackColor = Color.Green;
            label1.Text = "Opravljeno";
        }
    }
}
