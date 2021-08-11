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
    internal class AlarmyServiceProvider : TcpServiceProvider
    {
        private string _receivedStr;
        private UnifiedLogger Logger = new UnifiedLogger("AlarmyServiceProvider");
        private ServerStartParameters ServerParameters;

        public AlarmyServiceProvider(ServerStartParameters parameters)
        {
            Logger.EnableConsoleLogging();
            Logger.EnableEventLogLogging(EventLogger.EventLogSource.AlarmyManager);
            Logger.EnableFileLogging(SharedWriter.Writer);

            ServerParameters = parameters;
        }

        public override object Clone()
        {
            return new AlarmyServiceProvider(ServerParameters);
        }
        
        /// <summary>
        /// A connection has been esablished with a client.
        /// </summary>
        /// <param name="state">Client with which connection established.</param>
        public override void OnAcceptConnection(ConnectionState state)
        {
            // Add the connection to the pool.
            AlarmyServer.Clients.Add(state);
            Logger.Log(LoggingLevel.Trace, "Accepted a connection from {0}.", state.RemoteEndPoint);

            // Send a ping request to the new client.
            AlarmyServer.PingClient(state);
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

            try
            {
                // Raise an exception if our message doesn't contain the EOF Tag.
                if (!_receivedStr.Contains(Consts.EOFTag))
                {
                    throw new Exception("Missing EOF Tag.");
                }

                // If multiple messages accumulated, parse each.
                foreach (string message in _receivedStr.Split(new string[] { Consts.EOFTag }, StringSplitOptions.RemoveEmptyEntries))
                {
                    MessageWrapperContent mrc = ParseMessage(message, state);
                    HandleMessage(mrc, state);
                }
            }
            catch
            {
                Logger.Log(LoggingLevel.Warning,
                "Received a malformed message.\nOrigin: \n{0}\nRaw Content: \n{1}", state.RemoteEndPoint, _receivedStr);
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
            wrapper = MessageWrapper.Deserialize(msg);

            return wrapper;
        }

        private void HandleMessage(MessageWrapperContent mrc, ConnectionState connection) 
        {
            // Handle the message ("response" - a message from a client) based on its type.
            var @switch = new Dictionary<Type, Action> {
                { typeof(KeepAliveResponse), () => {
                    // TODO: Drop the use of KeepAlives. It uses too much bandwidth.

                    KeepAliveResponse kar = (KeepAliveResponse)mrc.Message;

                    // We don't send a response to KeepAlive messages.

                    // Keep track of all instances and their time of last appearance.
                    UpdateActiveInstances(kar.Instance);
                    ServerParameters.OnInstancesChange(this, new InstancesChangeEventArgs(kar.Instance, connection));
                } },
                { typeof(PingResponse), () => {
                    PingResponse pr = (PingResponse)mrc.Message;

                    // Since this is a ping response, no need to send a response,
                    // just update the client pool.

                    UpdateActiveInstances(pr.Instance);
                    ServerParameters.OnInstancesChange(this, new InstancesChangeEventArgs(pr.Instance, connection));
                } },
                { typeof(ServiceStartedResponse), () => { 
                    // We don't send a response.
                    
                    ServiceStartedResponse ssr = (ServiceStartedResponse)mrc.Message;

                    UpdateActiveInstances(ssr.Instance);
                    ServerParameters.OnInstancesChange(this, new InstancesChangeEventArgs(ssr.Instance, connection));
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
            if (instance == null)
            {
                Logger.Log(LoggingLevel.Error, "UpdateActiveInstances Received a null instance.");
            }

            if (!ManagerState.ActiveInstances.ContainsKey(instance))
            {
                lock (ManagerState.ActiveInstances)
                {
                    ManagerState.ActiveInstances.Add(instance, DateTime.Now);
                    Logger.Log(LoggingLevel.Trace, "Added a new instance to the ActiveInstances pool.");
                }
            }
            else
            {
                lock (ManagerState.ActiveInstances)
                {
                    ManagerState.ActiveInstances[instance] = DateTime.Now;
                    Logger.Log(LoggingLevel.Trace, "Updated an existing instance in the ActiveInstances pool.");
                }
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
            Logger.Log(LoggingLevel.Trace, "Dropped the connection with {0}.", state.RemoteEndPoint.ToString());
        }
    }
}
