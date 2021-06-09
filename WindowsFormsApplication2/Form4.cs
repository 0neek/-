using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication2
{
    public partial class Form4 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SqlDataReader dataReader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Наименование, Единица, Количество FROM SavingItem", sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                ListViewItem item = null;
                
                while (dataReader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(dataReader["Наименование"]), Convert.ToString(dataReader["Единица"]), Convert.ToString(dataReader["Количество"]) });
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\0neekОЗZ7\Desktop\эт\WindowsFormsApplication2\WindowsFormsApplication2\bin\Debug\MainData.mdf;Integrated Security=True");
            sqlConnection.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(); 
            f2.ShowDialog();    
        }
    }
}
