//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using System.Text;
//using System.Diagnostics;
//using AlarmyLib;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Threading.Tasks;
//using System.Management;
//using System.Linq;

//namespace AlarmyService
//{
//    // State object for receiving data from remote device.  
//    public class StateObject
//    {
//        // Client socket.  
//        public Socket workSocket = null;
//        // Size of receive buffer.  
//        public const int BufferSize = 256;
//        // Receive buffer.  
//        public byte[] buffer = new byte[BufferSize];
//        // Received data string.  
//        public StringBuilder sb = new StringBuilder();
//    }

//    public class AsynchronousClient
//    {
//        private static ManualResetEvent connectDone = new ManualResetEvent(false);
//        private static ManualResetEvent sendDone = new ManualResetEvent(false);
//        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

//        // Keep the instance information here. It won't change.
//        private static Instance instance = Instance.GetInstance();

//        // The response from the remote device.  
//        private static String response = String.Empty;

//        private static CancellationTokenSource periodicKeepAliveTokenSource = new CancellationTokenSource();
//        private static BackgroundWorker bgwReceiveHandler = new BackgroundWorker();
//        private static BackgroundWorker bgwSendKeepAlive = new BackgroundWorker();

//        public static void StartClient(string address, int port)
//        {
//            // Connect to a remote device.  
//            try
//            {
//                IPAddress ipAddress = IPAddress.Parse(address);
//                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

//                // Create a TCP/IP socket.  
//                Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

//                // Connect to the remote endpoint.  
//                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
//                connectDone.WaitOne();
                
//                // Prepare an initialization message.
//                MessageWrapper<ServiceStartedResponse> wrapper = new MessageWrapper<ServiceStartedResponse>();
//                ServiceStartedResponse ssm = new ServiceStartedResponse(instance);
//                wrapper.Message = ssm;

//                // Send the initialization message.
//                Send(client, wrapper.Serialize());

//                // Start sending KeepAlives.
//                bgwSendKeepAlive = new BackgroundWorker();
//                bgwSendKeepAlive.DoWork += PeriodicKeepAlive;
//                bgwSendKeepAlive.RunWorkerAsync(Tuple.Create(client, new TimeSpan(0, 1, 0)));

//                // Handle receive data.
//                bgwReceiveHandler = new BackgroundWorker();
//                bgwReceiveHandler.DoWork += ReceiveDataHandler;
//                bgwReceiveHandler.RunWorkerAsync(client);
//            }
//            catch (Exception e)
//            {
//                if (!ExceptionHandler.RaiseCustomException(e))
//                {
//                    EventLoggerUtils.LogError("Unexpected error during StartClient", e);
//                }
//            }
//        }

//        private static void PeriodicKeepAlive(object sender, DoWorkEventArgs e)
//        {
//            Tuple<Socket, TimeSpan> args = (Tuple<Socket, TimeSpan>)e.Argument;
//            Socket client = args.Item1;
//            TimeSpan interval = args.Item2;

//            while (!e.Cancel)
//            {
//                MessageWrapper<KeepAliveResponse> wrapper = new MessageWrapper<KeepAliveResponse>();
//                KeepAliveResponse kam = new KeepAliveResponse(instance);
//                wrapper.Message = kam;
//                Send(client, wrapper.Serialize());
//                Thread.Sleep(interval);
//            }
//        }

//        private static void ReceiveDataHandler(object sender, DoWorkEventArgs e)
//        {
//            while (true)
//            {
//                Socket client = (Socket)e.Argument;
//                // Wait for data.
//                Receive(client);
//                receiveDone.WaitOne();

//                // Parse the data. Currently only one message type is supported.
//                Tuple<object, Type> message = MessageWrapper.Deserialize(response);
//                var @switch = new Dictionary<Type, Action> {
                    
//                    // ShowAlarmMessage
//                    // Here we receive a request to display an alarm to the user.
//                    { typeof(ShowAlarmMessage), () => {
//                        ShowAlarmMessage sam = (ShowAlarmMessage)message.Item1;
//                        Program.ShowAlarm(sam.Alarm);
//                    } },

//                    // PingMessage
//                    // Here we receive a PingMessage and send back a simple ping response.
//                    { typeof(PingMessage), () =>
//                    {
//                        MessageWrapper<PingResponse> wrapper = new MessageWrapper<PingResponse>();
//                        PingResponse pr = new PingResponse(instance);
//                        wrapper.Message = pr;
//                        Send(client, wrapper.Serialize());
//                    } },
//                };

//                @switch[message.Item2]();
//            }
//        }

//        public static void StopClient()
//        {
//            periodicKeepAliveTokenSource.Cancel();
//        }

//        private static void ConnectCallback(IAsyncResult ar)
//        {
//            try
//            {
//                // Retrieve the socket from the state object.  
//                Socket client = (Socket)ar.AsyncState;

//                // Complete the connection.  
//                client.EndConnect(ar);

//                // Signal that the connection has been made.  
//                connectDone.Set();
//            }
//            catch (Exception e)
//            {
//                if (!ExceptionHandler.RaiseCustomException(e))
//                {
//                    EventLoggerUtils.LogError("Unexpected error during ConnectCallback", e);
//                }
//            }
//        }

//        public static void Receive(Socket client)
//        {
//            try
//            {
//                // Create the state object.  
//                StateObject state = new StateObject();
//                state.workSocket = client;

//                // Begin receiving the data from the remote device.  
//                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//                    new AsyncCallback(ReceiveCallback), state);
//            }
//            catch (Exception e)
//            {
//                EventLoggerUtils.LogError("Error during Receive", e);
//            }
//        }

//        private static void ReceiveCallback(IAsyncResult ar)
//        {
//            try
//            {
//                // Retrieve the state object and the client socket
//                // from the asynchronous state object.  
//                StateObject state = (StateObject)ar.AsyncState;
//                Socket client = state.workSocket;

//                // Read data from the remote device.  
//                int bytesRead = client.EndReceive(ar);

//                if (bytesRead > 0)
//                {
//                    // There might be more data, so store the data received so far.  
//                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

//                    // Get the rest of the data.  
//                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//                        new AsyncCallback(ReceiveCallback), state);
//                }
//                else
//                {
//                    // All the data has arrived; put it in response.  
//                    if (state.sb.Length > 1)
//                    {
//                        response = state.sb.ToString();
//                    }
//                    // Signal that all bytes have been received.  
//                    receiveDone.Set();
//                }
//            }
//            catch (Exception e)
//            {
//                // TODO: An error will be raised here when the server shuts down unexpectedly.
//                // TODO: Add logic that will attempt to re-connect every 30 seconds.
//                if (!ExceptionHandler.RaiseCustomException(e))
//                {
//                    EventLoggerUtils.LogError("Unexpected error during ReceiveCallback", e);
//                }
//            }
//        }

//        public static void Send(Socket client, String data)
//        {
//            // Convert the string data to byte data using ASCII encoding.  
//            byte[] byteData = Encoding.ASCII.GetBytes(string.Format("{0}{1}", data, Consts.EOFTag));

//            // Begin sending the data to the remote device.  
//            client.BeginSend(byteData, 0, byteData.Length, 0,
//                new AsyncCallback(SendCallback), client);
//        }

//        private static void SendCallback(IAsyncResult ar)
//        {
//            try
//            {
//                // Retrieve the socket from the state object.  
//                Socket client = (Socket)ar.AsyncState;

//                // Complete sending the data to the remote device.  
//                int bytesSent = client.EndSend(ar);

//                // Signal that all bytes have been sent.  
//                sendDone.Set();
//            }
//            catch (Exception e)
//            {
//                if (!ExceptionHandler.RaiseCustomException(e))
//                {
//                    EventLoggerUtils.LogError("Unexpected error during SendCallback", e);
//                }
//            }
//        }
//    }
//}