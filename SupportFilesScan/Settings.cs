using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SupportFilesScan
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < tabControl1.TabCount; x++)
            {
                listBox1.Items.Add(tabControl1.TabPages[x].Text);
            }
        }
    }
}
