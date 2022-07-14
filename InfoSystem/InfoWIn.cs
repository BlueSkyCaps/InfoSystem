using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace InfoSystem
{
    public partial class InfoWIn : Form
    {
        public static string _fileName = null;
        public static int _areaValue = 0;

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
                // 使用rtf写字板文件，而不是txt记事本,txt在win7可能不会换行
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), 
                     _fileName + ".rtf");
                var areaDirName = _areaValue == 1 ? "莱普" : "老厂";
                var shareFilePath = $@"\\192.168.99.97\共享文件夹\@配置信息接收\{areaDirName}\"+ _fileName + ".rtf";
                // 检测共享文件夹是否存在同名文件
                if (DetermineReplyFile(shareFilePath))
                {
                    var r = MessageBox.Show("文件夹存在同名文件，点击确认将覆盖最新，取消则中止操作。", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (r!=DialogResult.OK)
                    {
                        return;
                    }
                }
                var shareSaveFlag = false;
                try
                {
                    // FileMode.Create存在则覆
                    using (FileStream fs = new FileStream(shareFilePath, FileMode.Create))
                    {
                        using (StreamWriter stw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            stw.Write(this.richTextBox1.Text);
                        }
                    }
                    MessageBox.Show($@"已保存在共享文件夹‘@配置信息接收\{areaDirName}’中。");
                    shareSaveFlag = true;
                }
                catch (Exception){}
                // 若保存到共享文件夹目录失败，则保存到桌面
                if (!shareSaveFlag)
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        using (StreamWriter stw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            stw.Write(this.richTextBox1.Text);
                        }
                    }
                    MessageBox.Show("保存在共享文件夹失败，网络是否连接？已转而保存到桌面。");
                    shareSaveFlag = true;
                }
            }
        }

        /// <summary>
        /// 存在则true
        /// </summary>
        /// <param name="shareFilePath"></param>
        /// <returns></returns>
        private bool DetermineReplyFile(string shareFilePath)
        {
            try
            {
                if (File.Exists(shareFilePath))
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            
            return false;
        }
    }
}
