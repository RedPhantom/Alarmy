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

        internal static void Start()
        {
            try
            {
                Instance instance = Instance.GetInstance();
                _serviceThread = new Thread(new ThreadStart(() => {
                    ServiceProvider.StartProvider(instance);
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
                ServiceProvider.StopProvider();
                Program.Context.SetTrayIconStatus(AlarmyApplicationContext.TrayIconStatus.Error,
                    "Alarmy Service (Stopped)");
            }
            catch (Exception e)
            {
                s_logger.Error(e, "Failed stopping the service provider.");
            }
        }
    }

    internal class AlarmyApplicationContext : ApplicationContext
    {
        public const string DefaultTrayIconText = "Alarmy Service";
        
        private readonly NotifyIcon _trayIcon;
        
        public enum TrayIconStatus
        {
            Regular,
            Error
        }

        public AlarmyApplicationContext()
        {
            _trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.Alarmy,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Recent Alarms", OnRecentAlarms),
                    new MenuItem("Start Service", OnStart),
                    new MenuItem("Stop Service", OnStop),
                    new MenuItem("Exit", OnExit)
                }),
                Visible = true,
                Text = DefaultTrayIconText
            };

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

            if (TrayIconStatus.Error == status)
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

        private void OnRecentAlarms(object sender, EventArgs e)
        {
            frmPastAlarms frmPastAlarms = new frmPastAlarms();
            frmPastAlarms.Show();
        }


        public void OnStart(object sender, EventArgs e)
        {
            Start();
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
