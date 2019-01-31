using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PopupForm
{
    public partial class FrmInfo : Form
    {
        #region 声明的变量
        private Rectangle Rect;//定义一个存储矩形框的数组
        private FormState InfoStyle = FormState.Hide;//定义变量为隐藏
        static private FrmInfo singleForm;  // 当前窗体

        public const Int32 AW_HOR_POSITIVE = 0x00000001;    //自左向右显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;    //自右向左显示窗口。当使用了 AW_CENTER 标志时该标志被忽略
        public const Int32 AW_VER_POSITIVE = 0x00000004;    //自顶向下显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_VER_NEGATIVE = 0x00000008;    //自下向上显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_CENTER = 0x00000010;    //若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展
        public const Int32 AW_HIDE = 0x00010000;    //隐藏窗口，缺省则显示窗口
        public const Int32 AW_ACTIVATE = 0x00020000;    //激活窗口。在使用了AW_HIDE标志后不要使用这个标志
        public const Int32 AW_SLIDE = 0x00040000;    //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        public const Int32 AW_BLEND = 0x00080000;    //使用淡入效果。只有当hWnd为顶层窗口的时候才可以使用此标志
        #endregion

        #region 该窗体的构造方法
        private FrmInfo()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.FormClosing += new FormClosingEventHandler(FrmInfoClosing);
            //初始化工作区大小
            Rectangle rect = Screen.GetWorkingArea(this);   // 实例化一个当前窗口的对象
            this.Rect = new Rectangle(rect.Right - this.Width - 1, rect.Bottom - this.Height - 1, this.Width, this.Height);    // 为实例化的对象创建工作区域
        }
        #endregion

        #region 关闭当前窗体
        private void FrmInfoClosing(object sender, FormClosingEventArgs e)
        {
            singleForm = null;
        } 
        #endregion
   
        #region 调用API函数显示窗体
        /// <summary>
        /// API动态显示窗体
        /// </summary>
        /// <param name="hwnd">hwnd：目标窗口句柄。</param>
        /// <param name="dwTime">dwTime：动画的持续时间，数值越大动画效果的时间就越长。</param>
        /// <param name="dwFlags">DwFlags：DwFlags参数是动画效果类型选项，该参数在C#中的详细声明含义请看最下面的备注。</param>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        #endregion

        #region 定义标识窗体移动状态的枚举值
        protected enum FormState
        {
            Hide = 0,//隐藏窗体
            Display = 1,//显示窗体
            Displaying = 2,//显示窗体中
            Hiding = 3 //隐藏窗体中
        }
        #endregion

        #region 用属性标识当前状态
        protected FormState FormNowState
        {
            get { return this.InfoStyle; }   //返回窗体的当前状态
            set { this.InfoStyle = value; }  //设定窗体当前状态的值
        }
        #endregion

        #region 显示窗体
        public void ShowForm()
        {
            switch (this.FormNowState)
            {
                case FormState.Hide:
                    if (this.Height <= this.Rect.Height - 192)//当窗体没有完全显示时
                        this.SetBounds(Rect.X, this.Top - 192, Rect.Width, this.Height + 192);//使窗体不断上移
                    else
                    {
                        this.SetBounds(Rect.X, Rect.Y, Rect.Width, Rect.Height);//设置当前窗体的边界
                    }
                    AnimateWindow(this.Handle, 800, AW_SLIDE + AW_VER_NEGATIVE);//动态显示本窗体
                    break;
            }
        }
        #endregion

        #region 关闭窗体
        public void CloseForm()
        {
            AnimateWindow(this.Handle, 800, AW_SLIDE + AW_VER_POSITIVE + AW_HIDE);//动画隐藏窗体
            this.FormNowState = FormState.Hide;//设定当前窗体的状态为隐藏
        }
        #endregion

        #region 单例设计模式：返回当前窗体的实例化对象
        static public FrmInfo Instance()
        {
            if (singleForm == null)
            {
                singleForm = new FrmInfo();
            }
            return singleForm; //返回当前窗体的实例化对象
        }
        #endregion
    }
    /**
     * 【AnimateWindow说明】
     *  Windows提供了一个API函数Animate Window，该函数可以实现窗体的动画效果，AnimateWindow函数在C#中的声明如下：
     *  [DllImportAttribute("user32.dll")]
     *  private static extern bool AnimateWindow(IntPtr  hwnd, int  dwTime, int  dwFlags);
     *  
     *    【参数说明如下：】
     *        hwnd：目标窗口句柄。
     *        dwTime：动画的持续时间，数值越大动画效果的时间就越长。
     *        DwFlags：DwFlags参数是动画效果类型选项，该参数在C#中的声明如下：
     *		    public const Int32 AW_HOR_POSITIVE = 0x00000001;    //自左向右显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
     *		    public const Int32 AW_HOR_NEGATIVE = 0x00000002;    //自右向左显示窗口。当使用了 AW_CENTER 标志时该标志被忽略
     *		    public const Int32 AW_VER_POSITIVE = 0x00000004;    //自顶向下显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
     *		    public const Int32 AW_VER_NEGATIVE = 0x00000008;    //自下向上显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
     *		    public const Int32 AW_CENTER = 0x00000010;    //若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展
     *		    public const Int32 AW_HIDE = 0x00010000;    //隐藏窗口，缺省则显示窗口
     *		    public const Int32 AW_ACTIVATE = 0x00020000;    //激活窗口。在使用了AW_HIDE标志后不要使用这个标志
     *		    public const Int32 AW_SLIDE = 0x00040000;    //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
     *		    public const Int32 AW_BLEND = 0x00080000;    //使用淡入效果。只有当hWnd为顶层窗口的时候才可以使用此标志
     *
     *    【DwFlags参数可选值含义如下所示：】
     *	    标    志							描    述
     *	    AW_SLIDE 		使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
     *	    AW_ACTIVE		激活窗口。在使用了AW_HIDE标志后不要使用这个标志
     *	    AW_BLEND		使用淡入效果。只有当hWnd为顶层窗口的时候才可以使用此标志
     *	    AW_HIDE			隐藏窗口，缺省则显示窗口
     *	    AW_CENTER		若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展
     *	    AW_HOR_POSITIVE	自左向右显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
     *	    AW_HOR_NEGATIVE	自右向左显示窗口。当使用了 AW_CENTER 标志时该标志被忽略
     *	    AW_VER_POSITIVE	自顶向下显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
     *	    AW_VER_NEGATIVE	自下向上显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
     *	    
     * 3、在窗体构造中调用上面声明的方法，用于窗体显示时显示动态效果：
     * 　　AnimateWindow(this.Handle, 300, AW_SLIDE + AW_VER_NEGATIVE);    //开始窗体动画
     * 　　
     * 4、在窗体关闭后事件Form_FormClosed事件中调用上面声明的方法，用于在窗体退出时显示动态效果：
     * 　　AnimateWindow(this.Handle, 300, AW_SLIDE + AW_VER_NEGATIVE + AW_HIDE);    //结束窗体动画
     * */

    /**
     * 【SetBounds说明】
     * /// <summary>
     * /// 将控件的边界设置为指定的位置和大小
     * /// </summary>
     * /// <param name="x">控件的新Left属性值</param>
     * /// <param name="y">控件的新Top属性值</param>
     * /// <param name="width">控件的新Width属性值</param>
     * /// <param name="height">控件的新Height属性值</param>
     * public void SetBounds(int x, int y, int width, int height);
     * */

    /**
     * 【附注：使用Screen类获取桌面的大小】
     * 　　Screen类表示单个系统上的一个或多个显示设备，其PrimaryScreen属性用来获取主显示，该属性返回一个Screen对象，而调用Screen对象的WorkingArea属性可以获取显示器的工作区，也就是桌面的大小。
     * */
}
