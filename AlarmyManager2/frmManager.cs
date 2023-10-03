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
        private readonly Dictionary<Instance, ConnectionState> InstanceToConnection = new();

        // Last Seen data grid view
        private const string ColumnNameInstance = "colInstance";
        private const string ColumnHeaderInstance = "Instance";
        
        private const string ColumnNameLastSeen = "colLastSeen";
        private const string ColumnHeaderLastSeen = "Last Seen";

        private readonly System.Drawing.Color LastSeenLongAgoBackground = System.Drawing.Color.FromArgb(220, 255, 255);
        private readonly TimeSpan LastSeenLongAgo = new TimeSpan(hours: 1, minutes: 0, seconds: 0);

        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

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
            Text = $"Alarmy Manager v{Assembly.GetExecutingAssembly().GetName().Version}";

            // Start the server.
            StartServer();

            // Add columns to the Last Seen data grid view.
            dgvLastSeen.Columns.Add(ColumnNameInstance, ColumnHeaderInstance);
            dgvLastSeen.Columns.Add(ColumnNameLastSeen, ColumnHeaderLastSeen);

            // Set the alarm types.
            cbAlarmType.DataSource = Enum.GetValues<AlarmType>();
            cbAlarmType.SelectedItem = (AlarmType)Properties.Settings.Default.AlarmType;
            cbAlarmType.SelectedIndexChanged += cbAlarmType_SelectedIndexChanged;
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
                tsmiToggleServer.Enabled = false;
                ServerLauncher ServerLauncher = new ServerLauncher(Properties.Settings.Default.ServicePort);
                ServerThread = new Thread(() => ServerLauncher.Start(new(OnInstancesUpdate, OnServerStart)));
                ServerThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start the server: {ex}.");
                s_logger.Fatal(ex, "Server start failed.");
            }
        }

        private void StopServer()
        {
            try
            {
                tsmiToggleServer.Enabled = false;
                AlarmyServer.Stop();

                // TODO: make sure stopping and starting the server over and over
                // again doesn't create zombie threads. It shouldn't, since
                // we're re-setting the ServerThread variable each time.
                //ServerThread.Abort();
                tsmiToggleServer.Text = "Start Server";
                tsmiToggleServer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to stop the server: {ex.Message}.");
                s_logger.Error(ex, "Server stop failed.");
            }

            UpdateServerStatus();
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
            UpdateRecipientCount();
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
            UpdateRecipientCount();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Alarm alarm;

            tbUid.Text = Alarm.GenerateID();

            if (0 == clbUsers.CheckedItems.Count)
            {
                lblStatus.Text = "No users are selected.";
                s_logger.Info("Cannot deploy alarm: no users selected.");
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
                s_logger.Warn(ae, "Value error.");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                s_logger.Error(ex, "Unexpected error.");
                return;
            }

            // Construct the message for the alarm and send it.
            TriggerAlarmForClients(alarm);

            // Save the alarm so it's accessible for authenticity validation.
            CreateAlarmFile(alarm);
        }

        private void CreateAlarmFile(Alarm alarm)
        {
            // TODO: implement.
        }

        private void TriggerAlarmForClients(Alarm alarm)
        {
            clbUsers.Enabled = false;

            List<Instance> instancesToRemove = new();
            CheckedListBox.CheckedItemCollection targetedInstances = clbUsers.CheckedItems;

            foreach (Instance instance in targetedInstances)
            {
                ConnectionState client = InstanceToConnection[instance];

                lblStatus.Text = $"Sending alarm to {instance}...";

                try
                {
                    AlarmyServer.SendAlarmToClient(client, alarm, (AlarmType)cbAlarmType.SelectedItem);
                }
                catch (ObjectDisposedException)
                {
                    // This exception can occur if the client list has not been refreshed
                    // since a client disconnected.
                    instancesToRemove.Add(instance);
                }
            }

            lblStatus.Text = "Alarm deployment complete.";

            // Remove all irrelevant, disconnected instances.
            RemoveInstances(instancesToRemove);
            
            if (instancesToRemove.Count > 0)
            {
                lblStatus.Text += $" Dropped {instancesToRemove.Count} inactive clients.";
            }
            
            s_logger.Info(lblStatus.Text);
            clbUsers.Enabled = true;
        }

        /// <summary>
        /// Remove instances from UI and <see cref="ManagerState"/> elements.
        /// </summary>
        /// <param name="instancesToRemove">List of instances to remove.</param>
        private void RemoveInstances(List<Instance> instancesToRemove)
        {
            foreach (Instance instance in instancesToRemove)
            {
                List<Group> instanceGroups = ManagerState.GetInstanceGroups(instance);

                // Remove from the instance-to-connection dictionary.
                InstanceToConnection.Remove(instance);

                // Remove from the client listbox.
                clbUsers.Items.Remove(instance);

                // Remove from the group treeview.
                foreach (Group group in instanceGroups)
                {
                    // TODO: check if 'Remove from the group treeview.' works properly.
                    tvGroups.Nodes[group.ID.ToString()].Nodes[instance.ToString()].Remove();
                }

                // Remove from the ActiveInstances list.
                lock (ManagerState.ActiveInstances)
                {
                    ManagerState.ActiveInstances.Remove(instance);
                }

                // Remove from the group-to-instances dictionary.
                lock (ManagerState.GroupedInstances)
                {
                    foreach (Group group in instanceGroups)
                    {
                        ManagerState.GroupedInstances[group].Remove(instance);
                    }
                }
            }
        }

        private Alarm GetAlarmFromForm()
        {
            return (AlarmType)cbAlarmType.SelectedItem switch
            {
                AlarmType.RTF => new Alarm(tbUid.Text, cbRightToLeft.Checked, tbTitle.Text, rtbContent.Rtf),
                AlarmType.TextOnly => new Alarm(tbUid.Text, cbRightToLeft.Checked, tbTitle.Text, rtbContent.Text),
                _ => throw new Exception($"Invalid alarm type {cbAlarmType.SelectedItem}"),
            };
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            clbUsers.Items.Clear();
            tvGroups.Nodes.Clear();
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
            // Update InstanceToConnection.
            UpdateInstances(args.Instance, args.Connection);

            // Update the UI:
            // Users CheckedListBox
            if (ListBox.NoMatches == clbUsers.FindStringExact(args.Instance.ToString()))
            {
                clbUsers.Invoke((MethodInvoker)delegate
                {
                    clbUsers.Items.Add(args.Instance);
                });

                UpdateRecipientCount();
            }

            // Group TreeView
            UpdateGroupTreeView(args.Instance);
        }

        /// <summary>
        /// Update the available groups tree view.
        /// </summary>
        /// <param name="instance">A newly connected instance.</param>
        /// <remarks>This method is called from the server thread.</remarks>
        private void UpdateGroupTreeView(Instance instance)
        {
            tvGroups.Invoke((MethodInvoker)delegate 
            {
                foreach (Group group in ManagerState.GetInstanceGroups(instance))
                {
                    if (!tvGroups.Nodes.ContainsKey(group.ID.ToString()))
                    {
                        // Add the missing group node.
                        tvGroups.Nodes.Add(group.ID.ToString(), group.Name);
                    }

                    // Add the instance if it isn't already there.
                    TreeNode groupNode = tvGroups.Nodes[group.ID.ToString()];

                    if (!groupNode.Nodes.ContainsKey(instance.ToString()))
                    {
                        groupNode.Nodes.Add(instance.ToString(), instance.ToString());
                    }
                }
            });
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

                tsmiToggleServer.Text = "Stop Server";
                tsmiToggleServer.Enabled = true;

                UpdateServerStatus();
            });
        }

        /// <summary>
        /// Update the dictionary that links an instance and its last seen time to its index
        /// in the list box.
        /// </summary>
        /// <remarks>Called from the server thread.</remarks>
        private void UpdateInstances(Instance instance, ConnectionState connection)
        {
            if (InstanceToConnection.ContainsKey(instance))
            {
                // TODO: The comparison InstanceToConnection[instance].RemoteEndPoint != connection.RemoteEndPoint
                // throws an object disposed exception.
                if (InstanceToConnection[instance].Connected &&
                    InstanceToConnection[instance].RemoteEndPoint != connection.RemoteEndPoint)
                {
                    // Update the Instance's connection.
                    UpdateInstanceToConnection(instance, connection);
                }
            }
            else
            {
                // Add the instance's connection.
                UpdateInstanceToConnection(instance, connection);
            }
        }

        /// <summary>
        /// Add or update an <see cref="Instance"/>'s <see cref="ConnectionState"/>.
        /// </summary>
        private void UpdateInstanceToConnection(Instance instance, ConnectionState connection)
        {
            if (InstanceToConnection.ContainsKey(instance))
            {
                lock (InstanceToConnection)
                {
                    InstanceToConnection[instance] = connection;
                }
            }
            else
            {
                lock (InstanceToConnection)
                {
                    InstanceToConnection.Add(instance, connection);
                }
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
                DateTime lastSeen = ManagerState.ActiveInstances[instance];
                string humanizedLastSeen = Humanizer.TimeSpanHumanizeExtensions.Humanize(DateTime.Now -
                    lastSeen, precision: 2);
                bool foundCell = false;

                foreach (DataGridViewRow row in dgvLastSeen.Rows)
                {
                    if (row.Cells[ColumnNameInstance].Value == instance)
                    {
                        row.Cells[ColumnNameLastSeen].Value = humanizedLastSeen;

                        // Update the background of the cell if last seen time is a long time ago.
                        if ((DateTime.Now - lastSeen) > LastSeenLongAgo)
                        {
                            row.Cells[ColumnNameLastSeen].Style.BackColor = LastSeenLongAgoBackground;
                        }

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
            // Selection of dgvLastSeen
            if (tcDetails.SelectedTab == tpUsersLastSeen)
            {
                ApplyLastSeenSelection();
            }

            // Selection of tvGroups
            if (tcDetails.SelectedTab == tpGroups)
            {
                ApplyGroupSelection();
            }
        }

        private void ApplyLastSeenSelection()
        {
            List<DataGridViewRow> SelectedRows = new();

            // Transfer the selected cells to list of unique rows.
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
                AddRecipient(instance);
            }
        }

        private void ApplyGroupSelection()
        {
            // Check if the selected node is a group ID.
            // TODO: is there a better way to do that?
            Group selectedGroup = ManagerState.Groups.Find(x => x.ID.ToString() == tvGroups.SelectedNode.Name);

            if (selectedGroup is null)
            {
                return;
            }

            // Apply selection of all instances in that group.
            foreach (Instance instance in ManagerState.GroupedInstances[selectedGroup])
            {
                AddRecipient(instance);
            }
        }

        /// <summary>
        /// Check an instance for alarm deployment in clbUsers.
        /// </summary>
        private void AddRecipient(Instance instance)
        {
            if (clbUsers.Items.Contains(instance))
            {
                clbUsers.SetItemChecked(clbUsers.Items.IndexOf(instance), true);
                UpdateRecipientCount();
            }
            else
            {
                lblStatus.Text = $"Failed to find {instance} in the recipient list.";
                s_logger.Warn($"Could not find {instance} in the recipient list.");
            }
        }

        /// <summary>
        /// Updates the recipient count above the recipient list which displayes selected and total
        /// recipient count.
        /// </summary>
        private void UpdateRecipientCount()
        {
            lblReceipientCount.Invoke((MethodInvoker)delegate
            {
                lblReceipientCount.Text = $"({clbUsers.CheckedItems.Count}/{clbUsers.Items.Count})";
            });
        }

        #region HelpHints
        private void btnTransferSelection_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Apply the selected users/groups here to the recipient list.";
        }

        private void btnStopServer_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Gracefully stop or start the server.";
        }

        private void cbRightToLeft_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Mark the message to be displayed from right to left.";
        }

        private void btnSend_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Text = "Send the message to all selected recipients.";
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
        #endregion

        private void UpdateServerStatus()
        {
            if (AlarmyServer.IsRunning)
            {
                lblServerStatus.Text = "Running";
                lblServerStatus.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            }
            else
            {
                lblServerStatus.Text = "Stopped";
                lblServerStatus.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            }
        }

        private void cbAlarmType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAlarmType.SelectedItem is not null)
            {
                Properties.Settings.Default.AlarmType = (int)cbAlarmType.SelectedItem;
                Properties.Settings.Default.Save();
            }
        }

        private void tsmiGroupManager_Click(object sender, EventArgs e)
        {
            frmGroups frmGroups = new();
            frmGroups.ShowDialog();
            frmGroups.Dispose();
        }

        private void tsmiToggleServer_Click(object sender, EventArgs e)
        {
            if (AlarmyServer.IsRunning)
            {
                lblStatus.Text = "Stopping server...";
                s_logger.Info("Stopping server.");
                StopServer();
                lblStatus.Text = "Server stopped.";
                s_logger.Info("Server stopped.");
            }
            else
            {
                lblStatus.Text = "Starting server...";
                s_logger.Info("Starting server.");
                StartServer();
                // The server status and application status will change when the server starts up successfully.
            }
        }

        private void tsmiRightToLeft_Click(object sender, EventArgs e)
        {
            cbRightToLeft.Checked = true;
        }

        private void tsmiLeftToRight_Click(object sender, EventArgs e)
        {
            cbRightToLeft.Checked = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AlarmyServer.IsRunning)
            {
                StopServer();
            }

            Application.Exit();
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            frmSettings frmSettings = new();
            frmSettings.ShowDialog();
            frmSettings.Dispose();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            frmAbout frmAbout = new();
            frmAbout.ShowDialog();
            frmAbout.Dispose();
        }

        private void clbUsers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateRecipientCount();
        }

        private void clbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRecipientCount();
        }

        private void clbUsers_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateRecipientCount();
        }

        private void clbUsers_Click(object sender, EventArgs e)
        {
            UpdateRecipientCount();
        }
    }
}
