using AlarmyLib;
using System;
using System.Windows.Forms;

namespace Alarmy
{
    public partial class frmPastAlarms : Form
    {
        public frmPastAlarms()
        {
            InitializeComponent();
        }

        private void frmPastAlarms_Load(object sender, EventArgs e)
        {
            foreach (Alarm alarm in AlarmyState.PastAlarms)
            {
                lbPastAlarms.Items.Add(alarm);
            }
        }

        private void lbPastAlarms_SelectedIndexChanged(object sender, EventArgs e)
        {
            Alarm selectedAlarm = (Alarm)lbPastAlarms.SelectedItem;
            
            lblTitle.Text = selectedAlarm.Title;
            lblTitle.RightToLeft = selectedAlarm.IsRtl ? RightToLeft.Yes : RightToLeft.Inherit;

            rtbContent.Rtf = selectedAlarm.Content;
            rtbContent.RightToLeft = selectedAlarm.IsRtl ? RightToLeft.Yes : RightToLeft.Inherit;
        }
    }
}
