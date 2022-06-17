using Kursovaja.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaja
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            DataGridViewColumn column;

            dataGridView1.Columns.Clear();
            int val = (int)numericUpDown1.Value;
            if (val <= 1 || val > 10)
            {
                return;
            }
            else
            {
                for (int i = 0; i < val; i++)
                {
                    dataGridView1.Columns.Add("column" + i, "column" + i); //добавляем столбцы
                    column = dataGridView1.Columns[i];
                    column.Width = 43;
                    dataGridView1.Rows.Add(); //добавляем строки
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            #region prep
            label2.Visible = false;
            progressBar1.Visible = false;
            #endregion

            #region get matrix
            Matrix input = Methods.GetMatrixFromGrid(dataGridView1);
            if (input == null)
            {
                label2.Visible = true;
                label2.Text = "Некоректний формат вхідних даних! Матрицю можна заповнювати лише числами!";
                return;
            }
            #endregion

            #region get step
            double step;
            string step_s = textBox1.Text.Replace('.', ',');
            bool check = double.TryParse(step_s, out step);

            if (!check)
            {
                label2.Visible = true;
                label2.Text = "Некоректний формат вхідних даних! Крок заданий неправильно!";
                return;
            }
            #endregion

            #region get result
            TransferData.IterationsCount = 0;
            int method = int.Parse(comboBox1.GetItemText(comboBox1.SelectedIndex));

            List<double> roots;
            List<Matrix> vectors;

            switch (method)
            {
                case 0:
                    roots = KrilovMethod.Calculate(input, out vectors, step, progressBar1);
                    break;
                case 1:
                    roots = FadeefMethod.Calculate(input, out vectors, step, progressBar1);
                    break;
                default:
                    label2.Visible = true;
                    label2.Text = "Виберіть метод для пошуку!";
                    return;
            }
            #endregion

            #region saving in file

            if (checkBox1.Checked)
            {
                string path = DateTime.Today.ToString("dd/MM/yyyy").Replace('.', '_') + "_" + DateTime.Now.Ticks.ToString() + ".txt";
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    Methods.WriteResultInFile(sw, input, roots, vectors, method, step);
                }
            }



            #endregion

            #region generate result form

            TransferData.AnalyticData = checkBox2.Checked;
            TransferData.Roots = roots;
            TransferData.Vectors = vectors;

            ResultForm result_form = new ResultForm();
            result_form.ShowDialog();

            #endregion region


        }

        private void Form1_Activated(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Visible = false;
            progressBar1.Visible = false;
            // Define the border style of the form to a dialog box.
            FormBorderStyle = FormBorderStyle.FixedDialog;

            // Set the MaximizeBox to false to remove the maximize box.
            MaximizeBox = false;

            // Set the MinimizeBox to false to remove the minimize box.
            MinimizeBox = false;

            // Set the start position of the form to the center of the screen.
            StartPosition = FormStartPosition.CenterScreen;

            dataGridView1.Columns.Clear();
            DataGridViewColumn column;
            for (int i = 0; i < 2; i++)
            {
                dataGridView1.Columns.Add("column" + i, "column" + i); //добавляем столбцы
                column = dataGridView1.Columns[i];
                column.Width = 43;
                dataGridView1.Rows.Add(); //добавляем строки
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow row;
            Random rnd = new Random();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                row = dataGridView1.Rows[i];
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    row.Cells[j].Value = rnd.Next(-100, 100);
                }
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                double result;
                bool check = double.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace(".", ","), out result);


                if (check)
                {
                    if (Math.Abs(result) >= 1000)
                    {
                        if (result < 0)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = result.ToString().Substring(0, 4);
                        }
                        else
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = result.ToString().Substring(0, 3);
                        }
                        
                    }
                    return;
                }
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
            }

        }
    }
}
