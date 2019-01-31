namespace SpyTool.UIForm
{
    partial class FormSelectControl
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
            this.buttonAddControl = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picButton1 = new SpyTool.PicButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_WndName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxParentHwnd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picButton1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(815, 585);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // buttonAddControl
            // 
            this.buttonAddControl.Location = new System.Drawing.Point(616, 88);
            this.buttonAddControl.Name = "buttonAddControl";
            this.buttonAddControl.Size = new System.Drawing.Size(75, 30);
            this.buttonAddControl.TabIndex = 17;
            this.buttonAddControl.Text = "添加";
            this.buttonAddControl.UseVisualStyleBackColor = true;
            this.buttonAddControl.Click += new System.EventHandler(this.buttonAddControl_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOk.Location = new System.Drawing.Point(306, 19);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(172, 33);
            this.buttonOk.TabIndex = 16;
            this.buttonOk.Text = "确定";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column1,
            this.Column5});
            this.dataGridView1.Location = new System.Drawing.Point(12, 17);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(791, 280);
            this.dataGridView1.TabIndex = 15;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picButton1);
            this.groupBox2.Location = new System.Drawing.Point(511, 68);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(75, 65);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // picButton1
            // 
            this.picButton1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picButton1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picButton1.Image = global::SpyTool.Properties.Resources._111115458781103050;
            this.picButton1.Location = new System.Drawing.Point(11, 13);
            this.picButton1.Name = "picButton1";
            this.picButton1.Size = new System.Drawing.Size(54, 49);
            this.picButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picButton1.TabIndex = 0;
            this.picButton1.TabStop = false;
            this.picButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picButton1_MouseDown);
            this.picButton1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picButton1_MouseUp);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.textBox_WndName);
            this.groupBox3.Controls.Add(this.buttonAddControl);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.textBoxParentHwnd);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(12, 301);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(791, 212);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // textBox_WndName
            // 
            this.textBox_WndName.Location = new System.Drawing.Point(144, 178);
            this.textBox_WndName.Name = "textBox_WndName";
            this.textBox_WndName.Size = new System.Drawing.Size(291, 25);
            this.textBox_WndName.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "控件自定义名称：";
            // 
            // textBoxParentHwnd
            // 
            this.textBoxParentHwnd.Location = new System.Drawing.Point(88, 117);
            this.textBoxParentHwnd.Name = "textBoxParentHwnd";
            this.textBoxParentHwnd.ReadOnly = true;
            this.textBoxParentHwnd.Size = new System.Drawing.Size(347, 25);
            this.textBoxParentHwnd.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "父句柄：";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(74, 20);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(361, 25);
            this.textBox4.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "句柄：";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(121, 147);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(314, 25);
            this.textBox3.TabIndex = 5;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "窗口位置大小：";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(74, 83);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(361, 25);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "标题：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(74, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(361, 25);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "类名：";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonOk);
            this.groupBox4.Location = new System.Drawing.Point(6, 521);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(803, 60);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Brown;
            this.label7.Location = new System.Drawing.Point(446, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "(*必填项)";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "控件名称";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 140;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "详细信息";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "控件类型";
            this.Column4.Name = "Column4";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "窗体标识";
            this.Column1.Name = "Column1";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "父句柄";
            this.Column5.Name = "Column5";
            // 
            // FormSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 585);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormSelectControl";
            this.Load += new System.EventHandler(this.FormSelectControl_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picButton1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxParentHwnd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private PicButton picButton1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_WndName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonAddControl;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}