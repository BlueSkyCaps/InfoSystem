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
    public partial class Main : Form
    {
        public Main()
        {
            StringBuilder sb = new StringBuilder();
            InitializeComponent();
            Thread thread = new Thread(new ThreadStart(delegate {
                /* 逐个获取信息*/
                // 获取计算机名
                sb.AppendLine($"计算机名称：{HardwareInfo.GetComputerName()+Environment.NewLine}");
                sb.AppendLine("-----------------------");
                // 操作系统
                sb.AppendLine($"操作系统信息：");
                sb.AppendLine($"{HardwareInfo.GetOSInformation() + Environment.NewLine}");
                sb.AppendLine("-----------------------");

                // 处理器和时钟频率
                sb.AppendLine($"处理器：");
                sb.AppendLine($"{HardwareInfo.GetProcessorInformation() + Environment.NewLine}");
                sb.AppendLine("-----------------------");

                // BIOS
                sb.AppendLine($"BIOS：{HardwareInfo.GetBIOScaption() + Environment.NewLine}");
                sb.AppendLine("-----------------------");

                // 主板
                sb.AppendLine($"主板：{HardwareInfo.GetBoardMaker() + Environment.NewLine}");
                sb.AppendLine("-----------------------");

                // 内存总量
                sb.AppendLine($"已安装内存总量：{HardwareInfo.GetPhysicalMemoryCapacity() + Environment.NewLine}");
                sb.AppendLine("-----------------------");

                sb.AppendLine($"主板总内存插槽数：{HardwareInfo.GetPhysicalMemorySlots() + Environment.NewLine}");
                sb.AppendLine("-----------------------");

                // 内存信息
                sb.AppendLine($"内存详情信息：");
                sb.AppendLine($"{HardwareInfo.GetPhysicalMemory()}");
                sb.AppendLine("-----------------------");

                // 分区
                sb.AppendLine($"分区详情信息：");
                sb.AppendLine($"{HardwareInfo.GetLogicalDiskPartition()}");
                sb.AppendLine("-----------------------");

                // 硬盘
                sb.AppendLine($"硬盘详情信息：");
                sb.AppendLine($"{HardwareInfo.GetDiskDriveInfo()}");
                sb.AppendLine("-----------------------");

                // 网卡
                sb.AppendLine($"网络适配器：");
                sb.AppendLine($"{HardwareInfo.GetNetworkAdapters()}");
                sb.AppendLine("-----------------------");

                new InfoWIn(sb.ToString(), this).ShowDialog();
            }));
            thread.Start();
        }
    }
}
