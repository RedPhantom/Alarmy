using AlarmyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Alarmy
{
    // State object for receiving data from remote device.
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;

        // Size of receive buffer.
        public const int BufferSize = Consts.BufferSize;

        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new();

    }

    public class AsynchronousClient
    {
        // ManualResetEvent instances signal completion.
        private static readonly ManualResetEvent s_connectDone = new(false);
        private static readonly ManualResetEvent s_sendDone = new(false);
        private static readonly ManualResetEvent s_receiveDone = new(false);
        private static ManualResetEvent s_stopClient = new(false);

        // The response from the remote device.
        private static String s_response = String.Empty;

        // The remote end point we're connecting to.
        private static IPEndPoint s_remoteEP;

        // Our instance identity.
        private static Instance s_instance;

        // Our proxy for IPC purposes.
        private static InternalProxy s_proxy;

        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly TimeSpan s_reconnectAttemptWait = new(hours: 0, minutes: 0, 
            seconds: Properties.Settings.Default.ReconnectInterval);

        /// <summary>
        /// Start the alarmy client.
        /// </summary>
        /// <remarks>
        /// Note: this method blocks execution of the calling thread until stopClient is set.
        /// </remarks>
        internal static void StartClient(string address, int port, Instance instance, ManualResetEvent stopClient,
            InternalProxy proxy)
        {
            s_instance = instance;
            s_stopClient = stopClient;
            s_proxy = proxy;

            try
            {
                IPAddress ipAddress = IPAddress.Parse(address);
                s_remoteEP = new IPEndPoint(ipAddress, port);
                bool shouldTerminateThread = false;

                while (!shouldTerminateThread)
                {
                    // Create / re-create our socket.
                    Socket client = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    // Connect to the remote endpoint.
                    Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.NotRunning,
                        $"Attempting to connect...");

                    client.BeginConnect(s_remoteEP, new AsyncCallback(ConnectCallback), client);
                    s_connectDone.WaitOne();

                    // Check the connection result - successful or not.
                    // If the client is not connected and we're requested to stop, return.
                    if ((!client.Connected) && s_stopClient.WaitOne(0))
                    {
                        return;
                    }

                    // Success.

                    // Update tray icon.
                    lock (Program.Context)
                    {
                        Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.Regular);
                    }

                    // Sent initialization message.
                    Send(client, GetInitializationMessage(s_instance));
                    s_sendDone.WaitOne();

                    // Communication loop.
                    shouldTerminateThread = Communicate(client, s_stopClient);

                    // Disconnection.

                    // Release the socket.
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();

                    // Reset the StopClient flag so we don't disconnect the moment the client is started again.
                    s_stopClient.Reset();

                    // Update the tray icon.
                    lock (Program.Context)
                    {
                        Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.NotRunning,
                            AlarmyApplicationContext.StoppedTrayIconText);
                    }
                }
            }
            catch (Exception e)
            {
                // An exception here will cause StartClient to return.
                s_logger.Error(e, "Unexpected error in client connect. States: \n" +
                    $"s_connectDone = {s_connectDone.WaitOne(0)}\n" +
                    $"s_sendDone = {s_sendDone.WaitOne(0)}\n" +
                    $"s_recevieDone = {s_receiveDone.WaitOne(0)}\n" +
                    $"s_stopClient = {s_stopClient.WaitOne(0)}\n");
                MessageBox.Show("Unexpected error in client connect. Check log.");
            }
        }

        /// <summary>
        /// Wait for messages or a client request-to-stop signal.
        /// </summary>
        /// <param name="client">Client socket.</param>
        /// <param name="stopClient">
        /// Flag tracking whether a server-stop is requested. Set in case of an unrecoverable error
        /// or a user's request.
        /// </param>
        /// <returns>Whether a request for server stop has been made.</returns>
        private static bool Communicate(Socket client, ManualResetEvent stopClient)
        {
            int readyHandlerIndex = 0;
            byte[] internalToServerBuffer = new byte[Consts.BufferSize];

            while (true)
            {
                // Receive the response from the remote device.
                // The response is stored in `s_response`.
                if (!Receive(client))
                {
                    // Got an error but it's not fatal. We can re-create the socket and connect again.
                    return false;
                }

                // Check which handle has been set.
                readyHandlerIndex = WaitHandle.WaitAny(new WaitHandle[] { stopClient, s_receiveDone, s_proxy.SignalServer });

                if (0 == readyHandlerIndex)
                {
                    // It was requested that we stop the client.
                    return true;
                }
                else if (1 == readyHandlerIndex)
                {
                    // s_receiveDone has been set.

                    // If the response is empty, it means that the remove host has dropped the connection.
                    // In that case, we stop the client. The user will restart the service manually.
                    // TODO: Restart the service automatically.
                    if (s_response.Length == 0)
                    {
                        string gracefulServerStopMsg = "Server gracefully closed. Manually restart the Alarmy client.";

                        Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.NotRunning,
                            gracefulServerStopMsg);
                        MessageBox.Show(gracefulServerStopMsg, "Alarmy");

                        return true;
                    }

                    // We received data, handle each message separately.
                    foreach (string message in s_response.Split(Consts.EOFTag, StringSplitOptions.RemoveEmptyEntries))
                    {
                        PushToMessageQueue(client, message);
                    }

                    // TODO: Maybe call ProcessMessageQueue outside of this logic so it doesn't hang the server.
                    ProcessMessageQueue();

                    // Clean the buffer after parsing all messages.
                    s_response = "";

                    // Reset s_receiveDone. It will get set when the next data comes through.
                    s_receiveDone.Reset();
                    s_proxy.SignalClient.Set();
                }
                else if (2 == readyHandlerIndex)
                {
                    // Send data from proxy to server via the Socket client.
                    // TODO: allow messages larget than<BUFFER SIZE>
                    s_proxy.ServerToInternal.Receive(internalToServerBuffer);
                    s_logger.Trace($"AsynchronousClient Received to send via Proxy {Encoding.UTF8.GetString(internalToServerBuffer)}.");
                    client.Send(internalToServerBuffer);

                    s_proxy.SignalServer.Reset();
                }
            }
        }

        /// <summary>
        /// Process messages waiting in the buffer.
        /// </summary>
        private static void PushToMessageQueue(Socket client, string message)
        {
            // We received a MessageWrapper.
            MessageWrapperContent content = MessageWrapper.Deserialize(message);

            // Add the message to the queue.
            lock (AlarmyState.MessageQueue)
            {
                // TODO: Either only add specific messages that need to be transferred to the UI
                //       thread, or perform the rest of the HandleMessage logic based on messages in the message
                //       queue (i.e. move all of the @switch dictionary to another method which iterates on the
                //       message queue and empties it).
                AlarmyState.MessageQueue.Add(new (client, content));
            }
        }

        /// <summary>
        /// Send each message in the queue for handlig. Remove it from the queue if no more handling is required.
        /// </summary>
        private static void ProcessMessageQueue()
        {
            // Whether the message has been fully processed and can be removed from the buffer.
            bool treatmentComplete;
            List<MessageQueueItem> messagesToRemove = new();

            // TODO: Add MessageQueueItem object **which will have its own unique id** if the message content
            //       doesn't already have one???
            foreach (MessageQueueItem mqi in AlarmyState.MessageQueue)
            {
                // Must be outside of lock() in case processing takes a long time.
                // We don't want to block access to the queue.
                treatmentComplete = HandleMessage(mqi.Sender, mqi.Message);

                // TODO: Add a unique ID to the MWC or else the wrong MWC could be removed from the queue?
                if (treatmentComplete && AlarmyState.MessageQueue.Contains(mqi))
                {
                    messagesToRemove.Add(mqi);
                }
            }

            lock (AlarmyState.MessageQueue)
            {
                AlarmyState.MessageQueue.RemoveAll(x => messagesToRemove.Contains(x));
            }
        }


        /// <summary>
        /// Handle a message received from the buffer.
        /// </summary>
        /// <param name="client">Socket from which the message was received.</param>
        /// <param name="content">Message content.</param>
        /// <returns>
        /// Whether processing and treatment of the message is complete and then message
        /// can be removed from the queue.
        /// </returns>
        private static bool HandleMessage(Socket client, MessageWrapperContent content)
        {
            bool treatmentComplete = true;

            // TODO: Find a way to act on @switch without creating a dedicated variable each time.
            // Actions per message type.
            Dictionary<Type, Action> @switch = new()
            {
                // ShowAlarmMessage
                // Display an alarm to the user.
                {
                    typeof(ShowAlarmMessage),
                    () => {
                        ShowAlarmMessage sam = (ShowAlarmMessage)content.Message;

                        lock (AlarmyState.PastAlarms)
                        {
                            AlarmyState.PastAlarms.Add(new(sam.Alarm, DateTime.Now, sam.Type));
                        }

                        // We start the alarm on a new thread so after Show() is called and we return to wait
                        // on Socket.Receive(), the UI can be responsive.
                        Thread t = new(new ThreadStart(() => {
                            frmAlarm frmAlarm = new()
                            {
                                TopMost = AlarmStyle.Interruptive == (AlarmStyle)Properties.Settings.Default.AlarmStyle
                            };

                            frmAlarm.LoadAlarm(sam.Alarm, sam.Type);
                            Application.Run(frmAlarm);
                        }));
                        t.Start();
                    }
                },

                // PingMessage
                // Send a response ("pong") to the user.
                {
                    typeof(PingMessage),
                    () =>
                    {
                        MessageWrapper<PingResponse> prWrapper = new();
                        PingResponse pr = new(s_instance);
                        prWrapper.Message = pr;
                        Send(client, prWrapper.Serialize());
                    }
                },

                // ErrorMessage
                // Got an error from the server, display it to the user.
                {
                    typeof(ErrorMessage),
                    () =>
                    {
                        ErrorMessage em = (ErrorMessage)content.Message;
                        MessageBox.Show($"Received a code {em.Code} error message from the server: \n{em.Text ?? "<no additional data>"}",
                            "Alarmy: Got error from server");
                    }
                },

                // GroupQueryResponse
                // Get the response containing the Group information from the server.
                {
                    typeof(GroupQueryResponse),
                    () =>
                    {
                        // Message is not handled here, but used up from the Message Queue.
                        treatmentComplete = false;
                    }
                },
            };

            try
            {
                @switch[content.Type]();
            }
            catch (KeyNotFoundException ke)
            {
                s_logger.Warn(ke, $"Received an unexpected message type: {content.Type}. Message: {content.Repr()}.");
            }
            catch (Exception e)
            {
                s_logger.Error(e, $"Unexpected error during message handling. Message: {content.Repr()}.");
            }

            return treatmentComplete;
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            Socket client = null;

            try
            {
                // Retrieve the socket from the state object.
                client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                // Signal that the connection has been made.
                s_connectDone.Set();
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    lock (Program.Context)
                    {
                        Program.Context.SetTrayIconStatus(
                            AlarmyApplicationContext.TrayIconStatus.NotRunning,
                            "Failed to connect to the Alarmy Server. " +
                            $"Retyring in {s_reconnectAttemptWait.TotalSeconds} seconds.");

                        // Only attempt reconnection if the client is valid and
                        // the s_stopClient flag is not set.
                        if ((client is not null) && !s_stopClient.WaitOne(0))
                        {
                            Thread.Sleep(s_reconnectAttemptWait);
                            client.BeginConnect(s_remoteEP, ConnectCallback, client);
                        }
                        else if (s_stopClient.WaitOne(0))
                        {
                            s_connectDone.Set();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Receive data from the server.
        /// </summary>
        /// <param name="client"></param>
        /// <returns><c>false</c> when the socket received an error (usually due to disconnection).</returns>
        private static bool Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new();
                state.workSocket = client;

                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                return true;
            }
            catch (ObjectDisposedException)
            {
                // This is a "valid" exception when closing the socket.
                return false;
            }
            catch (Exception e)
            {
                s_logger.Error(e, "Unexpected error in client receive.");
                return false;
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // TODO: for some reason, when the stop the server, client.Conncted is still true.
                // Check if the callback was triggered by closing the socket.
                if (!client.Connected)
                {
                    // TODO: Put the client in an "attempting to reconnect..." mode.
                    // This can be done by stopping and re-starting the client, leaving
                    // it in the Connecting method.
                    // Stop the client since the server crashed.
                    s_stopClient.Set();
                    return;
                }

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                    // Check if that's all the data or if we have more retrieving to do.
                    if (state.sb.ToString().Contains(Consts.EOFTag))
                    {
                        // All the data has arrived; put it in response.
                        if (state.sb.Length >= 1)
                        {
                            s_response = state.sb.ToString();
                        }

                        s_proxy.ServerToInternal.Send(state.buffer);
                        s_proxy.SignalClient.Set();

                        // Signal that all bytes have been received.
                        s_receiveDone.Set();
                    }
                    else
                    {
                        // Get the rest of the data.
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                        s_receiveDone.Reset();
                    }
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        s_response = state.sb.ToString();
                    }

                    // Signal that all bytes have been received.
                    s_receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                s_logger.Error(e, "Unexpected error in receive callback.");
            }
        }

        private static void Send(Socket client, string data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = MessageUtils.BuildMessage(data);

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                s_sendDone.Set();
            }
            catch (Exception e)
            {
                s_logger.Error(e, "Error during send callback.");
            }
        }

        private static string GetInitializationMessage(Instance instance)
        {
            MessageWrapper<ServiceStartedResponse> initMessageWrapper = new();
            ServiceStartedResponse ssm = new(instance, new(AlarmyState.Groups.Select(x => x.ID)));
            initMessageWrapper.Message = ssm;
            return initMessageWrapper.Serialize();
        }
    }
}
