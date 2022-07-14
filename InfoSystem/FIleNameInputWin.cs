using System;
using System.Windows.Forms;

namespace InfoSystem
{
    public partial class FIleNameInputWin : Form
    {
        public FIleNameInputWin()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;//默认选中第一项，index为0，而不是空白格的-1
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
            if (this.comboBox1.SelectedIndex<=0)
            {
                MessageBox.Show("请选择区域，莱普或老厂。");
                return;
            }
               
            DialogResult = DialogResult.OK;
            InfoWIn._fileName = fileName;
            InfoWIn._areaValue = this.comboBox1.SelectedIndex;
            this.Close();
            this.Dispose();
        }
    }
}
