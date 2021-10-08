using AlarmyLib;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Alarmy
{
    internal partial class frmAlarm : Form
    {
        private readonly string IndividualAlarmUrl = $@"http://{Properties.Settings.Default.ServiceURL}/Alarms/";
        private Alarm _alarm;

        public frmAlarm()
        {
            InitializeComponent();
        }

        private void frmAlarm_Load(object sender, EventArgs e)
        {
            Text = $"Alarmy v{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        internal void LoadAlarm(Alarm alarm, AlarmType type)
        {
            if (alarm.IsRtl)
            {
                RightToLeft = RightToLeft.Yes;
            }

            lblTitle.Text = alarm.Title;

            switch (type)
            {
                case AlarmType.RTF:
                    rtbContent.Rtf = alarm.Content;

                    // Prevent interaction with possible binary objects in the RTF data.
                    rtbContent.Enabled = false;
                    break;

                case AlarmType.TextOnly:
                    rtbContent.Text = alarm.Content;
                    rtbContent.Enabled = true;
                    break;
                default:
                    break;
            }

            _alarm = alarm;
        }

        private void btnValidateAuthenticity_MouseHover(object sender, EventArgs e)
        {
        }

        private void tmrShowNotification_Tick(object sender, EventArgs e)
        {
            string trimmedMessage;

            if (rtbContent.Text.Length <= 30)
            {
                trimmedMessage = rtbContent.Text;
            }
            else
            {
                trimmedMessage = rtbContent.Text.Substring(0, 30);
            }

            notifyIcon1.BalloonTipTitle = lblTitle.Text;
            notifyIcon1.BalloonTipText = trimmedMessage;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Text = trimmedMessage;
            notifyIcon1.ShowBalloonTip(0);
            tmrShowNotification.Enabled = false;
        }

        private void btnValidateAuthenticity_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9\-_]+$");
            if (rgx.IsMatch(_alarm.ID))
            {
                System.Diagnostics.Process.Start(IndividualAlarmUrl + _alarm.ID);
            }
            else
            {
                MessageBox.Show($"Alarm possibly compromised: ID = {_alarm.ID}");
            }
        }
    }
}
