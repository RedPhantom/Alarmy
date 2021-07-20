using AlarmyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlarmyManager
{
    public partial class frmManager : Form
    {
        Thread serverThread;

        public frmManager()
        {
            InitializeComponent();
        }

        private void frmManager_Load(object sender, EventArgs e)
        {
            Text = string.Format("Manager v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            StartServer();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// Create a separate thread for the server and start it.
        /// </summary>
        private void StartServer()
        {
            AllocConsole();

            try
            {
                ServerLauncher server = new ServerLauncher(ManagerSettings.Default.ServicePort);
                serverThread = new Thread(server.Start);
                serverThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to start the server: {0}.", ex.Message));
            }
        }
        
        /// <summary>
        /// Select all users in the user list.
        /// </summary>
        private void btnAllUsers_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbUsers.Items.Count; i++)
            {
                clbUsers.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// Deselect all users in the user list.
        /// </summary>
        private void btnNoUsers_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbUsers.Items.Count; i++)
            {
                clbUsers.SetItemChecked(i, false);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Construct an Alarm.
            Alarm alarm = new Alarm(cbRightToLeft.Checked, tbTitle.Text, rtbContent.Text);

            // Send the Alarm.
            // TODO: Implement.
        }
    }
}
