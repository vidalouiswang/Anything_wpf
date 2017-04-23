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
using Anything_wpf_main_.cls;

namespace Anything_wpf_main_.UserControls
{
    /// <summary>
    /// SearchEngineItem.xaml 的交互逻辑
    /// </summary>
    public partial class SearchEngineItem : UserControl
    {
        public SearchEngineItem()
        {
            InitializeComponent();
        }

        public SearchEngineItem(string se_name,string url,string keyword)
        {
            InitializeComponent();
            SEName = se_name;
            URL = url;
            Keyword = keyword;
        }

        public string SEName
        {
            get { return (string)GetValue(SENameProperty); }
            set { SetValue(SENameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SEName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SENameProperty =
            DependencyProperty.Register("SEName", typeof(string), typeof(SearchEngineItem), new PropertyMetadata(""));




        public string URL
        {
            get { return (string)GetValue(URLProperty); }
            set { SetValue(URLProperty, value); }
        }

        // Using a DependencyProperty as the backing store for URL.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty URLProperty =
            DependencyProperty.Register("URL", typeof(string), typeof(SearchEngineItem), new PropertyMetadata(""));






        public string Keyword
        {
            get { return (string)GetValue(KeywordProperty); }
            set { SetValue(KeywordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Keyword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeywordProperty =
            DependencyProperty.Register("Keyword", typeof(string), typeof(SearchEngineItem), new PropertyMetadata(""));

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StackPanel sp=null;

            if (this.Parent is StackPanel)
            {
                sp = (StackPanel)this.Parent;

                sp.Children.Remove(this);

                Manage.listOfSearchEnginesVisualElement.Remove(this);

                foreach (Anoicess.Anoicess.Anoicess._Content item in Manage.listOfSearchEngineInnerData)
                {
                    if (item.Name==this.SEName)
                    {

                        Manage.mSearchEngine.Remove(item.Name, item.Key);

                        Manage.listOfSearchEngineInnerData.Remove(item);

                        Manage.TipPublic.ShowFixed(Manage.WindowMain, "Done");

                        break; 
                    }
                }
            }
            e.Handled = true;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(Keyword))
            {
                int rtnCode = Manage.SearchOnWeb(Keyword, URL);
                if (rtnCode != 0)
                {
                    if (rtnCode == -1)
                        Manage.TipPublic.ShowFixed(Manage.WindowMain, "Information wrong");
                    else if (rtnCode == -2)
                        Manage.TipPublic.ShowFixed(Manage.WindowMain, "Keyword can not be empty");
                }
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                if (!string.IsNullOrEmpty(Keyword))
                {
                    int rtnCode = Manage.SearchOnWeb(Keyword, URL);
                    if (rtnCode != 0)
                    {
                        if (rtnCode == -1)
                            Manage.TipPublic.ShowFixed(Manage.WindowMain, "Information wrong");
                        else if (rtnCode == -2)
                            Manage.TipPublic.ShowFixed(Manage.WindowMain, "Keyword can not be empty");
                    }
                }
            }
        }
    }
}
