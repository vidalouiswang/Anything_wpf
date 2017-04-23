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
    /// wndHotKeyManagement.xaml 的交互逻辑
    /// </summary>
    public partial class wndHotKeyManagement : Window
    {
        public wndHotKeyManagement()
        {
            InitializeComponent();

            LoadHotKeys();
        }

        private void LoadHotKeys()
        {
            foreach (HotKeyItem item in Manage.listOfApplicationHotKey)
            {
                TextBlock tb = new TextBlock();

                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;

                if (item.IParent is Item)
                {
                    tb.Text = (item.IParent as Item).Name_Property;
                    sp.Children.Add(tb);
                    //TODO:
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
