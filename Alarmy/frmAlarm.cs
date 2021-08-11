using AlarmyLib;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Alarmy
{
    public partial class frmAlarm : Form
    {
        public frmAlarm()
        {
            InitializeComponent();
        }

        private void frmAlarm_Load(object sender, EventArgs e)
        {
            Text = string.Format("Alarmy v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        public void LoadAlarm(Alarm alarm)
        {
            if (alarm.IsRtl)
            {
                RightToLeft = RightToLeft.Yes;
            }

            lblTitle.Text = alarm.Title;
            rtbContent.Rtf = alarm.Content;
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
    }
}
