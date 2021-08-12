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
        private readonly Dictionary<Instance, ConnectionState> InstanceToConnection = 
            new Dictionary<Instance, ConnectionState>();

        public frmManager()
        {
            InitializeComponent();
        }

        private void frmManager_Load(object sender, EventArgs e)
        {
            // Allocate and set up console.
            AllocConsole();
            Console.Title = "Alarmy Manager";

            // Set the windoe title.
            Text = string.Format("Alarmy Manager v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            
            // Start the server.
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
            lblStatus.Text = "Waiting for server start...";

            try
            {
                btnStopServer.Enabled = false;
                ServerLauncher ServerLauncher = new ServerLauncher(Properties.Settings.Default.ServicePort);
                ServerThread = new Thread(() => ServerLauncher.Start(new ServerStartParameters(OnInstancesUpdate, OnServerStart)));
                ServerThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to start the server: {0}.", ex.Message));
            }
        }
        
        private void StopServer()
        {
            try
            {
                btnStopServer.Enabled = false;
                AlarmyServer.Stop();

                // TODO: make sure stopping and starting the server over and over
                // again doesn't create zombie threads. It shouldn't, since
                // we're re-setting the ServerThread variable each time.
                ServerThread.Abort();
                btnStopServer.Text = "Start Server";
                btnStopServer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to stop the server: {0}.", ex.Message));
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
            Alarm alarm;
            
            if (0 == clbUsers.CheckedItems.Count)
            {
                lblStatus.Text = "No users are selected.";
                return;
            }

            try
            {
                alarm = GetAlarmFromForm();
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

            // Construct the message for the alarm and send it.
            TriggerAlarmForClients(alarm);
        }

        private void TriggerAlarmForClients(Alarm alarm)
        {
            clbUsers.Enabled = false;
            foreach (Instance instance in clbUsers.CheckedItems)
            {
                ConnectionState client = InstanceToConnection[instance];
                lblStatus.Text = string.Format("Sending alarm to {0}...", instance);
                AlarmyServer.TriggerAlarm(client, alarm);
            }
            lblStatus.Text = "Alarm deployment complete.";
            clbUsers.Enabled = true;
        }

        private Alarm GetAlarmFromForm()
        {
            return new Alarm(cbRightToLeft.Checked, tbTitle.Text, rtbContent.Rtf);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            clbUsers.Items.Clear();
            dgvLastSeen.Rows.Clear();

            lock (ManagerState.ActiveInstances)
            {
                ManagerState.ActiveInstances.Clear();
            }

            AlarmyServer.PingClients();
            UpdateLastSeen();
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

                btnStopServer.Text = "Stop Server";
                btnStopServer.Enabled = true;
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

        private void cbRightToLeft_CheckedChanged(object sender, EventArgs e)
        {
            rtbContent.RightToLeft = cbRightToLeft.Checked ? RightToLeft.Yes : RightToLeft.Inherit;
            tbTitle.RightToLeft = cbRightToLeft.Checked ? RightToLeft.Yes : RightToLeft.Inherit;
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (AlarmyServer.InternalServer.IsRunning)
            {
                lblStatus.Text = "Stopping server...";
                StopServer();
                lblStatus.Text = "Server stopped.";
            }
            else
            {
                lblStatus.Text = "Stopping server...";
                StartServer();
                lblStatus.Text = "Server stopped.";
            }
        }

        private void tmrLastSeen_Tick(object sender, EventArgs e)
        {
            UpdateLastSeen();
        }

        private void UpdateLastSeen()
        {
            foreach (Instance instance in ManagerState.ActiveInstances.Keys)
            {
                // Update the cell that holds the Last Seen time, or add the entire row if needed.
                string humanizedLastSeen = Humanizer.TimeSpanHumanizeExtensions.Humanize(DateTime.Now - ManagerState.ActiveInstances[instance], precision: 2);
                bool foundCell = false;

                foreach (DataGridViewRow row in dgvLastSeen.Rows)
                {
                    if (row.Cells[0].Value == instance)
                    {
                        row.Cells[1].Value = humanizedLastSeen;
                        foundCell = true;
                    }
                }

                if (!foundCell)
                {
                    dgvLastSeen.Rows.Add(instance, humanizedLastSeen);
                }
            }
        }

        private void btnTransferSelection_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> SelectedRows = new List<DataGridViewRow>();
            
            foreach (DataGridViewCell cell in dgvLastSeen.SelectedCells)
            {
                if (!SelectedRows.Contains(cell.OwningRow))
                {
                    SelectedRows.Add(cell.OwningRow);
                }
            }

            foreach (DataGridViewRow row in SelectedRows)
            {
                Instance instance = (Instance)row.Cells[0].Value;

                if (clbUsers.Items.Contains(instance))
                {
                    clbUsers.SetItemChecked(clbUsers.Items.IndexOf(instance), true);
                }
                else
                {
                    lblStatus.Text = "Failed to find " + instance + " in the users list.";
                }
            }
        }

        private void btnTransferSelection_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Apply the selected users here to the users list.";
        }

        private void btnStopServer_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Disconnect all clients and stop the server.";
        }

        private void cbRightToLeft_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Mark the message to be displayed from right to left.";
        }

        private void btnSend_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Send the message to all selected users.";
        }

        private void btnRefresh_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Refresh the user list by pinging all connected clients.";
        }

        private void btnNoUsers_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Deselect all users.";
        }

        private void btnAllUsers_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Select all users.";
        }
    }
}
