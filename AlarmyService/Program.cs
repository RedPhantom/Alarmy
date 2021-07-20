using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AlarmyService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AlarmySerivce()
            };
            ServiceBase.Run(ServicesToRun);
        }

        public static void ShowAlarm(Alarm alarm)
        {
            Alarmy.frmAlarm frmAlarm = new Alarmy.frmAlarm();
            frmAlarm.LoadAlarm(alarm);
            frmAlarm.Show();
        }
    }
}
