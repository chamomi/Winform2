using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms2
{
    public partial class Form1 : Form
    {
        int n = 8;
        Color[,] bgColors;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            bgColors = new Color[n, n];
            newGameToolStripMenuItem.PerformClick();
        }

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
            if(n>prevn)
            {
                while(prevn<n)
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
                    tableLayoutPanel1.ColumnStyles.RemoveAt(prevn-1);
                    tableLayoutPanel1.ColumnCount--;
                    tableLayoutPanel1.RowStyles.RemoveAt(prevn-1);
                    tableLayoutPanel1.RowCount--;
                    prevn--;
                }
            }
            bgColors = new Color[n, n];
            if(Settings.f==1) newGameToolStripMenuItem.PerformClick();
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
    }
}
