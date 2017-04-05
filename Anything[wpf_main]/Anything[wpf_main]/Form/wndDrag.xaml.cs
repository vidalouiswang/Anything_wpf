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

namespace Anything_wpf_main_.Form
{
    /// <summary>
    /// wndDrag.xaml 的交互逻辑
    /// </summary>
    public partial class wndDrag : Window
    {

        private WrapPanel _IParent;
        private Item innerObj = null;

        public wndDrag()
        {
            InitializeComponent();
            this.Topmost = true;
        }

        public WrapPanel IParent
        {
            get
            {
                return _IParent;
            }

            set
            {
                _IParent = value;
            }
        }

        public Item InnerObj
        {
            get
            {
                return innerObj;
            }

            set
            {
                innerObj = value;
                this.spMain.Children.Add(innerObj);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Remove(innerObj);
            IParent.Children.Add(innerObj);
            this.Close();
        }
    }
}
