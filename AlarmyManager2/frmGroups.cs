using AlarmyLib;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace AlarmyManager
{
    public partial class frmGroups : Form
    {
        private static bool s_changesMade = false;

        // Group Name
        private const string ColumnNameGroupName = "colGroupName";
        private const string ColumnHeaderGroupName = "Group Name";

        // Group ID
        private const string ColumnNameGroupId = "colGroupId";
        private const string ColumnHeaderGroupId = "Group ID";

        public frmGroups()
        {
            InitializeComponent();
        }

        private void frmGroups_Load(object sender, EventArgs e)
        {
            dgvGroups.Columns.Add(ColumnNameGroupName, ColumnHeaderGroupName);
            dgvGroups.Columns.Add(ColumnNameGroupId, ColumnHeaderGroupId);

            RefreshDataGridView();
        }

        private void RefreshDataGridView()
        {
            // TODO: is it worth optimizing this to skip adding items that already exist?
            if (dgvGroups.Rows.Count > 0)
            {
                dgvGroups.Rows.Clear();
            }

            foreach (Group group in ManagerState.Groups)
            {
                // Add all groups except the default one.
                if (group.ID != Guid.Empty)
                {
                    dgvGroups.Rows.Add(group.Name, group.ID);
                }
            }
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            AddGroup();
        }

        private void AddGroup()
        {
            if (string.IsNullOrWhiteSpace(tbGroupName.Text))
            {
                errorProvider.SetError(tbGroupName, "Invalid group name");
                return;
            }

            ManagerState.Groups.Add(new Group(Guid.NewGuid(), tbGroupName.Text));
            RefreshDataGridView();
            s_changesMade = true;
            tbGroupName.Text = string.Empty;
        }

        private void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            if (dgvGroups.SelectedCells.Count == 1)
            {
                Group selectedGroup = ManagerState.Groups.Find(x => 
                    x.ID == (Guid)dgvGroups.SelectedRows[0].Cells[ColumnNameGroupId].Value);
                
                if (selectedGroup is not null)
                {
                    ManagerState.Groups.Remove(selectedGroup);
                    RefreshDataGridView();
                    s_changesMade = true;
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Groups = JsonConvert.SerializeObject(ManagerState.Groups);
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (s_changesMade && (DialogResult.Yes == MessageBox.Show(this,
                "Exit without applying changes?", "Alarmy", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)))
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else if (!s_changesMade)
            {
                Close();
            }
        }

        private void tbGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                AddGroup();
            }
        }
    }
}
