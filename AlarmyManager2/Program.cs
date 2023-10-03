using AlarmyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AlarmyManager
{
    static class Program
    {
        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += AlarmyServer.OnApplicationExit;

            try
            {
                InitializeSettings();
                Application.Run(new frmManager());
            }
            catch (Exception e)
            {
                s_logger.Fatal(e, "Unrecoverable error in AlarmyManager.");
                MessageBox.Show($"Unrecoverable error in AlarmyManager. Quitting\n{e}", "Alarmy Manager");
            }
        }

        private static void InitializeSettings()
        {
            // Update the Settings from a previous version.
            if (Properties.Settings.Default.NeedsUpgrading)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.NeedsUpgrading = false;
                Properties.Settings.Default.Save();
            }

            // Initialize Groups.
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Groups))
            {
                ManagerState.Groups = JsonConvert.DeserializeObject<List<Group>>(Properties.Settings.Default.Groups);
                
                // Only add the default group if it doesn't already exist.
                if (!ManagerState.Groups.Contains(Group.CreateEmpty()))
                {
                    ManagerState.Groups.Add(Group.CreateEmpty());
                }
            }
        }
    }
}
