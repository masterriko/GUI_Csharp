using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kapljice
{
    public partial class Form1 : Form
    {
        private Timer rainTimer;
        private List<Point> rainDrops;
        public Form1()
        {
            InitializeComponent();
            rainDrops = new List<Point>();
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += new EventHandler(RainTimer);
            timer.Start(); 
        }

        private void Graphics(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Blue, 2);

            foreach (Point p in rainDrops)
            {
                g.DrawLine(pen, p, new Point(p.X, p.Y + 10));
            }
        }

        private void RainTimer(object sender, EventArgs e)
        {
            Random rand = new Random();
            int numDrops = rand.Next(3, 7);
            for (int i = 0; i < numDrops; i++)
            {
                int x = rand.Next(0, this.Width);
                rainDrops.Add(new Point(x, 0));
            }

            for (int i = rainDrops.Count - 1; i >= 0; i--)
            {
                Point p = rainDrops[i];
                if (p.Y >= this.Height)
                {
                    rainDrops.RemoveAt(i);
                }
                else
                {
                    rainDrops[i] = new Point(p.X, p.Y + 10);
                }
            }

            this.Invalidate();
        }
    }
}
