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
    public partial class Form1 : Form
    {
        int n = 8;
        Color[,] bgColors;
        //private Splash splashScreen;
        ToolStripMenuItem left;//, gr, w;
        public Form1()
        {
            //SplashScreen();
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            bgColors = new Color[n, n];
            newGameToolStripMenuItem.PerformClick();
        }

        //private void SplashScreen()
        //{
        //    splashScreen = new Splash();
        //    Thread thread = new Thread(new ThreadStart(SplashStart));
        //    thread.Start();

        //    Thread.Sleep(3000);

        //    thread.Abort();
        //}

        //private void SplashStart()
        //{
        //    Application.Run(splashScreen);
        //}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (CloseCancel() == false)
                e.Cancel = true;
        }

        public static bool CloseCancel()
        {
            var result = MessageBox.Show("Close app?", "Exit",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var set = new Settings();
            set.ShowDialog();
            int prevn = n;
            n = Settings.num;
            if (n > prevn)
            {
                while (prevn < n)
                {
                    tableLayoutPanel1.ColumnCount++;
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
                    tableLayoutPanel1.RowCount++;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12F));
                    prevn++;
                }
            }
            else
            {
                while (prevn > n)
                {
                    tableLayoutPanel1.ColumnStyles.RemoveAt(prevn - 1);
                    tableLayoutPanel1.ColumnCount--;
                    tableLayoutPanel1.RowStyles.RemoveAt(prevn - 1);
                    tableLayoutPanel1.RowCount--;
                    prevn--;
                }
            }
            bgColors = new Color[n, n];
            if (Settings.f == 1) newGameToolStripMenuItem.PerformClick();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int a;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a = r.Next(1, 3);
                    if (a == 1)
                    {
                        bgColors[i, j] = Color.ForestGreen;
                    }
                    else
                    {
                        bgColors[i, j] = Color.Maroon;
                    }
                }
            }
            tableLayoutPanel1.Refresh();

        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            using (var b = new SolidBrush(bgColors[e.Column, e.Row]))
            {
                e.Graphics.FillRectangle(b, e.CellBounds);
            }
        }

        private void editModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editModeToolStripMenuItem.Text == "Edit mode")
            {
                editModeToolStripMenuItem.Text = "Game mode";
                menuStrip1.BackColor = Color.CornflowerBlue;
                left = new ToolStripMenuItem();
                left.Text = "Left click";
                ToolStripMenuItem gr = new ToolStripMenuItem();
                ToolStripMenuItem w = new ToolStripMenuItem();
                gr.Text = "Grass";
                gr.Click += grassToolStripMenuItem_Click;
                w.Text = "Wall";
                w.Click += wallToolStripMenuItem_Click;
                w.Checked = true;
                left.DropDownItems.Add(gr);
                left.DropDownItems.Add(w);
                menuStrip1.Items.Add(left);
            }
            else
            {
                editModeToolStripMenuItem.Text = "Edit mode";
                menuStrip1.BackColor = Color.Transparent;
                menuStrip1.Items.Remove(left);
            }
        }

        private void grassToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            ((ToolStripMenuItem)left.DropDownItems[0]).Checked = true;
            ((ToolStripMenuItem)left.DropDownItems[1]).Checked = false;
        }
        private void wallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)left.DropDownItems[1]).Checked = true;
            ((ToolStripMenuItem)left.DropDownItems[0]).Checked = false;
        }

        private void tableLayoutPanel1_MouseClick(object sender, EventArgs e)
        {
            Point p = tableLayoutPanel1.PointToClient(Cursor.Position);
            if (((ToolStripMenuItem)left.DropDownItems[0]).Checked == true) bgColors[getColumn(p), getRow(p)] = Color.ForestGreen;
            if (((ToolStripMenuItem)left.DropDownItems[1]).Checked == true) bgColors[getColumn(p), getRow(p)] = Color.Maroon;
            tableLayoutPanel1.Refresh();
        }

        public int getColumn(Point point)
        {
            int w = tableLayoutPanel1.Width;
            int[] widths = tableLayoutPanel1.GetColumnWidths();

            int i;
            for (i = widths.Length - 1; i >= 0 && point.X < w; i--)
                w -= widths[i];
            return i+1;
        }
        public int getRow(Point point)
        {
            int h = tableLayoutPanel1.Height;
            int i;
            int[] heights = tableLayoutPanel1.GetRowHeights();
            for (i = heights.Length - 1; i >= 0 && point.Y < h; i--)
                h -= heights[i];
            return i+1;
        }
    }
}
