
namespace AlarmyManager
{
    partial class frmManager
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManager));
            this.btnAllUsers = new System.Windows.Forms.Button();
            this.btnNoUsers = new System.Windows.Forms.Button();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cbRightToLeft = new System.Windows.Forms.CheckBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblServerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHelp = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrLastSeen = new System.Windows.Forms.Timer(this.components);
            this.btnTransferSelection = new System.Windows.Forms.Button();
            this.tbUid = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tcDetails = new System.Windows.Forms.TabControl();
            this.tpUsersLastSeen = new System.Windows.Forms.TabPage();
            this.dgvLastSeen = new System.Windows.Forms.DataGridView();
            this.tpGroups = new System.Windows.Forms.TabPage();
            this.tvGroups = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.clbUsers = new System.Windows.Forms.CheckedListBox();
            this.lblReceipientCount = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.cbAlarmType = new System.Windows.Forms.ComboBox();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiToggleServer = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deploymentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alarmDirectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRightToLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLeftToRight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGroupManager = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usageGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tcDetails.SuspendLayout();
            this.tpUsersLastSeen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLastSeen)).BeginInit();
            this.tpGroups.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.msMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAllUsers
            // 
            this.btnAllUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAllUsers.Location = new System.Drawing.Point(3, 618);
            this.btnAllUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAllUsers.Name = "btnAllUsers";
            this.btnAllUsers.Size = new System.Drawing.Size(94, 39);
            this.btnAllUsers.TabIndex = 1;
            this.btnAllUsers.Text = "All";
            this.btnAllUsers.UseVisualStyleBackColor = true;
            this.btnAllUsers.Click += new System.EventHandler(this.btnAllUsers_Click);
            this.btnAllUsers.MouseHover += new System.EventHandler(this.btnAllUsers_MouseHover);
            // 
            // btnNoUsers
            // 
            this.btnNoUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNoUsers.Location = new System.Drawing.Point(103, 618);
            this.btnNoUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNoUsers.Name = "btnNoUsers";
            this.btnNoUsers.Size = new System.Drawing.Size(95, 39);
            this.btnNoUsers.TabIndex = 2;
            this.btnNoUsers.Text = "None";
            this.btnNoUsers.UseVisualStyleBackColor = true;
            this.btnNoUsers.Click += new System.EventHandler(this.btnNoUsers_Click);
            this.btnNoUsers.MouseHover += new System.EventHandler(this.btnNoUsers_MouseHover);
            // 
            // tbTitle
            // 
            this.tbTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTitle.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbTitle.Location = new System.Drawing.Point(83, 36);
            this.tbTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(465, 31);
            this.tbTitle.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Recipients";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Alarm ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Content";
            // 
            // rtbContent
            // 
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContent.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbContent.Location = new System.Drawing.Point(83, 81);
            this.rtbContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(465, 529);
            this.rtbContent.TabIndex = 5;
            this.rtbContent.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(83, 614);
            this.btnSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(115, 40);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.btnSend.MouseHover += new System.EventHandler(this.btnSend_MouseHover);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRefresh.Location = new System.Drawing.Point(204, 618);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 39);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.MouseHover += new System.EventHandler(this.btnRefresh_MouseHover);
            // 
            // cbRightToLeft
            // 
            this.cbRightToLeft.Enabled = false;
            this.cbRightToLeft.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbRightToLeft.Location = new System.Drawing.Point(3, 2);
            this.cbRightToLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbRightToLeft.Name = "cbRightToLeft";
            this.cbRightToLeft.Size = new System.Drawing.Size(158, 40);
            this.cbRightToLeft.TabIndex = 8;
            this.cbRightToLeft.Text = "Right to Left mode";
            this.cbRightToLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbRightToLeft.UseVisualStyleBackColor = true;
            this.cbRightToLeft.MouseHover += new System.EventHandler(this.cbRightToLeft_MouseHover);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblServerStatus,
            this.lblStatus,
            this.lblHelp});
            this.statusStrip.Location = new System.Drawing.Point(0, 746);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1239, 26);
            this.statusStrip.TabIndex = 9;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblServerStatus.Size = new System.Drawing.Size(80, 20);
            this.lblServerStatus.Text = "Starting...";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.White;
            this.lblStatus.Image = global::AlarmyManager.Properties.Resources.StatusInformation_16x;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(193, 20);
            this.lblStatus.Text = "Waiting for server start...";
            // 
            // lblHelp
            // 
            this.lblHelp.Image = global::AlarmyManager.Properties.Resources.StatusHelp_16x;
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblHelp.Size = new System.Drawing.Size(299, 20);
            this.lblHelp.Text = "Hover over an element to learn about it.";
            // 
            // tmrLastSeen
            // 
            this.tmrLastSeen.Enabled = true;
            this.tmrLastSeen.Interval = 3000;
            this.tmrLastSeen.Tick += new System.EventHandler(this.tmrLastSeen_Tick);
            // 
            // btnTransferSelection
            // 
            this.btnTransferSelection.Location = new System.Drawing.Point(3, 616);
            this.btnTransferSelection.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTransferSelection.Name = "btnTransferSelection";
            this.btnTransferSelection.Size = new System.Drawing.Size(144, 40);
            this.btnTransferSelection.TabIndex = 13;
            this.btnTransferSelection.Text = "Target Selected";
            this.btnTransferSelection.UseVisualStyleBackColor = true;
            this.btnTransferSelection.Click += new System.EventHandler(this.btnTransferSelection_Click);
            this.btnTransferSelection.MouseHover += new System.EventHandler(this.btnTransferSelection_MouseHover);
            // 
            // tbUid
            // 
            this.tbUid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbUid.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbUid.Location = new System.Drawing.Point(83, 2);
            this.tbUid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbUid.Name = "tbUid";
            this.tbUid.ReadOnly = true;
            this.tbUid.Size = new System.Drawing.Size(465, 23);
            this.tbUid.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Title";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1239, 718);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tcDetails, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnTransferSelection, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(869, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(367, 660);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tcDetails
            // 
            this.tcDetails.Controls.Add(this.tpUsersLastSeen);
            this.tcDetails.Controls.Add(this.tpGroups);
            this.tcDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDetails.Location = new System.Drawing.Point(3, 3);
            this.tcDetails.Name = "tcDetails";
            this.tcDetails.SelectedIndex = 0;
            this.tcDetails.Size = new System.Drawing.Size(361, 608);
            this.tcDetails.TabIndex = 15;
            // 
            // tpUsersLastSeen
            // 
            this.tpUsersLastSeen.Controls.Add(this.dgvLastSeen);
            this.tpUsersLastSeen.Location = new System.Drawing.Point(4, 29);
            this.tpUsersLastSeen.Name = "tpUsersLastSeen";
            this.tpUsersLastSeen.Padding = new System.Windows.Forms.Padding(3);
            this.tpUsersLastSeen.Size = new System.Drawing.Size(353, 575);
            this.tpUsersLastSeen.TabIndex = 0;
            this.tpUsersLastSeen.Text = "Users";
            this.tpUsersLastSeen.UseVisualStyleBackColor = true;
            // 
            // dgvLastSeen
            // 
            this.dgvLastSeen.AllowUserToAddRows = false;
            this.dgvLastSeen.AllowUserToDeleteRows = false;
            this.dgvLastSeen.AllowUserToOrderColumns = true;
            this.dgvLastSeen.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLastSeen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLastSeen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLastSeen.Location = new System.Drawing.Point(3, 3);
            this.dgvLastSeen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvLastSeen.Name = "dgvLastSeen";
            this.dgvLastSeen.ReadOnly = true;
            this.dgvLastSeen.RowHeadersWidth = 51;
            this.dgvLastSeen.RowTemplate.Height = 24;
            this.dgvLastSeen.Size = new System.Drawing.Size(347, 569);
            this.dgvLastSeen.TabIndex = 13;
            // 
            // tpGroups
            // 
            this.tpGroups.Controls.Add(this.tvGroups);
            this.tpGroups.Location = new System.Drawing.Point(4, 29);
            this.tpGroups.Name = "tpGroups";
            this.tpGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tpGroups.Size = new System.Drawing.Size(353, 575);
            this.tpGroups.TabIndex = 1;
            this.tpGroups.Text = "Groups";
            this.tpGroups.UseVisualStyleBackColor = true;
            // 
            // tvGroups
            // 
            this.tvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvGroups.Location = new System.Drawing.Point(3, 3);
            this.tvGroups.Name = "tvGroups";
            this.tvGroups.Size = new System.Drawing.Size(347, 569);
            this.tvGroups.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tbUid, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSend, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.tbTitle, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.rtbContent, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(312, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(551, 660);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel4.Controls.Add(this.btnAllUsers, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnNoUsers, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnRefresh, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.clbUsers, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblReceipientCount, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 4);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(303, 660);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // clbUsers
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.clbUsers, 3);
            this.clbUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbUsers.FormattingEnabled = true;
            this.clbUsers.Location = new System.Drawing.Point(3, 27);
            this.clbUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbUsers.Name = "clbUsers";
            this.clbUsers.Size = new System.Drawing.Size(297, 587);
            this.clbUsers.TabIndex = 8;
            this.clbUsers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbUsers_ItemCheck);
            this.clbUsers.Click += new System.EventHandler(this.clbUsers_Click);
            this.clbUsers.SelectedIndexChanged += new System.EventHandler(this.clbUsers_SelectedIndexChanged);
            this.clbUsers.SelectedValueChanged += new System.EventHandler(this.clbUsers_SelectedValueChanged);
            // 
            // lblReceipientCount
            // 
            this.lblReceipientCount.AutoSize = true;
            this.lblReceipientCount.Location = new System.Drawing.Point(103, 0);
            this.lblReceipientCount.Name = "lblReceipientCount";
            this.lblReceipientCount.Size = new System.Drawing.Size(27, 20);
            this.lblReceipientCount.TabIndex = 9;
            this.lblReceipientCount.Text = "---";
            // 
            // flowLayoutPanel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 3);
            this.flowLayoutPanel1.Controls.Add(this.cbRightToLeft);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.cbAlarmType);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 672);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1233, 42);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(167, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 40);
            this.label6.TabIndex = 13;
            this.label6.Text = "Alarm Type";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbAlarmType
            // 
            this.cbAlarmType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlarmType.FormattingEnabled = true;
            this.cbAlarmType.Location = new System.Drawing.Point(262, 3);
            this.cbAlarmType.Name = "cbAlarmType";
            this.cbAlarmType.Size = new System.Drawing.Size(151, 28);
            this.cbAlarmType.TabIndex = 12;
            this.cbAlarmType.SelectedIndexChanged += new System.EventHandler(this.cbAlarmType_SelectedIndexChanged);
            // 
            // msMain
            // 
            this.msMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.deploymentToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(1239, 28);
            this.msMain.TabIndex = 17;
            this.msMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiToggleServer,
            this.tsmiSettings,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // tsmiToggleServer
            // 
            this.tsmiToggleServer.Name = "tsmiToggleServer";
            this.tsmiToggleServer.Size = new System.Drawing.Size(203, 26);
            this.tsmiToggleServer.Text = "Server starting...";
            this.tsmiToggleServer.ToolTipText = "Toggle the server status - running or stopped";
            this.tsmiToggleServer.Click += new System.EventHandler(this.tsmiToggleServer_Click);
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.tsmiSettings.Size = new System.Drawing.Size(203, 26);
            this.tsmiSettings.Text = "&Settings...";
            this.tsmiSettings.Click += new System.EventHandler(this.tsmiSettings_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.ToolTipText = "Stop the server and quit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // deploymentToolStripMenuItem
            // 
            this.deploymentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alarmDirectionToolStripMenuItem,
            this.tsmiGroupManager});
            this.deploymentToolStripMenuItem.Name = "deploymentToolStripMenuItem";
            this.deploymentToolStripMenuItem.Size = new System.Drawing.Size(105, 24);
            this.deploymentToolStripMenuItem.Text = "&Deployment";
            // 
            // alarmDirectionToolStripMenuItem
            // 
            this.alarmDirectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRightToLeft,
            this.tsmiLeftToRight});
            this.alarmDirectionToolStripMenuItem.Name = "alarmDirectionToolStripMenuItem";
            this.alarmDirectionToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.alarmDirectionToolStripMenuItem.Text = "&Alarm Direction";
            this.alarmDirectionToolStripMenuItem.ToolTipText = "Define the alarm text direction";
            // 
            // tsmiRightToLeft
            // 
            this.tsmiRightToLeft.Name = "tsmiRightToLeft";
            this.tsmiRightToLeft.Size = new System.Drawing.Size(174, 26);
            this.tsmiRightToLeft.Text = "Right to Left";
            this.tsmiRightToLeft.Click += new System.EventHandler(this.tsmiRightToLeft_Click);
            // 
            // tsmiLeftToRight
            // 
            this.tsmiLeftToRight.Name = "tsmiLeftToRight";
            this.tsmiLeftToRight.Size = new System.Drawing.Size(174, 26);
            this.tsmiLeftToRight.Text = "Left to Right";
            this.tsmiLeftToRight.Click += new System.EventHandler(this.tsmiLeftToRight_Click);
            // 
            // tsmiGroupManager
            // 
            this.tsmiGroupManager.Name = "tsmiGroupManager";
            this.tsmiGroupManager.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.tsmiGroupManager.Size = new System.Drawing.Size(257, 26);
            this.tsmiGroupManager.Text = "&Group Manager...";
            this.tsmiGroupManager.ToolTipText = "Create and manage groups";
            this.tsmiGroupManager.Click += new System.EventHandler(this.tsmiGroupManager_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usageGuideToolStripMenuItem,
            this.toolStripSeparator2,
            this.tsmiAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // usageGuideToolStripMenuItem
            // 
            this.usageGuideToolStripMenuItem.Name = "usageGuideToolStripMenuItem";
            this.usageGuideToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.usageGuideToolStripMenuItem.Text = "&Usage Guide...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(185, 26);
            this.tsmiAbout.Text = "&About...";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // frmManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 772);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.msMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarmy Manager";
            this.Load += new System.EventHandler(this.frmManager_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tcDetails.ResumeLayout(false);
            this.tpUsersLastSeen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLastSeen)).EndInit();
            this.tpGroups.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAllUsers;
        private System.Windows.Forms.Button btnNoUsers;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtbContent;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox cbRightToLeft;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Timer tmrLastSeen;
        private System.Windows.Forms.Button btnTransferSelection;
        private System.Windows.Forms.ToolStripStatusLabel lblHelp;
        private System.Windows.Forms.ToolStripStatusLabel lblServerStatus;
        private System.Windows.Forms.TextBox tbUid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckedListBox clbUsers;
        private System.Windows.Forms.ComboBox cbAlarmType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiToggleServer;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deploymentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alarmDirectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiRightToLeft;
        private System.Windows.Forms.ToolStripMenuItem tsmiLeftToRight;
        private System.Windows.Forms.ToolStripMenuItem tsmiGroupManager;
        private System.Windows.Forms.TabControl tcDetails;
        private System.Windows.Forms.TabPage tpUsersLastSeen;
        private System.Windows.Forms.DataGridView dgvLastSeen;
        private System.Windows.Forms.TabPage tpGroups;
        private System.Windows.Forms.TreeView tvGroups;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usageGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.Label lblReceipientCount;
    }
}

