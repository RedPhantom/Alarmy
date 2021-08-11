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
    }
}
