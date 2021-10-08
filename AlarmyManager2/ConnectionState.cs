using System.Net;
using System.Net.Sockets;

namespace AlarmyManager
{
    /// <summary>
    /// Holds information about every connection to the server.
    /// </summary>
    public class ConnectionState
    {
        internal Socket _conn;
        internal TcpServer _server;
        internal TcpServiceProvider _provider;
        internal byte[] _buffer;

        /// <summary>
        /// Endpoint of the client.
        /// </summary>
        public EndPoint RemoteEndPoint
        {
            get { return _conn.RemoteEndPoint; }
        }

        /// <summary>
        /// Returns the number of bytes waiting to be read.
        /// </summary>
        public int AvailableData
        {
            get { return _conn.Available; }
        }

        /// <summary>
        /// Whether the client socket is connected.
        /// </summary>
        public bool Connected
        {
            get { return _conn.Connected; }
        }

        /// <summary>
        /// Reads data on the socket.
        /// </summary>
        /// <returns>
        /// Number of bytes read.
        /// </returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                if (_conn.Available > 0)
                {
                    return _conn.Receive(buffer, offset, count, SocketFlags.None);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Sends bytes to the remote host.
        /// </summary>
        /// <returns>
        /// <c>true</c> upon successful data sending.
        /// </returns>
        public void Write(byte[] buffer, int offset, int count)
        {
            _conn.Send(buffer, offset, count, SocketFlags.None);
        }

        /// <summary>
        /// Ends the connection with the remote host.
        /// </summary>
        public void EndConnection()
        {
            if (_conn != null && _conn.Connected)
            {
                _conn.Shutdown(SocketShutdown.Both);
                _conn.Close();
            }
            _server.DropConnection(this);
        }

        public string Repr()
        {
            return $"<Client Socket={_conn.LocalEndPoint} <-> {_conn.RemoteEndPoint}>";
        }
    }
}
