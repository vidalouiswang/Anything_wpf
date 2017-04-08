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
    /// wndSE.xaml 的交互逻辑
    /// </summary>
    public partial class wndSE : Window
    {
        public wndSE()
        {
            InitializeComponent();
            if (Manage.SEList.Count>0)
            {
                foreach (Anoicess.Anoicess.Anoicess._Content i in Manage.SEList)
                {
                    Button btn = new Button();
                    btn.Style = btn.FindResource("NormalButtonStyle") as Style;
                    //btn.Background = brushTrans;
                    btn.VerticalAlignment = VerticalAlignment.Center;
                    btn.HorizontalAlignment = HorizontalAlignment.Center;
                    btn.Content = i.Name;
                    btn.Click += Btn_Click;
                    btn.KeyDown += Btn_KeyDown;
                    btn.Width = this.Width-10;
                    btn.Margin = new Thickness(1);
                    btn.BorderThickness = new Thickness(1);
                    this.spMain.Children.Add(btn);
                }
            }
        }

        private void Btn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Run(sender);
            }
        }

        private string _Keyword="";

        public string Keyword
        {
            get
            {
                return _Keyword;
            }

            set
            {
                _Keyword = value;
            }
        }

        private void Run(object sender)
        {
            Button btn = sender as Button;
            foreach (Anoicess.Anoicess.Anoicess._Content i in Manage.SEList)
            {
                if (i.Name == btn.Content.ToString())
                {
                    System.Diagnostics.Process.Start(i.Content.Replace("#@#", Keyword));
                    this.Close();
                }
            }
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Run(sender);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.spMain.Focus();
        }
    }
}
