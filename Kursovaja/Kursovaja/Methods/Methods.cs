using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kursovaja.Classes;

namespace Kursovaja
{
    internal class Methods
    {

        public static  Matrix GetMatrixFromGrid(DataGridView grid)
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
                        check = double.TryParse(row.Cells[j].Value.ToString().Replace('.',','), out temp );

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

            const int grid_top = 40;
            const int cell_height = 37;

            DataGridView grid = new DataGridView();
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
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

            //Label label = new Label();
            //label.Height = 45;
            //label.Width = 40;
            //label.Text = "y" + (index + 1).ToString() + " = " + TransferData.Roots[index].ToString();

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
        }
    }
}
