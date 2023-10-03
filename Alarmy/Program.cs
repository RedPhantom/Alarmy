using AlarmyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Alarmy
{
    internal static class Program
    {
        internal static AlarmyApplicationContext Context;
        static readonly Mutex mutex = new(true, "{55469597-1821-4E49-B03E-02A34E86D86C}");

        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Context = new AlarmyApplicationContext();
                InitializeSettings();

                Application.Run(Context);

                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("An Alarmy instance is already running.\n" +
                    "Please allow ~15 seconds for a previous instance to gracefully close.\n" +
                    "Alternatively, terminate the process via TaskManager.");
            }
        }

        private static void InitializeSettings()
        {
            if (Properties.Settings.Default.NeedsUpgrading)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.NeedsUpgrading = false;
                Properties.Settings.Default.Save();
            }

            if (string.IsNullOrEmpty(Properties.Settings.Default.Groups))
            {
                AlarmyState.Groups = new() { Group.GlobalGroup };
            }
            else
            {
                AlarmyState.Groups = JsonConvert.DeserializeObject<List<Group>>(Properties.Settings.Default.Groups);
            }
        }
    }

    internal static class AlarmyService
    {
        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();
        private static Thread _serviceThread;

        // Trigger this event to cause the client thread to stop.
        private static readonly ManualResetEvent s_stopClient = new(false);

        /// <summary>
        /// Start the Alarmy service client.
        /// </summary>
        /// <param name="proxy">Instance of a proxy for IPC purposes.</param>
        internal static void Start(InternalProxy proxy)
        {
            try
            {
                Instance instance = Instance.GetInstance();
                _serviceThread = new Thread(new ThreadStart(() =>
                {
                    AsynchronousClient.StartClient(
                        Properties.Settings.Default.ServiceURL,
                        Properties.Settings.Default.ServicePort,
                        instance,
                        s_stopClient,
                        proxy
                        );
                }));
                _serviceThread.Start();
            }
            catch (Exception e)
            {
                Stop();
                MessageBox.Show($"Fatal error in server start: {e.Message}", "Alarmy Manager");
                s_logger.Error(e, "Fatal error starting the server.");
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
