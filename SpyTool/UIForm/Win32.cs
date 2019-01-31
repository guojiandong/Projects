using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SpyTool
{


    public class Win32
    {
        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;


        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("USER32.DLL", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, byte[] byBuffer, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, byte[] byBuffer, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hChildAfter, string lpszClass, string lpszWindowText);
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT pt);
        [DllImport("user32.dll")]
        public static extern bool GetWindowInfo(IntPtr hWnd, ref WINDOWINFO lpWindowInfo);
        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, ref int lpDwProcessId);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("USER32.DLL")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);
        public delegate bool CallBack(IntPtr hwnd, int lParam);
        [DllImport("user32", EntryPoint = "RegisterWindowMessage")]
        public static extern int RegisterWindowMessage(string lpString);
        //设置进程窗口到最前       
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("OLEACC.DLL", EntryPoint = "ObjectFromLresult")]
        public static extern int ObjectFromLresult(
            int lResult,
            ref System.Guid riid,
            int wParam,
            [MarshalAs(UnmanagedType.Interface), System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out]ref System.Object ppvObject
        );

        #region GetWindowCapture的dll引用 获取非前端窗口的截图
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rectangle rect);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(
         IntPtr hdc // handle to DC
         );
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(
         IntPtr hdc,         // handle to DC
         int nWidth,      // width of bitmap, in pixels
         int nHeight      // height of bitmap, in pixels
         );
        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(
         IntPtr hdc,           // handle to DC
         IntPtr hgdiobj    // handle to object
         );
        [DllImport("gdi32.dll")]
        private static extern int DeleteDC(
         IntPtr hdc           // handle to DC
         );
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(
         IntPtr hwnd,                // Window to copy,Handle to the window that will be copied.
         IntPtr hdcBlt,              // HDC to print into,Handle to the device context.
         UInt32 nFlags               // Optional flags,Specifies the drawing options. It can be one of the following values.
         );
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(
         IntPtr hwnd
         );


        #endregion

        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const long WS_VISIBLE = 0x10000000;
        public const long WS_EX_TRANSPARENT = 0x20;
        public const long GWL_HWNDPARENT = -8;

        public const uint WM_GETICON = 0x7F;
        public const int ICON_SMALL = 0;

        public const int SM_XVIRTUALSCREEN = 76;
        public const int SM_YVIRTUALSCREEN = 77;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;

        public struct POINT
        {
            public int X;
            public int Y;
        }

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public System.Drawing.Rectangle ToRectangle()
            {
                return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
        }

        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public short atomWindowType;
            public short wCreatorVersion;

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(rcWindow.Left).Append(",");
                builder.Append(rcWindow.Top).Append(",");
                builder.Append(rcWindow.Right).Append(",");
                builder.Append(rcWindow.Bottom).Append(",");
                return builder.ToString();
            }
        }

        public static mshtml.IHTMLDocument2 GetHtmlDocument(IntPtr hwnd)
        {
            System.Object domObject = new System.Object();
            System.Guid guidIEDocument2 = new Guid();
            int WM_HTML_GETOBJECT = Win32.RegisterWindowMessage("WM_Html_GETOBJECT");
            int W = Win32.SendMessage(hwnd, (uint)WM_HTML_GETOBJECT, IntPtr.Zero, IntPtr.Zero);
            int lreturn = Win32.ObjectFromLresult(W, ref guidIEDocument2, 0, ref domObject);
            mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2)domObject;
            return doc;
        }

        public static System.Drawing.Rectangle GetDesktopRect()
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            rect.X = Win32.GetSystemMetrics(Win32.SM_XVIRTUALSCREEN);
            rect.Y = Win32.GetSystemMetrics(Win32.SM_YVIRTUALSCREEN);
            rect.Width = Win32.GetSystemMetrics(Win32.SM_CXVIRTUALSCREEN);
            rect.Height = Win32.GetSystemMetrics(Win32.SM_CYVIRTUALSCREEN);
            return rect;
        }

        /// <summary>
        /// 查找窗体上控件句柄
        /// </summary>
        /// <param name="hwnd">父窗体句柄</param>
        /// <param name="lpszWindow">控件标题(Text)</param>
        /// <param name="bChild">设定是否在子窗体中查找</param>
        /// <returns>控件句柄，没找到返回IntPtr.Zero</returns>
        private static IntPtr FindWindowEx(IntPtr hwnd, string lpszWindow, bool bChild)
        {
            IntPtr iResult = IntPtr.Zero;
            // 首先在父窗体上查找控件
            iResult = FindWindowEx(hwnd, 0, null, lpszWindow);
            // 如果找到直接返回控件句柄
            if (iResult != IntPtr.Zero) return iResult;
            // 如果设定了不在子窗体中查找
            if (!bChild) return iResult;
            // 枚举子窗体，查找控件句柄
            int i = EnumChildWindows(
            hwnd,
            (h, l) =>
            {
                IntPtr f1 = FindWindowEx(h, 0, null, lpszWindow);
                if (f1 == IntPtr.Zero)
                    return true;
                else
                {
                    iResult = f1;
                    return false;
                }
            },
            0);
            // 返回查找结果
            return iResult;
        }

        /// <summary>
        /// 获取非前端窗口的截图
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Bitmap GetWindowCapture(IntPtr hWnd)
        {
            Bitmap bmp = null;
            try
            {
                IntPtr hscrdc = GetWindowDC(hWnd);
                Rectangle windowRect = new Rectangle();
                GetWindowRect(hWnd, ref windowRect);
                int width = Math.Abs(windowRect.X - windowRect.Width);
                int height = Math.Abs(windowRect.Y - windowRect.Height);

                IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, width, height);
                IntPtr hmemdc = CreateCompatibleDC(hscrdc);
                SelectObject(hmemdc, hbitmap);
                PrintWindow(hWnd, hmemdc, 0);

                bmp = Image.FromHbitmap(hbitmap);
                DeleteDC(hscrdc);//删除用过的对象
                DeleteDC(hmemdc);//删除用过的对象
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
            return bmp;
        }

        /// <summary>
        /// 获取非前端窗口的截图
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Image GetWindowCaptureImage(WindowInfo info)
        {
            Bitmap bmp = null;
            try
            {
                IntPtr hWnd = info.Hwnd;
                IntPtr hscrdc = GetWindowDC(hWnd);
                int width = info.WndInfo.rcWindow.Right - info.WndInfo.rcWindow.Left;
                int height = info.WndInfo.rcWindow.Bottom - info.WndInfo.rcWindow.Top;

                IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, width, height);
                IntPtr hmemdc = CreateCompatibleDC(hscrdc);
                SelectObject(hmemdc, hbitmap);
                PrintWindow(hWnd, hmemdc, 0);

                bmp = Image.FromHbitmap(hbitmap);
                DeleteDC(hscrdc);//删除用过的对象
                DeleteDC(hmemdc);//删除用过的对象
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
            return bmp;
        }




        public static Bitmap SubPicture(int location_X, int location_Y, int size_X, int size_Y)
        {
            Bitmap bitSize = new Bitmap(size_X, size_Y);
            Graphics g = Graphics.FromImage(bitSize);
            Point pl = new Point(location_X, location_Y);
            g.CopyFromScreen(pl, new Point(0, 0), bitSize.Size);
            g.Dispose();
            return bitSize;
        }


        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
           IntPtr hdcDest,//目标设备的句柄
           int nXDest,//目标对象的左上角x坐标
           int nYDest,//目标对象的左上角Y坐标
           int nWidth,//目标对象的矩形宽度
           int nHeight,//目标对象的矩形长度
           IntPtr hdcSrc,//源设备的句柄
           int nXSrc,//源对象的左上角x坐标
           int nYSrc,//源对象的左上角y坐标
           System.Int32 dwRop//光栅的操作值
           );
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern IntPtr CreateDC(
            string lpszDriver,//驱动名称
            string lpszDevice,//设备名称
            string lpszOutput,//无用，设为null
            IntPtr lpInitData//任意的打印机数据
            );


        public static Image BitBltCapture(Button button)
        {
            const int SRCCOPY = 0x00CC0020;
            Graphics g1 = button.CreateGraphics();
            Bitmap bit2 = new Bitmap(button.Width, button.Height, g1);
            Graphics g2 = Graphics.FromImage(bit2);
            IntPtr hdc1 = g1.GetHdc();
            IntPtr hdc2 = g2.GetHdc();
            BitBlt(hdc2,0,0, button.Width, button.Height, hdc1, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SRCCOPY);

            Bitmap b = (Bitmap)bit2.Clone();
            g1.ReleaseHdc(hdc1);
            g2.ReleaseHdc(hdc2);
            g1.Dispose();
            g2.Dispose();

            return b;
        }

        public static Image BitBltCapture(PictureBox pictureBox)
        {
            const int SRCCOPY = 0x00CC0020;
            Graphics g1 = pictureBox.CreateGraphics();
            Bitmap bit2 = new Bitmap(pictureBox.Width, pictureBox.Height, g1);
            Graphics g2 = Graphics.FromImage(bit2);
            IntPtr hdc1 = g1.GetHdc();
            IntPtr hdc2 = g2.GetHdc();
            BitBlt(hdc2, 0, 0, pictureBox.Width, pictureBox.Height, hdc1, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SRCCOPY);

            Bitmap b = (Bitmap)bit2.Clone();
            g1.ReleaseHdc(hdc1);
            g2.ReleaseHdc(hdc2);
            g1.Dispose();
            g2.Dispose();

            return b;
        }


        public static Image BitBltCapture(WindowInfo info)
        {
            try
            {
                IntPtr dc1 = GetWindowDC(info.Hwnd);
                //创建显示器的DC
                Graphics g1 = Graphics.FromHdc(dc1);
                int width = info.WndInfo.rcClient.Right - info.WndInfo.rcClient.Left;
                int height = info.WndInfo.rcClient.Bottom - info.WndInfo.rcClient.Top;
                //由一个指定设备的句柄创建一个新的Graphics对象
                Image MyImage = new Bitmap(width, height, g1);
                //根据屏幕大小创建一个与之相同大小的Bitmap对象
                Graphics g2 = Graphics.FromImage(MyImage);


                //获得屏幕的句柄
                IntPtr dc3 = g1.GetHdc();
                //获得位图的句柄
                IntPtr dc2 = g2.GetHdc();

                BitBlt(dc2, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, dc3, 0, 0, 13369376);
                g1.ReleaseHdc(dc3);
                //释放屏幕句柄
                g2.ReleaseHdc(dc2);
                //释放位图句柄

                return MyImage;
            }
            catch(Exception ex)
            {
                MessageBox.Show("检测目标已关闭 或 不存在！");
                throw ex;
            }
            return null;
        }

        //public Image CaptureWindow(IntPtr handle)
        //{
        //    IntPtr hdcSrc = GetWindowDC(handle);
        //    System.Drawing.Rectangle windowRect = new System.Drawing.Rectangle();
        //    GetWindowRect(handle, ref windowRect);
        //    int width = windowRect.Right - windowRect.Left;
        //    int height = windowRect.Bottom - windowRect.Top;
        //    IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
        //    IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);
        //    IntPtr hOld = SelectObject(hdcDest, hBitmap);
        //    //BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0,SRCCOPY);
        //    SelectObject(hdcDest, hOld);
        //    DeleteDC(hdcDest);
        //}
    }
}
