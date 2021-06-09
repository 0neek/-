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
    public partial class Form3 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private bool newRowAdding = false;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
                sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\0neekОЗZ7\Desktop\эт\WindowsFormsApplication2\WindowsFormsApplication2\bin\Debug\MainData.mdf;Integrated Security=True");
                sqlConnection.Open();
                LoadData();
            
        }
        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM SavingItem", sqlConnection);
               // dataGridView1.Columns["Расчет"].Visible = false;

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "SavingItem");
                dataGridView1.DataSource = dataSet.Tables["SavingItem"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
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
                dataSet.Tables["SavingItem"].Clear();
                sqlDataAdapter.Fill(dataSet, "SavingItem");
                dataGridView1.DataSource = dataSet.Tables["SavingItem"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                try
                {
                    if (e.ColumnIndex == 6)
                    {
                        string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                        if (task == "Delete")
                        {
                            if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                == DialogResult.Yes)
                            {
                                int rowIndex = e.RowIndex;
                                dataGridView1.Rows.RemoveAt(rowIndex);
                                dataSet.Tables["SavingItem"].Rows[rowIndex].Delete();
                                sqlDataAdapter.Update(dataSet, "SavingItem");
                            }
                        }
                        else if (task == "Insert")
                        {
                            int rowIndex = dataGridView1.Rows.Count - 2;
                            DataRow row = dataSet.Tables["SavingItem"].NewRow();
                            row["Наименование"] = dataGridView1.Rows[rowIndex].Cells["Наименование"].Value;
                            row["Единица"] = dataGridView1.Rows[rowIndex].Cells["Единица"].Value;
                            row["Количество"] = dataGridView1.Rows[rowIndex].Cells["Количество"].Value;
                            row["Цена"] = dataGridView1.Rows[rowIndex].Cells["Цена"].Value;
                            row["Необходимое количество"] = dataGridView1.Rows[rowIndex].Cells["Необходимое количество"].Value;
                            dataSet.Tables["SavingItem"].Rows.Add(row);
                            dataSet.Tables["SavingItem"].Rows.RemoveAt(dataSet.Tables["SavingItem"].Rows.Count - 1);
                            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                            dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
                            sqlDataAdapter.Update(dataSet, "SavingItem");
                            newRowAdding = false;

                        }
                        else if (task == "Update")
                        {
                            int r = e.RowIndex;
                            dataSet.Tables["SavingItem"].Rows[r]["Наименование"] = dataGridView1.Rows[r].Cells["Наименование"].Value;
                            dataSet.Tables["SavingItem"].Rows[r]["Единица"] = dataGridView1.Rows[r].Cells["Единица"].Value;
                            dataSet.Tables["SavingItem"].Rows[r]["Количество"] = dataGridView1.Rows[r].Cells["Количество"].Value;
                            dataSet.Tables["SavingItem"].Rows[r]["Цена"] = dataGridView1.Rows[r].Cells["Цена"].Value;
                            dataSet.Tables["SavingItem"].Rows[r]["Необходимое количество"] = dataGridView1.Rows[r].Cells["Необходимое количество"].Value;
                            sqlDataAdapter.Update(dataSet, "SavingItem");
                            dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
                        }
                        ReloadData();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, lastRow] = linkCell;
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
                    dataGridView1[6, rowIndex] = linkCell;
                    editingRow.Cells["Command"].Value = "Update";
                }

            }
            catch (Exception ex)
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
                if (textBox != null)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            Form4 f4 = new Form4();
            f4.ShowDialog();
            f3.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
