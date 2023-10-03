using AlarmyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Alarmy
{
    public partial class frmSettings : Form
    {
        private static bool s_changesMade = false;

        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<Group> groups = new();

            if (!string.IsNullOrWhiteSpace(tbServiceUrl.Text))
            {
                Properties.Settings.Default.ServiceURL = tbServiceUrl.Text;
            }
            else
            {
                errorProvider.SetError(tbServiceUrl, "Invalid Service URL");
            }

            Properties.Settings.Default.ServicePort = (int)nudServicePort.Value;

            foreach (Group group in lbGroups.Items)
            {
                groups.Add(group);
            }

            // Properties.Settings.Default.Groups is modified directly at btnManageGroups_Click.
            Properties.Settings.Default.AlarmStyle = cbAlarmStyle.SelectedIndex;
            Properties.Settings.Default.ReconnectInterval = (int)nudReconnectInterval.Value;

            Properties.Settings.Default.Save();
            MessageBox.Show("Settings have been saved.\nRestart Alarmy to apply changes.");
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool userConfirmation = DialogResult.Yes == MessageBox.Show(this,
                "Exit without applying changes?", "Alarmy", MessageBoxButtons.YesNo,
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

        private void frmSettings_Load(object sender, EventArgs e)
        {
            Instance me = Instance.GetInstance();
            tbInstance.Text = $"User={me.Username}, Computer={me.ComputerName}";
            lbGroups.DataSource = JsonConvert.DeserializeObject<List<Group>>(Properties.Settings.Default.Groups);
            cbAlarmStyle.DataSource = Enum.GetValues<AlarmStyle>();

            tbServiceUrl.Text = Properties.Settings.Default.ServiceURL;
            nudServicePort.Value = Properties.Settings.Default.ServicePort;
            cbAlarmStyle.SelectedIndex = Properties.Settings.Default.AlarmStyle;
            nudReconnectInterval.Value = Properties.Settings.Default.ReconnectInterval;

            s_changesMade = false;
        }

        private void tbServiceUrl_TextChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }

        private void nudServicePort_ValueChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }

        private void cbAlarmStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }

        private void nudReconnectInterval_ValueChanged(object sender, EventArgs e)
        {
            s_changesMade = true;
        }

        private void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            if (lbGroups.SelectedIndex >= 0)
            {
                lbGroups.Items.RemoveAt(lbGroups.SelectedIndex);
            }
        }

        private void btnManageGroups_Click(object sender, EventArgs e)
        {
            frmGroups frmGroups = new frmGroups(Program.Context.Proxy);
            frmGroups.ShowDialog();

            if (frmGroups.DialogResult == DialogResult.OK)
            {
                lbGroups.DataSource = frmGroups.Groups.FindAll(x => x.Enabled);
                Properties.Settings.Default.Groups = JsonConvert.SerializeObject(frmGroups.Groups);
            }
        }
    }
}
