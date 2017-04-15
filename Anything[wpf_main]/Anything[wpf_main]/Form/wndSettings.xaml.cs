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
using System.Text.RegularExpressions;
using ApplicationInformations.Anything;

namespace Anything_wpf_main_.Form
{
    /// <summary>
    /// wndSettings.xaml 的交互逻辑
    /// </summary>
    public partial class wndSettings : Window
    {

        /// <summary>
        /// 最大不透明度
        /// </summary>
        public string MaxOpacity
        {
            get { return (string)GetValue(MaxOpacityProperty); }
            set { SetValue(MaxOpacityProperty, value); }
        }
        public static readonly DependencyProperty MaxOpacityProperty =
            DependencyProperty.Register("MaxOpacity", typeof(string), typeof(wndSettings), new PropertyMetadata(""));



        /// <summary>
        /// 最小不透明度
        /// </summary>
        public string MinOpacity
        {
            get { return (string)GetValue(MinOpacityProperty); }
            set { SetValue(MinOpacityProperty, value); }
        }
        public static readonly DependencyProperty MinOpacityProperty =
            DependencyProperty.Register("MinOpacity", typeof(string), typeof(wndSettings), new PropertyMetadata(""));



        /// <summary>
        /// 淡入时长
        /// </summary>
        public string Fadein
        {
            get { return (string)GetValue(FadeinProperty); }
            set { SetValue(FadeinProperty, value); }
        }
        public static readonly DependencyProperty FadeinProperty =
            DependencyProperty.Register("Fadein", typeof(string), typeof(wndSettings), new PropertyMetadata(""));



        /// <summary>
        /// 淡出时长
        /// </summary>
        public string Fadeout
        {
            get { return (string)GetValue(FadeoutProperty); }
            set { SetValue(FadeoutProperty, value); }
        }
        public static readonly DependencyProperty FadeoutProperty =
            DependencyProperty.Register("Fadeout", typeof(string), typeof(wndSettings), new PropertyMetadata(""));



        /// <summary>
        /// 构造
        /// </summary>
        public wndSettings()
        {
            LoadData();
            InitializeComponent();
            
        }

        private void txtOpacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tbox = sender as TextBox;
            string src = tbox.Text;
            Match ma = Regex.Match(src, "[0-9\\.]+");
            tbox.Text = ma.ToString();
            tbox = null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LoadData()
        {
            this.MaxOpacity = AppInfoOperations.GetMaxOpacity().ToString();
            this.MinOpacity = AppInfoOperations.GetMinOpacity().ToString(); ;
            this.Fadein = AppInfoOperations.GetShowTimeSpan().ToString(); ;
            this.Fadeout = AppInfoOperations.GetHideTimeSpan().ToString(); ;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //保存数据
            AppInfoOperations.SetMaxOpacity(Convert.ToDouble(this.MaxOpacity));
            AppInfoOperations.SetMinOpacity(Convert.ToDouble(this.MinOpacity));
            AppInfoOperations.SetShowTimeSpan(Convert.ToDouble(this.Fadein));
            AppInfoOperations.SetHideTimeSpan(Convert.ToDouble(this.Fadeout));

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
                e.Handled = true;
            }
        }

        private void txt_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }
    }
}
