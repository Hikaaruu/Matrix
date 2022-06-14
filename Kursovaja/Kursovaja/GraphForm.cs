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

            if (TransferData.Points.Count == 0)
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

                return;
            }

            for (int i = 0; i < TransferData.Points.Count; i++)
            {
                    chart1.Series[0].Points.AddXY(Math.Round(TransferData.Points[i].X,3), TransferData.Points[i].Y);
            }
        }
    }
}
