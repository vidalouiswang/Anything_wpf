using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Anything_wpf_main_.Form
{
    /// <summary>
    /// wndTip.xaml 的交互逻辑
    /// </summary>
    public partial class wndTip : Window
    {
        public string Tip
        {
            get { return (string)GetValue(TipProperty); }
            set { SetValue(TipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TipProperty =
            DependencyProperty.Register("Tip", typeof(string), typeof(wndTip), new PropertyMetadata("This is tip"));

        private DispatcherTimer timerFollow = new DispatcherTimer();
        private DispatcherTimer timerFixed = new DispatcherTimer();
        private Window wnd = null;

        private int Mode = -1;

        private double OffsetX = 0;
        private double OffsetY = 0;

        public wndTip()
        {
            InitializeComponent();
            timerFollow.Interval = TimeSpan.FromMilliseconds(5);
            timerFollow.Tick += TimerFollow_Tick;
            timerFollow.Stop();

            timerFixed.Interval = TimeSpan.FromSeconds(3);
            timerFixed.Tick += TimerFixed_Tick;
            timerFixed.Stop();

            this.Topmost = true;
            this.ShowInTaskbar = false;
        }

        private void TimerFixed_Tick(object sender, EventArgs e)
        {
            if (this.Mode == 0)
            {
                this.HideMe();
                if (timerFixed.IsEnabled == true)
                    timerFixed.IsEnabled = false;
            }
            else
            {
                this.HideMe();
                if (timerFollow.IsEnabled == true)
                    timerFollow.IsEnabled = false;
            }
        }

        private void TimerFollow_Tick(object sender, EventArgs e)
        {
            if (wnd != null)
            {
                Point pt = GetPoint();
                this.Left = pt.X + OffsetX;
                this.Top = pt.Y + OffsetY;
            }
        }

        private Point GetPoint()
        {

            if (wnd != null)
            {
                double x = Mouse.GetPosition(wnd).X + wnd.Left;
                double y = Mouse.GetPosition(wnd).Y + wnd.Top;
                return new Point(x, y);
            }
            else
                return new Point(0, 0);
        }

        public void ClearOffset()
        {
            this.OffsetX = 0;
            this.OffsetY = 0;
        }

        public void ShowFixed(Window wnd, string Text, double OffsetX = 0, double OffsetY = 0)
        {
            ClearOffset();
            this.wnd = wnd;
            this.Mode = 0;
            this.Tip = Text;
            Point pt = GetPoint();
            this.Left = pt.X + OffsetX;
            this.Top = pt.Y + OffsetY;
            DoubleAnimation da = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1), FillBehavior.HoldEnd);
            this.BeginAnimation(OpacityProperty, da);
            timerFixed.Start();
        }

        public void ShowFollow(Window wnd, string Text, double OffsetX = 0, double OffsetY = 0)
        {
            this.Mode = 1;
            this.wnd = wnd;
            this.Tip = Text;
            Point pt = GetPoint();
            this.Left = pt.X + OffsetX;
            this.Top = pt.Y + OffsetY;
            this.OffsetX = OffsetX;
            this.OffsetY = OffsetY;
            DoubleAnimation da = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1), FillBehavior.HoldEnd);
            this.BeginAnimation(OpacityProperty, da);
            timerFollow.Start();
        }

        public void HideMe()
        {
            if (timerFollow.IsEnabled == true)
                timerFollow.IsEnabled = false;

            DoubleAnimation da = new DoubleAnimation(this.Opacity, 0, TimeSpan.FromSeconds(0.3), FillBehavior.HoldEnd);
            this.BeginAnimation(OpacityProperty, da);
        }
    }
}
