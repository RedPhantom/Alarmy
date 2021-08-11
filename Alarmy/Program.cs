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
        private static UnifiedLogger Logger = new UnifiedLogger("AlarmyService");

        internal static void Start()
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() => {
                    ServiceProvider.StartProvider();
                }));
                t.Start();
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
            }
            catch (Exception e)
            {
                Logger.Log(LoggingLevel.Error, "Failed stopping the service provider: \n{0}.", e);
            }
        }
    }

    class AlarmyApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private const string defaultTrayIconText = "Alarmy Service";
        
        public enum TrayIconStatus
        {
            Regular,
            Error
        }

        public AlarmyApplicationContext()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.NotificationAlert_16x,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Recent Alarms", OnRecentAlarms),
                    new MenuItem("Start Service", OnStart),
                    new MenuItem("Stop Service", OnStop),
                    new MenuItem("Exit", OnExit)
                }),
                Visible = true,
                Text = defaultTrayIconText
            };

            Start();
        }

        public void SetTrayIconStatus(TrayIconStatus status, string text = null)
        {
            if (null == text)
            {
                trayIcon.Text = defaultTrayIconText;
            }
            else
            {
                trayIcon.Text = text;
            }

            if (TrayIconStatus.Error == status)
            {
                trayIcon.Icon = Properties.Resources.NotificationAlert_Error;
            }
            else
            {
                trayIcon.Icon = Properties.Resources.NotificationAlert_16x;
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
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
