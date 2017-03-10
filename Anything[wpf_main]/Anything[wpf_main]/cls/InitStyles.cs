using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Input;

namespace Anything
{
    class InitStyles
    {
        public  InitStyles() {}
        public static void InitBdrStyle(ref Border bdr)
        {
            //获取最小透明度
            double minOpa = GetInfomation.GetMinOpacity();

            //获取最大透明度
            double maxOpa = GetInfomation.GetMaxOpacity();

            //获取显示时长
            double showTimeSpan = GetInfomation.GetShowTimeSpan();

            //获取隐藏时长
            double hideTimeSpan = GetInfomation.GetHideTimeSpan();

            //获取超时时长
            double TimeoutSpan = GetInfomation.GetTimeoutTimeSpan();


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

            sbShow.Children.Add(daShow);
            bsShow.Storyboard = sbShow;

            etShow.Actions.Add(bsShow);
            style.Triggers.Add(etShow);


            //hide

            daHide.To = minOpa;
            daHide.Duration = TimeSpan.FromSeconds(hideTimeSpan);
            Storyboard.SetTargetProperty(daHide, new PropertyPath(UIElement.OpacityProperty));

            sbHide.BeginTime = TimeSpan.FromSeconds(TimeoutSpan);
            sbHide.Children.Add(daHide);
            bsHide.Storyboard = sbHide;

            etHide.Actions.Add(bsHide);
            style.Triggers.Add(etHide);


            //apply
            bdr.Style = style;
        }

    }
}
