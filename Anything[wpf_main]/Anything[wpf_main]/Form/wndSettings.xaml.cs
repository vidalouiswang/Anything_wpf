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
using Anything_wpf_main_.cls;

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
        public double MaxOpacity
        {
            get { return (double)GetValue(MaxOpacityProperty); }
            set { SetValue(MaxOpacityProperty, value); }
        }
        public static readonly DependencyProperty MaxOpacityProperty =
            DependencyProperty.Register("MaxOpacity", typeof(double), typeof(wndSettings), new PropertyMetadata((double)1.0, PropertyChanged));





        /// <summary>
        /// 最小不透明度
        /// </summary>
        public double MinOpacity
        {
            get { return (double)GetValue(MinOpacityProperty); }
            set { SetValue(MinOpacityProperty, value); }
        }
        public static readonly DependencyProperty MinOpacityProperty =
            DependencyProperty.Register("MinOpacity", typeof(double), typeof(wndSettings), new PropertyMetadata((double)0.1, PropertyChanged));





        /// <summary>
        /// 淡入时长
        /// </summary>
        public double Fadein
        {
            get { return (double)GetValue(FadeinProperty); }
            set { SetValue(FadeinProperty, value); }
        }
        public static readonly DependencyProperty FadeinProperty =
            DependencyProperty.Register("Fadein", typeof(double), typeof(wndSettings), new PropertyMetadata((double)3.0, PropertyChanged));





        /// <summary>
        /// 淡出时长
        /// </summary>
        public double Fadeout
        {
            get { return (double)GetValue(FadeoutProperty); }
            set { SetValue(FadeoutProperty, value); }
        }
        public static readonly DependencyProperty FadeoutProperty =
            DependencyProperty.Register("Fadeout", typeof(double), typeof(wndSettings), new PropertyMetadata((double)3.0, PropertyChanged));



        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double ItemSize
        {
            get { return (double)GetValue(ItemSizeProperty); }
            set { SetValue(ItemSizeProperty, value); }
        }
        public static readonly DependencyProperty ItemSizeProperty =
            DependencyProperty.Register("ItemSize", typeof(double), typeof(wndSettings), new PropertyMetadata((double)128.0,PropertyChanged));




        private static void PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            wndSettings Base = (wndSettings)sender;

            switch (e.Property.ToString())
            {
                case "MaxOpacity":
                    AppInfoOperations.SetMaxOpacity((double)e.NewValue);
                    break;
                case "MinOpacity":
                    AppInfoOperations.SetMinOpacity((double)e.NewValue);
                    break;
                case "Fadein":
                    AppInfoOperations.SetShowTimeSpan((double)e.NewValue);
                    break;
                case "Fadeout":
                    AppInfoOperations.SetHideTimeSpan((double)e.NewValue);
                    break;
                case "ItemSize":
                    AppInfoOperations.SetItemSize((double)e.NewValue);

                    foreach (Item i in Manage.WindowMain.Recent.Children)
                    {
                        i.Length = (double)e.NewValue;
                    }

                    break;
                default:
                    break;
                    
            }
        }


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
            this.MaxOpacity = AppInfoOperations.GetMaxOpacity();
            this.MinOpacity = AppInfoOperations.GetMinOpacity(); 
            this.Fadein = AppInfoOperations.GetShowTimeSpan(); 
            this.Fadeout = AppInfoOperations.GetHideTimeSpan();
            this.ItemSize = AppInfoOperations.GetItemSize();
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

        private void sldr_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }
    }
}
