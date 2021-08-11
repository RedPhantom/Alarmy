using System;
using System.Windows.Forms;

namespace AlarmyManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SharedWriter.InitSharedWriter(Properties.Settings.Default.LogPath);
            Application.ApplicationExit += AlarmyServer.OnApplicationExit;

            Application.Run(new frmManager());
        }
    }
}
