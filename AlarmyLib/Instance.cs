using System;
using System.Linq;
using System.Management;

namespace AlarmyLib
{
    /// <summary>
    /// Describes an instance of an Alarmy Service installation.
    /// </summary>
    public class Instance : IComparable, IEquatable<Instance>
    {
        // Since this object is created from serialization, its public properties must be writable.
        public string UserName { get; set; }
        public string ComputerName { get; set; }

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

        /// <summary>
        /// Construct the instance specific to the current process and logged-on user.
        /// </summary>
        /// <returns>Instance specific to the current process and logged-on user.</returns>
        public static Instance GetInstance()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];
            string computerName = Environment.MachineName;
            return new Instance(username, computerName);
        }

        public int CompareTo(object obj)
        {
            Instance other = (Instance)obj;

            return string.Compare(UserName, other.UserName) + string.Compare(ComputerName, other.ComputerName);
        }

        public bool Equals(Instance other)
        {
            return (UserName == other.UserName) && (ComputerName == other.ComputerName);
        }

        public override int GetHashCode()
        {
            return UserName.GetHashCode() ^ ComputerName.GetHashCode();
        }

        public override string ToString()
        {
            if (UserName.Contains(ComputerName))
            {
                return UserName;
            }
            else
            {
                return string.Format(@"{0}\{1}", UserName, ComputerName);
            }
        }
    }
}
