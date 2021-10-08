using AlarmyLib;
using System;
using System.Collections.Generic;

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
        private readonly ServerStartParameters _serverParameters;

        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        public AlarmyServiceProvider(ServerStartParameters parameters)
        {
            _serverParameters = parameters;
        }

        public override object Clone()
        {
            return new AlarmyServiceProvider(_serverParameters);
        }

        /// <summary>
        /// A connection has been esablished with a client.
        /// </summary>
        /// <param name="state">Client with which connection established.</param>
        public override void OnAcceptConnection(ConnectionState state)
        {
            // Add the connection to the pool.
            s_logger.Trace($"Accepted a connection from {state.RemoteEndPoint}.");

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
                    _receivedStr += MessageUtils.Encoding.GetString(buffer, 0, readBytes);
                }
                else
                {
                    // Failure to read while there is data available will cause connection termination.
                    state.EndConnection();
                }
            }

            try
            {
                // If the message doesn't contain the EOF Tag, warn about it and try to parse it.
                if (!_receivedStr.Contains(Consts.EOFTag))
                {
                    s_logger.Warn("Received a message without an EOF tag.");
                }

                // If multiple messages accumulated, parse each.
                foreach (string message in _receivedStr.Split(new string[] { Consts.EOFTag }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmedMessage = message.TrimEnd('\0');

                    if (!string.IsNullOrWhiteSpace(trimmedMessage))
                    {
                        // We don't trim the EOF tag since it's been already removed in the Split().
                        MessageWrapperContent mrc = MessageUtils.ParseMessageString(trimmedMessage, removeEofTag: false);
                        HandleMessage(mrc, state);
                    }
                }
            }
            catch (Exception ex)
            {
                s_logger.Warn(ex, $"An error occurred while processing the received message." +
                    $"\nOrigin: \n{state.RemoteEndPoint}\nRaw Content: \n{_receivedStr}");
            }

            _receivedStr = string.Empty;
        }

        /// <summary>
        /// Dispatch actions according to the message information received.
        /// </summary>
        /// <param name="mrc">Received message contents.</param>
        /// <param name="client">The client from which the message was received.</param>
        private void HandleMessage(MessageWrapperContent mrc, ConnectionState client)
        {
            s_logger.Debug($"Handling message {mrc.Repr()} from {client.Repr()}.");

            // Handle the message ("response" - a message from a client) based on its type.
            var @switch = new Dictionary<Type, Action> {
                { typeof(PingResponse), () => {
                    PingResponse pr = (PingResponse)mrc.Message;

                    // Since this is a ping response, no need to send a response,
                    // just update the client pool.

                    UpdateActiveInstances(pr.Instance);
                    _serverParameters.OnInstancesChange(this, new InstancesChangeEventArgs(pr.Instance, client));
                } },
                { typeof(ServiceStartedResponse), () => { 
                    // We don't send a response.
                    
                    ServiceStartedResponse ssr = (ServiceStartedResponse)mrc.Message;

                    UpdateActiveInstances(ssr.Instance);
                    _serverParameters.OnInstancesChange(this, new InstancesChangeEventArgs(ssr.Instance, client));
                } },
                { typeof(GroupQueryMessage), () =>
                {
                    MessageWrapper<GroupQueryResponse> gqrWrapper = new();
                    GroupQueryMessage gcm = (GroupQueryMessage)mrc.Message;
                    GroupQueryResponse gqr;

                    gqr = new GroupQueryResponse(null, ManagerState.Groups.Find(x => x.ID == gcm.GroupID));
                    gqrWrapper.Message = gqr;

                    // Send the response to the client - Group if found, null if not.
                    AlarmyServer.ClientWriteString(client,
                        MessageUtils.BuildMessageString(gqrWrapper.Serialize()));
                } }
            };

            try
            {
                @switch[mrc.Type]();
            }
            catch (KeyNotFoundException knfe)
            {
                s_logger.Warn(knfe, "Received an unexpected message type.");
            }
        }

        /// <summary>
        /// Update the active instances list with the new time of last contact.
        /// </summary>
        /// <param name="instance"><see cref="Instance"/> information received from the client.</param>
        private void UpdateActiveInstances(Instance instance)
        {
            if (instance == null)
            {
                s_logger.Error("UpdateActiveInstances Received a null instance.");
            }

            if (ManagerState.ActiveInstances.ContainsKey(instance))
            {
                lock (ManagerState.ActiveInstances)
                {
                    ManagerState.ActiveInstances[instance] = DateTime.Now;
                    s_logger.Trace("Updated an existing instance in the ActiveInstances pool.");
                }
            }
            else
            {
                lock (ManagerState.ActiveInstances)
                {
                    ManagerState.ActiveInstances.Add(instance, DateTime.Now);
                    s_logger.Trace("Added a new instance to the ActiveInstances pool.");
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
            s_logger.Info($"Dropped the connection with {state.RemoteEndPoint}.");
        }
    }
}
