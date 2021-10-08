using AlarmyLib;
using System;
using System.Collections.Generic;

namespace AlarmyManager
{
    internal static class ManagerState
    {
        /// <summary>
        /// Holds all connected clients and time of last contact.
        /// </summary>
        public static Dictionary<Instance, DateTime> ActiveInstances = new Dictionary<Instance, DateTime>();

        // TODO: add global group to this list upon server start.
        public static List<Group> Groups = new();
    }
}
