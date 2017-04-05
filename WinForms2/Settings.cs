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
    public partial class Settings : Form
    {
        public static int num=8, f=0;
        public Settings()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            comboBox1.Items.Add("8x8");
            comboBox1.Items.Add("10x10");
            comboBox1.Items.Add("12x12");
            comboBox1.SelectedIndex = 0;
            f = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f = 1;
            string[] text = this.comboBox1.GetItemText(this.comboBox1.SelectedItem).Split('x');
            num = Int32.Parse(text[0]);
            this.Close();
        }
    }
}
