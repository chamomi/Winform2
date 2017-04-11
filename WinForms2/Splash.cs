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
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace WinForms2
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Splash_MouseDown);

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(new Rectangle(0, 0, 300, 300));
            Region = new Region(path);
        }

        private void Splash_Load(object sender, EventArgs e)
        {
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Splash_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        ////http://stackoverflow.com/questions/1364115/a-different-requirement-for-a-splash-screen-in-winforms-app  
        public static void Show(int fadeTimeInMilliseconds)
        {
            if (_instance == null)
            {
                _fadeTime = fadeTimeInMilliseconds;
                _instance = new Splash();

                _instance.Opacity = 0;
                ((Form)_instance).Show();

                Application.DoEvents();

                if (_fadeTime > 0)
                {
                    int fadeStep = (int)Math.Round((double)_fadeTime / 40);
                    _instance.timer1.Interval = fadeStep;
                    double step = 0.05;
                    for (int ii = 0; ii <= _fadeTime / 2; ii += fadeStep)
                    {
                        Thread.Sleep(fadeStep);
                        _instance.Opacity += step;
                    }
                    step = -0.05;
                    for (int ii = _fadeTime / 2; ii <= _fadeTime; ii += fadeStep)
                    {
                        Thread.Sleep(fadeStep);
                        _instance.Opacity += step;
                    }
                }
                else
                {
                    _instance.timer1.Tag = new object();
                }

                _instance.Opacity = 1;
            }
        }

        public new static void Hide()
        {
            if (_instance != null)
            {
                _instance.BeginInvoke(new MethodInvoker(_instance.Close));

                Application.DoEvents();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (Application.OpenForms.Count > 1)
            {
                if (Opacity > 0)
                {
                    Opacity -= 0.05;
                    timer1.Start();
                }
                else
                {
                    timer1.Stop();
                    e.Cancel = false;
                    _instance = null;
                }
            }
            else
            {
                if (Opacity > 0)
                {
                    Thread.Sleep(timer1.Interval);
                    Opacity -= 0.05;
                    Close();
                }
                else
                {
                    e.Cancel = false;
                    _instance = null;
                }
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            Close();
        }

        private static Splash _instance = null;
        private static int _fadeTime = 0;

    }
}

