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

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
            Logger.Info("Starting server on port {0}.", Port);
            AlarmyServer.Start(Port, parameters);
        }
    }

    internal class ServerStartParameters
    {
        internal EventHandler<InstancesChangeEventArgs> OnInstancesChange;
        internal EventHandler<EventArgs> OnServerStart;

        public ServerStartParameters(EventHandler<InstancesChangeEventArgs> onInstancesChange,
            EventHandler<EventArgs> onServerStart)
        {
            OnInstancesChange = onInstancesChange;
            OnServerStart = onServerStart;
        }
    }

    internal class InstancesChangeEventArgs : EventArgs
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
