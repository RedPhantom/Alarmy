using AlarmyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
    }
}
