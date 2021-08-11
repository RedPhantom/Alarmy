using AlarmyLib;
using System.Diagnostics;
using System.ServiceProcess;

namespace AlarmyService
{
    static class Program
    {
        // TODO: Create a new WinForms project in the solution that will repalce AlarmyService.
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AlarmyService()
            };
            ServiceBase.Run(ServicesToRun);
        }

        public static void ShowAlarm(ShowAlarmMessage sam)
        {
            MessageWrapper<ShowAlarmMessage> samWrapper = new MessageWrapper<ShowAlarmMessage>()
            {
                Message = sam
            };
            Process.Start()
        }
    }
}
