using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace InfoSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            StringBuilder sb = new StringBuilder();
            InitializeComponent();
            Thread thread = new Thread(new ThreadStart(delegate {
                /* 逐个获取信息*/
                // 获取计算机名
                sb.AppendLine($"计算机名称：{HardwareInfo.GetComputerName()+Environment.NewLine}");
                // 操作系统
                sb.AppendLine($"操作系统信息：");
                sb.AppendLine($"{HardwareInfo.GetOSInformation() + Environment.NewLine}");
                // 处理器和时钟频率
                sb.AppendLine($"处理器：");
                sb.AppendLine($"{HardwareInfo.GetProcessorInformation() + Environment.NewLine}");
                // BIOS
                sb.AppendLine($"BIOS：{HardwareInfo.GetBIOScaption() + Environment.NewLine}");
                // 主板
                sb.AppendLine($"主板：{HardwareInfo.GetBoardMaker() + Environment.NewLine}");
                // 内存总量
                sb.AppendLine($"已安装内存总量：{HardwareInfo.GetPhysicalMemoryCapacity() + Environment.NewLine}");
                sb.AppendLine($"主板总内存插槽数：{HardwareInfo.GetPhysicalMemorySlots() + Environment.NewLine}");
                // 内存信息
                sb.AppendLine($"内存详情信息：");
                sb.AppendLine($"{HardwareInfo.GetPhysicalMemory() + Environment.NewLine}");
                


                new InfoWIn(sb.ToString(), this).ShowDialog();
            }));
            thread.Start();
        }
    }
}
