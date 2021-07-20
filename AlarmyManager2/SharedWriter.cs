using System;
using System.IO;

namespace AlarmyManager
{
    internal static class SharedWriter
    {
        internal static StreamWriter Writer = new StreamWriter(
            Path.Combine(
                ManagerSettings.Default.LogPath, 
                string.Format("{0}.log", DateTime.Now.ToString("dd-MM-yyyy HH-mm"))),
            append: true);
    }
}
