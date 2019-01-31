using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyTool.UIForm
{
    public partial class FormSelectControl : Form
    {
        private FrmShowRect m_frmRect;                      //用于显示选框的窗体
        private IntPtr m_hWndTemp;                          //临时句柄
        private IntPtr m_hWndSelect;                        //选中的句柄
        private Color m_clrTemp;                            //TreeNode的ForeColor临时保存变量
        private TreeNode m_nodeSelect;                      //选中的节点
        private byte[] m_byTextBuffer;                      //GetWindowText GetClassName 
        private List<WindowInfo> m_windowInfosList = new List<WindowInfo>();
        private WindowInfo m_curSelecthWinfdowInfo;
        private IntPtr ParentHwnd = IntPtr.Zero;
        public List<ControlType> CurNodeInfoList = new List<ControlType>();

        public FormSelectControl(IntPtr parentHwnd)
        {
            CurNodeInfoList.Clear();
            this.ParentHwnd = parentHwnd;
            InitializeComponent();
        }


        private void FormSelectControl_Load(object sender, EventArgs e)
        {
            this.Location = new Point(758, 100);
            m_byTextBuffer = new byte[256];
            m_frmRect = new FrmShowRect();
        }

        /// <summary>
        /// 选取窗口 - 鼠标按下响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picButton1_MouseDown(object sender, MouseEventArgs e)
        {
            m_frmRect.Show();
            timer1.Enabled = true;
        }

        /// <summary>
        /// 选取窗口 - 鼠标抬起响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picButton1_MouseUp(object sender, MouseEventArgs e)
        {
            m_frmRect.Hide();
            timer1.Enabled = false;
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
            //Console.WriteLine(hWnd + "\t" + MousePosition.ToString());
            m_hWndTemp = hWnd;
            WindowInfo wi = this.GetWindowInfo(hWnd);
            m_curSelecthWinfdowInfo = wi;
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
            this.textBox2.Text = windowInfo.WindowText.ToString();
            this.textBox3.Text = windowInfo.WndInfo.rcWindow.ToRectangle().ToString();
            this.textBox4.Text = windowInfo.Hwnd.ToString();
            this.textBoxParentHwnd.Text = this.ParentHwnd.ToString();
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddControl_Click(object sender, EventArgs e)
        {
            string wndName = this.textBox_WndName.Text.Trim();
            if (string.IsNullOrEmpty(wndName))
            {
                MessageBox.Show("自定义控件名称不能为空");
                return;
            }

            foreach (WindowInfo info in m_windowInfosList)
            {
                if (info.Name == wndName)
                {
                    MessageBox.Show("列表张红存在同名的控件");
                    return;
                }
            }
            m_curSelecthWinfdowInfo.Name = wndName;
            if (m_curSelecthWinfdowInfo != null)
            {
                if (!m_windowInfosList.Contains(m_curSelecthWinfdowInfo))
                {
                    m_windowInfosList.Add(m_curSelecthWinfdowInfo);
                    int index = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Tag = m_curSelecthWinfdowInfo;
                    this.dataGridView1.Rows[index].Cells[0].Value = m_curSelecthWinfdowInfo.Name.ToString();
                    this.dataGridView1.Rows[index].Cells[1].Value = m_curSelecthWinfdowInfo.ClassName.ToString();
                    this.dataGridView1.Rows[index].Cells[2].Value = m_curSelecthWinfdowInfo.WndInfo.ToString();
                    this.dataGridView1.Rows[index].Cells[3].Value = m_curSelecthWinfdowInfo.Hwnd.ToString();
                    this.dataGridView1.Rows[index].Cells[4].Value = this.textBoxParentHwnd.Text;
                }
                else
                {
                    MessageBox.Show("当前句柄已在列表中");
                }
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.RowCount > 0)
            {
                for(int i = 0; i< this.dataGridView1.RowCount - 1;i++)
                {
                    WindowInfo info = (this.dataGridView1.Rows[i].Tag as WindowInfo);
                    string className = info.ClassName;//this.dataGridView1.Rows[i].Cells[1].Value.ToString();
                    IntPtr intPtr = info.Hwnd;// new IntPtr(int.Parse(this.dataGridView1.Rows[i].Cells[0].Value.ToString()));
                    IntPtr parentPtr = new IntPtr(int.Parse(this.dataGridView1.Rows[i].Cells[4].Value.ToString()));
                    if (className.Contains("Window.8") || className.Contains("BUTTON")) //WindowsForms10.Window.8.app.0.c940ee_r10_ad1 WindowsForms10.Window.8.app.0.c940ee_r10_ad1
                    {
                        CurNodeInfoList.Add(new PicType() {
                            Hwnd = intPtr,
                            Name = info.Name,// this.dataGridView1.Rows[i].Cells[3].Value.ToString(),
                            ParentHwnd = parentPtr,
                        });
                    }
                    if (className.Contains("STATIC"))     //STATIC WindowsForms10.STATIC.app.0.c940ee_r10_ad1
                    {
                        CurNodeInfoList.Add(new TextType() {
                            Hwnd = intPtr,
                            Name = info.Name,//this.dataGridView1.Rows[i].Cells[3].Value.ToString(),
                        });
                    }
                }
            }
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
