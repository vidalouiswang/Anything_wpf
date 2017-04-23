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
    /// wndPreSet.xaml 的交互逻辑
    /// </summary>
    public partial class wndPreSet : Window
    {
        public wndPreSet()
        {
            InitializeComponent();
        }

        public wndPreSet(string[] arr)
        {
            InitializeComponent();
            this.arrFiles = arr;
        }

        private string[] arrFiles;



        public string tagName
        {
            get { return (string)GetValue(tagNameProperty); }
            set { SetValue(tagNameProperty, value); }
        }

        public static readonly DependencyProperty tagNameProperty =
            DependencyProperty.Register("tagName", typeof(string), typeof(wndPreSet), new PropertyMetadata(""));




        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }


        private void Add()
        {
            wndProgressBar wpb = new wndProgressBar("Add Items...", "Please Wait", arrFiles.Length);
            if (string.IsNullOrEmpty(this.tagName))
            {
                //添加项目
                foreach (string s in this.arrFiles)
                {
                    Manage.WindowMain.Recent.Children.Add(Manage.AddItem(s));
                    wpb.Increase();
                }
                wpb.Increase();
            }
            else
            {
                Item item;
                //添加项目
                foreach (string s in this.arrFiles)
                {
                    item = Manage.AddItem(s,null,null,this.tagName);

                    Manage.FindAndInsert(item);
                    wpb.Increase();
                }
                wpb.Increase();
            }
        }

        private void btnAdd_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Add();
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            HotKey.SetForegroundWindow(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            this.txtTagName.Focus();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.btnAdd.Focus();
                this.Hide();
                Add();
                this.Close();
            }
        }
    }
}
