
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
            this.clbUsers = new System.Windows.Forms.CheckedListBox();
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
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tmrLastSeen = new System.Windows.Forms.Timer(this.components);
            this.dgvLastSeen = new System.Windows.Forms.DataGridView();
            this.colInstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastSeen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnTransferSelection = new System.Windows.Forms.Button();
            this.lblHelp = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLastSeen)).BeginInit();
            this.SuspendLayout();
            // 
            // clbUsers
            // 
            this.clbUsers.FormattingEnabled = true;
            this.clbUsers.Location = new System.Drawing.Point(12, 27);
            this.clbUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clbUsers.Name = "clbUsers";
            this.clbUsers.Size = new System.Drawing.Size(226, 293);
            this.clbUsers.TabIndex = 0;
            // 
            // btnAllUsers
            // 
            this.btnAllUsers.Location = new System.Drawing.Point(12, 334);
            this.btnAllUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAllUsers.Name = "btnAllUsers";
            this.btnAllUsers.Size = new System.Drawing.Size(65, 32);
            this.btnAllUsers.TabIndex = 1;
            this.btnAllUsers.Text = "All";
            this.btnAllUsers.UseVisualStyleBackColor = true;
            this.btnAllUsers.Click += new System.EventHandler(this.btnAllUsers_Click);
            this.btnAllUsers.MouseHover += new System.EventHandler(this.btnAllUsers_MouseHover);
            // 
            // btnNoUsers
            // 
            this.btnNoUsers.Location = new System.Drawing.Point(83, 334);
            this.btnNoUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNoUsers.Name = "btnNoUsers";
            this.btnNoUsers.Size = new System.Drawing.Size(65, 32);
            this.btnNoUsers.TabIndex = 2;
            this.btnNoUsers.Text = "None";
            this.btnNoUsers.UseVisualStyleBackColor = true;
            this.btnNoUsers.Click += new System.EventHandler(this.btnNoUsers_Click);
            this.btnNoUsers.MouseHover += new System.EventHandler(this.btnNoUsers_MouseHover);
            // 
            // tbTitle
            // 
            this.tbTitle.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTitle.Location = new System.Drawing.Point(314, 5);
            this.tbTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(525, 25);
            this.tbTitle.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Users";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Title";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(247, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Content";
            // 
            // rtbContent
            // 
            this.rtbContent.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbContent.Location = new System.Drawing.Point(314, 32);
            this.rtbContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(525, 288);
            this.rtbContent.TabIndex = 5;
            this.rtbContent.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(314, 334);
            this.btnSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(115, 32);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.btnSend.MouseHover += new System.EventHandler(this.btnSend_MouseHover);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(154, 334);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(84, 32);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.MouseHover += new System.EventHandler(this.btnRefresh_MouseHover);
            // 
            // cbRightToLeft
            // 
            this.cbRightToLeft.AutoSize = true;
            this.cbRightToLeft.Location = new System.Drawing.Point(435, 341);
            this.cbRightToLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbRightToLeft.Name = "cbRightToLeft";
            this.cbRightToLeft.Size = new System.Drawing.Size(107, 21);
            this.cbRightToLeft.TabIndex = 8;
            this.cbRightToLeft.Text = "Right to Left";
            this.cbRightToLeft.UseVisualStyleBackColor = true;
            this.cbRightToLeft.CheckedChanged += new System.EventHandler(this.cbRightToLeft_CheckedChanged);
            this.cbRightToLeft.MouseHover += new System.EventHandler(this.cbRightToLeft_MouseHover);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblHelp});
            this.statusStrip.Location = new System.Drawing.Point(0, 373);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1239, 26);
            this.statusStrip.TabIndex = 9;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(168, 20);
            this.lblStatus.Text = "Waiting for server start...";
            // 
            // btnStopServer
            // 
            this.btnStopServer.Location = new System.Drawing.Point(724, 334);
            this.btnStopServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(115, 32);
            this.btnStopServer.TabIndex = 10;
            this.btnStopServer.Text = "Stop Server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            this.btnStopServer.MouseHover += new System.EventHandler(this.btnStopServer_MouseHover);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(845, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Last Seen";
            // 
            // tmrLastSeen
            // 
            this.tmrLastSeen.Enabled = true;
            this.tmrLastSeen.Interval = 3000;
            this.tmrLastSeen.Tick += new System.EventHandler(this.tmrLastSeen_Tick);
            // 
            // dgvLastSeen
            // 
            this.dgvLastSeen.AllowUserToAddRows = false;
            this.dgvLastSeen.AllowUserToDeleteRows = false;
            this.dgvLastSeen.AllowUserToOrderColumns = true;
            this.dgvLastSeen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLastSeen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInstance,
            this.colLastSeen});
            this.dgvLastSeen.Location = new System.Drawing.Point(846, 34);
            this.dgvLastSeen.Name = "dgvLastSeen";
            this.dgvLastSeen.ReadOnly = true;
            this.dgvLastSeen.RowHeadersWidth = 51;
            this.dgvLastSeen.RowTemplate.Height = 24;
            this.dgvLastSeen.Size = new System.Drawing.Size(381, 286);
            this.dgvLastSeen.TabIndex = 12;
            // 
            // colInstance
            // 
            this.colInstance.HeaderText = "Instance";
            this.colInstance.MinimumWidth = 6;
            this.colInstance.Name = "colInstance";
            this.colInstance.ReadOnly = true;
            this.colInstance.Width = 125;
            // 
            // colLastSeen
            // 
            this.colLastSeen.HeaderText = "Last Seen";
            this.colLastSeen.MinimumWidth = 6;
            this.colLastSeen.Name = "colLastSeen";
            this.colLastSeen.ReadOnly = true;
            this.colLastSeen.Width = 125;
            // 
            // btnTransferSelection
            // 
            this.btnTransferSelection.Location = new System.Drawing.Point(848, 334);
            this.btnTransferSelection.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTransferSelection.Name = "btnTransferSelection";
            this.btnTransferSelection.Size = new System.Drawing.Size(115, 32);
            this.btnTransferSelection.TabIndex = 13;
            this.btnTransferSelection.Text = "Apply Selection";
            this.btnTransferSelection.UseVisualStyleBackColor = true;
            this.btnTransferSelection.Click += new System.EventHandler(this.btnTransferSelection_Click);
            this.btnTransferSelection.MouseHover += new System.EventHandler(this.btnTransferSelection_MouseHover);
            // 
            // lblHelp
            // 
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(0, 20);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "sefsefse";
            this.notifyIcon1.BalloonTipTitle = "awdawdawdawdawd";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // frmManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 399);
            this.Controls.Add(this.btnTransferSelection);
            this.Controls.Add(this.dgvLastSeen);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.cbRightToLeft);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.rtbContent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.btnNoUsers);
            this.Controls.Add(this.btnAllUsers);
            this.Controls.Add(this.clbUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "frmManager";
            this.Text = "Alarmy Manager";
            this.Load += new System.EventHandler(this.frmManager_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLastSeen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbUsers;
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
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer tmrLastSeen;
        private System.Windows.Forms.DataGridView dgvLastSeen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastSeen;
        private System.Windows.Forms.Button btnTransferSelection;
        private System.Windows.Forms.ToolStripStatusLabel lblHelp;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

