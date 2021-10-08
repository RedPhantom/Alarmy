using System;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Alarmy
{
    internal class AlarmyApplicationContext : ApplicationContext
    {
        public const string DefaultTrayIconText = "Alarmy Service";
        // TODO: Change to const when .NET 5 supports const interpolated strings.
        public static readonly string StoppedTrayIconText = $"{DefaultTrayIconText} (stopped)";

        internal InternalProxy Proxy = new();

        private readonly NotifyIcon _trayIcon;

        // We always start the service disconnected, i.e. false.
        private bool _isRunning = false;

        public enum TrayIconStatus
        {
            Regular,
            NotRunning
        }

        public AlarmyApplicationContext()
        {
            _trayIcon = new()
            {
                Icon = Properties.Resources.Alarmy,
                ContextMenuStrip = new(),
                Visible = true,
                Text = DefaultTrayIconText
            };

            _trayIcon.ContextMenuStrip.Items.Add("Recent Alarms", Properties.Resources.History_64px, OnRecentAlarms);
            _trayIcon.ContextMenuStrip.Items.Add("Start Service", Properties.Resources.StatusRun_64px, OnStart);
            _trayIcon.ContextMenuStrip.Items.Add("Stop Service", Properties.Resources.StatusStop_64px, OnStop);
            _trayIcon.ContextMenuStrip.Items.Add("Settings", Properties.Resources.Settings_64px, OnSettings);
            _trayIcon.ContextMenuStrip.Items.Add("Exit", Properties.Resources.Exit_64px, OnExit);

            Start();
        }

        public void SetTrayIconStatus(TrayIconStatus status, string statusText = null)
        {
            if (null == statusText)
            {
                _trayIcon.Text = DefaultTrayIconText;
            }
            else
            {
                if (statusText.Length >= 64)
                {
                    MessageBox.Show(statusText, "Alarmy");
                    _trayIcon.Text = StoppedTrayIconText;
                }
                else
                { 
                    _trayIcon.Text = statusText;
                }
            }

            if (TrayIconStatus.NotRunning == status)
            {
                _trayIcon.Icon = Properties.Resources.Alarmy_Red;
            }
            else
            {
                _trayIcon.Icon = Properties.Resources.Alarmy;
            }
        }

        public void Start()
        {
            AlarmyService.Start(Proxy);
        }

        public void Stop()
        {
            if (_isRunning)
            {
                AlarmyService.Stop();
            }
        }

        public void OnStart(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                Start();
            }
            else
            {
                MessageBox.Show("Service already running.", "Alarmy");
            }
        }

        public void OnStop(object sender, EventArgs e)
        {
            // Stop the service.
            AlarmyService.Stop();
        }

        public void OnExit(object sender, EventArgs e)
        {
            // Stop the service.
            // Notice wee don't check if the client is running
            // to stop even during attempting-to-connect logic.
            AlarmyService.Stop();

            // Hide the tray icon and stop the application.
            _trayIcon.Visible = false;
            Application.Exit();
        }

        private void OnRecentAlarms(object sender, EventArgs e)
        {
            frmPastAlarms frmPastAlarms = new frmPastAlarms();
            frmPastAlarms.ShowDialog();
        }


        private void OnSettings(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
        }
    }

    public class InternalProxy
    {
        // Run by AsynchronousClient, serves other parts in the application that require
        // access to the data from the Alarmy client-server communication.
        internal TcpListener ServerToInternalListener;
        internal Socket ServerToInternal;
        internal Socket InternalToServer;

        // Signal the AsynchronousClient data from the application is ready for sending to the server.
        internal ManualResetEvent SignalServer = new(false);

        // Signal the application data from the server is ready.
        internal ManualResetEvent SignalClient = new(false);

        public InternalProxy()
        {
            // Create the local server. Port 0 means the next free port.
            ServerToInternalListener = new(System.Net.IPAddress.Loopback, port: 0);
            ServerToInternalListener.Start();

            // Create the client. Will be connected to the server.
            InternalToServer = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            InternalToServer.Connect(ServerToInternalListener.LocalEndpoint);

            ServerToInternal = ServerToInternalListener.AcceptSocket();
        }
    }
}
