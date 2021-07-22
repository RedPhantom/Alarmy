using AlarmyLib;
using System;
using System.ServiceProcess;

namespace AlarmyService
{
    public partial class AlarmySerivce : ServiceBase
    {
        private UnifiedLogger Logger = new UnifiedLogger("AlarmyService");

        public AlarmySerivce()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            { 
                ServiceProvider.StartProvider();
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
