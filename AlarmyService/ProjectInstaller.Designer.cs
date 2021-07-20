
namespace AlarmyService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.alarmyProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.alarmyServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // alarmyProcessInstaller
            // 
            this.alarmyProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.alarmyProcessInstaller.Password = null;
            this.alarmyProcessInstaller.Username = null;
            // 
            // alarmyServiceInstaller
            // 
            this.alarmyServiceInstaller.Description = "Service responsible for displaying alarms to the user.";
            this.alarmyServiceInstaller.DisplayName = "Alarmy Service";
            this.alarmyServiceInstaller.ServiceName = "AlarmyService";
            this.alarmyServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.alarmyProcessInstaller,
            this.alarmyServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller alarmyProcessInstaller;
        private System.ServiceProcess.ServiceInstaller alarmyServiceInstaller;
    }
}