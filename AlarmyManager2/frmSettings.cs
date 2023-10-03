using System;
using System.IO;
using System.Windows.Forms;

namespace AlarmyManager
{
    public partial class frmSettings : Form
    {
        private static bool s_changesMade = false;

        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            nudServicePort.Value = Properties.Settings.Default.ServicePort;
            tbLogPath.Text = Properties.Settings.Default.LogPath;
            tbCertificatePath.Text = Properties.Settings.Default.ServerCertificatePath;
            cbEnableSsl.Checked = Properties.Settings.Default.UseSsl;
            tbAlarmExportPath.Text = Properties.Settings.Default.ExportDirectory;
            cbEnableExport.Checked = Properties.Settings.Default.ExportAlarms;

            s_changesMade = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate paths.
            if (!Directory.Exists(tbLogPath.Text))
            {
                MessageBox.Show(this, $"Could not find logging directory at {tbLogPath.Text}.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cbEnableSsl.Checked && !File.Exists(tbCertificatePath.Text))
            {
                MessageBox.Show(this, $"Could not find certificate at path {tbCertificatePath.Text}.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cbEnableExport.Checked && !Directory.Exists(tbAlarmExportPath.Text))
            {
                MessageBox.Show(this, $"Could not find export directory at {tbAlarmExportPath.Text}.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Checks pass. Save the settings.
            Properties.Settings.Default.ServicePort = (int)nudServicePort.Value;
            Properties.Settings.Default.LogPath = tbLogPath.Text;
            if (cbEnableSsl.Checked)
            {
                Properties.Settings.Default.ServerCertificatePath = tbCertificatePath.Text;
                Properties.Settings.Default.UseSsl = cbEnableSsl.Checked;
            }
            if (cbEnableExport.Checked)
            {
                Properties.Settings.Default.ExportDirectory = tbAlarmExportPath.Text;
                Properties.Settings.Default.ExportAlarms = cbEnableExport.Checked;
            }
            Properties.Settings.Default.Save();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool userConfirmation = DialogResult.Yes == MessageBox.Show(this,
                "Exit without saving changes?", "Alarmy", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (s_changesMade && userConfirmation)
            {
                Close();
            }

            if (!s_changesMade)
            {
                Close();
            }
        }

        private void btnSelectLoggingDir_Click(object sender, EventArgs e)
        {
            fbdDirectory.Description = "Select the directory to save logs to.";

            if (fbdDirectory.ShowDialog() == DialogResult.OK)
            {
                tbLogPath.Text = fbdDirectory.SelectedPath;
            }
        }

        private void btnSelectCert_Click(object sender, EventArgs e)
        {
            ofdCertificate.Filter = "PFX Certificate (*.pfx)|*.pfx";
            ofdCertificate.RestoreDirectory = true;

            if (ofdCertificate.ShowDialog() == DialogResult.OK)
            {
                tbCertificatePath.Text = ofdCertificate.FileName;
            }
        }

        private void btnSelectExportDir_Click(object sender, EventArgs e)
        {
            fbdDirectory.Description = "Select the directory to export all alarm to.";
            
            if (fbdDirectory.ShowDialog() == DialogResult.OK)
            {
                tbAlarmExportPath.Text = fbdDirectory.SelectedPath;
            }
        }

        private void tbLogPath_TextChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }

        private void tbCertificatePath_TextChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }

        private void tbAlarmExportPath_TextChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }

        private void cbEnableSsl_CheckedChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }

        private void cbEnableExport_CheckedChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }
    }
}
