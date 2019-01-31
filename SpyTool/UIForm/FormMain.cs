using SpyTool.UIForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyTool
{
    public partial class FormMain : Form
    {
        private FrmShowRect m_frmRect;                      //用于显示选框的窗体
        private IntPtr m_hWndTemp;                          //临时句柄
        private IntPtr m_hWndSelect;                        //选中的句柄
        private Color m_clrTemp;                            //TreeNode的ForeColor临时保存变量
        private TreeNode m_nodeSelect;                      //选中的节点
        private List<int> m_lst_error_pid;                  //有些进程无法获取到MainModule 将id保存
        private Dictionary<int, Process> m_dic_process;     //进程列表
        private Dictionary<IntPtr, TreeNode> m_dic_node;    //所有树节点
        private byte[] m_byTextBuffer;                      //GetWindowText GetClassName 需要的缓存
        private Dictionary<string, Bitmap> mDicBitmap = new Dictionary<string, Bitmap>();
        private Dictionary<string, Image> mDicImage = new Dictionary<string, Image>();
        private Dictionary<string, string> mDicColor2Wnd = new Dictionary<string, string>();
        private Dictionary<string, string> mDicBitmapString = new Dictionary<string, string>();
        private IntPtr mSelectProcessHwnd = IntPtr.Zero;
        private List<WindowInfo> m_windowInfosList = new List<WindowInfo>();
        private WindowInfo m_curSelecthWinfdowInfo;                        //选中的句柄
        private string mDesDir = @"C:\ProcessData\Log\";

        private List<ControlType> mSampleControlList = null;
        private List<string> mSampleTextList;
        private List<ControlType> mControlList;

        private List<string> CheckedColorList = new List<string>();
        private List<string> IgnoredColorList = new List<string>();
        private List<string> TextStatuList = new List<string>();
        private Dictionary<string, ControlType> mDicSample = new Dictionary<string, ControlType>();

        public FormMain()
        {
            InitializeComponent();
            this.Location = new Point(100,100);
            this.Text = string.Format("实时动态监测软件 - 启动");

#if false // 每次重启软件时 删除日志
            if (Directory.Exists(mDesDir))
                Directory.Delete(mDesDir, true);

            Directory.CreateDirectory(mDesDir);
            if (File.Exists(mDesDir + "Log.txt"))
            {
                File.Delete(mDesDir + "Log.txt");
                File.Create(mDesDir + "Log.txt");
            }
            else
            {
                File.Create(mDesDir + "Log.txt");
            }
#elif true
            if (!Directory.Exists(mDesDir))
                Directory.CreateDirectory(mDesDir);

            if (!File.Exists(mDesDir + "Log.txt"))
                File.Create(mDesDir + "Log.txt");
#endif
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
           //IgnoredColorList.Add("#F0F0F0");//背景色
            mControlList = new List<ControlType>();
            mSampleControlList = new List<ControlType>();
            mSampleTextList = new List<string>();
            this.buttonStepSelect.Visible = false;
            this.buttonSelectControl.Visible = false;
            UtilHelper.InitMessageType();
            m_byTextBuffer = new byte[256];
            m_lst_error_pid = new List<int>();
            m_dic_node = new Dictionary<IntPtr, TreeNode>();
            m_dic_process = new Dictionary<int, Process>();
            tree_wnd.ImageList = imglst_icon;
            m_frmRect = new FrmShowRect();
            //this.LoadTreeWnd(m_hWndSelect);                 //加载节点
            foreach (var p in Process.GetProcesses())
            {
               if (p.MainWindowHandle == IntPtr.Zero) continue;
                this.comboBox_Software.Items.Add(p.ProcessName);
            }

            // 设置默认轮询时间间隔
            if(string.IsNullOrEmpty(this.textBox_TimeInterval.Text.Trim()))
            {
                this.textBox_TimeInterval.Text = "3";
            }
        }

#region  TreeNode  暂时不用 Tree
        public void LoadTreeWnd(IntPtr hWndSelect)
        {        //加载树 并选中某个句柄节点
            m_dic_node.Clear();
            m_dic_process.Clear();
            m_lst_error_pid.Clear();
            foreach (var p in Process.GetProcesses())
            {     //获取进程快照
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                {
                    m_dic_process.Add(p.Id, p);
                }
            }
            tree_wnd.Nodes.Clear();
            TreeNode node = this.GetNodeFromWindowInfo(this.GetWindowInfo(Win32.GetDesktopWindow()), true);
            tree_wnd.Nodes.Add(node);                       //桌面作为根节点
            this.LoadTreeWnd(IntPtr.Zero, IntPtr.Zero, node, true); //递归节点
            if (m_dic_node.ContainsKey(hWndSelect))
            {       //若是存在选中的节点
                tree_wnd.SelectedNode = m_dic_node[hWndSelect];
                m_dic_node[hWndSelect].ForeColor = Color.Blue;
                m_nodeSelect = m_dic_node[hWndSelect];
            }
            node.Expand();                                  //展开桌面节点
        }

        private TreeNode GetNodeFromWindowInfo(WindowInfo wi, bool bTopWindow)
        {
            int pid = 0;
            Win32.GetWindowThreadProcessId(wi.Hwnd, ref pid);
            if (m_dic_process.ContainsKey(pid)) wi.Process = m_dic_process[pid];
            TreeNode node = new TreeNode(wi.Hwnd.ToString("X").PadLeft(8, '0') + "|" + wi.WindowText + "|" + wi.ClassName);
            node.ImageKey = "none";
            foreach (var v in imglst_icon.Images.Keys)
            {        //若ImageList中存在对应Class的图标
                if (wi.ClassName.ToLower().Contains(v))
                {
                    node.ImageKey = v;
                    break;
                }
            }
            if (node.ImageKey == "none")
            {                      //若不存在图标 则尝试直接去获取窗体图标
                IntPtr pi = (IntPtr)Win32.SendMessage(wi.Hwnd, Win32.WM_GETICON, (IntPtr)Win32.ICON_SMALL, IntPtr.Zero);
                if (pi != IntPtr.Zero)
                {
                    imglst_icon.Images.Add(wi.Hwnd.ToString(), Icon.FromHandle(pi));
                    node.ImageKey = wi.Hwnd.ToString();
                }
            }
            //若依然未获取到图标 并且窗口是顶级窗体 尝试从可执行文件获取图标
            if (node.ImageKey == "none" && bTopWindow && !m_lst_error_pid.Contains(pid))
            {
                if (m_dic_process.ContainsKey(pid))
                {
                    try
                    {
                        string strKey = "P" + pid.ToString("X");
                        if (!imglst_icon.Images.ContainsKey(strKey))
                        {
                            Icon i = Icon.ExtractAssociatedIcon(m_dic_process[pid].Modules[0].FileName);
                            imglst_icon.Images.Add(strKey, i);
                        }
                        node.ImageKey = strKey;
                    }
                    catch { m_lst_error_pid.Add(pid); }
                }
            }
            if (wi.ClassName.ToLower().Contains("internet explorer_server"))
            {
                wi.Document = Win32.GetHtmlDocument(wi.Hwnd);
            }
            m_dic_node.Add(wi.Hwnd, node);
            node.SelectedImageKey = node.ImageKey;
            node.Tag = wi;
            return node;
        }

        public void LoadTreeWnd(IntPtr hParent, IntPtr hAfter, TreeNode treeNode, bool bTopWindow)
        {
            while ((hAfter = Win32.FindWindowEx(hParent, hAfter, null, null)) != IntPtr.Zero)
            {
                WindowInfo wi = this.GetWindowInfo(hAfter);
                if ((wi.WndInfo.dwStyle & Win32.WS_VISIBLE) == 0) continue;
                if ((wi.WndInfo.dwExStyle & Win32.WS_EX_TRANSPARENT) != 0) continue;
                TreeNode node = this.GetNodeFromWindowInfo(wi, bTopWindow);
                if (wi.Process == null) wi.Process = ((WindowInfo)treeNode.Tag).Process;
                if ((wi.WndInfo.dwStyle & Win32.WS_VISIBLE) == 0)
                {
                    node.ForeColor = Color.Gray;            //非可视窗体 使用灰色
                }
                else if ((wi.WndInfo.dwExStyle & Win32.WS_EX_TRANSPARENT) != 0)
                {
                    node.ForeColor = Color.Red;             //透明窗体 使用红色
                }
                treeNode.Nodes.Add(node);
                LoadTreeWnd(hAfter, IntPtr.Zero, node, false);
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void SelectNode(TreeNode node)
        {
            if (m_nodeSelect != null)
            {
                m_nodeSelect.ForeColor = m_clrTemp;
            }
            m_nodeSelect = node;
            m_clrTemp = node.ForeColor;
            tree_wnd.SelectedNode = node;
            node.ForeColor = Color.Blue;
            m_hWndSelect = ((WindowInfo)node.Tag).Hwnd;
        }
#endregion

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
            this.textBox4.Text = windowInfo.Hwnd.ToString();
        }

        /// <summary>
        /// 监听点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Listen_Click(object sender, EventArgs e)
        {
            mControlList.Clear();
            for (int i = 0;i < this.dataGridView1.RowCount - 1;i++)
            {
                mControlList.Add(this.dataGridView1.Rows[i].Tag as ControlType);
            }

            if (mControlList.Count == 0)
            {
                MessageBox.Show("监测列表为空");
                return;
            }

            // IsIconic（HWND hWnd）
            this.timerPolling.Enabled = true;
            // 设置默认轮询时间间隔
            if (string.IsNullOrEmpty(this.textBox_TimeInterval.Text.Trim()))
            {
                this.textBox_TimeInterval.Text = "3";
            }

            this.timerPolling.Interval = int.Parse(this.textBox_TimeInterval.Text) * 1000;
            this.Text = string.Format("实时动态监测软件 - 监测中");
            this.Listen.Text = "监测中";
            SetWindowState(false);
        }

        /// <summary>
        /// 取消监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            if (mControlList.Count == 0)
            {
                MessageBox.Show("监测列表为空");
                return;
            }

            this.timerPolling.Enabled = false;
            this.Text = string.Format("实时动态监测软件 - 已停止监测");
            this.Listen.Text = "开始检测";
            SetWindowState(true);
        }

        /// <summary>
        /// 监听轮询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerPolling_Tick(object sender, EventArgs e)
        {
            if (mControlList.Count == 0)
            {
                return;
            }

            try
            {
                foreach (ControlType control in mControlList)
                {
                    bool isIconic = Win32.IsIconic(control.ParentHwnd);
                    if (isIconic)
                    {
                        // 强制将父窗口显示出来 
                        Win32.ShowWindow(control.ParentHwnd, Win32.SW_SHOWMAXIMIZED);
                        return;
                    }

                    if (control is PicType)
                    {
                        UpdateBitMap(control as PicType, control.Hwnd);
                    }
                    else if (control is TextType)
                    {
                        UpdateText(control as TextType);
                    }
                }
            }
            catch (Exception ex)
            {
                this.timerPolling.Enabled = false;
            }
        }

        /// <summary>
        /// Text 改变
        /// </summary>
        /// <param name="info"></param>
        public void UpdateText(TextType control)
        {
            byte[] buffer = new byte[1024];
            int textLength = Win32.GetWindowText(control.Hwnd, buffer,1024);
            string textValue = System.Text.Encoding.Default.GetString(buffer,0, textLength);
            Console.WriteLine("UpdateText  textValue = " + textValue);

#region Write Log to Txt
            Console.WriteLine("Hwnd = " + control.Hwnd.ToString() + "  Text = " + textValue);
            FileStream fs = new FileStream(mDesDir + "Log.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            string detail = string.Format("控件名称：{0}   控件类型： {1}  状态：{2}   时间： {3}", control.Name, "Text", textValue, DateTime.UtcNow.ToString());
            //开始写入
            sw.WriteLine(detail);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
#endregion
        }

        /// <summary>
        /// 图片改变
        /// </summary>
        /// <param name="info"></param>
        private void UpdateBitMap(PicType picType, IntPtr controlHwnd)
        {
            WindowInfo info = GetWindowInfo(controlHwnd);
            Image newImage = Win32.BitBltCapture(info);
            string oldColor = null;
            mDicColor2Wnd.TryGetValue(info.Hwnd.ToString(), out oldColor);
            
            string new_color = string.Empty;
            if (oldColor != null && newImage != null)
            {
                bool isChanged =  UtilHelper.CompareColorChanged(CheckedColorList,IgnoredColorList, new Bitmap(newImage), oldColor, out new_color);
                //string a = string.Format("Name :{0}  isChanged :{1}  Time:{2}", picType.Name, isChanged,DateTime.UtcNow.ToString());
               // Console.WriteLine(a);
                if (!isChanged)
                {
                    return;
                }
                else  
                {
                    if (!mDicColor2Wnd.ContainsKey(info.Hwnd.ToString()))
                        mDicColor2Wnd.Add(info.Hwnd.ToString(), new_color);
                    else
                        mDicColor2Wnd[info.Hwnd.ToString()] = new_color;

#region Save as jpg
                    if (newImage == null)
                        return;
                    string fileName1 = mDesDir + info.Hwnd.ToString() + ".jpg";
                    newImage.Save(fileName1);

                    FileStream fs = new FileStream(mDesDir + "Log.txt", FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);

                    string color = new_color;
                    string statu = string.Empty;
                    ControlType picTemp;
                    string detail = string.Empty;
                    if (mDicSample.TryGetValue(color, out picTemp))
                    {
                        detail = string.Format("控件名称：{0}   控件类型： {1}  状态：{2}   时间： {3}", picType.Name, "ImageColor", (picTemp as PicType).Status, DateTime.UtcNow.ToString());
                    }
                    else
                    {
                        detail = string.Format("控件名称：{0}   控件类型： {1}  状态：{2}   时间： {3}", picType.Name, "ImageColor", (picType as PicType).Color, DateTime.UtcNow.ToString());
                    }

                    //开始写入
                    sw.WriteLine(detail);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
#endregion
                }
            }
            else
            {
                if (oldColor == null)
                    oldColor = "#F0F0F0";

                if (!mDicColor2Wnd.ContainsKey(info.Hwnd.ToString()))
                    mDicColor2Wnd.Add(info.Hwnd.ToString(), oldColor);
                else
                    mDicColor2Wnd[info.Hwnd.ToString()] = oldColor;

#region Save as jpg
                if (newImage == null)
                    return;

                string fileName1 = mDesDir + info.Hwnd.ToString() + ".jpg";
                newImage.Save(fileName1);

                FileStream fs = new FileStream(mDesDir + "Log.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                string color = picType.Color;
                string statu = string.Empty;
                ControlType picTemp;
                string detail = string.Empty;
                //开始写入
                if (color != null && mDicSample.TryGetValue(color, out picTemp))
                {
                    detail = string.Format("控件名称：{0}   控件类型： {1}  状态：{2}   时间： {3}", picType.Name, "ImageColor", (picTemp as PicType).Status, DateTime.UtcNow.ToString());
                }
                else if (color != null)
                {
                    detail = string.Format("控件名称：{0}   控件类型： {1}  状态：{2}   时间： {3}", picType.Name, "ImageColor", (picType as PicType).Status, DateTime.UtcNow.ToString());
                }
                else
                {
                    detail = string.Format("控件名称：{0}   控件类型： {1}  状态：{2}   时间： {3}", picType.Name, "ImageColor", "初始化 :" + oldColor, DateTime.UtcNow.ToString());
                }
                sw.WriteLine(detail);
                sw.Flush();
                sw.Close();
                fs.Close();
#endregion
            }
        }

        /// <summary>
        /// 图片改变
        /// </summary>
        /// <param name="info"></param>
        private void UpdateBitMap1(PicType picType,IntPtr controlHwnd)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            System.Drawing.Imaging.EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            // Save the bitmap as a JPEG file with quality level 25.
            myEncoderParameter = new EncoderParameter(myEncoder, 75L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            WindowInfo info = GetWindowInfo(controlHwnd);
            //Bitmap bitmap = Win32.GetWindowCapture(info.Hwnd);//Win32.SubPicture(0,0,300,300);//Win32.GetWindowCapture(info.Hwnd);
            //bitmap.Save(mDesDir + info.Hwnd.ToString() + ".jpg");
            Image newImage = Win32.BitBltCapture(info);
            // Image newImage = Win32.GetWindowCaptureImage(info);
            //Button m_MapControl = System.Windows.Forms.Form.FromHandle(controlHwnd) as Button;

            Image oldImage = null;
            mDicImage.TryGetValue(info.Hwnd.ToString(), out oldImage);
            if (oldImage != null && newImage != null)
            {
                //Color colorNew = UtilHelper.GetMajorColor1(new Bitmap(UtilHelper.ReduceSize(newImage)));
                //Color colorOld = UtilHelper.GetMajorColor1(new Bitmap(UtilHelper.ReduceSize(oldImage)));
                //double diff = UtilHelper.GetDifference(colorNew, colorOld);
                bool isChanged = SimilarPhoto.IsChanged(oldImage, newImage);//UtilHelper.ImageCompareStringChanged(oldImage, newImage);// UtilHelper.ImageCompareMultiPixelChanged(oldImage, newImage);//ImageComparePixelChanged(oldImage, newImage);//UtilHelper.ImageCompareString(oldImage, newImage);  //OK
                //bool isChanged2 =true;// UtilHelper.ImageCompareMultiPixelChanged(oldImage, newImage);
                //string a = string.Format("Name :{0} isChanged :{1} isChanged2 :{2}  Result : {3}", picType.Name, isChanged, isChanged2,(isChanged || isChanged2).ToString());
                //string a = string.Format("Name :{0} diff :{1}", picType.Name, diff);

                //int[] newInt = UtilHelper.GetHisogram(new Bitmap(UtilHelper.ReduceSize(newImage)));
                //int[] oldInt = UtilHelper.GetHisogram(new Bitmap(UtilHelper.ReduceSize(oldImage)));
                //float aaa = UtilHelper.GetResult(newInt, oldInt);
                string a = string.Format("Name :{0} isChanged :{1}", picType.Name, isChanged);
                Console.WriteLine(a);


                if (!isChanged)
                {
                    return;
                }
                else  
                {
                    if (!mDicImage.ContainsKey(info.Hwnd.ToString()))
                        mDicImage.Add(info.Hwnd.ToString(), newImage);
                    else
                        mDicImage[info.Hwnd.ToString()] = newImage;

                    #region Save as jpg
                    string fileName1 = mDesDir + info.Hwnd.ToString() + ".jpg";
                    newImage.Save(fileName1);

                    //string color = UtilHelper.GetMajorColorImage(newImage);
                    //Color newColor = new Color();
                    //Console.WriteLine("newColor = " + newColor);

                    FileStream fs = new FileStream(mDesDir + "Log.txt", FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    //开始写入
                    sw.WriteLine(info.Hwnd.ToString() + " Name = "+ picType.Name);
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
                #endregion
                }
            }
            else
            {
                if (!mDicImage.ContainsKey(info.Hwnd.ToString()))
                    mDicImage.Add(info.Hwnd.ToString(), newImage);
                else
                    mDicImage[info.Hwnd.ToString()] = newImage;

                #region Save as jpg
                string fileName1 = mDesDir + info.Hwnd.ToString() + ".jpg";
                newImage.Save(fileName1);

                FileStream fs = new FileStream(mDesDir + "Log.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.WriteLine(info.Hwnd.ToString());
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
                #endregion
            }

#region UnDo BitMap
            /*
            Bitmap oldBitMap = null;
            mDicBitmap.TryGetValue(info.Hwnd.ToString(), out oldBitMap);

            // Bitmap oldBitMap = UtilHelper.ReadImageFile(mDesDir + info.Hwnd.ToString() + ".jpg");
            if (oldBitMap != null && bitmap != null)
            {
                bool isChanged2 = UtilHelper.ImageCompareString(oldBitMap, bitmap);  //OK
                Console.WriteLine("isChanged2 = " + isChanged2);
                if (!isChanged2)
                {
                    return;
                }
                else
                {
                    // TODO  UpLoad Message Mysql
                    if (!mDicBitmap.ContainsKey(info.Hwnd.ToString()))
                        mDicBitmap.Add(info.Hwnd.ToString(), bitmap);
                    else
                        mDicBitmap[info.Hwnd.ToString()] = bitmap;

#region Write Log to Txt
                    string majorColor = UtilHelper.GetMajorColor(bitmap);
                    Console.WriteLine("Hwnd = "+ info.Hwnd.ToString() +  "  majorColor = " + majorColor);
                    string detail = string.Format(" 窗口标识:{0}   控件名称：{1}   控件类型： {2}  值域：{3}   时间： {4}", info.Hwnd.ToString(), info.Name, info.ClassName, majorColor, DateTime.UtcNow.ToString());
                    FileStream fs = new FileStream(mDesDir + "Log.txt", FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    //开始写入
                    sw.WriteLine(detail);
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
#endregion
                }
            }
            else
            {
                try
                {
                    if (!mDicBitmap.ContainsKey(info.Hwnd.ToString()))
                        mDicBitmap.Add(info.Hwnd.ToString(), bitmap);
                    else
                        mDicBitmap[info.Hwnd.ToString()] = bitmap;

#region Save as jpg
                    string fileName1 = mDesDir + info.Hwnd.ToString() + ".jpg";
                    bitmap.Save(fileName1);

                    FileStream fs = new FileStream(mDesDir + "Log.txt", FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    //开始写入
                    sw.WriteLine(info.Hwnd.ToString());
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
#endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                }
            }
            */
#endregion
        }

        /// <summary>
        /// 限制输入框只能为整数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxInteger_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (e.KeyChar == '-' && ((TextBox)sender).Text.Length > 1)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 选择软件进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_Software_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox_Software.SelectedItem != null && this.comboBox_Software.SelectedItem.ToString() != null)
            {
                foreach (var p in Process.GetProcesses())
                {     //获取进程快照
                    if (p.MainWindowHandle == IntPtr.Zero) continue;
                    if (p.ProcessName.Contains(this.comboBox_Software.SelectedItem.ToString()))
                    {
                        mSelectProcessHwnd = p.MainWindowHandle;
                        SetProcessDetail(mSelectProcessHwnd);
                        SetParentHwnd(mSelectProcessHwnd);
                        this.buttonStepSelect.Visible = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 显示进程信息
        /// </summary>
        /// <param name="curProcessHwnd"></param>
        private void SetProcessDetail(IntPtr curProcessHwnd)
        {
            WindowInfo windowInfo = GetWindowInfo(curProcessHwnd);
            this.textBox1.Text = windowInfo.ClassName;
            this.textBox2.Text = windowInfo.WindowText.ToString();
            this.textBox4.Text = windowInfo.Hwnd.ToString();
        }

        /// <summary>
        /// 设置进程父句柄
        /// </summary>
        /// <param name="curProcessHwnd"></param>
        private void SetParentHwnd(IntPtr curProcessHwnd)
        {
            WindowInfo windowInfo = GetWindowInfo(curProcessHwnd);
        }

        /// <summary>
        /// 点击样本采集按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStepSelect_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormSample form = new FormSample();
            //form.TopLevel = false;
            //form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            //form.Dock = DockStyle.None;
            //this.Controls.Add(form);
            form.ShowDialog();
            this.Show();

            if (form.CurNodeList != null && form.CurNodeList.Count > 0)
            {
                foreach(ControlType controlType in form.CurNodeList)
                {
                    if (controlType is PicType)
                    {
                        string color = (controlType as PicType).Color;

                        if ((controlType as PicType).IsIngnore)
                        {
                            if (!IgnoredColorList.Contains(color))
                            {
                                IgnoredColorList.Add(color);
                            }
                        }
                        else
                        {
                            if (!CheckedColorList.Contains(color))
                            {
                                CheckedColorList.Add(color);
                            }
                        }

                        if (!mDicSample.ContainsKey(color))
                        {
                            mDicSample.Add(color, controlType as PicType);
                        }
                        else
                            mDicSample[color] = controlType as PicType;
                    }
                    else if (controlType is TextType)
                    {
                        TextStatuList.Add(controlType.Name);
                        if (!mDicSample.ContainsKey(controlType.Name))
                        {
                            mDicSample.Add(controlType.Name, controlType as TextType);
                        }
                        else
                            mDicSample[controlType.Name] = controlType as TextType;
                    }
                }
            }

            if (CheckedColorList.Count > 0)
            {
                this.buttonSelectControl.Visible = true;
            }

            //FormCollectSample formCollectSample = new FormCollectSample(mSelectProcessHwnd);
            //formCollectSample.ShowDialog();

            //if (formCollectSample.CurNodeInfo != null && formCollectSample.CurNodeInfo.Count > 0)
            //{
            //    mSampleControlList.AddRange(formCollectSample.CurNodeInfo);
            //}

            //if (formCollectSample.CurNodeTextInfo != null &&formCollectSample.CurNodeTextInfo.Count >0)
            //{
            //    mSampleTextList.AddRange(formCollectSample.CurNodeTextInfo);
            //}
        }

        /// <summary>
        /// 点击选取检测控件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectControl_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormSelectControl form = new FormSelectControl(mSelectProcessHwnd);
            form.ShowDialog();
            this.Show();

            if (form.CurNodeInfoList != null && form.CurNodeInfoList.Count > 0)
            {
               // mControlList.AddRange(form.CurNodeInfoList);

                foreach(ControlType control in form.CurNodeInfoList)
                {
                    if (control is PicType)
                    {
                        int index = this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index].Tag = control;
                        this.dataGridView1.Rows[index].Cells[0].Value = (control as PicType).Name;
                        this.dataGridView1.Rows[index].Cells[1].Value = "Pic";
                        this.dataGridView1.Rows[index].Cells[3].Value = (control as PicType).Hwnd;
                        this.dataGridView1.Rows[index].Cells[4].Value = (control as PicType).ParentHwnd;
                    }
                    else if (control is TextType)
                    {
                        int index = this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index].Tag = control;
                        this.dataGridView1.Rows[index].Cells[0].Value = (control as TextType).Name;
                        this.dataGridView1.Rows[index].Cells[1].Value = "Text";
                        this.dataGridView1.Rows[index].Cells[3].Value = (control as TextType).Hwnd;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新软件信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.comboBox_Software.Items.Clear();
            foreach (var p in Process.GetProcesses()) //"N01Monitor"))//"TestDemo"
            {
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                this.comboBox_Software.Items.Add(p.ProcessName);
            }
        }

        /// <summary>
        /// 设置窗口 各控件状态
        /// </summary>
        /// <param name="Interactive"></param>
        private void SetWindowState(bool Interactive)
        {
            this.buttonRefresh.Enabled = Interactive;
            this.buttonSelectControl.Enabled = Interactive;
            this.buttonStepSelect.Enabled = Interactive;
            this.textBox_TimeInterval.Enabled = Interactive;
        }
    }
}
