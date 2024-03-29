﻿using System;
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
            InitializeComponent();
            Thread thread = new Thread(new ThreadStart(delegate
            {
                try
                {
                    GetInfo();
                }
                catch (Exception e)
                {
                    var iw = new InfoWIn(e.Message + Environment.NewLine + e.StackTrace, this);
                    iw.Show();
                    iw.menuStrip1.Enabled = false;
                    iw.richTextBox1.Enabled = false;
                    MessageBox.Show("引发异常，错误信息已输出到界面，操作已锁定，请关闭重试。", "程序错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }));
            thread.Start();
        }

        public void GetInfo()
        {
            StringBuilder sb = new StringBuilder();

            /* 逐个获取信息*/
            // 获取计算机名
            sb.AppendLine($"计算机名称：{HardwareInfo.GetComputerName() + Environment.NewLine}");
            sb.AppendLine("-----------------------");

            // 操作系统
            sb.AppendLine($"操作系统信息：");
            sb.AppendLine($"{HardwareInfo.GetOSInformation() + Environment.NewLine}");
            sb.AppendLine("-----------------------");

            // 计算机账户
            sb.AppendLine($"账户列表：");
            sb.AppendLine($"{HardwareInfo.GetAccountName()}");
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

            // 显卡
            sb.AppendLine($"显示适配器：");
            sb.AppendLine($"{HardwareInfo.GetDisplayAdapters()}");
            sb.AppendLine("-----------------------");

            // 声卡
            sb.AppendLine($"声卡信息：");
            sb.AppendLine($"{HardwareInfo.GetSoundAdapters()}");

            new InfoWIn(sb.ToString(), this).ShowDialog();
        }
    }
}
