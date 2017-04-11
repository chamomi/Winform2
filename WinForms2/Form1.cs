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
        ToolStripMenuItem left;
        Bitmap knightImage;
        Bitmap reversedKnightImage;
        Bitmap keyImage;
        Bitmap openDoorImage;
        Bitmap closedDoorImage;
        private int knight_x = 0;
        private int knight_y = 0;
        private int key_x = 0;
        private int key_y = 0;
        private int door_x = 0;
        private int door_y = 0;
        private bool reversed = false, opened = false, iskey = true;
        public Form1()
        {
            Splash.Show(3000);
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            bgColors = new Color[n, n];
            KeyDown += new KeyEventHandler(Form1_KeyPress);
            
            newGameToolStripMenuItem.PerformClick();
        }

        private void Form1_KeyPress(object sender, KeyEventArgs e)
        {
            if (editModeToolStripMenuItem.Text == "Edit mode")
            {
                Point kp = new Point(knight_x, knight_y);
                Point keyp = new Point(key_x, key_y);
                Point dp = new Point(door_x, door_y);
                switch (e.KeyValue)
                {
                    case (char)Keys.Up:
                        if (knight_y > 0) kp = new Point(knight_x, knight_y-1);
                        if ((knight_y>0)&&(bgColors[knight_x, knight_y - 1] == Color.ForestGreen))
                            if((opened== true)||(dp!=kp)) knight_y--;
                        if((knight_x==key_x)&&(knight_y==key_y))
                        {
                            opened = true;
                            iskey = false;
                        }
                        if ((knight_x == door_x) && (knight_y == door_y))
                        {
                            newGameToolStripMenuItem.PerformClick();
                        }
                        break;
                    case (char)Keys.Right:
                        reversed = false;
                        if (knight_x < (n - 1)) kp = new Point(knight_x+1, knight_y);
                        if ((knight_x< (n-1))&&(bgColors[knight_x + 1, knight_y] == Color.ForestGreen))
                            if ((opened == true) || (dp != kp)) knight_x++;
                        if ((knight_x == key_x) && (knight_y == key_y))
                        {
                            opened = true;
                            iskey = false;
                        }
                        if ((knight_x == door_x) && (knight_y == door_y))
                        {
                            newGameToolStripMenuItem.PerformClick();
                        }
                        break;
                    case (char)Keys.Down:
                        if (knight_y < (n - 1)) kp = new Point(knight_x, knight_y+1);
                        if ((knight_y<(n-1))&&(bgColors[knight_x, knight_y + 1] == Color.ForestGreen))
                            if ((opened == true) || (dp != kp)) knight_y++;
                        if ((knight_x == key_x) && (knight_y == key_y))
                        {
                            opened = true;
                            iskey = false;
                        }
                        if ((knight_x == door_x) && (knight_y == door_y))
                        {
                            newGameToolStripMenuItem.PerformClick();
                        }
                        break;
                    case (char)Keys.Left:
                        reversed = true;
                        if (knight_x > 0) kp = new Point(knight_x-1, knight_y);
                        if ((knight_x>0)&&(bgColors[knight_x - 1, knight_y] == Color.ForestGreen))
                            if ((opened == true) || (dp != kp)) knight_x--;
                        if ((knight_x == key_x) && (knight_y == key_y))
                        {
                            opened = true;
                            iskey = false;
                        }
                        if ((knight_x == door_x) && (knight_y == door_y))
                        {
                            newGameToolStripMenuItem.PerformClick();
                        }
                        break;
                    case (char)Keys.Space:
                        if (knight_x - 1 >= 0) bgColors[knight_x - 1, knight_y] = Color.ForestGreen;
                        if (knight_x + 1 < n) bgColors[knight_x + 1, knight_y] = Color.ForestGreen;
                        if (knight_y + 1 < n) bgColors[knight_x, knight_y + 1] = Color.ForestGreen;
                        if (knight_y - 1 >=0) bgColors[knight_x, knight_y - 1] = Color.ForestGreen;
                        break;
                }

                tableLayoutPanel1.Invalidate();
            }
        }

        public void LoadStaticFiles()
        {
            knightImage = Properties.Resources.knight;
            reversedKnightImage = Properties.Resources.knight2;
            keyImage = Properties.Resources.key2;
            openDoorImage = Properties.Resources.opened_door;
            closedDoorImage = Properties.Resources.closed_door;
            knightImage.MakeTransparent();
            reversedKnightImage.MakeTransparent();
            keyImage.MakeTransparent();
            openDoorImage.MakeTransparent();
            closedDoorImage.MakeTransparent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Splash.Hide();
            Show();
            LoadStaticFiles();
            Activate();
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
            reversed = false;
            opened = false;
            iskey = true;
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
            while(true)
            {
                int i = r.Next(0, n),j = r.Next(0, n);
                if (bgColors[i, j]==Color.ForestGreen)
                {
                    knight_x = i;
                    knight_y = j;
                    break;
                }
            }
            while (true)
            {
                int i = r.Next(0, n), j = r.Next(0, n);
                if ((bgColors[i, j] == Color.ForestGreen)&&(i!=knight_x)&&(j!=knight_y))
                {
                    key_x = i;
                    key_y = j;
                    break;
                }
            }
            while (true)
            {
                int i = r.Next(0, n), j = r.Next(0, n);
                if ((bgColors[i, j] == Color.ForestGreen) && (i != knight_x) && (j != knight_y)&& (i != key_x) && (j != key_y))
                {
                    door_x = i;
                    door_y = j;
                    break;
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
            if (e.Column == knight_x && e.Row == knight_y && reversed == false)
            {
                e.Graphics.DrawImageUnscaledAndClipped(Properties.Resources.knight, e.CellBounds);
            }
            if (e.Column == knight_x && e.Row == knight_y && reversed == true)
            {
                e.Graphics.DrawImageUnscaledAndClipped(Properties.Resources.knight2, e.CellBounds);
            }
            if (e.Column == key_x && e.Row == key_y && iskey == true)
                e.Graphics.DrawImageUnscaledAndClipped(Properties.Resources.key2, e.CellBounds);
            if (e.Column == door_x && e.Row == door_y && opened == true)
            {
                e.Graphics.DrawImageUnscaledAndClipped(Properties.Resources.opened_door, e.CellBounds);
            }
            if (e.Column == door_x && e.Row == door_y && opened == false)
            {
                e.Graphics.DrawImageUnscaledAndClipped(Properties.Resources.closed_door, e.CellBounds);
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
            if (editModeToolStripMenuItem.Text == "Game mode")
            {
                Point p = tableLayoutPanel1.PointToClient(Cursor.Position);
                if (((ToolStripMenuItem)left.DropDownItems[0]).Checked == true) bgColors[getColumn(p), getRow(p)] = Color.ForestGreen;
                if (((ToolStripMenuItem)left.DropDownItems[1]).Checked == true) bgColors[getColumn(p), getRow(p)] = Color.Maroon;
                tableLayoutPanel1.Refresh();
            }
        }

        private void tableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //edit mode right click
            }
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
