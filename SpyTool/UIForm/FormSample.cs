using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xianfen.Net.GetColor;

namespace SpyTool.UIForm
{
    public partial class FormSample : Form
    {
        public List<ControlType> CurNodeList;

        public FormSample()
        {
            CurNodeList = new List<ControlType>();
            InitializeComponent();
        }

        private void btnPickColor_Click(object sender, EventArgs e)
        {
            using (ColorPicker picker = new ColorPicker())
            {
                this.WindowState = FormWindowState.Minimized;
                picker.ShowDialog();

                for (int i = 7; i > 0; i--)
                {
                    ((Label)(this.Controls.Find("lblColor" + i.ToString(), true)[0])).BackColor = ((Label)(this.Controls.Find("lblColor" + (i - 1).ToString(), true)[0])).BackColor;
                }

                lblColor0.BackColor = picker.SelectedColor;

                toolShow_Click(null, null);
                lblColor_Click(lblColor0, null);
            }
        }

        private void lblColor_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;

            if (lbl != null)
            {
                Color color = lbl.BackColor;
                txtRGB.Text = string.Format("#{0,2:X2}{1,2:X2}{2,2:X2}", color.R.ToString("X"), color.G.ToString("X"), color.B.ToString("X")).Replace(" ", "0");
                txtARGB.Text = string.Format("{0,3:D3} {1,3:D3} {2,3:D3} {3,2:D3}", color.A, color.R, color.G, color.B);

                int hue = (int)((color.GetHue() / 360.0f) * 240 + 0.5);
                int sat = (int)(color.GetSaturation() * 241 + 0.5);
                int bri = (int)(color.GetBrightness() * 241 + 0.5);

                if (hue > 239)
                {
                    hue = 239;
                }

                if (sat > 240)
                {
                    sat = 240;
                }

                if (bri > 240)
                {
                    bri = 240;
                }

                txtHSB.Text = string.Format("{0,3:D3} {1,3:D3} {2,3:D3}", hue, sat, bri);
            }
        }

        private void toolShow_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            colorDlg.Color = lblColor0.BackColor;

            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                lblColor0.BackColor = colorDlg.Color;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblColor_DoubleClick(object sender, EventArgs e)
        {
            Label lbl = sender as Label;

            if (lbl != null)
            {
                Color color = lbl.BackColor;
                colorDlg.Color = color;

                if (colorDlg.ShowDialog() == DialogResult.OK)
                {
                    lbl.BackColor = colorDlg.Color;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string rgb = this.txtRGB.Text.Trim();
            if (string.IsNullOrEmpty(rgb))
            {
                MessageBox.Show("没有选取的颜色");
                return;
            }

            string statu = this.txtStatusName.Text.Trim();
            if (string.IsNullOrEmpty(statu))
            {
                MessageBox.Show("颜色状态名称为空");
                return;
            }

            PicType pic = new PicType();
            pic.Status = statu;
            pic.Color = rgb;

            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Tag = pic;
            this.dataGridView1.Rows[index].Cells[0].Value = statu;
            this.dataGridView1.Rows[index].Cells[1].Value = rgb;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.RowCount > 0)
            {
                for (int i = 0; i < this.dataGridView1.RowCount - 1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0] == null || dataGridView1.Rows[i].Cells[0].Value == null)
                        continue;
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells["Ignore"];
                    Boolean flag = Convert.ToBoolean(checkCell.Value);
                    (this.dataGridView1.Rows[i].Tag as ControlType).IsIngnore = flag;
                    this.CurNodeList.Add(this.dataGridView1.Rows[i].Tag as ControlType);
                }
            }

            if (this.dataGridViewText.RowCount > 0)
            {
                for (int i = 0; i < this.dataGridViewText.RowCount - 1; i++)
                {
                    string statu = this.dataGridViewText.Rows[i].Cells[0].Value.ToString();
                    this.CurNodeList.Add(new TextType() { Name = statu });
                }
            }

            string xml = UtilHelper.Serializer(typeof(List<ControlType>), CurNodeList);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "请选择要保存的文件路径";
            dlg.Filter = "文本文件|*.xml|所有文件|*.*";
            string path = string.Empty;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                path = dlg.FileName;
            }
            //获得用户要保存的文件的路径
            if (path == "")
            {
                return;
            }
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(xml);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
                MessageBox.Show("保存成功");
            }

            this.Close();
        }

        private void FormSample_Load(object sender, EventArgs e)
        {
            this.Location = new Point(758, 100);

        }

        private void buttonLoadSample_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            //打开文件
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "xml";
            dlg.Filter = "xml Files|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
                fileName = dlg.FileName;
            if (string.IsNullOrEmpty(fileName))
                return;

            //读取文件内容
            StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default);
            String xml = sr.ReadToEnd().TrimStart();
            sr.Close();

            List<ControlType> colorList = UtilHelper.Deserialize(typeof(List<ControlType>), xml) as List<ControlType>;
            UpdateDataGridView(colorList);
        }

        private void UpdateDataGridView(List<ControlType> colorList)
        {
            foreach (ControlType controlType in colorList)
            {
                if (controlType is PicType)
                {
                    int index = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Tag = controlType;
                    this.dataGridView1.Rows[index].Cells[0].Value = controlType.Status;
                    this.dataGridView1.Rows[index].Cells[1].Value =( controlType as PicType ).Color;
                    this.dataGridView1.Rows[index].Cells[2].Value = (controlType as PicType).IsIngnore;
                }
            }
        }
    }
}
