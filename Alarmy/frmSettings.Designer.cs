
namespace Alarmy
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbServiceUrl = new System.Windows.Forms.TextBox();
            this.nudServicePort = new System.Windows.Forms.NumericUpDown();
            this.cbAlarmStyle = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbGroups = new System.Windows.Forms.ListBox();
            this.btnManageGroups = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.tbInstance = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudServicePort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Service URL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Service Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Enabled Groups";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(320, 307);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 29);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(420, 307);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbServiceUrl
            // 
            this.tbServiceUrl.Location = new System.Drawing.Point(132, 50);
            this.tbServiceUrl.Name = "tbServiceUrl";
            this.tbServiceUrl.Size = new System.Drawing.Size(382, 27);
            this.tbServiceUrl.TabIndex = 3;
            this.tbServiceUrl.TextChanged += new System.EventHandler(this.tbServiceUrl_TextChanged);
            // 
            // nudServicePort
            // 
            this.nudServicePort.Location = new System.Drawing.Point(132, 83);
            this.nudServicePort.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.nudServicePort.Minimum = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            this.nudServicePort.Name = "nudServicePort";
            this.nudServicePort.Size = new System.Drawing.Size(133, 27);
            this.nudServicePort.TabIndex = 4;
            this.nudServicePort.Value = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            this.nudServicePort.ValueChanged += new System.EventHandler(this.nudServicePort_ValueChanged);
            // 
            // cbAlarmStyle
            // 
            this.cbAlarmStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlarmStyle.FormattingEnabled = true;
            this.cbAlarmStyle.Items.AddRange(new object[] {
            "Interruptive",
            "Background"});
            this.cbAlarmStyle.Location = new System.Drawing.Point(132, 266);
            this.cbAlarmStyle.Name = "cbAlarmStyle";
            this.cbAlarmStyle.Size = new System.Drawing.Size(133, 28);
            this.cbAlarmStyle.TabIndex = 6;
            this.cbAlarmStyle.SelectedIndexChanged += new System.EventHandler(this.cbAlarmStyle_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Alarm Style";
            // 
            // lbGroups
            // 
            this.lbGroups.FormattingEnabled = true;
            this.lbGroups.ItemHeight = 20;
            this.lbGroups.Location = new System.Drawing.Point(132, 116);
            this.lbGroups.Name = "lbGroups";
            this.lbGroups.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbGroups.Size = new System.Drawing.Size(345, 144);
            this.lbGroups.TabIndex = 8;
            // 
            // btnManageGroups
            // 
            this.btnManageGroups.Location = new System.Drawing.Point(483, 116);
            this.btnManageGroups.Name = "btnManageGroups";
            this.btnManageGroups.Size = new System.Drawing.Size(31, 144);
            this.btnManageGroups.TabIndex = 9;
            this.btnManageGroups.Text = "...";
            this.btnManageGroups.UseVisualStyleBackColor = true;
            this.btnManageGroups.Click += new System.EventHandler(this.btnManageGroups_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Instance";
            // 
            // tbInstance
            // 
            this.tbInstance.Location = new System.Drawing.Point(132, 15);
            this.tbInstance.Name = "tbInstance";
            this.tbInstance.ReadOnly = true;
            this.tbInstance.Size = new System.Drawing.Size(381, 27);
            this.tbInstance.TabIndex = 11;
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(525, 348);
            this.Controls.Add(this.tbInstance);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnManageGroups);
            this.Controls.Add(this.lbGroups);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbAlarmStyle);
            this.Controls.Add(this.nudServicePort);
            this.Controls.Add(this.tbServiceUrl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudServicePort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbServiceUrl;
        private System.Windows.Forms.NumericUpDown nudServicePort;
        private System.Windows.Forms.ComboBox cbAlarmStyle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lbGroups;
        private System.Windows.Forms.Button btnManageGroups;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox tbInstance;
        private System.Windows.Forms.Label label5;
    }
}