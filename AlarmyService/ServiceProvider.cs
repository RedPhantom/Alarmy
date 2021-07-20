using AlarmyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace AlarmyService
{
    /// <summary>
    /// Provide an interface to the Alarmy server.
    /// </summary>
    public static class ServiceProvider
    {
        private static SynchronousClient client =
            new SynchronousClient(AlarmySettings.Default.ServiceURL, AlarmySettings.Default.SerivcePort);

        /// <summary>
        /// Start communication with the Alarmy server.
        /// </summary>
        public static void StartProvider()
        {
            BackgroundWorker bgwSendKeepAlive = new BackgroundWorker();
            Instance instance = Instance.GetInstance();
            string data = string.Empty;

            client.Start();

            try
            {
                // Prepare an initialization message.
                MessageWrapper<ServiceStartedResponse> initMessageWrapper = new MessageWrapper<ServiceStartedResponse>();
                ServiceStartedResponse ssm = new ServiceStartedResponse(instance);
                initMessageWrapper.Message = ssm;

                // Send the initialization message.
                client.Send(initMessageWrapper.Serialize());

                EventLoggerUtils.Log("Creating KeepAlive BGW.", EventLogEntryType.Information);
                // Start sending KeepAlives.
                bgwSendKeepAlive.DoWork += PeriodicKeepAlive;
                bgwSendKeepAlive.RunWorkerAsync(new KeepAliveParams(Consts.KeepAliveInterval, instance));
                    
                while (true)
                {
                    data = client.Receive();

                    // Parse the data. Currently only one message type is supported.
                    MessageWrapperContent content = MessageWrapper.Deserialize(data);
                    Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {
                            // ShowAlarmMessage
                            // Here we receive a request to display an alarm to the user.
                            { typeof(ShowAlarmMessage), () => {
                                ShowAlarmMessage sam = (ShowAlarmMessage)content.Message;
                                Program.ShowAlarm(sam.Alarm);
                            } },

                            // PingMessage
                            // Here we receive a PingMessage and send back a simple ping response.
                            { typeof(PingMessage), () =>
                            {
                                MessageWrapper<PingResponse> wrapper = new MessageWrapper<PingResponse>();
                                PingResponse pr = new PingResponse(instance);
                                wrapper.Message = pr;
                                client.Send(wrapper.Serialize());
                            } },
                        };

                    @switch[content.Type]();
                }
            }
            catch (SocketException se)
            {
                switch ((SocketError)se.ErrorCode)
                {
                    case SocketError.TimedOut:
                        // No data was received so timeout was received. This is okay and does not require extra-handling.
                        break;

                    default:
                        EventLoggerUtils.Log(string.Format("Got an unhandled Socket exception, code: {0} ({1}).",
                                se.ErrorCode,
                                ((SocketError)se.ErrorCode).ToString()),
                            EventLogEntryType.Error);
                        break;
                }
            }
            catch (Exception e)
            {
                EventLoggerUtils.LogError("Unexpected error during client operation.", e);
            }
        }

        /// <summary>
        /// Stop communication with the Alarmy server.
        /// </summary>
        public static void StopProvider()
        {
            client.Stop();
        }

        /// <summary>
        /// Send a KeepAliveResponse message to the Alarmy server. Receives arguments via <see cref="KeepAliveParams"/>.
        /// </summary>
        private static void PeriodicKeepAlive(object sender, DoWorkEventArgs e)
        {
            EventLoggerUtils.Log("Entering PeriodicKeepAlive. Cancel: " + e.Cancel.ToString(), EventLogEntryType.Information);

            KeepAliveParams args = (KeepAliveParams)e.Argument;

            while (!e.Cancel)
            {
                MessageWrapper<KeepAliveResponse> wrapper = new MessageWrapper<KeepAliveResponse>();
                KeepAliveResponse kam = new KeepAliveResponse(args.Instance);
                wrapper.Message = kam;
                client.Send(wrapper.Serialize());
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
