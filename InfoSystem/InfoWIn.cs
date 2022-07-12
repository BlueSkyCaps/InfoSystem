using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InfoSystem
{
    public partial class InfoWIn : Form
    {
        public static string _fileName = null;
        public InfoWIn()
        {
            InitializeComponent();
        }

        public InfoWIn(string info, Form winObj=null)
        {
            InitializeComponent();
            this.richTextBox1.Text = info;
            // 若Main窗口已经关闭后执行代码又引发异常，此时再调用此窗口句柄即为无效句柄，将报错，加此判断
            if (!winObj.IsDisposed)
            {
                // 进入，通常是第一次启动Main窗口之后，关闭
                winObj.Invoke(new MethodInvoker(delegate
                {
                    winObj.Close();
                }));
            }
            this.Focus();
           
            
        }

        private void 复制到剪贴板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();
            this.richTextBox1.Copy();
            MessageBox.Show("文本内容已复制，您可以粘贴发送了。");
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FIleNameInputWin fIleNameInputWin = new FIleNameInputWin();
            var res = fIleNameInputWin.ShowDialog();
            if (res == DialogResult.OK)
            {
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), 
                     _fileName + ".txt");
                // FileMode.Create存在则覆
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter stw = new StreamWriter(fs, Encoding.GetEncoding("utf-8")))
                    {
                        stw.WriteLine(this.richTextBox1.Text);
                    }
                }
                MessageBox.Show("已在您的桌面创建了存储了配置信息的文本文件。");
            }
        }
    }
}
