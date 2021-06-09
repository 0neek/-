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
    public partial class Form6 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private bool newRowAdding = false;

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\50we\Desktop\эт\WindowsFormsApplication2\WindowsFormsApplication2\MainData.mdf;Integrated Security=True");
            sqlConnection.Open();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'ggg' AS [Пункт назначения] FROM Simplex", sqlConnection);
                // dataGridView1.Columns["Расчет"].Visible = false;

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Simplex");
                dataGridView1.DataSource = dataSet.Tables["Simplex"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns["Столбец"].DisplayIndex = 4;
        }

        


        private bool IsRepeatedCellValue(int rowIndex, int colIndex)
        {
            DataGridViewCell currCell = dataGridView1.Rows[rowIndex].Cells[0];
            DataGridViewCell prevCell = dataGridView1.Rows[rowIndex - 1].Cells[0];

            if (dataGridView1.Rows[rowIndex].Cells[1].Value.Equals(dataGridView1.Rows[rowIndex - 1].Cells[1].Value)
                && dataGridView1.Rows[rowIndex].Cells[2].Value.Equals(dataGridView1.Rows[rowIndex - 1].Cells[2].Value)
                && dataGridView1.Rows[rowIndex].Cells[3].Value.Equals(dataGridView1.Rows[rowIndex - 1].Cells[3].Value)
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Первую строку всегда показывать
            if (e.RowIndex == 0)
                return;


            if (IsRepeatedCellValue(e.RowIndex, e.ColumnIndex) && e.ColumnIndex < 4)
            {
                e.Value = string.Empty;
                e.FormattingApplied = true;
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            // Пропуск заголовков колонок и строк, и первой строки
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
                return;

            if (IsRepeatedCellValue(e.RowIndex, e.ColumnIndex) && e.ColumnIndex < 4)
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.Single;
            }
        }
    }
}
