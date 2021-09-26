
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.benRestoreDefaults = new System.Windows.Forms.Button();
            this.tbServiceUrl = new System.Windows.Forms.TextBox();
            this.nudServicePort = new System.Windows.Forms.NumericUpDown();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnManageGroups = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudServicePort)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Service URL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Service Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Groups";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(292, 287);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 29);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(392, 287);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // benRestoreDefaults
            // 
            this.benRestoreDefaults.Location = new System.Drawing.Point(12, 287);
            this.benRestoreDefaults.Name = "benRestoreDefaults";
            this.benRestoreDefaults.Size = new System.Drawing.Size(143, 29);
            this.benRestoreDefaults.TabIndex = 2;
            this.benRestoreDefaults.Text = "Restore Defaults";
            this.benRestoreDefaults.UseVisualStyleBackColor = true;
            this.benRestoreDefaults.Click += new System.EventHandler(this.benRestoreDefaults_Click);
            // 
            // tbServiceUrl
            // 
            this.tbServiceUrl.Location = new System.Drawing.Point(104, 12);
            this.tbServiceUrl.Name = "tbServiceUrl";
            this.tbServiceUrl.Size = new System.Drawing.Size(382, 27);
            this.tbServiceUrl.TabIndex = 3;
            // 
            // nudServicePort
            // 
            this.nudServicePort.Location = new System.Drawing.Point(104, 47);
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
            this.nudServicePort.Size = new System.Drawing.Size(115, 27);
            this.nudServicePort.TabIndex = 4;
            this.nudServicePort.Value = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Interruptive",
            "Background"});
            this.comboBox1.Location = new System.Drawing.Point(104, 230);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(151, 28);
            this.comboBox1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 233);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Alarm Style";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(104, 80);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(345, 144);
            this.listBox1.TabIndex = 8;
            // 
            // btnManageGroups
            // 
            this.btnManageGroups.Location = new System.Drawing.Point(455, 80);
            this.btnManageGroups.Name = "btnManageGroups";
            this.btnManageGroups.Size = new System.Drawing.Size(31, 144);
            this.btnManageGroups.TabIndex = 9;
            this.btnManageGroups.Text = "...";
            this.btnManageGroups.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(504, 328);
            this.Controls.Add(this.btnManageGroups);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.nudServicePort);
            this.Controls.Add(this.tbServiceUrl);
            this.Controls.Add(this.benRestoreDefaults);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.nudServicePort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button benRestoreDefaults;
        private System.Windows.Forms.TextBox tbServiceUrl;
        private System.Windows.Forms.NumericUpDown nudServicePort;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnManageGroups;
    }
}