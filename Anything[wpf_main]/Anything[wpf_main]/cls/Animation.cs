using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Input;
using Anything_wpf_main_;
using ApplicationInformations.Anything;

namespace Anything
{
    class Animation
    {
        private Window wnd = null;
        private int Way = 0;
        private bool Closing = false;
        
        public Animation()
        {

        }
        public void InitBdrStyle(ref Border bdr)
        {
            //获取最小透明度
            double minOpa = AppInfoOperations.GetMinOpacity();

            //获取最大透明度
            double maxOpa = AppInfoOperations.GetMaxOpacity();

            //获取显示时长
            double showTimeSpan = AppInfoOperations.GetShowTimeSpan();

            //获取隐藏时长
            double hideTimeSpan = AppInfoOperations.GetHideTimeSpan();

            //获取超时时长
            double TimeoutSpan = AppInfoOperations.GetTimeoutTimeSpan();


            DoubleAnimation daShow = new DoubleAnimation();
            DoubleAnimation daHide = new DoubleAnimation();

            Storyboard sbShow = new Storyboard();
            Storyboard sbHide = new Storyboard();

            BeginStoryboard bsShow = new BeginStoryboard();
            BeginStoryboard bsHide = new BeginStoryboard();

            EventTrigger etShow = new EventTrigger(Mouse.MouseEnterEvent);
            EventTrigger etHide = new EventTrigger(Mouse.MouseLeaveEvent);

            Style style = new Style();

            
            //show
            daShow.To = maxOpa;
            daShow.Duration = TimeSpan.FromSeconds(showTimeSpan);
            Storyboard.SetTargetProperty(daShow, new PropertyPath(UIElement.OpacityProperty));

            Storyboard.SetDesiredFrameRate(sbShow, 60);
            sbShow.Children.Add(daShow);
            bsShow.Storyboard = sbShow;

            etShow.Actions.Add(bsShow);
            style.Triggers.Add(etShow);


            //hide

            daHide.To = minOpa;
            daHide.Duration = TimeSpan.FromSeconds(hideTimeSpan);
            Storyboard.SetTargetProperty(daHide, new PropertyPath(UIElement.OpacityProperty));

            sbHide.BeginTime = TimeSpan.FromSeconds(TimeoutSpan);
            Storyboard.SetDesiredFrameRate(sbHide, 60);
            sbHide.Children.Add(daHide);
            bsHide.Storyboard = sbHide;

            etHide.Actions.Add(bsHide);
            style.Triggers.Add(etHide);


            //apply
            bdr.Style = style;
        }

        private void OpacityChange(Window wnd,double From, double To,TimeSpan ts,bool Max=false)
        {

            DoubleAnimation da = new DoubleAnimation();
            da.From = From;
            da.To = To;
            if (Max)
            {
                da.Completed += Da_Completed;
            }
            da.Duration = ts;
            wnd.BeginAnimation(Window.OpacityProperty, da);

        }

        private void Da_Completed(object sender, EventArgs e)
        {
            if (Closing)
            {
                this.wnd.Close();
            }
            if (Way==0)
            {
                wnd.WindowState = WindowState.Maximized;
                OpacityChange(wnd, 0, AppInfoOperations.GetMaxOpacity(), TimeSpan.FromSeconds(0.3));
            }
            else
            {
                wnd.WindowState = WindowState.Normal;
                OpacityChange(wnd, 0, AppInfoOperations.GetMaxOpacity(), TimeSpan.FromSeconds(0.3));
                Way = 0;
            }
        }

        #region 操作
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="wnd"></param>
        public void SetMin(Window wnd)
        {
            DoubleAnimation da1 = new DoubleAnimation();

            da1.Duration = TimeSpan.FromSeconds(0.5);

            da1.From = wnd.Top ;
            da1.To = da1.From + 50;

            wnd.BeginAnimation(Window.TopProperty, da1);
            OpacityChange(wnd, wnd.Opacity, 0, TimeSpan.FromSeconds(0.5));
        }
        /// <summary>
        /// 还原
        /// </summary>
        /// <param name="wnd"></param>
        public  void SetNormal(Window wnd)
        {
            DoubleAnimation da1 = new DoubleAnimation();

            da1.Duration = TimeSpan.FromSeconds(0.5);

            wnd.BeginAnimation(Window.TopProperty, da1);
            OpacityChange(wnd, 0, AppInfoOperations.GetMaxOpacity(), TimeSpan.FromSeconds(0.5));

        }
        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="Way"></param>
        public void SetMax(Window wnd,int Way=0)
        {
            if (Way==0)
            {
                this.wnd = wnd;
                OpacityChange(wnd, wnd.Opacity, 0, TimeSpan.FromSeconds(0.3), true);
            }
            else
            {
                this.wnd = wnd;
                this.Way = Way;
                OpacityChange(wnd, wnd.Opacity, 0, TimeSpan.FromSeconds(0.3), true);
            }
            
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="wnd"></param>
        public void Close(Window wnd)
        {
            this.Closing = true;
            this.wnd = wnd;
            OpacityChange(wnd, wnd.Opacity, 0, TimeSpan.FromSeconds(0.3), true);
        }

        #endregion

        #region 事件响应
        public void Window_StateChanged(object sender, EventArgs e)
        {
            MainWindow wnd = (MainWindow)sender;
            if (wnd.Opacity == 0 && wnd.WindowState == WindowState.Minimized)
            {
                wnd.WindowState = WindowState.Normal;
                wnd.Opacity = 0.01;
                SetNormal(wnd);
            }

        }

        public void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            Window wnd = (Window)sender;
            wnd.UpdateLayout();
            if (wnd.Opacity == 0 && wnd.WindowState == WindowState.Normal)
            {
                wnd.WindowState = WindowState.Minimized;

            }

        }
        #endregion
    }
}
