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
            {
                this.DragMove();
                if (this.Left>=Manage.WindowMainRect.left
                    && (this.Left+this.ActualWidth)<=Manage.WindowMainRect.right
                    && this.Top>=Manage.WindowMainRect.top
                    && (this.Top+this.ActualHeight)<=Manage.WindowMainRect.bottom)
                {
                    SendBack();
                }

                if (this.Left <=100)
                    this.Left = 0;
                if (this.Top <= 100)
                    this.Top = 0;
                if (this.Left >= (SystemParameters.FullPrimaryScreenWidth - this.ActualWidth-100))
                    this.Left = SystemParameters.FullPrimaryScreenWidth - this.ActualWidth;
                if (this.Top >= (SystemParameters.FullPrimaryScreenHeight - this.ActualHeight-100))
                    this.Top = SystemParameters.FullPrimaryScreenHeight - this.ActualHeight;
            }

        }
        public void SendBack()
        {
            this.spMain.Children.Remove(innerObj);
            innerObj.Bdr.Style = innerObj.FindResource("BdrStyle") as Style;
            IParent.Children.Add(innerObj);
            this.Close();
        }

    }
}
