using AlarmyLib;
using System;
using System.Windows.Forms;

namespace Alarmy
{
    internal partial class frmPastAlarms : Form
    {
        public frmPastAlarms()
        {
            InitializeComponent();
        }

        private void frmPastAlarms_Load(object sender, EventArgs e)
        {
            foreach (HistoricAlarm historicAlarm in AlarmyState.PastAlarms)
            {
                lbPastAlarms.Items.Add(historicAlarm);
            }
        }

        private void lbPastAlarms_SelectedIndexChanged(object sender, EventArgs e)
        {
            HistoricAlarm selectedAlarm = (HistoricAlarm)lbPastAlarms.SelectedItem;

            string humanizedLastSeen = Humanizer.TimeSpanHumanizeExtensions.Humanize(DateTime.Now -
                   selectedAlarm.DateTime);

            lblTitle.Text = selectedAlarm.Alarm.Title;
            lblTitle.RightToLeft = selectedAlarm.Alarm.IsRtl ? RightToLeft.Yes : RightToLeft.Inherit;

            switch (selectedAlarm.Type)
            {
                case AlarmType.RTF:
                    rtbContent.Rtf = selectedAlarm.Alarm.Content;
                    rtbContent.Enabled = false;
                    break;

                case AlarmType.TextOnly:
                    rtbContent.Text = selectedAlarm.Alarm.Content;
                    rtbContent.Enabled = true;
                    break;
            }

            rtbContent.RightToLeft = selectedAlarm.Alarm.IsRtl ? RightToLeft.Yes : RightToLeft.Inherit;
            lbAlarmReceivedTime.Text = $"Received: {selectedAlarm.DateTime} ({humanizedLastSeen} ago)";
        }
    }
}
