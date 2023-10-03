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
        public static Dictionary<Instance, DateTime> ActiveInstances = new();

        /// <summary>
        /// Available groups.
        /// </summary>
        /// <remarks>
        /// The global group is added to this list upon server start.</item>
        /// </remarks>
        public static List<Group> Groups { 
            get; 
            set; } = new();

        /// <summary>
        /// Dictionary of the available groups and the users that are part of these groups.
        /// </summary>
        /// <remarks>
        /// Updated by the OnInstancesChange event.
        /// </remarks>
        public static Dictionary<Group, List<Instance>> GroupedInstances = new();

        /// <summary>
        /// Get all groups an instance is a member of.
        /// </summary>
        /// <returns>List of groups, empty if none found.</returns>
        public static List<Group> GetInstanceGroups(Instance instance)
        {
            List<Group> groups = new();

            foreach (var pair in GroupedInstances)
            {
                if (pair.Value.Contains(instance))
                {
                    groups.Add(pair.Key);
                }
            }

            return groups;
        }
    }
}
