using AlarmyLib;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Alarmy
{
    internal static class Program
    {
        internal static AlarmyApplicationContext Context;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Context = new AlarmyApplicationContext();
            Application.Run(Context);
        }
    }

    internal static class AlarmyService
    {
        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();
        private static Thread _serviceThread;

        // Trigger this event to cause the client thread to stop.
        private static ManualResetEvent s_stopClient = new(false);

        internal static void Start()
        {
            try
            {
                Instance instance = Instance.GetInstance();
                _serviceThread = new Thread(new ThreadStart(() => {
                    AsynchronousClient.StartClient(Properties.Settings.Default.ServiceURL,
                        Properties.Settings.Default.ServicePort,
                        instance,
                        s_stopClient);
                }));
                _serviceThread.Start();
            }
            catch
            {
                Stop();
            }
        }

        internal static void Stop()
        {
            try
            {
                s_stopClient.Set();
                Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.NotRunning,
                    AlarmyApplicationContext.StoppedTrayIconText);
            }
            catch (Exception e)
            {
                s_logger.Error(e, "Failed stopping the service provider.");
            }
        }
    }

    internal class AlarmyApplicationContext : ApplicationContext
    {
        // TODO: Change to consts when .NET 5 supports const interpolated strings.
        public static readonly string DefaultTrayIconText = "Alarmy Service";
        public static readonly string StoppedTrayIconText = $"{DefaultTrayIconText} (stopped)";

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
                _trayIcon.Text = statusText;
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
            AlarmyService.Start();
        }

        public void Stop()
        {
            if (_isRunning)
            {
                AlarmyService.Stop();
            }
        }

        private void OnRecentAlarms(object sender, EventArgs e)
        {
            frmPastAlarms frmPastAlarms = new frmPastAlarms();
            frmPastAlarms.Show();
        }


        public void OnStart(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                Start();
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
            AlarmyService.Stop();

            // Hide the tray icon and stop the application.
            _trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
