using AlarmyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AlarmyManager
{
    public partial class frmManager : Form
    {
        private Thread ServerThread;
        private Dictionary<Instance, ConnectionState> InstanceToConnection = new Dictionary<Instance, ConnectionState>();

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
                ServerLauncher ServerLauncher = new ServerLauncher(Properties.Settings.Default.ServicePort);
                ServerThread = new Thread(() => ServerLauncher.Start(new ServerStartParameters(OnInstancesUpdate, OnServerStart)));
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
            Alarm alarm;

            try
            {
                alarm = new Alarm(cbRightToLeft.Checked, tbTitle.Text, rtbContent.Text);
                ShowAlarmMessage sam = new ShowAlarmMessage(alarm);
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show(this, ae.Message, "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            clbUsers.Enabled = false;
            // Send the Alarm.
            // TODO: Implement.
            MessageBox.Show(clbUsers.SelectedItems.Count.ToString());

            foreach (Instance instance in clbUsers.SelectedItems)
            {
                ConnectionState client = InstanceToConnection[instance];
                lblStatus.Text = string.Format("Sending alarm to {0}...", instance);
                AlarmyServer.TriggerAlarm(client, alarm);
            }
            lblStatus.Text = "Alarm deployment complete.";
            clbUsers.Enabled = true;
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
        /// <remarks>Called from the server thread.</remarks>
        private void OnInstancesUpdate(object sender, InstancesChangeEventArgs args)
        {
            UpdateInstances(args.Instance, args.Connection);
        }

        /// <summary>
        /// Event handler for the server's successful start. Used to update the status label.
        /// </summary>
        /// <remarks>Called from the server thread.</remarks>
        private void OnServerStart(object sender, EventArgs e)
        {
            lblStatus.Owner.Invoke((MethodInvoker)delegate 
            {
                lblStatus.Text = "Ready.";
            });
        }

        /// <summary>
        /// Update the users list box and the dictionary that links an instance and its last seen time to its index
        /// in the list box.
        /// </summary>
        /// <remarks>Called from the server thread.</remarks>
        private void UpdateInstances(Instance instance, ConnectionState connection)
        {
            if (ListBox.NoMatches == clbUsers.FindStringExact(instance.ToString()))
            {
                clbUsers.Invoke((MethodInvoker)delegate
                {
                    clbUsers.Items.Add(instance);
                });

                lock (InstanceToConnection)
                {
                    InstanceToConnection.Add(instance, connection);
                }
            }
        }
    }
}
