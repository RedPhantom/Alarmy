using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Alarmy
{
    /// <summary>
    /// Holds Alarmy message (in wrapper form) data in the queue.
    /// </summary>
    internal class MessageQueueItem
    {
        public Socket Sender { get; internal set; }
        public MessageWrapperContent Message { get; internal set; }

        public MessageQueueItem(Socket sender, MessageWrapperContent message)
        {
            Sender = sender;
            Message = message;
        }

        public override bool Equals(object obj)
        {
            MessageQueueItem _other;

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            _other = (MessageQueueItem)obj;

            return _other.Message == Message &&
                   _other.Sender.RemoteEndPoint == Sender.RemoteEndPoint;
        }

        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Holds an alarm that have been received in the past.
    /// </summary>
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

    /// <summary>
    /// Holds the client's state.
    /// </summary>
    internal static class AlarmyState
    {
        internal static List<HistoricAlarm> PastAlarms = new List<HistoricAlarm>();
        internal static List<Group> Groups = new List<Group>();
        internal static List<MessageQueueItem> MessageQueue = new();
    }
}
