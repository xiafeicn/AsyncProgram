using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Thread thread = null;
        int index = 0;

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(1000);

                        textBox1.Invoke(new Action(() =>
                                   {
                                       textBox1.AppendText(string.Format("{0},", index++));
                                   }));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("{0}, {1}", ex.Message, index++));
                    }
                }
            }));

            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread.ThreadState == ThreadState.Running || thread.ThreadState == ThreadState.WaitSleepJoin)
            {
                thread.Suspend();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread.ThreadState == ThreadState.Suspended)
            {
                thread.Resume();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            thread.Interrupt();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            thread.Abort();
        }
    }
}
