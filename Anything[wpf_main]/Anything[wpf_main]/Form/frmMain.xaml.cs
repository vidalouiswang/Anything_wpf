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
using System.Windows.Media.Animation;
using Anything;
using Anything_wpf_main_.cls;
using ApplicationInformations.Anything;

namespace Anything_wpf_main_
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class MainWindow : Window
    {
        #region 改变大小

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

        private Animation animation = new Animation();

        //关闭按钮的计数
        private byte btnCloseOnceClick = 0;

        //用于自动存储位置大小的开关指示
        public bool IsInformationsInitialized = false;

        MouseEventArgs me = new MouseEventArgs(Mouse.PrimaryDevice, 0);

        private wndTip tipMainForm = new wndTip();

        private double ItemLength = 128;
        


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

        /// <summary>
        /// 清空搜索框
        /// </summary>
        public void ClearSearch()
        {
            this.txtMain.Text = "Use keyword to search";
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
            //关联事件处理
            this.StateChanged += new EventHandler(this.animation.Window_StateChanged);
            this.SizeChanged += new SizeChangedEventHandler(this.animation.Window_SizeChanged);

            double ScreenWidth = SystemParameters.PrimaryScreenWidth;
            double ScreenHeight = SystemParameters.PrimaryScreenHeight;


            //获取位置
            //并检查合法性
            double readLeft = AppInfoOperations.GetLeft();
            double readTop = AppInfoOperations.GetTop();

            if (readLeft <= 2)
                this.Left = readLeft - 2;
            else
                this.Left = 0;

            if (readTop <= 1)
                this.Top = readTop - 1;
            else
                this.Top = 0;

            //获取宽高
            double readWidth= AppInfoOperations.GetWidth();
            double readHeight = AppInfoOperations.GetHeight();
            
            //检查数值合法性
            if (readWidth > ScreenWidth) readWidth = ScreenWidth;
            if (readHeight > ScreenHeight) readHeight = ScreenHeight;

            //应用宽高
            this.Width = readWidth;
            this.Height = readHeight;

            //设置窗体渐隐与显示
            animation.InitBdrStyle(ref this.bdrMain);

            //用于自动存储位置大小的开关指示
            IsInformationsInitialized = true;

            Manage.InitializeData(ref this.Recent);

            Manage.timer.Interval = TimeSpan.FromSeconds(3);
            Manage.timer.Stop();
            Manage.timer.Tick += Timer_Tick;

            me.RoutedEvent = Border.MouseLeaveEvent;

            tipMainForm.Show();
        }

        private void MainWindow_AfterExecuted(object sender, RoutedEventArgs e)
        {
            this.txtMain.Text = "";
        }





        /// <summary>
        /// 位置改变时响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            if (IsInformationsInitialized)
            {
                AppInfoOperations.SetLeft(this.Left);
                AppInfoOperations.SetTop(this.Top);
            }
            
        }

        #endregion

        #region 按钮事件响应

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Manage.RemoveList.Count>0)
            {
                foreach(Item i in Manage.RemoveList)
                {
                    Manage.MAIN.RemoveChild(i.ID);

                    FileOperation.DeleteFile(Manage.IconPath + i.ID + ".ib");

                    this.Recent.Children.Remove(i);
                }
            }

            
            Manage.timer.Stop();
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.btnCloseOnceClick++;
            if (this.btnCloseOnceClick >= 2)
            {
                tipMainForm.Close();
                this.animation.Close(this);
            }
            else
                tipMainForm.ShowFixed(this, "Click again",10,15);
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
            tipMainForm.HideMe();
        }

        /// <summary>
        /// 当拖放文件进入区域时的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdrMainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 当拖放文件拖放时的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdrMainForm_Drop(object sender, DragEventArgs e)
        {
            //获取拖放的文件
            String[] arr = (String[])e.Data.GetData(DataFormats.FileDrop);

            //添加项目
            foreach (string s in arr)
            {
                //
                this.Recent.Children.Add(Manage.AddItem(s));
                byte[] b = GetIcon.GetIconByteArray(s);
            }

        }




        #endregion

        private void txtMain_GotFocus(object sender, RoutedEventArgs e)
        {
            this.BdrFunction.Style = null;
            if (this.txtMain.Text.Trim() == "Use keyword to search") 
                this.txtMain.Text = "";

        }

        private void txtMain_LostFocus(object sender, RoutedEventArgs e)
        {
            this.BdrFunction.Style = this.FindResource("BdrFunctionStyle") as Style;
            if (this.txtMain.Text.Trim()=="")
                this.txtMain.Text = "Use keyword to search";

            this.BdrFunction.RaiseEvent(me);
        }

        private void Me_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void txtMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInformationsInitialized)
            {
                string str = this.txtMain.Text.Trim();
                if (!string.IsNullOrEmpty(str) && str!="Use keyword to search")
                {
                    foreach (Item item in this.Recent.Children)
                    {
                        item.Hide();
                    }
                    List<string> value = Manage.GetObjsByNameAndPath(str, str, ref this.Recent);

                    if (value != null)
                    {
                        foreach (string s in value)
                        {
                            foreach (Item i in this.Recent.Children)
                            {
                                if (i.Name_Property == s)
                                    i.Show();
                            }
                        }
                    }
                }
                else
                {
                    foreach (Item item in this.Recent.Children)
                    {
                        item.Show();
                    }
                }
            }
        }

        private void txtMain_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void scrlist_KeyDown(object sender, KeyEventArgs e)
        {
            
            this.txtMain.Focus();
            DoubleAnimation da = new DoubleAnimation(this.BdrFunction.ActualHeight, 70, TimeSpan.FromSeconds(0.2), FillBehavior.HoldEnd);
            this.BdrFunction.BeginAnimation(Border.HeightProperty, da);
        }

        private void Me_KeyDown(object sender, KeyEventArgs e)
        {
            this.txtMain.Focus();
            DoubleAnimation da = new DoubleAnimation(this.BdrFunction.ActualHeight, 70, TimeSpan.FromSeconds(0.2), FillBehavior.HoldEnd);
            this.BdrFunction.BeginAnimation(Border.HeightProperty, da);
        }
    }
}
