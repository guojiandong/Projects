using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpyTool.UIForm
{
    public partial class FormCollectSample : Form
    {
        string DirPath = @"C:\Users\luxshare-ict\Desktop\Test\";
        private FrmShowRect m_frmRect;                      //用于显示选框的窗体
        private IntPtr m_hWndTemp;                          //临时句柄
        private byte[] m_byTextBuffer;                      //GetWindowText GetClassName 需要的缓存
        private Image m_curSaveImage;
        private IntPtr m_parentHwnd;
        public List<PicType> CurNodeInfo { get; set; }
        public List<string> CurNodeTextInfo { get; set; }

        public FormCollectSample(IntPtr parentHwnd)
        {
            this.m_parentHwnd = parentHwnd;
            CurNodeInfo = new List<PicType>();
            CurNodeTextInfo = new List<string>();
            InitializeComponent();
        }

        private void FormCollectSample_Load(object sender, EventArgs e)
        {
            m_byTextBuffer = new byte[256];
            m_frmRect = new FrmShowRect();
        }

        /// <summary>
        /// 选取窗口 - 鼠标按下响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_spy_MouseDown(object sender, MouseEventArgs e)
        {
            m_frmRect.Show();
            timer1.Enabled = true;
        }

        /// <summary>
        /// 选取窗口 - 鼠标抬起响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_spy_MouseUp(object sender, MouseEventArgs e)
        {
            m_frmRect.Hide();
            timer1.Enabled = false;
            //if (m_dic_node.ContainsKey(m_hWndTemp)) this.SelectNode(m_dic_node[m_hWndTemp]);
            //else this.LoadTreeWnd(m_hWndTemp);

            if (m_hWndTemp != null || m_hWndTemp != IntPtr.Zero)
            {
                Bitmap bitmap = Win32.GetWindowCapture(m_hWndTemp);//Win32.SubPicture(0,0,300,300);//Win32.GetWindowCapture(info.Hwnd);
                string fileName = DirPath + m_hWndTemp.ToString() + ".jpg";
                //bitmap.Save(fileName);//, myImageCodecInfo, myEncoderParameters);
                WindowInfo info = GetWindowInfo(m_hWndTemp);
                m_curSaveImage = Win32.BitBltCapture(info);
               // image.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                picButton2.Image = m_curSaveImage;
            }
        }

        /// <summary>
        /// 选取窗口 - 计时刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            Win32.POINT pt = new Win32.POINT();
            pt.X = MousePosition.X;
            pt.Y = MousePosition.Y;
            IntPtr hWnd = Win32.WindowFromPoint(pt);
            if (hWnd == m_hWndTemp) return;
            m_hWndTemp = hWnd;
            WindowInfo wi = this.GetWindowInfo(hWnd);
            ShowDetail(wi);
            m_frmRect.AnimateSelect(wi.WndInfo.rcWindow.ToRectangle());

        }

           /// <summary>
        /// 获取窗口信息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        private WindowInfo GetWindowInfo(IntPtr hWnd)
        {
            WindowInfo windowInfo = new WindowInfo(hWnd);
            Win32.WINDOWINFO wi = new Win32.WINDOWINFO();
            Win32.GetWindowInfo(hWnd, ref wi);
            //GetWindowInfo返回的ExStyle貌似有些问题
            wi.dwExStyle = (uint)Win32.GetWindowLong(hWnd, Win32.GWL_EXSTYLE);
            int len = Win32.GetClassName(hWnd, m_byTextBuffer, m_byTextBuffer.Length);
            windowInfo.ClassName = Encoding.Default.GetString(m_byTextBuffer, 0, len);
            len = Win32.GetWindowText(hWnd, m_byTextBuffer, m_byTextBuffer.Length);
            windowInfo.WindowText = Encoding.Default.GetString(m_byTextBuffer, 0, len);
            windowInfo.WndInfo = wi;
            return windowInfo;
        }

        /// <summary>
        /// 展示窗口信息
        /// </summary>
        /// <param name="windowInfo"></param>
        private void ShowDetail(WindowInfo windowInfo)
        {
            this.textBox1.Text = windowInfo.ClassName;
            this.textBox3.Text = windowInfo.WndInfo.rcWindow.ToRectangle().ToString();
            this.textBox4.Text = windowInfo.Hwnd.ToString();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string picName = this.textBox_PicName.Text.Trim();
            string picType = this.textBox_PicType.Text.Trim();

            if (string.IsNullOrEmpty(picName))
            {
                MessageBox.Show("图片自定义名称为空");
                return;
            }

            if (string.IsNullOrEmpty(picType))
            {
                MessageBox.Show("图片自定义状态类型为空");
                return;
            }

            PicType pic = new PicType();
            pic.Name = picName;
            pic.Status = picType;
            pic.Image = m_curSaveImage;
            pic.ParentHwnd = m_parentHwnd;
            pic.Hwnd = m_hWndTemp;

            string savePath = DirPath + m_hWndTemp.ToString();
            string fileName = string.Format("-{0}-{1}-{2}", picName, picType, ".jpg");
            m_curSaveImage.Save(savePath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Tag = pic;
            this.dataGridView1.Rows[index].Cells[0].Value = picName;
            this.dataGridView1.Rows[index].Cells[1].Value = picType;
        }

        private void FormCollectSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            for(int i= 0;i<this.dataGridView1.RowCount;i++)
            {
                DataGridViewRow row = this.dataGridView1.Rows[i];
                if (row!= null && row.Tag != null)
                {
                    CurNodeInfo.Add(row.Tag as PicType);
                }
            }

            for (int i = 0; i < this.dataGridViewText.RowCount; i++)
            {
                DataGridViewRow row = this.dataGridViewText.Rows[i];
                if (row != null && row.Cells[0].Value != null)
                {
                    CurNodeTextInfo.Add(row.Cells[0].Value.ToString());
                }
            }
        }
    }
}
