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
        public const int MaxServerConnections = 100;

        /// <summary>
        /// Whether the server is currently operating.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Port from which to serve clients.
        /// </summary>
        private readonly int Port;

        /// <summary>
        /// Socket on which to listen for connections.
        /// </summary>
        private Socket Listener;

        /// <summary>
        /// Service provider.
        /// </summary>
        internal readonly TcpServiceProvider Provider;

        /// <summary>
        /// Maximum number of concurrent connections.
        /// </summary>
        private int _maxConnections = MaxServerConnections;

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

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Server's SSL certificate.
        /// </summary>
        static X509Certificate serverCertificate = null;

        /// <summary>
        /// Initializes server. 
        /// To start accepting connections, call <see cref="Start(string, SecureString)"/>.
        /// </summary>
        public TcpServer(TcpServiceProvider provider, int port)
        {
            Provider = provider;
            Port = port;
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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
                Logger.Trace("Certificate activation successful.");

                Listener.Bind(new IPEndPoint(IPAddress.Loopback, Port));
                Listener.Listen(100);
                Listener.BeginAccept(ConnectionReady, null);
                Logger.Trace("Starting to listen for connections.");

                IsRunning = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("password"))
                {
                    Logger.Error(ex, "Certificate activation error.");
                }
                else
                {
                    Logger.Error(ex, "Error starting server.");
                }

                IsRunning = false;
            }

            return IsRunning;
        }

        /// <summary>
        /// Callback function: A new connection is waiting.
        /// </summary>
        private void ConnectionReady_Handler(IAsyncResult ar)
        {
            lock (this)
            {
                if (Listener == null)
                { 
                    return;
                }

                Socket conn = Listener.EndAccept(ar);

                if (CurrentConnections.Count >= _maxConnections)
                {
                    // Max number of connections reached. Terminate the connection with the client.
                    // TODO: send error message
                    ErrorMessage ce = new ErrorMessage(ErrorMessage.ErrorCode.MaxConnectionsExceeded);
                    MessageWrapper<ErrorMessage> messageWrapper = new MessageWrapper<ErrorMessage>();

                    conn.Send(Encoding.UTF8.GetBytes(messageWrapper.Serialize()), SocketFlags.None);
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
                        _provider = (TcpServiceProvider)Provider.Clone(),
                        _buffer = new byte[4]
                    };

                    CurrentConnections.Add(st);

                    // Queue the rest of the job to be executed later.
                    ThreadPool.QueueUserWorkItem(AcceptConnection, st);
                }

                // Resume the listening callback loop.
                Listener.BeginAccept(ConnectionReady, null);
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
                Logger.Error(ex, "Error accepting a connection.");
            }

            // Start the ReceiveData callback loop.
            try
            {
                SslStream sslStream = new SslStream(new NetworkStream(st._conn), false);
                sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);
            }
            catch (System.IO.IOException ioe)
            {
                Logger.Error(ioe, "Failed to authenticate client via SSL.");
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
            
            // No data is available so we drop the connection.
            if (st._conn.Available == 0)
            { 
                DropConnection(st); 
            }
            else
            {
                try 
                { 
                    st._provider.OnReceiveData(st); 
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error propagating OnReceiveData.");
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
            if (!IsRunning)
            {
                return;
            }

            lock (this)
            {
                Listener.Close();
                Listener = null;

                // Close all active connections.
                foreach (object obj in CurrentConnections)
                {
                    ConnectionState st = obj as ConnectionState;
                    try 
                    { 
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

                IsRunning = false;
            }
        }

        /// <summary>
        /// Removes a connection from the <see cref="CurrentConnections"/> list.
        /// </summary>
        internal void DropConnection(ConnectionState st)
        {
            Logger.Trace("Dropping a connection from {0}.", st.RemoteEndPoint.ToString());
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
