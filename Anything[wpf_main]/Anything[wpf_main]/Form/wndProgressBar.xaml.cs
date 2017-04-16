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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Anything_wpf_main_.Form
{
    /// <summary>
    /// wndProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class wndProgressBar : Window
    {
        public wndProgressBar()
        {
            InitializeComponent();
        }
        public wndProgressBar(string Head,string Foot,double Max=100,double Value=0)
        {
            InitializeComponent();
            this.Head = Head;
            this.Foot = Foot;
            this.Max = Max;
            this.Value = Value;
            this.Topmost = true;
            this.Show();
        }


        public string Head
        {
            get { return (string)GetValue(HeadProperty); }
            set { SetValue(HeadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Head.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadProperty =
            DependencyProperty.Register("Head", typeof(string), typeof(wndProgressBar), new PropertyMetadata("This is Head"));



        public string  Foot
        {
            get { return (string )GetValue(FootProperty); }
            set { SetValue(FootProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FootProperty =
            DependencyProperty.Register("Foot", typeof(string ), typeof(wndProgressBar), new PropertyMetadata("This is Foot"));




        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(wndProgressBar), new PropertyMetadata(0.0));





        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(wndProgressBar), new PropertyMetadata(100.0));



        public void Increase()
        {
            if (Value < Max)
            {
                Value++;
                DoEvents();
            }
            else
            {
                Value = Max;
                this.Close();
            }
        }

        /// <summary>
        /// 用于更新进度显示UI
        /// </summary>
        public static void DoEvents()
        {
            DispatcherFrame tf = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(delegate (object tmp)
                {
                    (tmp as DispatcherFrame).Continue = false;
                    return null;
                }
            ), tf);
            Dispatcher.PushFrame(tf);
        }

        public void Decrease()
        {
            if (Value > 0)
            {
                Value--;
                DoEvents();
            }
            else
                Value = 0;
        }
    }
}
