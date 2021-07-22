using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmyLib
{
    public static class Consts
    {
        /// <summary>
        /// Tag that should end each message and response bween the server and the clients.
        /// </summary>
        public const string EOFTag = "<EOF>";

        /// <summary>
        /// Size of the buffer in server-client communications, in bytes.
        /// </summary>
        public const int BufferSize = 1024;

        /// <summary>
        /// Timeout for the client when waiting for data to be received.
        /// </summary>
        public const int ReceiveTimeout = 3000;

        /// <summary>
        /// Maximum number of concurrent clients the server supports.
        /// </summary>
        public const int MaxServerConnections = 100;

        /// <summary>
        /// Interval of sending a <see cref="KeepAliveResponse"/> message.
        /// </summary>
        public static readonly TimeSpan KeepAliveInterval = new TimeSpan(hours: 0, minutes: 0, seconds: 5);
    }
}
