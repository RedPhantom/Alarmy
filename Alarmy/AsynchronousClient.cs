﻿using AlarmyLib;
using System;
using System.Collections.Generic;
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
        public StringBuilder sb = new StringBuilder();

    }

    public class AsynchronousClient
    {
        // ManualResetEvent instances signal completion.
        private static ManualResetEvent s_connectDone = new(false);
        private static ManualResetEvent s_sendDone    = new(false);
        private static ManualResetEvent s_receiveDone = new(false);
        private static ManualResetEvent s_stopClient  = new(false);

        // The response from the remote device.
        private static String s_response = String.Empty;

        // The remote end point we're connecting to.
        private static IPEndPoint s_remoteEP;

        // Our instance identity.
        private static Instance s_instance;

        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly TimeSpan s_reconnectAttemptWait = new TimeSpan(hours: 0, minutes: 0, seconds: 15);

        internal static void StartClient(string address, int port, Instance instance, ManualResetEvent stopClient)
        {
            s_instance = instance;
            s_stopClient = stopClient;

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
                    if ((!client.Connected) && s_stopClient.WaitOne(0))
                    {
                        return;
                    }

                    lock (Program.Context)
                    {
                        Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.Regular);
                    }

                    // Sent initialization message.
                    Send(client, "              " + GetInitializationMessage(s_instance));
                    s_sendDone.WaitOne();

                    shouldTerminateThread = Communicate(client, s_stopClient);

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
                s_logger.Error(e, "Unexpected error in client connect. States: \n" +
                    $"s_connectDone = {s_connectDone.WaitOne(0)}\n" +
                    $"s_sendDone = {s_sendDone.WaitOne(0)}\n" +
                    $"s_recevieDone = {s_receiveDone.WaitOne(0)}\n" +
                    $"s_stopClient = {s_stopClient.WaitOne(0)}\n");
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
            while (true)
            {
                // Receive the response from the remote device.
                // The response is stored in `s_response`.
                if (!Receive(client))
                {
                    // Got an error but it's not fatal. We can re-create the socket and connect again.
                    return false;
                }

                if (0 == WaitHandle.WaitAny(new WaitHandle[] { stopClient, s_receiveDone }))
                {
                    // It was requested that we stop the client.
                    return true;
                }
                else
                {
                    // If the response is empty, it means that the remove host has dropped the connection.
                    // In that case, we stop the client. The user will restart the service manually.
                    if (0 == s_response.Length)
                    {
                        string gracefulServerStopMsg = "Server gracefully closed. Manually restart the service to try again.";
                        
                        Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.NotRunning,
                            gracefulServerStopMsg);
                        MessageBox.Show(gracefulServerStopMsg, "Alarmy");
                        
                        return true;
                    }

                    foreach (string message in s_response.Split(Consts.EOFTag, StringSplitOptions.RemoveEmptyEntries))
                    {
                        HandleMessage(client, message);
                    }

                    // Clean the buffer after parsing all messages.
                    s_response = "";

                    // Reset s_receiveDone. It will get set when the next data comes through.
                    s_receiveDone.Reset();
                }
            }
        }

        private static void HandleMessage(Socket client, string message)
        {
            // Assume we received a MessageWrapper.
            MessageWrapperContent content = MessageWrapper.Deserialize(message);
            
            // Actions per message type.
            Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
                // ShowAlarmMessage
                // Display an alarm to the user.
                { typeof(ShowAlarmMessage), () => {
                    ShowAlarmMessage sam = (ShowAlarmMessage)content.Message;

                    lock (AlarmyState.s_pastAlarms)
                    {
                        AlarmyState.s_pastAlarms.Add(sam.Alarm);
                    }

                    // We start the alarm on a new thread so after Show() is called and we return to wait
                    // on Socket.Receive(), the UI can be responsive.
                    Thread t = new Thread(new ThreadStart(() => {
                        frmAlarm frmAlarm = new() 
                        {
                            TopMost = Properties.Settings.Default.AlarmsInterruptive
                        };

                        frmAlarm.LoadAlarm(sam.Alarm);
                        Application.Run(frmAlarm);
                    }));
                    t.Start();
                } },

                // PingMessage
                // Send a response ("pong") to the user.
                { typeof(PingMessage), () =>
                {
                    MessageWrapper<PingResponse> prWrapper = new MessageWrapper<PingResponse>();
                    PingResponse pr = new PingResponse(s_instance);
                    prWrapper.Message = pr;
                    Send(client, prWrapper.Serialize());
                } },

                // ErrorMessage
                // Got an error from the server, display it to the user.
                { typeof(ErrorMessage), () =>
                {
                    ErrorMessage em = (ErrorMessage)content.Message;
                    MessageBox.Show($"Received a code {em.Code} error message from the server: \n{em.Text ?? "<no additional data>"}", 
                        "Alarmy: Got error from server");
                } }
            };

            try
            {
                @switch[content.Type]();
            }
            catch (KeyNotFoundException ke)
            {
                s_logger.Warn(ke, $"Received an unexpected message type: {content.Type}.");
            }
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
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns><c>false</c> when the socket received an error (usually due to disconnection).</returns>
        private static bool Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
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
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Check if that's all the data or if we have more retrieving to do.
                    if (state.sb.ToString().Contains(Consts.EOFTag))
                    {
                        // All the data has arrived; put it in response.
                        if (state.sb.Length > 1)
                        {
                            s_response = state.sb.ToString();
                        }
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

        private static void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.UTF8.GetBytes(data + Consts.EOFTag);

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
                Console.WriteLine(e.ToString());
            }
        }

        private static string GetInitializationMessage(Instance instance)
        {
            MessageWrapper<ServiceStartedResponse> initMessageWrapper = new MessageWrapper<ServiceStartedResponse>();
            ServiceStartedResponse ssm = new ServiceStartedResponse(instance);
            initMessageWrapper.Message = ssm;
            return initMessageWrapper.Serialize();
        }
    }
}
