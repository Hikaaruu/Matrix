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
    public partial class ResultForm : Form
    {
        public ResultForm()
        {
            InitializeComponent();
        }

        private void result_form_Load(object sender, EventArgs e)
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

            #region no roots situation
            if (TransferData.Roots.Count == 0)
            {
                Label label = new Label();


                label.Text = "Корені не знайдено ( ˘︹˘ )\n Можливо варто змінити крок пошуку!";
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


                Button btn1 = new Button();
                btn1.Text = "Побудувати графік";
                btn1.Height = 25;
                btn1.Width = 120;
                btn1.Left = Width / 2 - 70;
                btn1.Top = Height - 75;
                btn1.Click += new EventHandler(btn_Click);
                Controls.Add(btn1);

                return;
            }

            #endregion

            #region loading result

            #region displaying main part
            Width = 120 * (TransferData.Vectors.Count - 1) + 130;
            Height = TransferData.Vectors[0].Rows * 37 + 160;

            for (int i = 0; i < TransferData.Vectors.Count; i++)
            {

                GroupBox gb = new GroupBox();
                gb.Left = 120 * i;
                Methods.PopulateContainer(gb, this, i);
                Controls.Add(gb);
            }
            #endregion


            #region graph button
            Button btn = new Button();
            btn.Text = "Побудувати графік";
            btn.Height = 25;
            btn.Width = 120;
            btn.Left = Width / 2 - 70;
            btn.Top = Height - 80;
            btn.Click += new EventHandler(btn_Click);
            Controls.Add(btn);
            #endregion


            #region analytic data
            if (TransferData.AnalyticData)
            {

                Height += 120;

                Label data_label = new Label();
                data_label.Text = "Кількість кроків :\n" + TransferData.StepsCount.ToString() + "\n" +
               "\n" + "Кількість ітерацій методу бісекції :\n" + TransferData.IterationsCount.ToString();
                data_label.Width = Width - 50;
                data_label.Height = 100;
                data_label.ForeColor = Color.Black;
                data_label.Font = new Font("Arial", TransferData.Roots.Count <= 2 ? 8 : 9);
                data_label.Left = 15;
                data_label.Top = Height - 150;
                data_label.TextAlign = ContentAlignment.MiddleCenter;
                Controls.Add(data_label);
            }
            #endregion


            #endregion

        }

        private void btn_Click(object sender, EventArgs e)
        {
            GraphForm graphForm = new GraphForm();
            graphForm.ShowDialog();
        }
    }
}
