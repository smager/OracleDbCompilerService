﻿using System;
using System.Windows.Forms;
namespace OracleDbCompilerService
{
    public partial class frmMain : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        public frmMain()
        {
            InitializeComponent();
            //new monitorSQLData();
            new monitorDirectory();
        }
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to stop the service?", "Nexgen Compiler Service",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.Close();
            }

        }
    }
}