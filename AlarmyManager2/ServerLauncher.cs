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

        private UnifiedLogger Logger = new UnifiedLogger("ServerLauncher");

        public ServerLauncher(int port)
        {
            Logger.EnableConsoleLogging();
            Logger.EnableEventLogLogging(EventLogger.EventLogSource.AlarmyManager);
            Logger.EnableFileLogging(SharedWriter.Writer);

            if (port < 1024)
            {
                throw new ArgumentException("Port must be >= 1024.");
            }
            Port = port;
        }

        /// <summary>
        /// Launch the server with the current configuration.
        /// </summary>
        public void Start()
        {
            Logger.Log(LoggingLevel.Information, "Starting server on port {0}.", Port);
            AlarmyServer.Start(Port);
        }
    }
}
