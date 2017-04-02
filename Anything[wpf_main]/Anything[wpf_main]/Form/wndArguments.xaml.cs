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

namespace Anything_wpf_main_
{
    /// <summary>
    /// wndArguments.xaml 的交互逻辑
    /// </summary>
    public partial class wndArguments : Window
    {





        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(wndArguments), new PropertyMetadata("Name"));

        public string Arguments
        {
            get { return (string)GetValue(ArgumentsProperty); }
            set { SetValue(ArgumentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Arguments.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArgumentsProperty =
            DependencyProperty.Register("Arguments", typeof(string), typeof(wndArguments), new PropertyMetadata(""));


        public wndArguments()
        {
            InitializeComponent();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
