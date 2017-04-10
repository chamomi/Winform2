using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WinForms2
{
    public partial class Splash : Form
    {
        // Threading
        private static Splash ms_frmSplash = null;
        private static Thread ms_oThread = null;

        // Fade in and out.
        private double m_dblOpacityIncrement = .05;
        private double m_dblOpacityDecrement = .08;
        private const int TIMER_INTERVAL = 50;

        public Splash()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 0.0;
            timer1.Interval = TIMER_INTERVAL;
            timer1.Start();
            //this.ClientSize = this.BackgroundImage.Size;
        }

        private void Splash_Load(object sender, EventArgs e)
        {

        }

        static public void ShowSplash()
        {
            // Make sure it's only launched once.
            if (ms_frmSplash != null)
                return;
            ms_oThread = new Thread(new ThreadStart(Splash.ShowForm));
            ms_oThread.IsBackground = true;
            ms_oThread.SetApartmentState(ApartmentState.STA);
            ms_oThread.Start();
            while (ms_frmSplash == null || ms_frmSplash.IsHandleCreated == false)
            {
                System.Threading.Thread.Sleep(TIMER_INTERVAL);
            }
        }

        static private void ShowForm()
        {
            ms_frmSplash = new Splash();
            Application.Run(ms_frmSplash);
        }

        static public void CloseForm()
        {
            if (ms_frmSplash != null)
            {
                // Make it start going away.
                ms_frmSplash.m_dblOpacityIncrement = -ms_frmSplash.m_dblOpacityDecrement;
            }
            ms_oThread = null;  // we do not need these any more.
            ms_frmSplash = null;
            //ms_frmSplash.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_dblOpacityIncrement > 0)
            {
                if (this.Opacity < 1)

                    this.Opacity += m_dblOpacityIncrement;
            }
            else
            {
                if (this.Opacity > 0)
                    this.Opacity += m_dblOpacityIncrement;
                else
                    this.Close();
            }
        }
    }
}
