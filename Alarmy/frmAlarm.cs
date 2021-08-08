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
            rtbContent.Text = alarm.Content;
        }

        private void btnValidateAuthenticity_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("View this message online.", this, MousePosition);
        }
    }
}
