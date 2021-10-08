using AlarmyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Alarmy
{
    public partial class frmGroups : Form
    {
        // Only modified during btnApply click.
        internal List<Group> Groups { get; private set; }

        private static bool s_changesMade = false;
        private static InternalProxy s_proxy;

        public frmGroups(InternalProxy proxy)
        {
            InitializeComponent();

            s_proxy = proxy;
        }

        private void frmGroups_Load(object sender, EventArgs e)
        {
            foreach (Group group in AlarmyState.Groups)
            {
                clbGroups.Items.Add(group, group.Enabled);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool userConfirmation = DialogResult.Yes == MessageBox.Show(this,
                "Exit without applying changes?", "Alarmy", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (s_changesMade && userConfirmation)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }

            if (!s_changesMade)
            {
                Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GroupQueryResponse gqr = null;
            MessageWrapperContent mrcResult = null;
            MessageWrapper<GroupQueryMessage> gqmWrapper = new();

            lblVerifying.Visible = true;

            s_proxy.SignalServer.Reset();

            // Validate GUID.
            try
            {
                GroupQueryMessage gqm = new(Guid.Parse(mtbGroupUID.Text));
                gqmWrapper.Message = gqm;
            }
            catch
            {
                errorProvider.SetError(mtbGroupUID, "Failed to parse Group ID");
                return;
            }

            // Send message to the server via the socket.
            s_proxy.InternalToServer.Send(MessageUtils.BuildMessage(gqmWrapper.Serialize()));
            s_proxy.SignalServer.Set();

            // Give the server an opportunity to respond.
            Thread.Sleep(250);
            s_proxy.SignalClient.WaitOne();

            // Receive the message from the server.
            lock (AlarmyState.MessageQueue)
            {
                foreach (MessageWrapperContent mrc in AlarmyState.MessageQueue)
                {
                    if (typeof(GroupQueryResponse) == mrc.Type)
                    {
                        mrcResult = mrc;
                        gqr = (GroupQueryResponse)mrc.Message;
                    }
                }

                if (mrcResult is not null)
                {
                    AlarmyState.MessageQueue.Remove(mrcResult);
                }
            }

            // Add the group to the list box if it's not enabled already.
            if (gqr is not null && clbGroups.Items.Contains(gqr.Group))
            {
                clbGroups.Items.Add(gqr.Group, gqr.Group.Enabled);
                s_changesMade = true;
            }
            else
            {
                MessageBox.Show("Could not receive a reponse from the server. Try again.", "Alarmy");
            }

            lblVerifying.Visible = false;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            s_changesMade = true;

            if (clbGroups.SelectedIndex >= 0)
            {
                clbGroups.Items.RemoveAt(clbGroups.SelectedIndex);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbGroups.Items.Count; i++)
            {
                bool isChecked = clbGroups.GetItemChecked(i);
                Group group = (Group)clbGroups.Items[i];

                Groups.Add(new Group(group.ID, group.Name, isChecked));
            }

            Properties.Settings.Default.Groups = JsonConvert.SerializeObject(Groups);

            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
