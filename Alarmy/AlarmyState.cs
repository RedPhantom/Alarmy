using AlarmyLib;
using System;
using System.Collections.Generic;

namespace Alarmy
{
    internal class HistoricAlarm
    {
        public Alarm Alarm { get; set; }
        public DateTime DateTime { get; set; }
        public AlarmType Type { get; set; }

        public HistoricAlarm(Alarm alarm, DateTime dateTime, AlarmType type)
        {
            Alarm = alarm;
            DateTime = dateTime;
            Type = type;
        }

        public override string ToString()
        {
            return Alarm.ToString();
        }
    }

    internal static class AlarmyState
    {
        internal static List<HistoricAlarm> PastAlarms = new List<HistoricAlarm>();
        internal static List<Group> Groups = new List<Group>();
        internal static List<MessageWrapperContent> MessageQueue = new();
    }
}
