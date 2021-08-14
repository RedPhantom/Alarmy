using System.Net;

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
            return $"{Instance} ({EndPoint.Address})";
        }
    }
}
