using AlarmyLib;
using System;
using System.ServiceProcess;
using System.Threading;

namespace AlarmyService
{
    public partial class AlarmyService : ServiceBase
    {
        private UnifiedLogger Logger = new UnifiedLogger("AlarmyService");

        public AlarmyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() => {
                    ServiceProvider.StartProvider(this);
                }));
                t.Start();
            }
            catch
            {
                Stop();
            }
        }

        protected override void OnStop()
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
}
