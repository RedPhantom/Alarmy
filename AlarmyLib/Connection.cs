using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlarmyLib
{
    /// <summary>
    /// Describes an active connection to the Alarmy Service.
    /// </summary>
    public class Connection
    {
        public Instance Instance { get; internal set; }
        public IPEndPoint EndPoint { get; internal set; }

        public Connection(Instance instance, IPEndPoint endPoint)
        {
            Instance = instance;
            EndPoint = endPoint;
        }

        public override string ToString()
        {
            return string.Format("{0}@{1} ({2})",
                Instance.UserName,
                Instance.ComputerName,
                EndPoint.Address.ToString());
        }
    }
}
