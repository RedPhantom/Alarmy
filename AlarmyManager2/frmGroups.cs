using AlarmyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        // Buffer that holds the altered state of ManagerState.Groups
        // before applying changes.
        private List<Group> _modifiedGroups = new();

        public frmGroups()
        {
            InitializeComponent();

            _modifiedGroups = ManagerState.Groups;
        }

        private void frmGroups_Load(object sender, EventArgs e)
        {
            dgvGroups.Columns.Add(ColumnNameGroupName, ColumnHeaderGroupName);
            dgvGroups.Columns.Add(ColumnNameGroupId, ColumnHeaderGroupId);

            RefreshDataGridView();

            s_changesMade = false;
        }

        private void RefreshDataGridView()
        {
            // TODO: is it worth optimizing this to skip adding items that already exist?
            if (dgvGroups.Rows.Count > 0)
            {
                dgvGroups.Rows.Clear();
            }

            foreach (Group group in _modifiedGroups)
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

            _modifiedGroups.Add(new Group(Guid.NewGuid(), tbGroupName.Text));
            RefreshDataGridView();
            tbGroupName.Text = string.Empty;
            s_changesMade = true;
        }

        private void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            if (dgvGroups.SelectedCells.Count == 1)
            {
                Group selectedGroup = _modifiedGroups.Find(x =>
                    x.ID == (Guid)dgvGroups.SelectedCells[0].OwningRow.Cells[ColumnNameGroupId].Value);

                if (selectedGroup is not null)
                {
                    _modifiedGroups.Remove(selectedGroup);
                    RefreshDataGridView();
                    s_changesMade = true;
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ManagerState.Groups = _modifiedGroups;
            Properties.Settings.Default.Groups = JsonConvert.SerializeObject(_modifiedGroups);
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (s_changesMade)
            {
                if (DialogResult.Yes == MessageBox.Show(this,
                    "Exit without applying changes?", "Alarmy", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question))
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
            else
            {
                DialogResult = DialogResult.Cancel;
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

        private void btnEditGroup_Click(object sender, EventArgs e)
        {
            int selectedGroupIndex;

            if (dgvGroups.SelectedRows.Count == 1)
            {
                Group selectedGroup = _modifiedGroups.Find(x =>
                    x.ID == (Guid)dgvGroups.SelectedCells[0].OwningRow.Cells[ColumnNameGroupId].Value);

                if (selectedGroup is not null)
                {
                    // Store the group index to insert the edited one at the same place.
                    selectedGroupIndex = _modifiedGroups.IndexOf(selectedGroup);
                    
                    selectedGroup.Name = tbGroupName.Text;
                    _modifiedGroups.Remove(selectedGroup);
                    _modifiedGroups.Insert(selectedGroupIndex, selectedGroup);
                    RefreshDataGridView();
                    s_changesMade = true;
                }
            }
        }
    }
}
