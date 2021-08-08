using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static AlarmyManager.ServerStartParameters;

namespace AlarmyManager
{
    public partial class frmManager : Form
    {
        private Thread ServerThread;

        public frmManager()
        {
            InitializeComponent();
        }

        private void frmManager_Load(object sender, EventArgs e)
        {
            Text = string.Format("Alarmy Manager v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
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
            Console.Title = "Alarmy Manager";

            try
            {
                ServerLauncher ServerLauncher = new ServerLauncher(ManagerSettings.Default.ServicePort);
                ServerThread = new Thread(() => ServerLauncher.Start(new ServerStartParameters(OnInstancesUpdate)));
                ServerThread.Start();
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
            for (int i = 0; i < clbUsers.Items.Count; ++i)
            {
                clbUsers.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// Deselect all users in the user list.
        /// </summary>
        private void btnNoUsers_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbUsers.Items.Count; ++i)
            {
                clbUsers.SetItemChecked(i, false);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Construct an Alarm and its message.
            Alarm alarm = new Alarm(cbRightToLeft.Checked, tbTitle.Text, rtbContent.Text);
            ShowAlarmMessage sam = new ShowAlarmMessage(alarm);

            // Send the Alarm.
            // TODO: Implement.
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            clbUsers.Items.Clear();
            lock (ManagerState.ActiveInstances)
            {
                ManagerState.ActiveInstances.Clear();
            }

            AlarmyServer.PingClients();
        }
        
        /// <summary>
        /// Event handler for the service provider's instances update.
        /// </summary>
        private void OnInstancesUpdate(object sender, InstancesChangeEventArgs args)
        {
            UpdateInstances(args.Instance);
        }

        /// <summary>
        /// Update the users list box and the dictionary that links an instance and its last seen time to its index
        /// in the list box.
        /// </summary>
        private void UpdateInstances(Instance instance)
        {
            if (ListBox.NoMatches == clbUsers.FindStringExact(instance.ToString()))
            {
                clbUsers.Items.Add(instance);
            }
        }
    }
}
