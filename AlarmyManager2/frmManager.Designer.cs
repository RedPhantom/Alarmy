
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
            this.SuspendLayout();
            // 
            // clbUsers
            // 
            this.clbUsers.FormattingEnabled = true;
            this.clbUsers.Location = new System.Drawing.Point(12, 34);
            this.clbUsers.Name = "clbUsers";
            this.clbUsers.Size = new System.Drawing.Size(226, 378);
            this.clbUsers.TabIndex = 0;
            // 
            // btnAllUsers
            // 
            this.btnAllUsers.Location = new System.Drawing.Point(12, 418);
            this.btnAllUsers.Name = "btnAllUsers";
            this.btnAllUsers.Size = new System.Drawing.Size(65, 29);
            this.btnAllUsers.TabIndex = 1;
            this.btnAllUsers.Text = "All";
            this.btnAllUsers.UseVisualStyleBackColor = true;
            this.btnAllUsers.Click += new System.EventHandler(this.btnAllUsers_Click);
            // 
            // btnNoUsers
            // 
            this.btnNoUsers.Location = new System.Drawing.Point(83, 418);
            this.btnNoUsers.Name = "btnNoUsers";
            this.btnNoUsers.Size = new System.Drawing.Size(65, 29);
            this.btnNoUsers.TabIndex = 2;
            this.btnNoUsers.Text = "None";
            this.btnNoUsers.UseVisualStyleBackColor = true;
            this.btnNoUsers.Click += new System.EventHandler(this.btnNoUsers_Click);
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(314, 6);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(525, 27);
            this.tbTitle.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Users";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Title";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(247, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Content";
            // 
            // rtbContent
            // 
            this.rtbContent.Location = new System.Drawing.Point(314, 40);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(525, 342);
            this.rtbContent.TabIndex = 5;
            this.rtbContent.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(314, 418);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(115, 29);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(154, 418);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(84, 29);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // cbRightToLeft
            // 
            this.cbRightToLeft.AutoSize = true;
            this.cbRightToLeft.Location = new System.Drawing.Point(314, 388);
            this.cbRightToLeft.Name = "cbRightToLeft";
            this.cbRightToLeft.Size = new System.Drawing.Size(113, 24);
            this.cbRightToLeft.TabIndex = 8;
            this.cbRightToLeft.Text = "Right to Left";
            this.cbRightToLeft.UseVisualStyleBackColor = true;
            // 
            // frmManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 454);
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
            this.MaximizeBox = false;
            this.Name = "frmManager";
            this.Text = "Manager";
            this.Load += new System.EventHandler(this.frmManager_Load);
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
    }
}

