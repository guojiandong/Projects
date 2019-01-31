namespace PopupForm
{
    partial class Frm_Main
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Hidden = new System.Windows.Forms.Button();
            this.btn_Display = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Hidden);
            this.groupBox1.Controls.Add(this.btn_Display);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 92);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作类型";
            // 
            // btn_Hidden
            // 
            this.btn_Hidden.Location = new System.Drawing.Point(164, 32);
            this.btn_Hidden.Name = "btn_Hidden";
            this.btn_Hidden.Size = new System.Drawing.Size(75, 23);
            this.btn_Hidden.TabIndex = 0;
            this.btn_Hidden.Text = "隐  藏";
            this.btn_Hidden.UseVisualStyleBackColor = true;
            this.btn_Hidden.Click += new System.EventHandler(this.btn_Hidden_Click);
            // 
            // btn_Display
            // 
            this.btn_Display.Location = new System.Drawing.Point(42, 32);
            this.btn_Display.Name = "btn_Display";
            this.btn_Display.Size = new System.Drawing.Size(75, 23);
            this.btn_Display.TabIndex = 0;
            this.btn_Display.Text = "显  示";
            this.btn_Display.UseVisualStyleBackColor = true;
            this.btn_Display.Click += new System.EventHandler(this.btn_Display_Click);
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 116);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Main";
            this.ShowIcon = false;
            this.Text = "从桌面右下角显示的Popup窗口提醒";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Hidden;
        private System.Windows.Forms.Button btn_Display;
    }
}

