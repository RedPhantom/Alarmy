using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlarmyManager
{
    /// <summary>
    /// A service provider that handles all created events:
    /// receives the IDs of all created events from the BlueEye devices.
    /// Sends all new event IDs to Mashlat applications.
    /// </summary>
    class AlarmyServiceProvider : TcpServiceProvider
    {
        public Dictionary<Instance, DateTime> ActiveInstances = new Dictionary<Instance, DateTime>();

        private string _receivedStr;
        private UnifiedLogger Logger = new UnifiedLogger("AlarmyServiceProvider");

        public AlarmyServiceProvider()
        {
            Logger.EnableConsoleLogging();
            Logger.EnableEventLogLogging(EventLogger.EventLogSource.AlarmyManager);
            Logger.EnableFileLogging(SharedWriter.Writer);
        }

        public override object Clone()
        {
            return new AlarmyServiceProvider();
        }

        /// <summary>
        /// A connection has been esablished with a client.
        /// </summary>
        /// <param name="state">Client with which connection established.</param>
        public override void OnAcceptConnection(ConnectionState state)
        {
            Logger.Log(LoggingLevel.Trace, "Accepted a connection from {0}.", state.RemoteEndPoint);
        }

        /// <summary>
        /// Data has been received by the socket.
        /// </summary>
        /// <param name="state">Client from which data has been received.</param>
        public override void OnReceiveData(ConnectionState state)
        {
            byte[] buffer = new byte[Consts.BufferSize];

            // We should not run into a situation where we can't keep up with incoming data,
            // so deciding that the message is ready for parsing soley based on the fact that no more data 
            // is available is okay.
            while (state.AvailableData > 0)
            {
                int readBytes = state.Read(buffer, 0, Consts.BufferSize);
                if (readBytes > 0)
                {
                    _receivedStr += Encoding.UTF8.GetString(buffer, 0, readBytes);
                }
                else
                {
                    // Failure to read while there is data available will cause connection termination.
                    state.EndConnection();
                }
            }

            // Reading data is complete, parsing is now possible.
            // Check if the message has been received to its end.
            if (_receivedStr.Contains(Consts.EOFTag))
            {
                MessageWrapperContent mrc = ParseMessage(_receivedStr.Replace(Consts.EOFTag, ""), state);
                HandleMessage(mrc);
            }
            else
            {
                Logger.Log(LoggingLevel.Warning, "Received an incomplete message (EOF Tag missing).");
            }

            _receivedStr = string.Empty;
        }

        /// <summary>
        /// Parse the data received from the client.
        /// </summary>
        /// <param name="msg">Message received from the client.</param>
        /// <param name="state">Client.</param>
        private MessageWrapperContent ParseMessage(string msg, ConnectionState state)
        {
            MessageWrapperContent wrapper;

            try
            {
                wrapper = MessageWrapper.Deserialize(msg);
                Logger.Log(LoggingLevel.Trace, "Received a message of type {0}.", wrapper.Type.Name);
            }
            catch
            {
                Logger.Log(LoggingLevel.Warning,
                    "Received a malformed message.\n\tOrigin: \n\t{0}\n\tRaw Content: \n\t{1}", state.RemoteEndPoint, msg);
                return null;
            }

            return wrapper;
        }

        private void HandleMessage(MessageWrapperContent mrc) 
        {
            // Handle the message (response - message from a client) based on its type.
            var @switch = new Dictionary<Type, Action> {
                { typeof(KeepAliveResponse), () => {
                    KeepAliveResponse kar = (KeepAliveResponse)mrc.Message;

                    // We don't respond to KeepAlive messages.

                    // Keep track of all instances and their time of last appearance.
                    UpdateActiveInstances(kar.Instance);
                } },
                { typeof(PingResponse), () => {
                    PingResponse pr = (PingResponse)mrc.Message;

                    // Since this is a ping response, no need to respond.

                    UpdateActiveInstances(pr.Instance);
                } },
                { typeof(ServiceStartedResponse), () => { 
                    // We don't respond to ServiceStarted messages.
                } },
            };
            
            @switch[mrc.Type]();
        }

        /// <summary>
        /// Update the active instances list with the new time of last contact.
        /// </summary>
        /// <param name="instance"><see cref="Instance"/> information received from the client.</param>
        private void UpdateActiveInstances(Instance instance)
        {
            if (!ActiveInstances.ContainsKey(instance))
            {
                ActiveInstances.Add(instance, DateTime.Now);
            }
            else
            {
                ActiveInstances[instance] = DateTime.Now;
            }
        }

        /// <summary>
        /// Called when a connection is dropped.
        /// Any cleanup logic for a given <see cref="ConnectionState"/> should be here.
        /// </summary>
        /// <param name="state">The <see cref="ConnectionState"/> being dropped from the connection pool.</param>
        public override void OnDropConnection(ConnectionState state)
        {
            AlarmyServer.Clients.Remove(state);
            Logger.Log(LoggingLevel.Trace, "Connection dropped: {0}.", state.RemoteEndPoint.ToString());
        }
    }
}
