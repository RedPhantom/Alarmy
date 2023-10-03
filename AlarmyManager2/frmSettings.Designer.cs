
namespace AlarmyManager
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.nudServicePort = new System.Windows.Forms.NumericUpDown();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tbLogPath = new System.Windows.Forms.TextBox();
            this.btnSelectLoggingDir = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCertificatePath = new System.Windows.Forms.TextBox();
            this.btnSelectCert = new System.Windows.Forms.Button();
            this.tbAlarmExportPath = new System.Windows.Forms.TextBox();
            this.btnSelectExportDir = new System.Windows.Forms.Button();
            this.fbdDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdCertificate = new System.Windows.Forms.OpenFileDialog();
            this.cbEnableExport = new System.Windows.Forms.CheckBox();
            this.cbEnableSsl = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudServicePort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Service Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Export Directory";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(328, 335);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 29);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(428, 335);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // nudServicePort
            // 
            this.nudServicePort.Location = new System.Drawing.Point(147, 7);
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
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Logging Directory";
            // 
            // tbLogPath
            // 
            this.tbLogPath.Location = new System.Drawing.Point(147, 40);
            this.tbLogPath.Name = "tbLogPath";
            this.tbLogPath.Size = new System.Drawing.Size(332, 27);
            this.tbLogPath.TabIndex = 16;
            this.tbLogPath.TextChanged += new System.EventHandler(this.tbLogPath_TextChanged);
            // 
            // btnSelectLoggingDir
            // 
            this.btnSelectLoggingDir.Location = new System.Drawing.Point(485, 40);
            this.btnSelectLoggingDir.Name = "btnSelectLoggingDir";
            this.btnSelectLoggingDir.Size = new System.Drawing.Size(31, 27);
            this.btnSelectLoggingDir.TabIndex = 17;
            this.btnSelectLoggingDir.Text = "...";
            this.btnSelectLoggingDir.UseVisualStyleBackColor = true;
            this.btnSelectLoggingDir.Click += new System.EventHandler(this.btnSelectLoggingDir_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "SSL Certificate";
            // 
            // tbCertificatePath
            // 
            this.tbCertificatePath.Location = new System.Drawing.Point(116, 63);
            this.tbCertificatePath.Name = "tbCertificatePath";
            this.tbCertificatePath.Size = new System.Drawing.Size(351, 27);
            this.tbCertificatePath.TabIndex = 19;
            this.tbCertificatePath.TextChanged += new System.EventHandler(this.tbCertificatePath_TextChanged);
            // 
            // btnSelectCert
            // 
            this.btnSelectCert.Location = new System.Drawing.Point(473, 63);
            this.btnSelectCert.Name = "btnSelectCert";
            this.btnSelectCert.Size = new System.Drawing.Size(31, 27);
            this.btnSelectCert.TabIndex = 20;
            this.btnSelectCert.Text = "...";
            this.btnSelectCert.UseVisualStyleBackColor = true;
            this.btnSelectCert.Click += new System.EventHandler(this.btnSelectCert_Click);
            // 
            // tbAlarmExportPath
            // 
            this.tbAlarmExportPath.Location = new System.Drawing.Point(129, 61);
            this.tbAlarmExportPath.Name = "tbAlarmExportPath";
            this.tbAlarmExportPath.Size = new System.Drawing.Size(338, 27);
            this.tbAlarmExportPath.TabIndex = 21;
            this.tbAlarmExportPath.TextChanged += new System.EventHandler(this.tbAlarmExportPath_TextChanged);
            // 
            // btnSelectExportDir
            // 
            this.btnSelectExportDir.Location = new System.Drawing.Point(473, 61);
            this.btnSelectExportDir.Name = "btnSelectExportDir";
            this.btnSelectExportDir.Size = new System.Drawing.Size(31, 27);
            this.btnSelectExportDir.TabIndex = 22;
            this.btnSelectExportDir.Text = "...";
            this.btnSelectExportDir.UseVisualStyleBackColor = true;
            this.btnSelectExportDir.Click += new System.EventHandler(this.btnSelectExportDir_Click);
            // 
            // ofdCertificate
            // 
            this.ofdCertificate.FileName = "cert.pfx";
            // 
            // cbEnableExport
            // 
            this.cbEnableExport.AutoSize = true;
            this.cbEnableExport.Location = new System.Drawing.Point(6, 26);
            this.cbEnableExport.Name = "cbEnableExport";
            this.cbEnableExport.Size = new System.Drawing.Size(167, 24);
            this.cbEnableExport.TabIndex = 23;
            this.cbEnableExport.Text = "Enable Alarm Export";
            this.cbEnableExport.UseVisualStyleBackColor = true;
            this.cbEnableExport.CheckedChanged += new System.EventHandler(this.cbEnableExport_CheckedChanged);
            // 
            // cbEnableSsl
            // 
            this.cbEnableSsl.AutoSize = true;
            this.cbEnableSsl.Location = new System.Drawing.Point(6, 26);
            this.cbEnableSsl.Name = "cbEnableSsl";
            this.cbEnableSsl.Size = new System.Drawing.Size(185, 24);
            this.cbEnableSsl.TabIndex = 24;
            this.cbEnableSsl.Text = "Enable SSL Capabilities";
            this.cbEnableSsl.UseVisualStyleBackColor = true;
            this.cbEnableSsl.CheckedChanged += new System.EventHandler(this.cbEnableSsl_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbEnableSsl);
            this.groupBox1.Controls.Add(this.tbCertificatePath);
            this.groupBox1.Controls.Add(this.btnSelectCert);
            this.groupBox1.Location = new System.Drawing.Point(12, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 125);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SSL";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbEnableExport);
            this.groupBox2.Controls.Add(this.tbAlarmExportPath);
            this.groupBox2.Controls.Add(this.btnSelectExportDir);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(510, 125);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Alarm Export";
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(542, 379);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSelectLoggingDir);
            this.Controls.Add(this.tbLogPath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.nudServicePort);
            this.Controls.Add(this.label2);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown nudServicePort;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button btnSelectExportDir;
        private System.Windows.Forms.TextBox tbAlarmExportPath;
        private System.Windows.Forms.Button btnSelectCert;
        private System.Windows.Forms.TextBox tbCertificatePath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSelectLoggingDir;
        private System.Windows.Forms.TextBox tbLogPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog fbdDirectory;
        private System.Windows.Forms.OpenFileDialog ofdCertificate;
        private System.Windows.Forms.CheckBox cbEnableExport;
        private System.Windows.Forms.CheckBox cbEnableSsl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}