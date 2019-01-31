using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PopupForm
{
    public partial class Frm_Main : Form
    {
        public Frm_Main()
        {
            InitializeComponent();
        }

        private void btn_Display_Click(object sender, EventArgs e)
        {
            FrmInfo.Instance().ShowForm();//显示窗体
        }

        private void btn_Hidden_Click(object sender, EventArgs e)
        {
            FrmInfo.Instance().CloseForm();//隐藏窗体
        }
    }
}
