using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Мой ключ для АПИ
            string myAPI = "";
        }


        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        Point lastPoint;
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;

            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            Form1 f1 = new Form1();             
            f2.ShowDialog();
            f1.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            Form1 f1 = new Form1();
            f3.ShowDialog();
            f1.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string myAPI = "";
            Form5 form= new Form5(myAPI);
            form.ShowDialog();
            Close();
        }
    }
}
