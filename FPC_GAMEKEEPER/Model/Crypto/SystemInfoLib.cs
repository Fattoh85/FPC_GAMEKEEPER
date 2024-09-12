using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FPC.Model.Crypto
{


    public static class SystemInfoLib
    {

        public static log4net.ILog log = FPC.Model.Logger.Logger.Get(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetMacAddress()
        {
            var macAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();

            return macAddress;
        }



        public static string GetHardDiskSerialNumber()
        {
            try
            {
                string serialNumber = string.Empty;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE MediaType = 'Fixed hard disk media'");

                foreach (ManagementObject disk in searcher.Get())
                {
                    serialNumber = disk["SerialNumber"]?.ToString().Trim();
                    break; // Assuming we need the serial number of the first hard disk
                }

                return serialNumber ?? "Serial number not found";
            }
            catch (Exception ex)
            {
                log.Error($"hddsn|er:{ex}");
                return $"Error: {ex.Message}";
            }
        }

        public static string GetMotherboardSn()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    log.Debug($"Serial Number: {queryObj["SerialNumber"]}");
                    if (queryObj["SerialNumber"].ToString().Contains("To be filled by O.E.M."))
                    {
                        return $"{GetProcessorId()}";
                    }
                    else
                    {
                        string baseBoardSerial = queryObj["SerialNumber"].ToString();

                        if (baseBoardSerial.ToLower().Contains("string"))
                        {
                            return $"{GetProcessorId()}";
                        }
                        else
                        {
                            return baseBoardSerial;
                        }
                    }

                }
            }
            catch (ManagementException e)
            {
                log.Error("An error occurred while querying for WMI data: " + e.Message);

                return $"{GetProcessorId()}";
            }
            log.Error("WMI data|no data found");

            return $"{GetProcessorId()}";
        }

        public static string GetProcessorId()
        {
            try
            {
                string processorId = string.Empty;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");

                foreach (ManagementObject obj in searcher.Get())
                {
                    processorId = obj["ProcessorId"].ToString();
                    break;
                }
                log.Debug($"cpu Serial Number: {processorId}");

                if (processorId.ToString().Contains("To be filled by O.E.M.") || processorId.ToString().ToLower().Contains("string"))
                {
                    return $"{GetMacAndHdd()}";
                }

                return processorId;
            }
            catch (Exception ex)
            {
                log.Error($"cpu|er:{ex}");
                return $"{GetMacAndHdd()}";

            }
        }

        internal static string GetMacAndHdd()
        {
            string macAddress = SystemInfoLib.GetMacAddress();
            string hddSerial = SystemInfoLib.GetHardDiskSerialNumber();
            return $"{macAddress}-{hddSerial}";
        }
    }
}
