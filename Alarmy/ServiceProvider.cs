using System;
using System.Collections.Generic;
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
        private static SynchronousClient s_client;
        private static Instance s_instance;

        private static bool s_shouldAttemtReconnecting = true;
        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Start communication with the Alarmy server.
        /// </summary>
        /// <param name="instance">The identity instance of this client.</param>
        public static void StartProvider(Instance instance)
        {
            s_instance = instance;
            s_shouldAttemtReconnecting = true;

            while (s_shouldAttemtReconnecting)
            {
                try
                {
                    s_client = new SynchronousClient(Properties.Settings.Default.ServiceURL,
                    Properties.Settings.Default.ServicePort);
                    s_client.Start();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Failed to start the client:\n{e}", "Error");
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
                s_client.Send(bufferFlush + initMessageWrapper.Serialize());

                // Start the communication loop.
                Communicate();

                // If we returned from the loop, something went wrong. Close the client and try again.
                s_client.Stop();
            }
        }

        private static void Communicate()
        {
            string data;

            while (true)
            {
                try
                {
                    data = s_client.Receive();

                    // Parse the data. Currently only one message type is supported.

                    if (null != data)
                    {
                        MessageWrapperContent content = MessageWrapper.Deserialize(data);
                        HandleMessageContent(content);
                    }
                }
                catch (SocketException se)
                {
                    // The server crashed / closed without disconnecting the client.
                    if (SocketError.ConnectionReset == se.SocketErrorCode)
                    {
                        s_logger.Warn("The server stopped responding without disconnecting the client.");
                    }
                    else
                    {
                        s_logger.Warn(se, "Failed to retrieve data from the server.");
                    }

                    return;
                }
                catch (Exception e)
                {
                    s_logger.Fatal(e, "Stopping service due to an unhandeled exception.");

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
        private static void HandleMessageContent(MessageWrapperContent content)
        {
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
                    PingResponse pr = new PingResponse(s_instance);
                    prWrapper.Message = pr;
                    s_client.Send(prWrapper.Serialize());
                } },

                { typeof(ErrorMessage), () =>
                {
                    ErrorMessage em = (ErrorMessage)content.Message;
                    s_logger.Error($"Received a code {em.Code} error message from the server: \n{em.Text ?? "<no additional data>"}");
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

        /// <summary>
        /// Stop communication with the Alarmy server.
        /// </summary>
        public static void StopProvider()
        {
            s_shouldAttemtReconnecting = false;
            s_client.Stop();
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
