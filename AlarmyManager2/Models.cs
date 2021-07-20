using AlarmyLib;
using System;

namespace AlarmyManager
{
    internal class InstanceItem
    {
        public Instance Instance;
        public DateTime LastReceived;

        public InstanceItem(Instance instance, DateTime lastReceived)
        {
            Instance = instance;
            LastReceived = lastReceived;
        }
    }
}
