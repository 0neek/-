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
    public partial class Form2 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private bool newRowAdding = false;
        public Form2()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Удалить], 'Kick' AS [Черный список] FROM Purchase WHERE BlackList = 0", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Purchase");
                dataGridView1.DataSource = dataSet.Tables["Purchase"];
                dataGridView1.Columns[6].Visible = false;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkCell;
                    DataGridViewLinkCell linkKick = new DataGridViewLinkCell();
                    dataGridView1[8, i] = linkKick;
                }

            }
            catch(Exception ex)
            {
               MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
        }
        private void LoadBlackList()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Удалить], 'Remove' AS [Черный список] FROM Purchase WHERE BlackList = 1", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Purchase");
                dataGridView1.DataSource = dataSet.Tables["Purchase"];
                dataGridView1.Columns[6].Visible = false;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    DataGridViewLinkCell linkKick = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkKick;
                    dataGridView1[8, i] = linkCell;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Purchase"].Clear();
                sqlDataAdapter.Fill(dataSet, "Purchase");
                dataGridView1.DataSource = dataSet.Tables["Purchase"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkCell;
                }

            }
            catch (Exception ex) // test
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\0neekОЗZ7\Desktop\эт\WindowsFormsApplication2\WindowsFormsApplication2\bin\Debug\MainData.mdf;Integrated Security=True");
            sqlConnection.Open();
            LoadData();
        }


       private void label2_Click(object sender, EventArgs e)
       {
           this.Close();
       }

       private void label3_Click(object sender, EventArgs e)
       {
           this.WindowState = FormWindowState.Minimized;
       }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string task = string.Empty;
                if (e.ColumnIndex == 8)
                {
                    task = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                    if (task == "Remove")
                    {
                        if (MessageBox.Show("Убрать из черного списка? Поставщик вернется в белый список", "Изменение", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataSet.Tables["Purchase"].Rows[rowIndex]["BlackList"] = "0";
                            sqlDataAdapter.Update(dataSet, "Purchase");
                        }
                    }
                    if (task == "Kick")
                    {
                        if (MessageBox.Show("Добавить в черный список? Поставщик попадет в черный список", "Изменение", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataSet.Tables["Purchase"].Rows[rowIndex]["BlackList"] = "1";
                            sqlDataAdapter.Update(dataSet, "Purchase");
                        }
                    }
                }
                
                if (e.ColumnIndex == 7)
                {
                    task = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["Purchase"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, "Purchase");
                        }
                    }
                    
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["Purchase"].NewRow();
                        row["Поставщик"] = dataGridView1.Rows[rowIndex].Cells["Поставщик"].Value;
                        row["Наименование"] = dataGridView1.Rows[rowIndex].Cells["Наименование"].Value;
                        row["Срок"] = dataGridView1.Rows[rowIndex].Cells["Срок"].Value;
                        row["Цена"] = dataGridView1.Rows[rowIndex].Cells["Цена"].Value;
                        row["Телефон"] = dataGridView1.Rows[rowIndex].Cells["Телефон"].Value;
                        dataSet.Tables["Purchase"].Rows.Add(row);
                        dataSet.Tables["Purchase"].Rows.RemoveAt(dataSet.Tables["Purchase"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "Delete";
                        sqlDataAdapter.Update(dataSet, "Purchase");
                        newRowAdding = false;

                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;
                        dataSet.Tables["Purchase"].Rows[r]["Поставщик"] = dataGridView1.Rows[r].Cells["Поставщик"].Value;
                        dataSet.Tables["Purchase"].Rows[r]["Наименование"] = dataGridView1.Rows[r].Cells["Наименование"].Value;
                        dataSet.Tables["Purchase"].Rows[r]["Срок"] = dataGridView1.Rows[r].Cells["Срок"].Value;
                        dataSet.Tables["Purchase"].Rows[r]["Цена"] = dataGridView1.Rows[r].Cells["Цена"].Value;
                        dataSet.Tables["Purchase"].Rows[r]["Телефон"] = dataGridView1.Rows[r].Cells["Телефон"].Value;
                        sqlDataAdapter.Update(dataSet, "Purchase");
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
                    }
                    
                }
                ReloadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow_1(object sender, DataGridViewRowEventArgs e)
        {
       
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, lastRow] = linkCell;
                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) 
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, rowIndex] = linkCell;
                    editingRow.Cells["Command"].Value = "Update";
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex == 3)
            {
                TextBox textBox = e.Control as TextBox;
                if(textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
            if (dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
            
            if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
            
        }
        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Наименование LIKE '%{textBox1.Text}%'";

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        Point lastPoint;
        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;

            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void поставщикиToolStripMenuItem_Click(object sender, EventArgs e) // Поставщики 
        {
            поставщикиToolStripMenuItem.BackColor = Color.Red;
            черныйСписокToolStripMenuItem.BackColor = Color.Black;
            LoadData();
        }

        private void черныйСписокToolStripMenuItem_Click(object sender, EventArgs e) // Черный список
        {
            поставщикиToolStripMenuItem.BackColor = Color.Black;
            черныйСписокToolStripMenuItem.BackColor = Color.Red;
            LoadBlackList();
        }
    }
}
