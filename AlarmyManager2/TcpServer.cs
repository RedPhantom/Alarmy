using AlarmyLib;
using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace AlarmyManager
{
    class TcpServer
    {
        /// <summary>
        /// Port from which to serve clients.
        /// </summary>
        private readonly int _port;

        /// <summary>
        /// Socket on which to listen for connections.
        /// </summary>
        private Socket _listener;

        /// <summary>
        /// Service provider.
        /// </summary>
        private readonly TcpServiceProvider _provider;

        /// <summary>
        /// Maximum number of concurrent connections.
        /// </summary>
        private int _maxConnections = Consts.MaxServerConnections;

        /// <summary>
        /// Callback for when the connection is ready.
        /// </summary>
        private readonly AsyncCallback ConnectionReady;

        /// <summary>
        /// Callback for when the server accepts a new connection.
        /// </summary>
        private readonly WaitCallback AcceptConnection;

        /// <summary>
        /// Callback when data is received from a client.
        /// </summary>
        private readonly AsyncCallback ReceivedDataReady;

        private readonly UnifiedLogger Logger = new UnifiedLogger("TcpServer");

        /// <summary>
        /// Server certificate.
        /// </summary>
        static X509Certificate serverCertificate = null;

        /// <summary>
        /// Initializes server. 
        /// To start accepting connections, call <see cref="Start(string, SecureString)"/>.
        /// </summary>
        public TcpServer(TcpServiceProvider provider, int port)
        {
            Logger.EnableConsoleLogging();
            Logger.EnableEventLogLogging(EventLogger.EventLogSource.AlarmyManager);
            Logger.EnableFileLogging(SharedWriter.Writer);

            _provider = provider;
            _port = port;
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            CurrentConnections = new ArrayList();
            ConnectionReady = new AsyncCallback(ConnectionReady_Handler);
            AcceptConnection = new WaitCallback(AcceptConnection_Handler);
            ReceivedDataReady = new AsyncCallback(ReceivedDataReady_Handler);
        }

        /// <summary>
        /// Start accepting connections.
        /// </summary>
        /// <returns>
        /// A <see cref="bool"/> whether starting the server was successful or not.
        /// </returns>
        /// <param name="certificatePath">Path to the SSL certificate of the server.</param>
        public bool Start(string certificatePath, SecureString certificatePassword)
        {
            try
            {
                serverCertificate = new X509Certificate2(certificatePath, certificatePassword);
                Logger.Log(LoggingLevel.Trace, "Certificate activation successful.");

                _listener.Bind(new IPEndPoint(IPAddress.Loopback, _port));
                _listener.Listen(100);
                _listener.BeginAccept(ConnectionReady, null);
                Logger.Log(LoggingLevel.Trace, "Starting to listen for connections.");
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("password"))
                {
                    Logger.Log(LoggingLevel.Critical, ex.Message);
                }
                else
                {
                    Logger.Log(LoggingLevel.Critical, "Error starting server: {0}.", ex.Message);
                }
                return false;
            }
        }

        /// <summary>
        /// Callback function: A new connection is waiting.
        /// </summary>
        private void ConnectionReady_Handler(IAsyncResult ar)
        {
            lock (this)
            {
                if (_listener == null)
                { 
                    return; 
                }

                Socket conn = _listener.EndAccept(ar);

                if (CurrentConnections.Count >= _maxConnections)
                {
                    // Max number of connections reached.
                    // TODO: find a better way to inform the client the server is busy.
                    string msg = "SE001: Server busy";
                    conn.Send(Encoding.UTF8.GetBytes(msg), 0, msg.Length, SocketFlags.None);
                    conn.Shutdown(SocketShutdown.Both);
                    conn.Close();
                }
                else
                {
                    // Construct the newly received connection/
                    ConnectionState st = new ConnectionState
                    {
                        _conn = conn,
                        _server = this,
                        _provider = (TcpServiceProvider)_provider.Clone(),
                        _buffer = new byte[4]
                    };

                    CurrentConnections.Add(st);
                    // Queue the rest of the job to be executed later.
                    ThreadPool.QueueUserWorkItem(AcceptConnection, st);
                }

                // Resume the listening callback loop.
                _listener.BeginAccept(ConnectionReady, null);
            }
        }

        /// <summary>
        /// Executes OnAcceptConnection method from the service provider.
        /// </summary>
        private void AcceptConnection_Handler(object state)
        {
            ConnectionState st = state as ConnectionState;

            try
            {
                st._provider.OnAcceptConnection(st);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, "Error accepting a connection: {0}.", ex.Message);
            }

            // Start the ReceiveData callback loop.
            try
            {
                SslStream sslStream = new SslStream(new NetworkStream(st._conn), false);
                sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);
            }
            catch (System.IO.IOException ioe)
            {
                Logger.Log(LoggingLevel.Error, "Failed to authenticate client via SSL: {0}", ioe.Message);
            }
            

            if (st._conn.Connected)
            {
                st._conn.BeginReceive(st._buffer, 0, 0, SocketFlags.None, ReceivedDataReady, st);
            }
        }

        /// <summary>
        /// Executes OnReceiveData method from the service provider.
        /// </summary>
        private void ReceivedDataReady_Handler(IAsyncResult ar)
        {
            ConnectionState st = ar.AsyncState as ConnectionState;
            try
            {
                st._conn.EndReceive(ar);
            }
            catch (Exception)
            {
                //Server.Write($"Connection with a client was terminated.", "tcp", messageTypes.INFO);
                return;
            }
            //Im considering the following condition as a signal that the
            //remote host droped the connection.
            if (st._conn.Available == 0)
            { 
                DropConnection(st); 
            }
            else
            {
                try { 
                    st._provider.OnReceiveData(st); 
                }
                catch (Exception ex)
                {
                    Logger.Log(LoggingLevel.Error, "Critical error: {0}", ex.Message);
                    //TODO: Report error in the provider
                }

                // Resume the ReceivedData callback loop.
                if (st._conn.Connected)
                {
                    st._conn.BeginReceive(st._buffer, 0, 0, SocketFlags.None, ReceivedDataReady, st);
                }
            }
        }

        /// <summary>
        /// Disconnect clients and stop the server.
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                _listener.Close();
                _listener = null;

                // Close all active connections.
                foreach (object obj in CurrentConnections)
                {
                    ConnectionState st = obj as ConnectionState;
                    try { 
                        st._provider.OnDropConnection(st); 
                    }
                    catch
                    {
                        //some error in the provider
                    }
                    st._conn.Shutdown(SocketShutdown.Both);
                    st._conn.Close();
                }
                CurrentConnections.Clear();
            }
        }

        /// <summary>
        /// Removes a connection from the list
        /// </summary>
        internal void DropConnection(ConnectionState st)
        {
            lock (this)
            {
                st._conn.Shutdown(SocketShutdown.Both);
                st._conn.Close();
                if (CurrentConnections.Contains(st))
                {
                    CurrentConnections.Remove(st);
                }
            }
        }

        public int MaxConnections
        {
            get
            {
                return _maxConnections;
            }
            set
            {
                _maxConnections = value;
            }
        }

        public int CurrentConnectionNumber
        {
            get
            {
                lock (this) { return CurrentConnections.Count; }
            }
        }

        /// <summary>
        /// Current clients connected to the server.
        /// </summary>
        public ArrayList CurrentConnections
        {
            get;
            internal set;
        }
    }
}
