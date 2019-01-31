namespace SpyTool
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_TimeInterval = new System.Windows.Forms.TextBox();
            this.comboBox_Software = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSelectControl = new System.Windows.Forms.Button();
            this.buttonStepSelect = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tree_wnd = new System.Windows.Forms.TreeView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.imglst_icon = new System.Windows.Forms.ImageList(this.components);
            this.timerPolling = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Listen = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonRefresh);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox_TimeInterval);
            this.groupBox1.Controls.Add(this.comboBox_Software);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(2, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(869, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "选择检测软件：";
            // 
            // textBox_TimeInterval
            // 
            this.textBox_TimeInterval.Location = new System.Drawing.Point(738, 22);
            this.textBox_TimeInterval.Name = "textBox_TimeInterval";
            this.textBox_TimeInterval.Size = new System.Drawing.Size(61, 25);
            this.textBox_TimeInterval.TabIndex = 10;
            this.textBox_TimeInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxInteger_KeyPress);
            // 
            // comboBox_Software
            // 
            this.comboBox_Software.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Software.FormattingEnabled = true;
            this.comboBox_Software.Location = new System.Drawing.Point(145, 24);
            this.comboBox_Software.Name = "comboBox_Software";
            this.comboBox_Software.Size = new System.Drawing.Size(244, 23);
            this.comboBox_Software.TabIndex = 14;
            this.comboBox_Software.SelectedIndexChanged += new System.EventHandler(this.comboBox_Software_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(606, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "轮询间隔（秒）：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(2, 207);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(869, 315);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "监控列表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4,
            this.Column2,
            this.Column1,
            this.Column5});
            this.dataGridView1.Location = new System.Drawing.Point(12, 24);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(851, 285);
            this.dataGridView1.TabIndex = 14;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonSelectControl);
            this.groupBox3.Controls.Add(this.buttonStepSelect);
            this.groupBox3.Location = new System.Drawing.Point(460, 66);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(411, 135);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // buttonSelectControl
            // 
            this.buttonSelectControl.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSelectControl.Location = new System.Drawing.Point(17, 90);
            this.buttonSelectControl.Name = "buttonSelectControl";
            this.buttonSelectControl.Size = new System.Drawing.Size(176, 30);
            this.buttonSelectControl.TabIndex = 20;
            this.buttonSelectControl.Text = "采集控件";
            this.buttonSelectControl.UseVisualStyleBackColor = true;
            this.buttonSelectControl.Click += new System.EventHandler(this.buttonSelectControl_Click);
            // 
            // buttonStepSelect
            // 
            this.buttonStepSelect.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonStepSelect.Location = new System.Drawing.Point(17, 27);
            this.buttonStepSelect.Name = "buttonStepSelect";
            this.buttonStepSelect.Size = new System.Drawing.Size(188, 30);
            this.buttonStepSelect.TabIndex = 16;
            this.buttonStepSelect.Text = "采集并编辑样本状态";
            this.buttonStepSelect.UseVisualStyleBackColor = true;
            this.buttonStepSelect.Click += new System.EventHandler(this.buttonStepSelect_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(81, 27);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(316, 25);
            this.textBox4.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "句柄：";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(81, 92);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(316, 25);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "标题：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(81, 58);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(316, 25);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "类名：";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tree_wnd
            // 
            this.tree_wnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree_wnd.Location = new System.Drawing.Point(3, 21);
            this.tree_wnd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tree_wnd.Name = "tree_wnd";
            this.tree_wnd.Size = new System.Drawing.Size(29, 0);
            this.tree_wnd.TabIndex = 3;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tree_wnd);
            this.groupBox5.Location = new System.Drawing.Point(1081, 414);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(35, 14);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            // 
            // imglst_icon
            // 
            this.imglst_icon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglst_icon.ImageStream")));
            this.imglst_icon.TransparentColor = System.Drawing.Color.Transparent;
            this.imglst_icon.Images.SetKeyName(0, "bitbug_favicon.ico");
            this.imglst_icon.Images.SetKeyName(1, "Device.png");
            this.imglst_icon.Images.SetKeyName(2, "Section.png");
            this.imglst_icon.Images.SetKeyName(3, "Station.png");
            // 
            // timerPolling
            // 
            this.timerPolling.Interval = 2000;
            this.timerPolling.Tick += new System.EventHandler(this.timerPolling_Tick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Listen);
            this.groupBox4.Controls.Add(this.Cancel);
            this.groupBox4.Location = new System.Drawing.Point(2, 519);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(869, 65);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            // 
            // Listen
            // 
            this.Listen.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Listen.Location = new System.Drawing.Point(262, 24);
            this.Listen.Name = "Listen";
            this.Listen.Size = new System.Drawing.Size(110, 30);
            this.Listen.TabIndex = 12;
            this.Listen.Text = "开始检测";
            this.Listen.UseVisualStyleBackColor = true;
            this.Listen.Click += new System.EventHandler(this.Listen_Click);
            // 
            // Cancel
            // 
            this.Cancel.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Cancel.Location = new System.Drawing.Point(526, 24);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(114, 30);
            this.Cancel.TabIndex = 13;
            this.Cancel.Text = "暂停检测";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.textBox1);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.textBox4);
            this.groupBox6.Controls.Add(this.textBox2);
            this.groupBox6.Location = new System.Drawing.Point(2, 67);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(431, 134);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "控件名称";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "控件类型";
            this.Column4.Name = "Column4";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "详细信息";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 200;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "窗体标识";
            this.Column1.Name = "Column1";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "父控件";
            this.Column5.Name = "Column5";
            this.Column5.Width = 150;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Image = global::SpyTool.Properties.Resources.refresh;
            this.buttonRefresh.Location = new System.Drawing.Point(399, 18);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(39, 36);
            this.buttonRefresh.TabIndex = 16;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 585);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "实时动态监测软件";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_TimeInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView tree_wnd;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ImageList imglst_icon;
        private System.Windows.Forms.Timer timerPolling;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBox_Software;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonStepSelect;
        private System.Windows.Forms.Button buttonSelectControl;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Listen;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Button buttonRefresh;
    }
}

