﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using AlarmyLib;

namespace Alarmy
{
    /// <summary>
    /// Provide an interface to the Alarmy server.
    /// </summary>
    public static class ServiceProvider
    {
        private static SynchronousClient Client;

        private static bool ShouldAttemtReconnecting = true;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Start communication with the Alarmy server.
        /// </summary>
        public static void StartProvider()
        {
            Instance instance = Instance.GetInstance();

            while (ShouldAttemtReconnecting)
            {
                try
                {
                    Client = new SynchronousClient(Properties.Settings.Default.ServiceURL,
                    Properties.Settings.Default.ServicePort);
                    Client.Start();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to start the client:\n" + e.ToString(), "Error");
                    return;
                }

                // Prepare an initialization message.

                // Flush the buffer with some spaces.
                // TODO: Super arabic. Need to figure out why the socket skips some bytes.
                string bufferFlush = "".PadRight(20, ' ');
                MessageWrapper<ServiceStartedResponse> initMessageWrapper = new MessageWrapper<ServiceStartedResponse>();
                ServiceStartedResponse ssm = new ServiceStartedResponse(instance);
                initMessageWrapper.Message = ssm;

                // Send the initialization message. Adding a few space characters to flush the buffer.
                Client.Send(bufferFlush + initMessageWrapper.Serialize());

                // Start the communication loop.
                Communicate(instance);

                // If we returned from the loop, something went wrong. Close the client and try again.
                Client.Stop();
            }
        }

        private static void Communicate(Instance instance)
        {
            string data;

            while (true)
            {
                try
                {
                    data = Client.Receive();

                    // Parse the data. Currently only one message type is supported.

                    if (null != data)
                    {
                        MessageWrapperContent content = MessageWrapper.Deserialize(data);
                        HandleMessageContent(content, instance);
                    }
                }
                catch (SocketException se)
                {
                    // The server crashed / closed without disconnecting the client.
                    if (SocketError.ConnectionReset == se.SocketErrorCode)
                    {
                        Logger.Warn("The server stopped responding without disconnecting the client.");
                    }
                    else
                    {
                        Logger.Warn(se, "Failed to retrieve data from the server.");
                    }

                    return;
                }
                catch (Exception e)
                {
                    Logger.Fatal(e, "Stopping service due to an exception.");

                    // No matter the exception, stop the provider.
                    StopProvider();
                    return;
                }
            }
        }

        /// <summary>
        /// Handle possible messages received from the server.
        /// </summary>
        /// <param name="content">Object containing the message and its type for casting.</param>
        /// <param name="instance">Instance of the service.</param>
        private static void HandleMessageContent(MessageWrapperContent content, Instance instance)
        {
            Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
                // ShowAlarmMessage
                // Display an alarm to the user.
                { typeof(ShowAlarmMessage), () => {
                    ShowAlarmMessage sam = (ShowAlarmMessage)content.Message;

                    lock (AlarmyState.PastAlarms)
                    {
                        AlarmyState.PastAlarms.Add(sam.Alarm);
                    }

                    // We start the alarm on a new thread so after Show() is called and we return to wait
                    // on Socket.Receive(), the UI can be responsive.
                    Thread t = new Thread(new ThreadStart(() => {
                        frmAlarm frmAlarm = new frmAlarm();
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
                    PingResponse pr = new PingResponse(instance);
                    prWrapper.Message = pr;
                    Client.Send(prWrapper.Serialize());
                } },

                { typeof(ErrorMessage), () =>
                {
                    ErrorMessage em = (ErrorMessage)content.Message;
                    Logger.Error("Received a code {0} error message from the server: \n{1}", em.Code, em.Text ?? "<no additional data>");
                } }
            };

            try
            {
                @switch[content.Type]();
            }
            catch (KeyNotFoundException ke)
            {
                Logger.Warn(ke, "Received an unexpected message type: {0}.", content.Type);
            }
        }

        /// <summary>
        /// Stop communication with the Alarmy server.
        /// </summary>
        public static void StopProvider()
        {
            Client.Stop();
        }

        /// <summary>
        /// Send a KeepAliveResponse message to the Alarmy server. Receives arguments via <see cref="KeepAliveParams"/>.
        /// </summary>
        private static void PeriodicKeepAlive(object sender, DoWorkEventArgs e)
        {
            KeepAliveParams args = (KeepAliveParams)e.Argument;

            while (!e.Cancel)
            {
                MessageWrapper<KeepAliveResponse> wrapper = new MessageWrapper<KeepAliveResponse>();
                KeepAliveResponse kam = new KeepAliveResponse(args.Instance);
                wrapper.Message = kam;

                Client.Send(wrapper.Serialize());
                Thread.Sleep(args.Interval);
            }
        }
    }

    /// <summary>
    /// Holds parameters to pass to a BackgroundProcess performing KeepAlive sending.
    /// </summary>
    class KeepAliveParams
    {
        public TimeSpan Interval { get; set; }
        public Instance Instance { get; set; }

        public KeepAliveParams(TimeSpan interval, Instance instance)
        {
            Interval = interval;
            Instance = instance;
        }
    }
}
