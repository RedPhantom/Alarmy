using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AlarmyLib
{
    /// <summary>
    /// Describes an instance of an Alarmy Service installation.
    /// </summary>
    public class Instance
    {
        public string UserName { get; }
        public string ComputerName { get; }

        public Instance(string userName, string computerName)
        {
            if (userName.Length == 0)
            {
                throw new ArgumentException("UserName can't be empty.");
            }
            UserName = userName;

            if (computerName.Length == 0)
            {
                throw new ArgumentException("ComputerName can't be empty.");
            }
            ComputerName = computerName;
        }

        public static Instance GetInstance()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];
            string computerName = Environment.MachineName;
            return new Instance(username, computerName);
        }
    }
}
