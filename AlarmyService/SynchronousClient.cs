using AlarmyLib;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AlarmyService
{
    class SynchronousClient
    {
        public IPAddress IPAddress { get; internal set; }
        public int Port { get; internal set; }

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
            sender.ReceiveTimeout = Consts.ReceiveTimeout;
        }

        /// <summary>
        /// Start the client.
        /// </summary>
        /// <remarks>This method may raise any exception that <see cref="Socket.Connect(EndPoint)"/> may raise.</remarks>
        /// <returns>The endpoint to which a successful connection was made.</returns>
        public EndPoint Start()
        {
            sender.Connect(remoteEP);
            return sender.RemoteEndPoint;
        }

        /// <summary>
        /// Gracefully shut down the connection.
        /// </summary>
        /// <remarks>This method may raise any exception that <see cref="Socket.Shutdown(SocketShutdown)"/> may raise.</remarks>
        public void Stop()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
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
            // Protect from parallel data sending, potentially causing corruption.
            lock (this)
            {
                byte[] msg = Encoding.UTF8.GetBytes(data + Consts.EOFTag);
                int messageSize = msg.Length;
                int bytesSent = 0;
                int totalBytesSent = 0;
                int zeroBytesSentCounter = 0;

                while (totalBytesSent != messageSize)
                {
                    // If we attempted to send data but failed for three consecutive times, raise exception.
                    if (zeroBytesSentCounter == 2)
                    {
                        Logger.Log(LoggingLevel.Error, "Attempted to send data but failed for three consecutive times.");
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
                int bytesReceived = -1;

                while (0 != bytesReceived)
                {
                    bytesReceived = sender.Receive(buffer);
                    data += Encoding.UTF8.GetString(buffer);

                    // Clean the buffer.
                    buffer = new byte[Consts.BufferSize];
                }

                // Make sure the EOF tag exists in the received data and is at the correct position.
                if (!data.EndsWith(Consts.EOFTag))
                {
                    Logger.Log(LoggingLevel.Error, "Received data that doesn't end an the EOF Tag.");
                }

                // Returen the data without the EOF Tag.
                return data.Replace(Consts.EOFTag, "");
            }
        }
    }
}
