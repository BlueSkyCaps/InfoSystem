using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InfoSystem
{
    public partial class FIleNameInputWin : Form
    {
        public FIleNameInputWin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileName = this.textBox1.Text.Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("请输入内容，或点击取消。");
                return;
            }
            DialogResult = DialogResult.OK;
            InfoWIn._fileName = fileName;
            this.Close();
            this.Dispose();
        }
    }
}
