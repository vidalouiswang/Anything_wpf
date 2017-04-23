using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Anything_wpf_main_.cls;
using Anything_wpf_main_.UserControls;

namespace Anything_wpf_main_.Form
{
    /// <summary>
    /// wndSE.xaml 的交互逻辑
    /// </summary>
    public partial class wndSE : Window
    {

        private DispatcherTimer timer = new DispatcherTimer();

        public wndSE()
        {
            InitializeComponent();
            InitTimer();
        }

        private void InitTimer()
        {
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (new WindowInteropHelper(this).Handle != HotKey.GetActiveWindow())
            {
                this.timer.Stop();
                this.Close();
            }
        }

        public wndSE(string keyword)
        {
            InitializeComponent();
            InitTimer();
            if (Manage.listOfSearchEngineInnerData.Count > 0)
            {
                foreach (SearchEngineItem item in Manage.listOfSearchEnginesVisualElement)
                {
                    item.Keyword = keyword;
                    this.spMain.Children.Add(item);
                }
            }
        }


        private void Window_Activated(object sender, EventArgs e)
        {
            if (this.spMain.Children.Count>0)
                this.spMain.Children[0].Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.spMain.Children.Clear();
        }

    }
}
