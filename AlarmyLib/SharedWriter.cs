using System;
using System.IO;

namespace AlarmyManager
{
    /// <summary>
    /// Shared log writer. To be used in projects that require logging to a file.
    /// </summary>
    public static class SharedWriter
    {
        public static StreamWriter Writer { get; private set; }

        public static void InitSharedWriter(string path)
        {
            Writer = new StreamWriter(
            Path.Combine(
                path,
                string.Format("{0}.log", DateTime.Now.ToString("dd-MM-yyyy HH-mm"))),
            append: true);
        }
    }
}
