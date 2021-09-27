using System;
using System.Windows.Forms;

namespace Alarmy
{
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
            // We don't check if the client is running to stop the attempting-connection logic.
            AlarmyService.Stop();

            // Hide the tray icon and stop the application.
            _trayIcon.Visible = false;
            Application.Exit();
        }

        private void OnRecentAlarms(object sender, EventArgs e)
        {
            frmPastAlarms frmPastAlarms = new frmPastAlarms();
            frmPastAlarms.Show();
        }
    }
}
