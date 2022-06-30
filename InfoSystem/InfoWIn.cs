using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InfoSystem
{
    public partial class InfoWIn : Form
    {
        public InfoWIn()
        {
            InitializeComponent();
        }

        public InfoWIn(string info, Form winObj=null)
        {
            InitializeComponent();
            this.richTextBox1.Text = info;
            winObj.Invoke(new MethodInvoker(delegate { winObj.Close(); }));
            this.Focus();
        }
    }
}
