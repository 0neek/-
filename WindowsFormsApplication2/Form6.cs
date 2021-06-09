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
using System.Globalization;
using SpannedDataGridView;
using System.IO;
using LinearEquationNS;
using SimplexMethodNS;






namespace WindowsFormsApplication2
{
    public partial class Form6 : Form
    {

        public Form6()
        {
            InitializeComponent();


            if (textBox1.Text == "" || textBox2.Text == "")
            {
                //Ничего не делаем
                dataGridView1.RowCount = 7;
                dataGridView1.ColumnCount = 6;
            }
            Otrisovka();

        }
        
        public void Otrisovka()
        { 
            dataGridView1[0, 0].Value = string.Format("Пункт отправления");
            int f = dataGridView1.ColumnCount -1;
            int g = dataGridView1.RowCount - 1;
            dataGridView1[f, 0].Value = string.Format("Запасы");
            dataGridView1[0, g].Value = string.Format("Потребности");
            ((DataGridViewTextBoxCellEx)dataGridView1[1, 0]).ColumnSpan = f-1;
            dataGridView1[1, 0].Value = string.Format("Пункты назначения");
            var cell = (DataGridViewTextBoxCellEx)dataGridView1[0, 0];
            cell.ColumnSpan = 1;
            cell.RowSpan = 2;
            for (int j = 1; j < f; j++)
            {
                dataGridView1[j, 1].Value = string.Format(CultureInfo.CurrentCulture,
                    "{0}", j);
            }
            for (int i= 2; i < g; i++)
            {
                dataGridView1[0, i].Value = string.Format(CultureInfo.CurrentCulture,
                    "{0}", i-1);
            }
            dataGridView1.Columns[0].Width = 120;
        }


        #region Presenters
        public class CellPresenter
        {
            #region Properties
            [Browsable(false)]
            public DataGridViewCell Cell { get; private set; }
            public bool ReadOnly
            {
                get { return Cell.ReadOnly; }
                set { Cell.ReadOnly = value; }
            }
            public string Value
            {
                get { return (string)Cell.Value; }
                set { Cell.Value = value; }
            }
            public int ColumnSpan
            {
                get
                {
                    var cell = Cell as DataGridViewTextBoxCellEx;
                    if (cell != null)
                        return cell.ColumnSpan;
                    return 1;
                }
                set
                {
                    var cell = Cell as DataGridViewTextBoxCellEx;
                    if (cell != null)
                        cell.ColumnSpan = value;

                }
            }
            public int RowSpan
            {
                get
                {
                    var cell = Cell as DataGridViewTextBoxCellEx;
                    if (cell != null)
                        return cell.RowSpan;
                    return 1;
                }
                set
                {
                    var cell = Cell as DataGridViewTextBoxCellEx;
                    if (cell != null)
                        cell.RowSpan = value;
                }
            }
            public bool ColumnFrozen
            {
                get { return Cell.OwningColumn.Frozen; }
                set { Cell.OwningColumn.Frozen = value; }
            }
            public bool RowFrozen
            {
                get { return Cell.OwningRow.Frozen; }
                set { Cell.OwningRow.Frozen = value; }
            }
            public DataGridViewCellStyle CellStyle
            {
                get { return Cell.Style; }
                set { Cell.Style = value; }
            }
            public int ColumnDividerWidth
            {
                get { return Cell.OwningColumn.DividerWidth; }
                set { Cell.OwningColumn.DividerWidth = value; }
            }
            public int RowDividerHeight
            {
                get { return Cell.OwningRow.DividerHeight; }
                set { Cell.OwningRow.DividerHeight = value; }
            }
            #endregion

            #region ctor
            public CellPresenter(DataGridViewCell cell)
            {
                Cell = cell;
            }
            #endregion
        }
        public class RowPresenter
        {
            DataGridViewRow m_Row;
            public bool Visible
            {
                get { return m_Row.Visible; }
                set { m_Row.Visible = value; }
            }
            public bool Frozen
            {
                get { return m_Row.Frozen; }
                set { m_Row.Frozen = value; }
            }
            public int Index
            {
                get { return m_Row.Index; }
            }

            #region ctor
            public RowPresenter(DataGridViewRow row)
            {
                m_Row = row;
            }
            #endregion
        }
        #endregion

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0 && e.ColumnIndex == 0)
            {
                e.CellStyle.BackColor = Color.Azure;
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int txtCol = int.Parse(textBox1.Text);
            int txtRow = int.Parse(textBox2.Text);
            dataGridView1.ColumnCount = txtCol + 2;
            dataGridView1.RowCount = txtRow + 3;
            Otrisovka();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(@"1.txt", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fs);

            try
            {
                for (int j = 2; j < dataGridView1.Rows.Count - 1; j++)
                {
                    for (int i = 1; i < dataGridView1.Rows[j].Cells.Count - 1; i++)
                    {
                        int m = dataGridView1.Rows[j].Cells[i].RowIndex - 1;

                        int n = dataGridView1.Rows[j].Cells[i].ColumnIndex;

                        streamWriter.Write("-1*x" + m + n);//Записывает в столбцы(для ограничений) горизонтально
                        int k = dataGridView1.ColumnCount - 2;
                        if (n == k)
                        {
                            var p = dataGridView1[k + 1, j].Value;
                            streamWriter.Write("= -" + p);
                            streamWriter.WriteLine();
                        }

                    }

                }
                int Cc = dataGridView1.RowCount - 2;
                int t = 1;

            Mark:
                for (int i = 1; i < Cc; i++)
                {
                    streamWriter.Write("-1*x" + i + t);
                }

                int cc = dataGridView1.RowCount - 1;
                var tt = dataGridView1.Rows[cc].Cells[t].Value;
                streamWriter.Write("= -" + tt);
                t++;
                if (t < dataGridView1.ColumnCount - 1)
                {

                    streamWriter.WriteLine();
                    goto Mark;
                }

                streamWriter.WriteLine();


                for (int j = 2; j < dataGridView1.Rows.Count - 1; j++)
                {
                    for (int i = 1; i < dataGridView1.Rows[j].Cells.Count - 1; i++)
                    {
                        int m = dataGridView1.Rows[j].Cells[i].RowIndex - 1;

                        int n = dataGridView1.Rows[j].Cells[i].ColumnIndex;
                        var b = dataGridView1.Rows[j].Cells[i].Value;

                        streamWriter.Write("-" + b + "*x" + m + n);
                    }

                }
                streamWriter.Write("=>min");

                streamWriter.Close();
                fs.Close();



                var text = File.ReadAllText(@"1.txt");
                var les = LinearEquationSystem.Parse(text);
                var solver = new SimplexSolver();
                var res = solver.Solve(les);

                FileStream fstr = new FileStream(@"11.txt", FileMode.Create);
                StreamWriter strWriter = new StreamWriter(fstr);
                try
                {

                    textBox3.Text = "";
                    strWriter.WriteLine("Решение:");
                    foreach (var pair in res)
                    {

                        strWriter.WriteLine("{0} = {1}", pair.Key, pair.Value);



                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                strWriter.Close();
                fstr.Close();
                textBox3.Text = File.ReadAllText("11.txt");

                //_________________________________________________________

                FileStream fst = new FileStream(@"111.txt", FileMode.Create);
                StreamWriter strWrite = new StreamWriter(fst);

                foreach (var pair in res)
                {

                    strWrite.Write(pair.Value + " ");

                }
                strWrite.WriteLine();
                for (int j = 2; j < dataGridView1.Rows.Count - 1; j++)
                {
                    for (int i = 1; i < dataGridView1.Rows[j].Cells.Count - 1; i++)
                    {

                        var b = dataGridView1.Rows[j].Cells[i].Value;

                        strWrite.Write(b+" ");
                    }

                }

                strWrite.Close();
                fst.Close();
                // var txt = File.ReadAllText(@"111.txt");
                // int u = int.Parse(txt);
                //label3.Text = Convert.ToString(u);
                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            streamWriter.Close();
            fs.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.SelectionStart = textBox3.Text.Length;
            textBox3.ScrollToCaret();
            textBox3.Refresh();
        }
    }
}
