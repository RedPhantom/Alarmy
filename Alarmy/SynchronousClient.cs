using AlarmyLib;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Alarmy
{
    class SynchronousClient
    {
        public IPAddress IPAddress { get; internal set; }
        public int Port { get; internal set; }

        private static readonly TimeSpan ReconnectAttemptWait = new TimeSpan(hours: 0, minutes: 0, seconds: 15);
        private const int ZeroBytesReceivedAttempts = 3;

        private IPEndPoint remoteEP;
        private Socket sender;
        private UnifiedLogger Logger = new UnifiedLogger("AlarmyService.SynchronousClient");

        /// <summary>
        /// Create a new TCP Synchronous client.
        /// </summary>
        /// <param name="address">The IP Address to connect to.</param>
        /// <param name="port">The port to connect to.</param>
        public SynchronousClient(string address, int port)
        {
            Logger.EnableEventLogLogging(EventLogger.EventLogSource.AlarmyService);

            IPAddress = IPAddress.Parse(address);
            Port = port;

            remoteEP = new IPEndPoint(IPAddress, Port);
            sender = new Socket(IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            // We must set a Receive timeout, otherwise the socket would wait for data to
            // be received and won't send its KeepAlive messages.
            //sender.ReceiveTimeout = Consts.ReceiveTimeout;
        }

        /// <summary>
        /// Start the client. If initial connection failes, an attempt will be made every 
        /// <see cref="ReconnectAttemptMilliseconds"/> milliseconds.
        /// </summary>
        /// <remarks>
        /// This method may raise any exception that <see cref="Socket.Connect(EndPoint)"/> may raise.
        /// In addition, during this method, it will appear like the service is still in its starting process.
        /// If the user attempted to start it manually, then the progress bar dialog will remain on screen.
        /// </remarks>
        /// <returns>The endpoint to which a successful connection was made.</returns>
        public EndPoint Start()
        {
            while (!sender.Connected)
            {
                try
                {
                    sender.Connect(remoteEP);
                    Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.Regular);
                }
                catch (SocketException se)
                {
                    Logger.Log(LoggingLevel.Error, "Failed to connect to the server (code {0}):\n{1}",
                        se.ErrorCode,
                        se.Message);
                    Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.Error,
                        "Failed to connect to the Alarmy Server. Retyring in " + ReconnectAttemptWait.TotalSeconds + " seconds.");
                    System.Threading.Thread.Sleep(ReconnectAttemptWait);
                }
            }
            return sender.RemoteEndPoint;
        }

        /// <summary>
        /// Gracefully shut down the connection.
        /// </summary>
        /// <remarks>This method may raise any exception that <see cref="Socket.Shutdown(SocketShutdown)"/> may raise.</remarks>
        public void Stop()
        {
            try 
            {
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch 
            { 
            // Best effort.
            }
        }

        /// <summary>
        /// Send ASCII string data to the remote endpoint.
        /// </summary>
        /// <remarks>
        /// <see cref="Exception"/> if we attempted to send data but failed for three consecutive times.
        /// Additionally, this method may raise any exception that the following methods may raise:
        /// <see cref="Encoding.GetBytes(char[])"/>, <see cref="Socket.Send(byte[])"/>.
        /// </remarks>
        /// <param name="data">String data to send to the remote endpoint.</param>
        public void Send(string data)
        {
            // TODO: We get a deadlock here.
            lock (this)
            {
                byte[] msg = Encoding.UTF8.GetBytes(data + Consts.EOFTag);

                int messageSize = msg.Length;
                int bytesSent = 0;
                int totalBytesSent = 0;
                int zeroBytesSentCounter = 1;   // 1-based so ZeroByteReceivedAttempts is human readable.

                while (totalBytesSent != messageSize)
                {
                    // If we attempted to send data but failed for three consecutive times, raise exception.
                    if (zeroBytesSentCounter == ZeroBytesReceivedAttempts)
                    {
                        Logger.Log(LoggingLevel.Error, "Attempted to send data but failed for {0} consecutive times.", 
                            ZeroBytesReceivedAttempts);
                    }

                    bytesSent = sender.Send(msg);
                    if (bytesSent == 0)
                    {
                        zeroBytesSentCounter++;
                    }
                    else
                    {
                        // Reset the zero-bytes-sent counter.
                        zeroBytesSentCounter = 0;

                        // Remove the already-sent bytes.
                        msg = msg.Skip(bytesSent).ToArray();
                        totalBytesSent += bytesSent;
                    }
                }
            }
        }

        /// <summary>
        /// Receive data from the remote endpoint.
        /// </summary>
        /// <remarks>
        /// <see cref="Exception"/> if we did not receive the EOF Tag or it was received at the incorrect position.
        /// Additionally, this method may raise any exception that the following methods may raise:
        /// <see cref="Socket.Receive(byte[])"/>, <see cref="Encoding.GetString(byte[])"/>, <see cref="string.Substring(int, int)"/>.
        /// </remarks>
        /// <returns>String of received data from the remote endpoint.</returns>
        public string Receive()
        {
            // Protect from parallel data sending, potentially causing data corruption.
            // Note that this means calling Receive() may block other flows until data is recieved from the server,
            // or until a ReceiveTimeout is reached.
            lock (this)
            {
                string data = string.Empty;
                byte[] buffer = new byte[Consts.BufferSize];
                int bytesReceived = 0;

                do
                {
                    bytesReceived = sender.Receive(buffer);

                    // Get rid of all \0 bytes.
                    data += Encoding.UTF8.GetString(buffer.Take(bytesReceived).ToArray());

                    // Clean the buffer.
                    buffer = new byte[Consts.BufferSize];
                }
                while (sender.Available > 0);

                // Make sure the EOF tag exists in the received data and is at the correct position.
                if (!data.EndsWith(Consts.EOFTag))
                {
                    Logger.Log(LoggingLevel.Error, "Received data that doesn't end with an EOF Tag.");
                }

                // Returen the data without the EOF Tag.
                return data.Replace(Consts.EOFTag, "");
            }
        }
    }
}
