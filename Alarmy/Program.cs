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
}
