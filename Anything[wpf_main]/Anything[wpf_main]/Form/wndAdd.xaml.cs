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
using Anything_wpf_main_.cls;

namespace Anything_wpf_main_.Form
{
    /// <summary>
    /// wndAdd.xaml 的交互逻辑
    /// </summary>
    public partial class wndAdd : Window
    {

        public MainWindow wnd = null;
        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }
        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(wndAdd), new PropertyMetadata(""));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(wndAdd), new PropertyMetadata(""));

        public string Arguments
        {
            get { return (string)GetValue(ArgumentsProperty); }
            set { SetValue(ArgumentsProperty, value); }
        }
        public static readonly DependencyProperty ArgumentsProperty =
            DependencyProperty.Register("Arguments", typeof(string), typeof(wndAdd), new PropertyMetadata(""));

        private wndTip Tip = new wndTip();

        public wndAdd()
        {
            InitializeComponent();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemName.Trim()) && !string.IsNullOrEmpty(Path.Trim()))
            {
                wnd.Recent.Children.Add( Manage.AddItem(Path, ItemName, Arguments));
                this.Close();
            }
            else
            {
                Tip.ShowFixed(this, "Please checkout information");
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

