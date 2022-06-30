using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Kursovaja.Classes;

namespace Kursovaja
{
    internal class Methods
    {

        public static Matrix GetMatrixFromGrid(DataGridView grid)
        {
            Matrix result = new Matrix(grid.RowCount, grid.ColumnCount);
            DataGridViewRow row;
            double temp;
            bool check;
            for (int i = 0; i < grid.RowCount; i++)
            {
                row = grid.Rows[i];
                for (int j = 0; j < grid.ColumnCount; j++)
                {
                    if (row.Cells[j].Value == null)
                    {
                        result[i, j] = 0;
                    }
                    else
                    {
                        check = double.TryParse(row.Cells[j].Value.ToString().Replace('.', ','), out temp);

                        if (!check)
                        {
                            return null;
                        }
                        else
                        {
                            result[i, j] = temp;
                        }
                    }

                }
            }

            return result;
        }

        public static void PopulateContainer(GroupBox gb, ResultForm form, int index)
        {
            #region prep
            const int grid_top = 40;
            const int cell_height = 37;

            DataGridView grid = new DataGridView();
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.AllowUserToOrderColumns = false;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToResizeColumns = false;
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = form.BackColor;
            grid.Columns.Add("1", "1");
            grid.RowTemplate.Height = cell_height;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridViewColumn column = grid.Columns[0];
            column.Width = 43;
            grid.Width = 45;
            grid.Top = grid_top;
            grid.Left = 20;
            grid.Height = TransferData.Vectors[0].Rows * cell_height + 4;
            #endregion

            #region populating vector
            for (int i = 0; i < TransferData.Vectors[index].Rows; i++)
            {
                grid.Rows.Add();
                grid.Rows[i].Cells[0].Value = string.Format("{0:0.00}", TransferData.Vectors[index][i, 0]);
            }

            gb.AutoSize = true;
            gb.Height = TransferData.Vectors[0].Rows * cell_height + grid_top;
            gb.Width = 100;
            gb.Text = "y" + (index + 1).ToString() + " = " + string.Format("{0:0.00}", TransferData.Roots[index]).ToString();
            gb.Controls.Add(grid);
            #endregion
        }

        public static void WriteResultInFile(StreamWriter sw, Matrix a, List<double> roots, List<Matrix> vectors, int method, double step)
        {

            #region writing matrix in file
            sw.WriteLine("Matrix :");
            sw.WriteLine();

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    sw.Write(string.Format("{0:0.00}", a[i, j]).ToString().PadRight(9));
                }
                sw.WriteLine();
                sw.WriteLine();
            }
            #endregion

            #region writing in file method and step of search
            string s_method = method == 0 ? "Krilova" : "Fadeeva";

            sw.WriteLine();
            sw.WriteLine();
            sw.WriteLine("Method : {0}        Step = {1}", s_method, step.ToString());
            sw.WriteLine();
            sw.WriteLine();
            #endregion

            #region writing result
            sw.WriteLine("Result :");
            sw.WriteLine();


            if (roots.Count == 0)
            {
                sw.WriteLine("Корені не знайдено :(\nМожливо варто змінити крок пошуку!");
                return;
            }
            else
            {
                for (int i = 0; i < roots.Count; i++)
                {
                    sw.Write("y{0} = {1}    ", i, string.Format("{0:0.00}", roots[i]));
                    sw.Write("vec{0} = ", i);


                    sw.Write("( ");
                    for (int j = 0; j < vectors[0].Rows; j++)
                    {
                        sw.Write(string.Format("{0:0.00}", vectors[i][j, 0]));
                        if (j!=vectors[0].Rows-1)
                        {
                            sw.Write("   ");
                        }
                        else
                        {
                            sw.Write(" ");
                        }
                    }
                    sw.Write(")");

                    sw.WriteLine();
                    sw.WriteLine();
                }
            }
            #endregion


        }

    }
}
