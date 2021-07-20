using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AlarmyService
{
    public static class EventLoggerUtils
    {
        public static string GetEventSource()
        {
            if (!EventLog.SourceExists(Consts.ServiceEventLogName))
            {
                EventLog.CreateEventSource(Consts.ServiceEventLogName, "Service Log");
            }

            return Consts.ServiceEventLogName;
        }

        public static void Log(string message, EventLogEntryType type)
        {
            using (EventLog eventLog = new EventLog())
            {
                eventLog.Source = GetEventSource();
                eventLog.WriteEntry(message, type);
            }
        }

        public static void LogError(string message, Exception e)
        {
           Log(string.Format("{0}\n {1}.\n{2}", message, e.Message, e.StackTrace), EventLogEntryType.Error);
        }
    }
}
