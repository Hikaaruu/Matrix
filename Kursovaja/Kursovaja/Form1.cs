using Kursovaja.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            if (val<=1 || val>10)
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
            Matrix input = Methods.GetMatrixFromGrid(dataGridView1);
            if (input==null)
            {
                label2.Visible = true;
                label2.Text = "Некоректний формат вхідних даних! Матрицю можна заповнювати лише числами!";
                return;
            }

            int method = int.Parse(comboBox1.GetItemText(comboBox1.SelectedIndex));

            List<double> roots;
            List<Matrix> vectors;

            switch (method)
            {
                case 0:
                    roots = KrilovMethod.Calculate(input, out vectors);
                    break;
                case 1:
                    roots = FadeefMethod.Calculate(input, out vectors);
                    break;
                default:
                    label2.Visible = true;
                    label2.Text = "Виберіть метод для пошуку!";
                    return;
            }

            TransferData.Roots = roots;
            TransferData.Vectors = vectors;

            ResultForm result_form = new ResultForm();
            result_form.ShowDialog();


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

            Form1 form1 = new Form1();
            // Define the border style of the form to a dialog box.
            form1.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Set the MaximizeBox to false to remove the maximize box.
            form1.MaximizeBox = false;

            // Set the MinimizeBox to false to remove the minimize box.
            form1.MinimizeBox = false;

            // Set the start position of the form to the center of the screen.
            form1.StartPosition = FormStartPosition.CenterScreen;

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
    }
}
