﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

public static class HardwareInfo
{
    ///
    /// Retrieving Processor Id.
    /// 
    /// 
    /// 
    public static String GetProcessorId()
    {

        ManagementClass mc = new ManagementClass("win32_processor");
        ManagementObjectCollection moc = mc.GetInstances();
        String Id = String.Empty;
        foreach (ManagementObject mo in moc)
        {

            Id = mo.Properties["processorID"].Value.ToString();
            break;
        }
        return Id;

    }
    /// <summary>
    /// 盘符分区
    /// </summary>
    /// <returns></returns>
    public static String GetLogicalDiskPartition()
    {
        ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
        ManagementObjectCollection mcol = mangnmt.GetInstances();
        StringBuilder sb = new StringBuilder();
        uint i = 1;
        foreach (ManagementObject strt in mcol)
        {
            var serialNumber = Convert.ToString(strt["VolumeSerialNumber"]);
            // 盘符名称
            var n = strt["Name"];
            // 磁盘类型
            var d = strt["Description"];
            // 文件系统(分区类型、格式)
            var fs = strt["FileSystem"];
            // 可用空间
            var fr = Convert.ToInt64(strt["FreeSpace"]) /1024 / 1024 / 1024;
            // 总空间
            var s = Convert.ToInt64(strt["Size"]) / 1024 / 1024 / 1024;
            sb.AppendLine($"盘符{n}>>{d}, 文件系统{fs}, 可用空间{fr}GB, 总空间{s}GB");
            sb.AppendLine();
            i++;
        }
        return sb.ToString();
    }


    /// <summary>
    /// 硬盘信息
    /// </summary>
    /// <returns></returns>
    public static String GetDiskDriveInfo()
    {
        ManagementClass mangnmt = new ManagementClass("Win32_DiskDrive");
        ManagementObjectCollection mcol = mangnmt.GetInstances();
        StringBuilder sb = new StringBuilder();
        uint i = 1;
        foreach (ManagementObject mo in mcol)
        {
            var m = mo["Model"];
            var size = Convert.ToInt64(mo["Size"]);
            var sn = mo["SerialNumber"];
            var it = mo["InterfaceType"];
            sb.AppendLine($"序号{i}>>{m}, {size / 1024 / 1024 / 1024}GB, 序列号{sn}, {it}");
            sb.AppendLine();
            i++;
        }
        return sb.ToString();
    }


    ///
    /// 获取主板信息
    /// 
    /// 
    public static string GetBoardMaker()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Manufacturer").ToString()+ ", "+wmi.GetPropertyValue("Product").ToString();
            }

            catch { }

        }

        return "(Board)Unknown";

    }
    ///
    /// Retrieving CD-DVD Drive Path.
    /// 
    /// 
    public static string GetCdRomDrive()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Drive").ToString();

            }

            catch { }

        }

        return "CD ROM Drive Letter: Unknown";

    }
    ///
    /// Retrieving BIOS Serial No.
    /// 
    /// 
    public static string GetBIOSserNo()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("SerialNumber").ToString();

            }

            catch { }

        }

        return "BIOS Serial Number: Unknown";

    }
    ///
    /// BIOS信息
    /// 
    /// 
    public static string GetBIOScaption()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                // 品牌, BIOS型号
                return wmi.GetPropertyValue("Manufacturer").ToString()+", "+wmi.GetPropertyValue("Caption").ToString();

            }
            catch { }
        }
        return "(Win32_BIOS)Unknown";
    }
    ///
    /// Retrieving System Account Name.
    /// 
    /// 
    public static string GetAccountName()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {

                return wmi.GetPropertyValue("Name").ToString();
            }
            catch { }
        }
        return "User Account Name: Unknown";

    }
    ///
    /// 获取内存信息
    /// 
    public static string GetPhysicalMemory()
    {
        ManagementScope oMs = new ManagementScope();
        ObjectQuery oQuery = new ObjectQuery("SELECT Capacity,ConfiguredClockSpeed,Manufacturer,Caption FROM Win32_PhysicalMemory");
        ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
        ManagementObjectCollection oCollection = oSearcher.Get();

        StringBuilder sb = new StringBuilder();
        uint i = 1;
        foreach (ManagementObject obj in oCollection)
        {
            var indexSize = Convert.ToInt64(obj["Capacity"]);
            var m = obj["Manufacturer"];
            var c = obj["Caption"];
            sb.AppendLine($"序号{i}>>{indexSize / 1024 / 1024 / 1024}GB, {obj["ConfiguredClockSpeed"]}MHz, {m}, {c}");
            sb.AppendLine();
            i++;
        }
        
        return sb.ToString();
    }

    ///
    /// 获取已安装内存总量
    /// 
    /// 
    public static string GetPhysicalMemoryCapacity()
    {
        ManagementScope oMs = new ManagementScope();
        ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
        ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
        ManagementObjectCollection oCollection = oSearcher.Get();

        long installedMemSize = 0;
        foreach (ManagementObject obj in oCollection)
        {
            installedMemSize += Convert.ToInt64(obj["Capacity"]);
        }
        return $"{installedMemSize / 1024 / 1024 / 1024}GB";
    }

    /// <summary>
    /// 获取主板内存插槽数
    /// </summary>
    /// <returns></returns>
    public static string GetPhysicalMemorySlots()
    {

        int MemSlots = 0;
        ManagementScope oMs = new ManagementScope();
        ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
        ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
        ManagementObjectCollection oCollection2 = oSearcher2.Get();
        foreach (ManagementObject obj in oCollection2)
        {
            MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

        }
        var info = MemSlots+"个内存插槽数";
        return MemSlots.ToString();
    }

    ///
    /// method to retrieve the network adapters
    /// default IP gateway using WMI
    /// 
    /// adapters default IP gateway
    public static string GetNetworkAdapters()
    {
        ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection objCol = mgmt.GetInstances();
        
        List<ManagementObject> objList = new List<ManagementObject>();
        foreach (ManagementObject obj in objCol) 
        {
            objList.Add(obj);
            //var d = obj["Description"]?.ToString();
            //var ipEnabled = obj["IPEnabled"];
            //var gateway = obj["DefaultIPGateway"];
            //var mac = obj["MACAddress"];
            //var ip = obj["IPAddress"];
        }
        objList = objList.OrderByDescending(o => (bool)o.Properties["IPEnabled"].Value == true).ToList();
        StringBuilder sb = new StringBuilder();
        uint i = 1;
        foreach (ManagementObject obj in objList)
        {
            var d = obj["Description"]?.ToString();
            var ipEnabled = obj["IPEnabled"];
            var ip = obj["IPAddress"];
            var gateway = obj["DefaultIPGateway"];
            var mac = obj["MACAddress"];
            if (ip!=null)
            {
                if (((string[])ip).Length > 1)
                {
                    ip = $"IP地址{((string[])ip)[0]}|{((string[])ip)[1]}";
                }
                else
                {
                    ip = $"IP地址{((string[])ip)[0]}";
                }

            }
            else
            {
                ip = $"IP地址暂无";
            }

            if (gateway != null)
            {
                gateway = $"网关{((string[])gateway)[0]}";
            }
            else
            {
                gateway = $"网关暂无";
            }

            if (mac != null)
            {
                mac = $"MAC地址{mac}";

            }
            else
            {
                mac = $"MAC地址暂无";
            }
            sb.AppendLine($"序号{i}>>{d}, {ip}, {gateway}, {mac}");
            sb.AppendLine();
            i++;
        }
        return sb.ToString();
    }

    /// <summary>
    /// cpu时钟频率
    /// </summary>
    /// <returns></returns>
    public static double? GetCpuSpeedInGHz()
    {
        double? GHz = null;
        using (ManagementClass mc = new ManagementClass("Win32_Processor"))
        {
            foreach (ManagementObject mo in mc.GetInstances())
            {
                // CurrentClockSpeed:单位是MHz
                GHz = 0.001 * (UInt32)mo.Properties["CurrentClockSpeed"].Value;
                break;
            }
        }
        return GHz;
    }
    ///
    /// Retrieving Current Language
    /// 
    /// 
    public static string GetCurrentLanguage()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("CurrentLanguage").ToString();

            }

            catch { }

        }

        return "Unknown";

    }
    ///
    /// 获取操作系统信息.
    /// 
    /// 
    public static string GetOSInformation()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
        ManagementObjectCollection moc = searcher.Get();
        foreach (ManagementObject mo in moc)
        {
            try
            {
                // Windows 10 专业版, 10.0.0000, 64位
                return ((string)mo["Caption"]).Trim() + ", " + (string)mo["Version"] + ", " + (string)mo["OSArchitecture"];
            }
            catch { }
        }
        return "(Win32_OperatingSystem)Unknown";
    }
    ///
    /// 获取处理器信息
    /// 
    /// 
    public static String GetProcessorInformation()
    {
        ManagementClass mc = new ManagementClass("win32_processor");
        ManagementObjectCollection moc = mc.GetInstances();
        String info = String.Empty;
        foreach (ManagementObject mo in moc)
        {
            // 处理器型号,说明,插槽类型,时钟频率
            string name = (string)mo["Name"];
            name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©");
            name = name.TrimEnd(' ');
            info = name + ", " + (string)mo["Caption"] + ", " + (string)mo["SocketDesignation"]+ "插槽";
            info += ", "+GetCpuSpeedInGHz()+"GHz";
        }
        return info;
    }
    ///
    /// 获取计算机名
    /// 
    /// 
    public static String GetComputerName()
    {
        ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
        ManagementObjectCollection moc = mc.GetInstances();
        String info = String.Empty;
        foreach (ManagementObject mo in moc)
        {
            info = (string)mo["Name"];
        }
        return info;
    }

}