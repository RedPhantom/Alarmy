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
        public static Dictionary<Instance, DateTime> ActiveInstances = new Dictionary<Instance, DateTime>();
    }
}
