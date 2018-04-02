using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;

namespace AsyncProgram
{
    public partial class FrmMain : CCSkinMain
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void showForm(Form frmForm)
        {
            frmForm.StartPosition = FormStartPosition.CenterScreen;
            frmForm.ShowDialog();
        }

        private void btnAPM_Click(object sender, EventArgs e)
        {
            showForm(new Form1());
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            showForm(new Form2());
        }

        private void buttonX1_Click_1(object sender, EventArgs e)
        {
            showForm(new FrmThread()); ;
        }
    }
}
