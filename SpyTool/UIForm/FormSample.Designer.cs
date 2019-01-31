namespace SpyTool.UIForm
{
    partial class FormSample
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonLoadSample = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gpbValue = new System.Windows.Forms.GroupBox();
            this.txtStatusName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHSB = new System.Windows.Forms.TextBox();
            this.txtARGB = new System.Windows.Forms.TextBox();
            this.txtRGB = new System.Windows.Forms.TextBox();
            this.lblCMYK = new System.Windows.Forms.Label();
            this.lblARGB = new System.Windows.Forms.Label();
            this.lblRGB = new System.Windows.Forms.Label();
            this.gpbColorStack = new System.Windows.Forms.GroupBox();
            this.lblColor7 = new System.Windows.Forms.Label();
            this.lblColor6 = new System.Windows.Forms.Label();
            this.lblColor5 = new System.Windows.Forms.Label();
            this.lblColor4 = new System.Windows.Forms.Label();
            this.lblColor3 = new System.Windows.Forms.Label();
            this.lblColor2 = new System.Windows.Forms.Label();
            this.lblColor1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSelectColor = new System.Windows.Forms.Button();
            this.btnPickColor = new System.Windows.Forms.Button();
            this.lblColor0 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ignore = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewText = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorDlg = new System.Windows.Forms.ColorDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gpbValue.SuspendLayout();
            this.gpbColorStack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewText)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 585);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonLoadSample);
            this.groupBox2.Controls.Add(this.buttonSave);
            this.groupBox2.Location = new System.Drawing.Point(8, 530);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(560, 44);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            // 
            // buttonLoadSample
            // 
            this.buttonLoadSample.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLoadSample.Location = new System.Drawing.Point(312, 12);
            this.buttonLoadSample.Name = "buttonLoadSample";
            this.buttonLoadSample.Size = new System.Drawing.Size(175, 30);
            this.buttonLoadSample.TabIndex = 15;
            this.buttonLoadSample.Text = "加载样本";
            this.buttonLoadSample.UseVisualStyleBackColor = true;
            this.buttonLoadSample.Click += new System.EventHandler(this.buttonLoadSample_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSave.Location = new System.Drawing.Point(57, 12);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(175, 30);
            this.buttonSave.TabIndex = 14;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 21);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(571, 510);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gpbValue);
            this.tabPage1.Controls.Add(this.gpbColorStack);
            this.tabPage1.Controls.Add(this.btnClose);
            this.tabPage1.Controls.Add(this.btnSelectColor);
            this.tabPage1.Controls.Add(this.btnPickColor);
            this.tabPage1.Controls.Add(this.lblColor0);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.buttonAdd);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(563, 481);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图片颜色采集";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gpbValue
            // 
            this.gpbValue.Controls.Add(this.txtStatusName);
            this.gpbValue.Controls.Add(this.label1);
            this.gpbValue.Controls.Add(this.txtHSB);
            this.gpbValue.Controls.Add(this.txtARGB);
            this.gpbValue.Controls.Add(this.txtRGB);
            this.gpbValue.Controls.Add(this.lblCMYK);
            this.gpbValue.Controls.Add(this.lblARGB);
            this.gpbValue.Controls.Add(this.lblRGB);
            this.gpbValue.Location = new System.Drawing.Point(289, 5);
            this.gpbValue.Margin = new System.Windows.Forms.Padding(4);
            this.gpbValue.Name = "gpbValue";
            this.gpbValue.Padding = new System.Windows.Forms.Padding(4);
            this.gpbValue.Size = new System.Drawing.Size(265, 121);
            this.gpbValue.TabIndex = 13;
            this.gpbValue.TabStop = false;
            this.gpbValue.Text = "颜色值";
            // 
            // txtStatusName
            // 
            this.txtStatusName.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusName.Location = new System.Drawing.Point(92, 86);
            this.txtStatusName.Margin = new System.Windows.Forms.Padding(4);
            this.txtStatusName.Name = "txtStatusName";
            this.txtStatusName.Size = new System.Drawing.Size(159, 20);
            this.txtStatusName.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "状态名称：";
            // 
            // txtHSB
            // 
            this.txtHSB.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHSB.Location = new System.Drawing.Point(92, 62);
            this.txtHSB.Margin = new System.Windows.Forms.Padding(4);
            this.txtHSB.Name = "txtHSB";
            this.txtHSB.ReadOnly = true;
            this.txtHSB.Size = new System.Drawing.Size(159, 20);
            this.txtHSB.TabIndex = 5;
            // 
            // txtARGB
            // 
            this.txtARGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtARGB.Location = new System.Drawing.Point(92, 40);
            this.txtARGB.Margin = new System.Windows.Forms.Padding(4);
            this.txtARGB.Name = "txtARGB";
            this.txtARGB.ReadOnly = true;
            this.txtARGB.Size = new System.Drawing.Size(159, 20);
            this.txtARGB.TabIndex = 3;
            // 
            // txtRGB
            // 
            this.txtRGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRGB.Location = new System.Drawing.Point(92, 18);
            this.txtRGB.Margin = new System.Windows.Forms.Padding(4);
            this.txtRGB.Name = "txtRGB";
            this.txtRGB.ReadOnly = true;
            this.txtRGB.Size = new System.Drawing.Size(159, 20);
            this.txtRGB.TabIndex = 1;
            // 
            // lblCMYK
            // 
            this.lblCMYK.AutoSize = true;
            this.lblCMYK.Location = new System.Drawing.Point(32, 66);
            this.lblCMYK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCMYK.Name = "lblCMYK";
            this.lblCMYK.Size = new System.Drawing.Size(39, 15);
            this.lblCMYK.TabIndex = 4;
            this.lblCMYK.Text = "HSB:";
            // 
            // lblARGB
            // 
            this.lblARGB.AutoSize = true;
            this.lblARGB.Location = new System.Drawing.Point(21, 44);
            this.lblARGB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblARGB.Name = "lblARGB";
            this.lblARGB.Size = new System.Drawing.Size(47, 15);
            this.lblARGB.TabIndex = 2;
            this.lblARGB.Text = "ARGB:";
            // 
            // lblRGB
            // 
            this.lblRGB.AutoSize = true;
            this.lblRGB.Location = new System.Drawing.Point(31, 21);
            this.lblRGB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRGB.Name = "lblRGB";
            this.lblRGB.Size = new System.Drawing.Size(39, 15);
            this.lblRGB.TabIndex = 0;
            this.lblRGB.Text = "RGB:";
            // 
            // gpbColorStack
            // 
            this.gpbColorStack.Controls.Add(this.lblColor7);
            this.gpbColorStack.Controls.Add(this.lblColor6);
            this.gpbColorStack.Controls.Add(this.lblColor5);
            this.gpbColorStack.Controls.Add(this.lblColor4);
            this.gpbColorStack.Controls.Add(this.lblColor3);
            this.gpbColorStack.Controls.Add(this.lblColor2);
            this.gpbColorStack.Controls.Add(this.lblColor1);
            this.gpbColorStack.Location = new System.Drawing.Point(16, 140);
            this.gpbColorStack.Margin = new System.Windows.Forms.Padding(4);
            this.gpbColorStack.Name = "gpbColorStack";
            this.gpbColorStack.Padding = new System.Windows.Forms.Padding(4);
            this.gpbColorStack.Size = new System.Drawing.Size(265, 56);
            this.gpbColorStack.TabIndex = 12;
            this.gpbColorStack.TabStop = false;
            this.gpbColorStack.Text = "颜色栈";
            // 
            // lblColor7
            // 
            this.lblColor7.BackColor = System.Drawing.Color.White;
            this.lblColor7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor7.Location = new System.Drawing.Point(224, 20);
            this.lblColor7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor7.Name = "lblColor7";
            this.lblColor7.Size = new System.Drawing.Size(27, 25);
            this.lblColor7.TabIndex = 6;
            this.lblColor7.Click += new System.EventHandler(this.lblColor_Click);
            this.lblColor7.DoubleClick += new System.EventHandler(this.lblColor_DoubleClick);
            // 
            // lblColor6
            // 
            this.lblColor6.BackColor = System.Drawing.Color.White;
            this.lblColor6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor6.Location = new System.Drawing.Point(189, 20);
            this.lblColor6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor6.Name = "lblColor6";
            this.lblColor6.Size = new System.Drawing.Size(27, 25);
            this.lblColor6.TabIndex = 5;
            this.lblColor6.Click += new System.EventHandler(this.lblColor_Click);
            this.lblColor6.DoubleClick += new System.EventHandler(this.lblColor_DoubleClick);
            // 
            // lblColor5
            // 
            this.lblColor5.BackColor = System.Drawing.Color.White;
            this.lblColor5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor5.Location = new System.Drawing.Point(155, 20);
            this.lblColor5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor5.Name = "lblColor5";
            this.lblColor5.Size = new System.Drawing.Size(27, 25);
            this.lblColor5.TabIndex = 4;
            this.lblColor5.Click += new System.EventHandler(this.lblColor_Click);
            this.lblColor5.DoubleClick += new System.EventHandler(this.lblColor_DoubleClick);
            // 
            // lblColor4
            // 
            this.lblColor4.BackColor = System.Drawing.Color.White;
            this.lblColor4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor4.Location = new System.Drawing.Point(120, 20);
            this.lblColor4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor4.Name = "lblColor4";
            this.lblColor4.Size = new System.Drawing.Size(27, 25);
            this.lblColor4.TabIndex = 3;
            this.lblColor4.Click += new System.EventHandler(this.lblColor_Click);
            this.lblColor4.DoubleClick += new System.EventHandler(this.lblColor_DoubleClick);
            // 
            // lblColor3
            // 
            this.lblColor3.BackColor = System.Drawing.Color.White;
            this.lblColor3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor3.Location = new System.Drawing.Point(85, 20);
            this.lblColor3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor3.Name = "lblColor3";
            this.lblColor3.Size = new System.Drawing.Size(27, 25);
            this.lblColor3.TabIndex = 2;
            this.lblColor3.Click += new System.EventHandler(this.lblColor_Click);
            this.lblColor3.DoubleClick += new System.EventHandler(this.lblColor_DoubleClick);
            // 
            // lblColor2
            // 
            this.lblColor2.BackColor = System.Drawing.Color.White;
            this.lblColor2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor2.Location = new System.Drawing.Point(51, 20);
            this.lblColor2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor2.Name = "lblColor2";
            this.lblColor2.Size = new System.Drawing.Size(27, 25);
            this.lblColor2.TabIndex = 1;
            this.lblColor2.Click += new System.EventHandler(this.lblColor_Click);
            this.lblColor2.DoubleClick += new System.EventHandler(this.lblColor_DoubleClick);
            // 
            // lblColor1
            // 
            this.lblColor1.BackColor = System.Drawing.Color.White;
            this.lblColor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor1.Location = new System.Drawing.Point(16, 20);
            this.lblColor1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor1.Name = "lblColor1";
            this.lblColor1.Size = new System.Drawing.Size(27, 25);
            this.lblColor1.TabIndex = 0;
            this.lblColor1.Click += new System.EventHandler(this.lblColor_Click);
            this.lblColor1.DoubleClick += new System.EventHandler(this.lblColor_DoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(200, 73);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(81, 29);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSelectColor
            // 
            this.btnSelectColor.Location = new System.Drawing.Point(200, 42);
            this.btnSelectColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectColor.Name = "btnSelectColor";
            this.btnSelectColor.Size = new System.Drawing.Size(81, 29);
            this.btnSelectColor.TabIndex = 9;
            this.btnSelectColor.Text = "调色板";
            this.btnSelectColor.UseVisualStyleBackColor = true;
            this.btnSelectColor.Click += new System.EventHandler(this.btnSelectColor_Click);
            // 
            // btnPickColor
            // 
            this.btnPickColor.Location = new System.Drawing.Point(200, 12);
            this.btnPickColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnPickColor.Name = "btnPickColor";
            this.btnPickColor.Size = new System.Drawing.Size(81, 29);
            this.btnPickColor.TabIndex = 8;
            this.btnPickColor.Text = "取色";
            this.btnPickColor.UseVisualStyleBackColor = true;
            this.btnPickColor.Click += new System.EventHandler(this.btnPickColor_Click);
            // 
            // lblColor0
            // 
            this.lblColor0.BackColor = System.Drawing.Color.White;
            this.lblColor0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor0.Location = new System.Drawing.Point(16, 12);
            this.lblColor0.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor0.Name = "lblColor0";
            this.lblColor0.Size = new System.Drawing.Size(173, 121);
            this.lblColor0.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Ignore});
            this.dataGridView1.Location = new System.Drawing.Point(3, 212);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(551, 266);
            this.dataGridView1.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "样本名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 160;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "状态名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Ignore
            // 
            this.Ignore.FalseValue = "false";
            this.Ignore.HeaderText = "是否忽略";
            this.Ignore.Name = "Ignore";
            this.Ignore.TrueValue = "true";
            // 
            // buttonAdd
            // 
            this.buttonAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonAdd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonAdd.Location = new System.Drawing.Point(352, 143);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(150, 60);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "添加";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridViewText);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(563, 481);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "文本采集";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewText
            // 
            this.dataGridViewText.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridViewText.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewText.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3});
            this.dataGridViewText.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewText.Name = "dataGridViewText";
            this.dataGridViewText.RowTemplate.Height = 27;
            this.dataGridViewText.Size = new System.Drawing.Size(552, 469);
            this.dataGridViewText.TabIndex = 0;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "状态内容";
            this.Column3.Name = "Column3";
            // 
            // FormSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 585);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSample";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormSample";
            this.Load += new System.EventHandler(this.FormSample_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gpbValue.ResumeLayout(false);
            this.gpbValue.PerformLayout();
            this.gpbColorStack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewText)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridViewText;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.GroupBox gpbValue;
        private System.Windows.Forms.TextBox txtHSB;
        private System.Windows.Forms.TextBox txtARGB;
        private System.Windows.Forms.TextBox txtRGB;
        private System.Windows.Forms.Label lblCMYK;
        private System.Windows.Forms.Label lblARGB;
        private System.Windows.Forms.Label lblRGB;
        private System.Windows.Forms.GroupBox gpbColorStack;
        private System.Windows.Forms.Label lblColor7;
        private System.Windows.Forms.Label lblColor6;
        private System.Windows.Forms.Label lblColor5;
        private System.Windows.Forms.Label lblColor4;
        private System.Windows.Forms.Label lblColor3;
        private System.Windows.Forms.Label lblColor2;
        private System.Windows.Forms.Label lblColor1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSelectColor;
        private System.Windows.Forms.Button btnPickColor;
        private System.Windows.Forms.Label lblColor0;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ColorDialog colorDlg;
        private System.Windows.Forms.TextBox txtStatusName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonLoadSample;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Ignore;
    }
}