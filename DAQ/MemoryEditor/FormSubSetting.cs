﻿using MemoryEditor.Station;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryEditor
{
    public partial class FormSubSetting : Form
    {
        public EventSection ParentNode { get; set; }

        public SubSetting CurrentNodeInfo { get; set; }

        public FormSubSetting(EventSection parent, SubSetting info = null)
        {
            this.CurrentNodeInfo = info;
            this.ParentNode = parent;

            InitializeComponent();
        }

        private void FormSubSetting_Load(object sender, EventArgs e)
        {
            if (this.CurrentNodeInfo != null)
            {
                this.txtEventName.Text = this.CurrentNodeInfo.EventName;
                this.txtMemoryBegin.Text = this.CurrentNodeInfo.MemoryBegin.ToString();
                this.txtMemoryEnd.Text = this.CurrentNodeInfo.MemoryEnd.ToString();
                this.txtDataSize.Text = this.CurrentNodeInfo.DataSize.ToString();
                this.txtDescription.Text = this.CurrentNodeInfo.Description;
            }
            else if (ParentNode != null)
            {

            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEventName.Text.Trim()) || string.IsNullOrEmpty(txtMemoryBegin.Text.Trim()) || string.IsNullOrEmpty(txtMemoryEnd.Text.Trim()))
            {
                MessageBox.Show("空值！");
                return;
            }

            if (this.CurrentNodeInfo == null)
            {
                this.CurrentNodeInfo = new SubSetting()
                {
                    EventName = txtEventName.Text.Trim(),
                    MemoryBegin = int.Parse(txtMemoryBegin.Text.Trim()),
                    MemoryEnd = int.Parse(txtMemoryEnd.Text.Trim()),
                    DataSize = int.Parse(txtMemoryEnd.Text.Trim()) - int.Parse(txtMemoryBegin.Text.Trim()) + 1,
                    Description = txtEventName.Text.Trim(),
                };
            }
            else
            {
                this.CurrentNodeInfo.EventName = txtEventName.Text.Trim();
                this.CurrentNodeInfo.MemoryBegin = int.Parse(txtMemoryBegin.Text.Trim());
                this.CurrentNodeInfo.MemoryEnd = int.Parse(txtMemoryEnd.Text.Trim());
                this.CurrentNodeInfo.DataSize = int.Parse(txtMemoryEnd.Text.Trim()) - int.Parse(txtMemoryBegin.Text.Trim()) + 1;
                this.CurrentNodeInfo.Description = txtEventName.Text.Trim();
            }

            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtMemoryEnd_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
