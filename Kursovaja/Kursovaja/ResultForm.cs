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
            DataGridView grid;
            Label label;
            for (int i = 0; i < TransferData.Vectors.Count; i++)
            {


                GroupBox gb  = new GroupBox();
                gb.Left = 120 * i;
                Methods.PopulateContainer(gb, this, i);
                Controls.Add(gb);
            }

        }
    }
}
