namespace SpyTool.UIForm
{
    partial class FormCollectSample
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.picButton2 = new SpyTool.PicButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_PicType = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_PicName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picButton1 = new SpyTool.PicButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewText = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picButton1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewText)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1003, 608);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 21);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(997, 584);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.picButton2);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.buttonSave);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(989, 555);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图片采集";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // picButton2
            // 
            this.picButton2.Location = new System.Drawing.Point(28, 17);
            this.picButton2.Name = "picButton2";
            this.picButton2.Size = new System.Drawing.Size(364, 280);
            this.picButton2.TabIndex = 6;
            this.picButton2.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Location = new System.Drawing.Point(3, 337);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(983, 210);
            this.dataGridView1.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "图片名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "状态类型";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSave.Location = new System.Drawing.Point(778, 281);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 30);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox_PicType);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.textBox_PicName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(532, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(421, 219);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // textBox_PicType
            // 
            this.textBox_PicType.Location = new System.Drawing.Point(143, 173);
            this.textBox_PicType.Name = "textBox_PicType";
            this.textBox_PicType.Size = new System.Drawing.Size(269, 25);
            this.textBox_PicType.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "自定义状态类型：";
            // 
            // textBox_PicName
            // 
            this.textBox_PicName.Location = new System.Drawing.Point(143, 139);
            this.textBox_PicName.Name = "textBox_PicName";
            this.textBox_PicName.Size = new System.Drawing.Size(269, 25);
            this.textBox_PicName.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "自定义图片名称：";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(74, 26);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(316, 25);
            this.textBox4.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "句柄：";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(121, 95);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(269, 25);
            this.textBox3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "窗口位置大小：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(74, 59);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(316, 25);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "类名：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picButton1);
            this.groupBox2.Location = new System.Drawing.Point(632, 255);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(82, 74);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // picButton1
            // 
            this.picButton1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picButton1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picButton1.Image = global::SpyTool.Properties.Resources._111115458781103050;
            this.picButton1.Location = new System.Drawing.Point(16, 16);
            this.picButton1.Name = "picButton1";
            this.picButton1.Size = new System.Drawing.Size(54, 49);
            this.picButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picButton1.TabIndex = 0;
            this.picButton1.TabStop = false;
            this.picButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picbox_spy_MouseDown);
            this.picButton1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picbox_spy_MouseUp);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridViewText);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(989, 555);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "文本采集";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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
            this.dataGridViewText.Size = new System.Drawing.Size(421, 448);
            this.dataGridViewText.TabIndex = 0;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "状态内容";
            this.Column3.Name = "Column3";
            // 
            // FormCollectSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 608);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCollectSample";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "样本采集";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCollectSample_FormClosed);
            this.Load += new System.EventHandler(this.FormCollectSample_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picButton1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewText)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private PicButton picButton1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox_PicName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_PicType;
        private System.Windows.Forms.Label label6;
        private PicButton picButton2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridView dataGridViewText;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}