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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Anything_wpf_main_.UserControls
{
    /// <summary>
    /// ProgressBarEx.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressBarEx : UserControl
    {
        public ProgressBarEx()
        {
            InitializeComponent();

        }

        #region dp


        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ProgressBarEx), new PropertyMetadata(0.0,PropertyChanged));



        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(ProgressBarEx), new PropertyMetadata(100.0));



        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(ProgressBarEx), new PropertyMetadata(0.0));


        #endregion

        private static void PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ProgressBarEx pbe = (ProgressBarEx)sender;

            pbe.bdrInner.Width = (pbe.Value / pbe.Maximum) * (pbe.bdrOuter.ActualWidth - (pbe.bdrOuter.BorderThickness.Left+pbe.bdrOuter.BorderThickness.Right)-(pbe.bdrInner.Margin.Left+pbe.bdrInner.Margin.Right)-1);
        }

    }
}
