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
    ///
    /// Retrieving HDD Serial No.
    /// 
    /// 
    public static String GetHDDSerialNo()
    {
        ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
        ManagementObjectCollection mcol = mangnmt.GetInstances();
        string result = "";
        foreach (ManagementObject strt in mcol)
        {
            result += Convert.ToString(strt["VolumeSerialNumber"]);
        }
        return result;
    }
    ///
    /// Retrieving System MAC Address.
    /// 
    /// 
    public static string GetMACAddress()
    {
        ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection moc = mc.GetInstances();
        string MACAddress = String.Empty;
        foreach (ManagementObject mo in moc)
        {
            if (MACAddress == String.Empty)
            {
                if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
            }
            mo.Dispose();
        }

        MACAddress = MACAddress.Replace(":", "");
        return MACAddress;
    }
    ///
    /// Retrieving Motherboard Manufacturer.
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
    /// Retrieving BIOS Caption.
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
    ///
    /// 获取主板内存插槽数
    /// 
    /// 
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
    //Get CPU Temprature.
    ///
    /// method for retrieving the CPU Manufacturer
    /// using the WMI class
    /// 
    /// CPU Manufacturer
    public static string GetCPUManufacturer()
    {
        string cpuMan = String.Empty;
        //create an instance of the Managemnet class with the
        //Win32_Processor class
        ManagementClass mgmt = new ManagementClass("Win32_Processor");
        //create a ManagementObjectCollection to loop through
        ManagementObjectCollection objCol = mgmt.GetInstances();
        //start our loop for all processors found
        foreach (ManagementObject obj in objCol)
        {
            if (cpuMan == String.Empty)
            {
                // only return manufacturer from first CPU
                cpuMan = obj.Properties["Manufacturer"].Value.ToString();
            }
        }
        return cpuMan;
    }
    ///
    /// method to retrieve the CPU's current
    /// clock speed using the WMI class
    /// 
    /// Clock speed
    //public static int GetCPUCurrentClockSpeed()
    //{
    //    int cpuClockSpeed = 0;
    //    //create an instance of the Managemnet class with the
    //    //Win32_Processor class
    //    ManagementClass mgmt = new ManagementClass("Win32_Processor");
    //    //create a ManagementObjectCollection to loop through
    //    ManagementObjectCollection objCol = mgmt.GetInstances();
    //    //start our loop for all processors found
    //    foreach (ManagementObject obj in objCol)
    //    {
    //        if (cpuClockSpeed == 0)
    //        {
    //            // only return cpuStatus from first CPU
    //            cpuClockSpeed = Convert.ToInt32(obj.Properties["CurrentClockSpeed"].Value.ToString());
    //        }
    //    }
    //    //return the status
    //    return cpuClockSpeed;
    //}
    ///
    /// method to retrieve the network adapters
    /// default IP gateway using WMI
    /// 
    /// adapters default IP gateway
    public static string GetDefaultIPGateway()
    {
        //create out management class object using the
        //Win32_NetworkAdapterConfiguration class to get the attributes
        //of the network adapter
        ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //create our ManagementObjectCollection to get the attributes with
        ManagementObjectCollection objCol = mgmt.GetInstances();
        string gateway = String.Empty;
        //loop through all the objects we find
        foreach (ManagementObject obj in objCol)
        {
            if (gateway == String.Empty)  // only return MAC Address from first card
            {
                //grab the value from the first network adapter we find
                //you can change the string to an array and get all
                //network adapters found as well
                //check to see if the adapter's IPEnabled
                //equals true
                if ((bool)obj["IPEnabled"] == true)
                {
                    gateway = obj["DefaultIPGateway"].ToString();
                }
            }
            //dispose of our object
            obj.Dispose();
        }
        //replace the ":" with an empty space, this could also
        //be removed if you wish
        gateway = gateway.Replace(":", "");
        //return the mac address
        return gateway;
    }
    ///
    /// Retrieve CPU Speed.
    /// 
    /// 
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

        return "BIOS Maker: Unknown";

    }
    ///
    /// Retrieving Current Language.
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
    /// Retrieving Processor Information.
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
    /// Retrieving Computer Name.
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