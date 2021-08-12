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
            Application.ApplicationExit += AlarmyServer.OnApplicationExit;

            Application.Run(new frmManager());
        }
    }
}
