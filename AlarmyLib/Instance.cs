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
        // Since this object is created from serialization, its public properties must be publicly writable.
        public string Username { get; set; }
        public string ComputerName { get; set; }

        public Instance(string username, string computerName)
        {
            if (username.Length == 0)
            {
                throw new ArgumentException("Username can't be empty.");
            }
            Username = username;

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

            return string.Compare(Username, other.Username) + string.Compare(ComputerName, other.ComputerName);
        }

        public bool Equals(Instance other)
        {
            return (Username == other.Username) && (ComputerName == other.ComputerName);
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode() ^ ComputerName.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Username}@{ComputerName}";
        }

        public string Repr()
        {
            return $"<Instance Username={Username}, ComputerName={ComputerName}>";
        }
    }
}
