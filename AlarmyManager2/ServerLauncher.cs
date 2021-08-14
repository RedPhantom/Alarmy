using System;
using AlarmyLib;

namespace AlarmyManager
{
    /// <summary>
    /// Provides an interface to launch the server.
    /// </summary>
    class ServerLauncher
    {
        public int Port { get; }

        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        public ServerLauncher(int port)
        {
            if (port < 1024)
            {
                throw new ArgumentException("Port must be >= 1024.");
            }
            Port = port;
        }

        /// <summary>
        /// Launch the server with the current configuration.
        /// </summary>
        public void Start(ServerStartParameters parameters)
        {
            s_logger.Info($"Starting server on port {Port}.");
            AlarmyServer.Start(Port, parameters);
        }
    }

    /// <summary>
    /// Information to pass to the Alarmy service provider.
    /// </summary>
    public class ServerStartParameters
    {
        public EventHandler<InstancesChangeEventArgs> OnInstancesChange;
        public EventHandler<EventArgs> OnServerStart;

        /// <summary>
        /// Create information to pass the server.
        /// </summary>
        /// <param name="onInstancesChange">Called when the instance pool changes.</param>
        /// <param name="onServerStart">Called when the server starts successfully.</param>
        public ServerStartParameters(
            EventHandler<InstancesChangeEventArgs> onInstancesChange,
            EventHandler<EventArgs> onServerStart)
        {
            OnInstancesChange = onInstancesChange;
            OnServerStart = onServerStart;
        }
    }

    /// <summary>
    /// The instance pool has changed: a client has been added/removed/pinged.
    /// </summary>
    public class InstancesChangeEventArgs : EventArgs
    {
        public Instance Instance { get; private set; }
        public ConnectionState Connection { get; private set; }

        public InstancesChangeEventArgs(Instance instance, ConnectionState connection)
        {
            Instance = instance;
            Connection = connection;
        }
    }
}
