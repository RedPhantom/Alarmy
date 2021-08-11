using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading;
using AlarmyLib;

namespace AlarmyService
{
    /// <summary>
    /// Provide an interface to the Alarmy server.
    /// </summary>
    public static class ServiceProvider
    {
        // Reference to the service that uses this provider. Required so we can stop the service in an error.
        private static ServiceBase ServiceBase;

        private static SynchronousClient Client;
        
        private static UnifiedLogger Logger = new UnifiedLogger("AlarmyService.ServiceProvider");
        private static bool ShouldAttemtReconnecting = true;

        /// <summary>
        /// Start communication with the Alarmy server.
        /// </summary>
        public static void StartProvider(ServiceBase serviceBase)
        {
            ServiceBase = serviceBase;
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            Logger.EnableEventLogLogging(EventLogger.EventLogSource.AlarmyService);

            BackgroundWorker bgwSendKeepAlive = new BackgroundWorker();
            Instance instance = Instance.GetInstance();

            while (ShouldAttemtReconnecting)
            {
                Client = new SynchronousClient(Properties.Settings.Default.ServiceURL, 
                    Properties.Settings.Default.SerivcePort);
                Client.Start();

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
                    MessageWrapperContent content = MessageWrapper.Deserialize(data);
                    HandleMessageContent(content, instance);
                }
                catch (SocketException se)
                {
                    // The server crashed / closed without disconnecting the client.
                    if (SocketError.ConnectionReset == se.SocketErrorCode)
                    {
                        Logger.Log(LoggingLevel.Warning, "The server stopped responding without disconnecting the client.");
                    }
                    else
                    {
                        Logger.Log(LoggingLevel.Error, "Failed to retrieve data from the server ({0}), stopping service:\n{1}.", se.SocketErrorCode, se);
                    }

                    return;
                }
                catch (Exception e)
                {
                    Logger.Log(LoggingLevel.Error, "Stopping service due to an exception:\n{0}", e);

                    // No matter the exception, stop the provider.
                    StopProvider();
                    ServiceBase.Stop();
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
                    Program.ShowAlarm(sam);
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
                    Logger.Log(LoggingLevel.Error, "Received a code {0} error message from the server: \n{1}",
                        em.Code,
                        em.Text == null ? "<no additional data>" : em.Text);
                } }
            };

            try
            {
                @switch[content.Type]();
            }
            catch (KeyNotFoundException)
            {
                Logger.Log(LoggingLevel.Warning, "Received an unexpected message type: {0}.", content.Type.Name);
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
