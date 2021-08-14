using System;
using System.Collections.Generic;
using AlarmyLib;

namespace AlarmyManager
{
    internal static class ManagerState
    {
        /// <summary>
        /// Holds all connected clients and time of last contact.
        /// </summary>
        public static Dictionary<Instance, DateTime> s_activeInstances = new Dictionary<Instance, DateTime>();
    }
}
