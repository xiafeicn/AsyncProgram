using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            copy();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                copy();
            }
        }

        private void copy()
        {
            string filePath = textBox1.Text.Trim();
            if (File.Exists(filePath))
            {
                System.Collections.Specialized.StringCollection strcoll = new System.Collections.Specialized.StringCollection();

                strcoll.Add(filePath);


                //strcoll.Add(dirPath);
                Clipboard.SetFileDropList(strcoll);
                MessageBox.Show("已复制到剪切板");
            }
            else
            {
                MessageBox.Show("不存在的文件路径");
            }
        }
    }
}
