using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmyLib
{
    public static class Consts
    {
        public const string EOFTag = "<EOF>";
        public const string ServiceEventLogName = "AlarmyService";
        public const int BufferSize = 1024;
        public const int ReceiveTimeout = 3000;
        public const int MaxServerConnections = 100;
        public static readonly TimeSpan KeepAliveInterval = new TimeSpan(0, 0, 5);
    }
}
