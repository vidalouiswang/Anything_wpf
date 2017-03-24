using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Anything;
using Anything_wpf_main_.cls;

namespace Anything_wpf_main_
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>



    public partial class MainWindow : Window
    {
        #region 额外

        /// <summary>
        /// 改变窗体大小
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //定义距离
            int GripSize = 8;
            int BorderSize = 5;

            //定义Window
            Window wnd = (Window)HwndSource.FromHwnd(hwnd).RootVisual;

            //判断消息
            if (msg == NativeMethods.WM_NCHITTEST)
            {
                int x = lParam.ToInt32() << 16 >> 16, y = lParam.ToInt32() >> 16;
                Point pt = wnd.PointFromScreen(new Point(x, y));

                //底部
                if (pt.X > GripSize && pt.X < wnd.ActualWidth - GripSize && pt.Y >= wnd.ActualHeight - BorderSize)
                {
                    handled = true;
                    return (IntPtr)NativeMethods.HTBOTTOM;
                }

                //右侧
                if (pt.Y > GripSize && pt.X > wnd.ActualWidth - BorderSize && pt.Y < wnd.ActualHeight - GripSize)
                {
                    handled = true;
                    return (IntPtr)NativeMethods.HTRIGHT;
                }

                //右下角
                if (pt.X > wnd.ActualWidth - GripSize && pt.Y >= wnd.ActualHeight - GripSize)
                {
                    handled = true;
                    return (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// 注册钩子
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
            }
        }

        #endregion

        #region 成员变量

        //边界颜色
        private Color bdrColor = new Color();

        //窗体效果对象
        //private FormResize effect = null;

        private Animation animation = new Animation();

        //关闭按钮的计数
        private byte btnCloseOnceClick = 0;

        //最大最小透明度
        private double MinOpacity = 0.03;
        private double MaxOpacity = 0.95;

        //指示窗体是否最大化
        private bool IsMaximized = false;

        //系统缩放
        public double ScaleX;
        public double ScaleY;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();

            //边界颜色定义
            bdrColor = Color.FromArgb(0xff, 0x28, 0x28, 0x28);
            this.bdrMainForm.Background = new SolidColorBrush(bdrColor);



        }

        #endregion

        #region 窗体事件响应

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //窗体事件处理
            this.StateChanged += new EventHandler(this.animation.Window_StateChanged);
            

            //设置窗体渐隐与显示
            animation.InitBdrStyle(ref this.bdrMain);


        }

        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        /// <summary>
        /// 位置改变时响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LocationChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region 按钮事件响应

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.btnCloseOnceClick++;
            if (this.btnCloseOnceClick >= 2) 
                this.animation.Close(this);
            else
                this.btnClose.ToolTip = "Click Once";
        }

        /// <summary>
        /// 最大化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.animation.SetMax(this);
            }
            else
            {
                this.animation.SetMax(this, 1);
            }

        }

        /// <summary>
        /// 最小化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            //effect.Hide(true);
            this.animation.SetMin(this);
            
        }

        /// <summary>
        /// 关闭按钮离开处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnCloseOnceClick = 0;
        }

        private void bdrMainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;
            else e.Effects = DragDropEffects.None;
        }

        private void bdrMainForm_Drop(object sender, DragEventArgs e)
        {
            String[] arr = (String[])e.Data.GetData(DataFormats.FileDrop);
            int i = 0;
            String str = "";
            while (i < arr.Length)
            {
                str = arr[i].ToString();
                this.Recent.Children.Add(Manage.AddItem(str));
                byte[] b = GetIcon.GetIconByteArray(str);
                i++;
            }

        }

        #endregion


    }
}
