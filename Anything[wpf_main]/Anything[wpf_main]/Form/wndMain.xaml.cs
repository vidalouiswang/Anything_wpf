﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Anything;
using Anything_wpf_main_.cls;
using Anything_wpf_main_.Form;
using ApplicationInformations.Anything;

namespace Anything_wpf_main_
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class MainWindow : Window
    {



        #region 重载

        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
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
                    handle = true;
                    return (IntPtr)NativeMethods.HTBOTTOM;
                }

                //右侧
                if (pt.Y > GripSize && pt.X > wnd.ActualWidth - BorderSize && pt.Y < wnd.ActualHeight - GripSize)
                {
                    handle = true;
                    return (IntPtr)NativeMethods.HTRIGHT;
                }

                //右下角
                if (pt.X > wnd.ActualWidth - GripSize && pt.Y >= wnd.ActualHeight - GripSize)
                {
                    handle = true;
                    return (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                }
            }
            else if (msg==HotKey.WM_HOTKEY)
            {
                if (wParam.ToInt32() == HotKey.HOTKEYID_)
                {
                    //设置活动窗体
                    HotKey.SetForegroundWindow(new WindowInteropHelper(this).Handle);

                    //聚焦到检索框
                    SetFocus();
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

        //用于设置动画相关
        private Animation.animation animationInstance = Animation.GetInstance();

        //关闭按钮的计数
        private byte btnCloseOnceClick = 0;

        //用于自动存储位置大小的开关指示
        public bool IsInformationsInitialized = false;

        //用去触发上部标题栏的卷起动画，暂时不用
        MouseEventArgs me = new MouseEventArgs(Mouse.PrimaryDevice, 0);

        //提示窗体
        private wndTip tipMainForm = new wndTip();

        //项目图标大小，暂时不用
        private double ItemLength = 128;

        //用于搜索后的快速打开操作
        private bool QuickStart = false;

        //指示是否正在重命名项目
        public bool NowReName = false;



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

            IntPtr handle = new WindowInteropHelper(this).Handle;
            HotKey.RegisterHotKey(handle, HotKey.HOTKEYID_,(uint)HotKey.KeyModifiers.Ctrl|(uint)HotKey.KeyModifiers.Alt, (uint)System.Windows.Forms.Keys.D1);


            #region first layer
            this.WindowState = WindowState.Normal;
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

            #endregion

            #region second layer
            //用于自动存储位置大小的开关指示
            IsInformationsInitialized = true;

            //初始化后台数据
            Manage.InitializeData(this,ref this.Recent);

            //初始化删除计时器
            Manage.timer.Interval = TimeSpan.FromSeconds(3);
            Manage.timer.Stop();
            Manage.timer.Tick += Timer_Tick;

            //其他
            me.RoutedEvent = Border.MouseLeaveEvent;
            tipMainForm.Show();

            //获取插件
            Anything_wpf_main_.cls.Plugins.GetPlugins();

            //关联事件处理
            this.StateChanged += new EventHandler(animationInstance.Window_StateChanged);
            this.SizeChanged += new SizeChangedEventHandler(animationInstance.Window_SizeChanged);

            //设置窗体渐隐与显示
            animationInstance.InitBdrStyle(ref this.bdrMain);

            //关闭加载窗体
            Manage.WindowLoading.Close();

            #endregion
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

                Manage.WindowMainRect.left = (int)this.Left;
                Manage.WindowMainRect.right = (int)(this.Left + this.ActualWidth);
                Manage.WindowMainRect.top = (int)this.Top;
                Manage.WindowMainRect.bottom = (int)(this.Top + this.ActualHeight);
            }
            
        }

        /// <summary>
        /// 防止因意外不能关闭强制措施
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HotKey.UnregisterHotKey(new WindowInteropHelper(this).Handle, HotKey.HOTKEYID_);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 主窗体鼠标移动的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
                e.Handled = true;
            }
            else
            {
                if (this.Left <= 50 && this.Left > 0)
                    this.Left = 0;

                if (this.Top <= 50 && this.Top > 0)
                    this.Top = 0;
            }


        }

        /// <summary>
        /// 当窗体被激活时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_Activated(object sender, EventArgs e)
        {
            //当窗体被激活时无论如何都要正常显示
            this.WindowState = WindowState.Normal;

            //检查是否初始化完成
            if (IsInformationsInitialized)
            {

                //如果透明度小于设定的最大值且为最小化状态
                if (this.WindowState == WindowState.Minimized)
                {
                    //直接还原窗体
                    animationInstance.SetNormal(this);
                }
                else
                {
                    //否则只是未最小化而临时隐藏的，所以直接还原透明度
                    CheckHidden();
                }
            }
        }


        #endregion

        #region 控件事件响应

        /// <summary>
        /// 光标进入输入框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_MouseEnter(object sender, MouseEventArgs e)
        {
            Manage.ClearOrFillText(ref this.txtMain, true);
        }

        /// <summary>
        /// 光标离开输入框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_MouseLeave(object sender, MouseEventArgs e)
        {
            Manage.ClearOrFillText(ref this.txtMain, false);
        }

        /// <summary>
        /// 点击网络搜索按钮时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenSearchWindow(this.txtMain.Text);
        }

        /// <summary>
        /// 清空检索框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.BdrFunction.Style = null;
            Manage.ClearOrFillText(ref this.txtMain, true);
        }

        /// <summary>
        /// 检索框获得焦点的响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_GotFocus(object sender, RoutedEventArgs e)
        {
            this.BdrFunction.Style = null;
            Manage.ClearOrFillText(ref this.txtMain, true);
            
            CheckHidden();
        }

        /// <summary>
        /// 检索框丢失焦点的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_LostFocus(object sender, RoutedEventArgs e)
        {
            PackUp();
        }

        /// <summary>
        /// 移除定时器的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Manage.RemoveList.Count>0)
            {
                foreach(Item i in Manage.RemoveList)
                {
                    Manage.mMAIN.RemoveChild(i.ID);

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
                foreach (ItemData itemdata in Manage.listData)
                {
                    itemdata.SaveIcon();
                }
                tipMainForm.Close();
                animationInstance.Close(this);
            }
            else
                tipMainForm.ShowFixed(this, "Click again");
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
                animationInstance.SetMax(this);
            }
            else
            {
                animationInstance.SetMax(this, 1);
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
            animationInstance.SetMin(this);
            //this.Recent.Children.Add(StyleManagement.GetXAML() as Button);

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

        /// <summary>
        /// 检索框内容改变时的消息响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInformationsInitialized)
            {
                
                string str = this.txtMain.Text.Trim();
                if (!string.IsNullOrEmpty(str) && str != "Use keyword to search")
                {
                    this.btnSearch.Visibility = Visibility.Visible;

                    foreach (Item item in this.Recent.Children)
                    {
                        item.Hide();
                    }
                    List<Item> value = Manage.GetObjsByNameAndPath(str, str, ref this.Recent);

                    if (value != null)
                    {

                        if (value.Count <= 3)
                            QuickStart = true;
                        else
                            QuickStart = false;


                        foreach (Item i in value)
                        {
                            i.Show();
                        }
                    }

                }
                else
                {
                    this.btnSearch.Visibility = Visibility.Collapsed;
                    foreach (Item item in this.Recent.Children)
                    {
                        item.Show();
                    }
                }
            }
        }

        /// <summary>
        /// 检索框的鼠标移动的消息响应，用于取消事件冒泡，以防止鼠标选择文字时窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 内容按键的消息响应，设置检索框焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrlist_KeyDown(object sender, KeyEventArgs e)
        {
            SetFocus();
        }

        /// <summary>
        /// 主窗体按键时的消息响应，同上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_KeyDown(object sender, KeyEventArgs e)
        {
            SetFocus();
        }

        /// <summary>
        /// 快速打开搜索后的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.Key.ToString());
            if (e.Key == Key.Enter && !((e.KeyboardDevice.Modifiers & ModifierKeys.Control)==ModifierKeys.Control))
            {

                if (QuickStart)
                {
                    foreach (Item i in this.Recent.Children)
                    {
                        if (i.Visibility == Visibility.Visible)
                        {
                            i.RefItemData.Execute();
                            Manage.SaveKeyword(this.txtMain.Text);
                            this.txtMain.Text = "";
                            break;
                        }
                    }
                }
            }
            else if (e.Key == Key.Enter && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                Manage.OpenSearchWindow(this.txtMain.Text);
            }
            else if (e.Key == Key.F && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                foreach (Item i in this.Recent.Children)
                {
                    if (i.Visibility == Visibility.Visible)
                    {
                        i.refItemData.FindLocation();
                        Manage.SaveKeyword(this.txtMain.Text);
                        this.txtMain.Text = "";
                        break;
                    }
                }
            }
            else if (e.Key == Key.C && ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                this.txtMain.Text = "";
            }
            
        }

        /// <summary>
        /// 离开窗体时收起标题栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdrMain_MouseLeave(object sender, MouseEventArgs e)
        {
            PackUp();
        }

        #endregion

        #region 菜单项事件响应

        /// <summary>
        /// 从菜单项进入网络搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchWebMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenSearchWindow(this.txtMain.Text);
        }

        /// <summary>
        /// 打开手动添加窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenAddWindow();
        }

        /// <summary>
        /// 添加我的电脑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddComputerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddComputer());
        }

        /// <summary>
        /// 添加我的文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMyDocumentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddMyDocument());
        }

        /// <summary>
        /// 添加回收站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRecycleBinMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddRecycleBin());
        }

        /// <summary>
        /// 添加控制面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddControlPanelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddControlPanel());
        }

        /// <summary>
        /// 添加网络邻居
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNetworkNeighborhoodMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Recent.Children.Add(Manage.AddNetworkNeighbor());
        }
        #endregion

        #region 私有

        /// <summary>
        /// 收起标题栏
        /// </summary>
        private void PackUp()
        {
            if (IsInformationsInitialized)
            {
                this.BdrFunction.Style = this.FindResource("BdrFunctionStyle") as Style;
                Manage.ClearOrFillText(ref this.txtMain, false);
                me.RoutedEvent = Mouse.MouseLeaveEvent;
                this.BdrFunction.RaiseEvent(me);

                this.Topmost = false;
            }
        }

        /// <summary>
        /// 设置焦点到检索框
        /// </summary>
        private void SetFocus()
        {
            if (!NowReName)
            {
                this.BdrFunction.Style = null;
                CheckHidden();
                Manage.ClearOrFillText(ref this.txtMain, true);
                if (Keyboard.FocusedElement!=this.txtMain)
                {
                    this.txtMain.Focus();
                    Animation.UniversalBeginDoubleAnimation<Border>(ref this.BdrFunction,Border.HeightProperty,0.2,70,this.BdrFunction.ActualHeight);
                    Animation.UniversalBeginDoubleAnimation<Border>(ref this.BdrFunction, Border.OpacityProperty,0.2,1,this.BdrFunction.Opacity);
                }
            }
        }

        /// <summary>
        /// 检查窗体是否隐藏
        /// </summary>
        private void CheckHidden()
        {
            if (this.bdrMain.Opacity < AppInfoOperations.GetMaxOpacity())
            {
                Animation.UniversalBeginDoubleAnimation<Border>(ref this.bdrMain, OpacityProperty, 0.2, AppInfoOperations.GetMaxOpacity());
            }
        }



        #endregion

        private void scrlist_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
            {
                if (Keyboard.FocusedElement is ScrollViewer)
                {
                    foreach (Item i in this.Recent.Children)
                    {
                        i.Bdr.Focus();
                        break;
                    }
                }
            }
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            wndSettings ws = new wndSettings();
            ws.Show();
        }
    }
}
