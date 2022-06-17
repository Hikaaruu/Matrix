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
    public partial class GraphForm : Form
    {
        public GraphForm()
        {
            InitializeComponent();
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {

            #region prep
            // Define the border style of the form to a dialog box.
            FormBorderStyle = FormBorderStyle.FixedDialog;

            // Set the MaximizeBox to false to remove the maximize box.
            MaximizeBox = false;

            // Set the MinimizeBox to false to remove the minimize box.
            MinimizeBox = false;

            // Set the start position of the form to the center of the screen.
            StartPosition = FormStartPosition.CenterScreen;
            #endregion

            #region 0 or small amount of  points case
            if (TransferData.Points.Count < 15)
            {
                if (TransferData.DopPoints.Count != 0 && !double.IsNaN(TransferData.DopPoints[0].Y))
                {
                    #region checking max and min func value
                    double max_ = TransferData.DopPoints[0].Y;
                    double min_ = TransferData.DopPoints[0].Y;
                    for (int i = 1; i < TransferData.DopPoints.Count; i++)
                    {
                        if (TransferData.DopPoints[i].Y > max_)
                        {
                            max_ = TransferData.DopPoints[i].Y;
                        }
                        if (TransferData.DopPoints[i].Y < min_)
                        {
                            min_ = TransferData.DopPoints[i].Y;
                        }
                    }
                    #endregion

                    if (max_ < Math.Pow(10, 6) && Math.Abs(min_) < Math.Pow(10, 6))
                    {
                        for (int i = 0; i < TransferData.DopPoints.Count; i++)
                        {
                            chart1.Series[0].Points.AddXY(Math.Round(TransferData.DopPoints[i].X, 3), Math.Round(TransferData.DopPoints[i].Y, 3));
                        }
                    }
                    else
                    {

                        chart1.Visible = false;
                        Label label = new Label();


                        label.Text = "Неможливо побудувати графік ( ˘︹˘ )\n\nМожливо варто змінити крок пошуку";
                        label.Width = 400;
                        label.Height = 90;
                        label.ForeColor = Color.Red;
                        label.Font = new Font("Arial", 10);
                        label.Left = 40;
                        label.Top = 0;
                        label.TextAlign = ContentAlignment.MiddleCenter;


                        Width = 500;
                        Height = 150;
                        Controls.Add(label);

                    }
                }
                else
                {
                    chart1.Visible = false;
                    Label label = new Label();


                    label.Text = "Неможливо побудувати графік ( ˘︹˘ )\n Можливо варто змінити метод пошуку!";
                    label.Width = 200;
                    label.Height = 90;
                    label.ForeColor = Color.Red;
                    label.Font = new Font("Arial", 10);
                    label.Left = 38;
                    label.Top = 0;
                    label.TextAlign = ContentAlignment.MiddleCenter;


                    Width = 300;
                    Height = 150;
                    Controls.Add(label);

                }

                return;

            }
            #endregion


            #region buidlding graph

            #region checking max and min func value
            double max = TransferData.Points[0].Y;
            double min = TransferData.Points[0].Y;
            for (int i = 1; i < TransferData.Points.Count; i++)
            {
                if (TransferData.Points[i].Y > max)
                {
                    max = TransferData.Points[i].Y;
                }
                if (TransferData.Points[i].Y < min)
                {
                    min = TransferData.Points[i].Y;
                }
            }
            #endregion

            #region adding points to graph
            if (max < Math.Pow(10, 20) && Math.Abs(min) < Math.Pow(10, 20))
            {
                for (int i = 0; i < TransferData.Points.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(Math.Round(TransferData.Points[i].X, 3), Math.Round(TransferData.Points[i].Y, 3));
                }
            }
            else
            {
                chart1.Visible = false;
                Label label = new Label();


                label.Text = "Неможливо побудувати графік ( ˘︹˘ )\n\nЗначення функції зандто великі/малі";
                label.Width = 400;
                label.Height = 90;
                label.ForeColor = Color.Red;
                label.Font = new Font("Arial", 10);
                label.Left = 40;
                label.Top = 0;
                label.TextAlign = ContentAlignment.MiddleCenter;


                Width = 500;
                Height = 150;
                Controls.Add(label);
                return;
            }
            #endregion


            #endregion

        }

    }
}
