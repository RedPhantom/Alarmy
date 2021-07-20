using System;
using System.ServiceProcess;

namespace AlarmyService
{
    public partial class AlarmySerivce : ServiceBase
    {
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
                EventLoggerUtils.LogError(string.Format("Error client stopping: {}.\n{}", e.Message, e.StackTrace), e);
            }
        }
    }
}
