using System;
using System.Linq;
using System.Management;

namespace QuikSharp.Starter
{
    internal class MachineIdentificator
    {
        /// <summary>
        /// Constant hash specific to machine
        /// Solution from
        /// http://stackoverflow.com/questions/3474940/unique-computer-id-c/3474966#3474966
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueStringID(int size = 20)
        {
            if (size > 20) throw new ArgumentException("Size must be 20 or less");

            string uniqueId;

            var cpuInfo = string.Empty;
            try
            {

                var cpu = new ManagementClass("win32_processor");
                var cpuCol = cpu.GetInstances();
                foreach (var o in cpuCol)
                {
                    var mo = (ManagementObject) o;
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            catch { }

            var volumeSerial = string.Empty; ;
            try
            {
                const string drive = "C";
                var dsk = new ManagementObject(
                    @"win32_logicaldisk.deviceid=""" + drive + @":""");
                dsk.Get();
                volumeSerial = dsk["VolumeSerialNumber"].ToString();

            }
            catch { }

            var boardInfo = string.Empty;
            try
            {
                var board = new ManagementClass("Win32_baseboard");
                var boardCol = board.GetInstances();

                foreach (var bmo in boardCol.Cast<ManagementObject>()) {
                    boardInfo = bmo.Properties["SerialNumber"].Value.ToString();
                    break;
                }
            }
            catch { }


            uniqueId = cpuInfo + volumeSerial + boardInfo;


            return uniqueId.GetStringHash().Substring(0, size * 2);
        }

        public static byte[] GetUniqueBytesID(int size = 20)
        {
            return GetUniqueStringID().GetBytesHash().Take(size).ToArray();
        }


        public static bool GenerateKey(string containerName)
        {
            RsaUtilities.GenKeySaveInContainer(containerName);
            return true;
        }

        public static bool DeleteKey(string containerName)
        {
            return RsaUtilities.DeleteKeyFromContainer(containerName);
        }

        public static byte[] GetKey(string containerName, bool includePrivateParameters = true, int size = 20)
        {
            if (size > 20) throw new ArgumentException("Size must be 20 or less");
            return RsaUtilities.GetKeyFromContainer(containerName, includePrivateParameters).GetBytesHash().Take(size).ToArray();
        }

        public static string GetHash(string containerName, bool includePrivateParameters = true, int size = 20)
        {
            if (size > 20) throw new ArgumentException("Size must be 20 or less");
            return RsaUtilities.GetKeyFromContainer(containerName, includePrivateParameters).GetStringHash().Substring(0, size * 2);
        }
    }
}
