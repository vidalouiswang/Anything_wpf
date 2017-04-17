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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Anything_wpf_main_.cls;

namespace Anything_wpf_main_.Form
{
    /// <summary>
    /// wndLoading.xaml 的交互逻辑
    /// </summary>
    public partial class wndLoading : Window
    {
        public wndLoading()
        {
            InitializeComponent();
            Color c1 = Color.FromArgb(0xea,16,82,233);
            Color c2 = Color.FromArgb(0xea, 228, 27, 75);
            Color c3 = Color.FromArgb(0xea, 6, 230, 128);

            BeginColorAnimation(ref txtFront0, c1, c3, c2);
            BeginColorAnimation(ref txtFront1, c3, c1, c2);
            BeginColorAnimation(ref txtFront2, c1, c2, c3);
            BeginColorAnimation(ref txtFront3, c2, c3, c1);
            BeginColorAnimation(ref txtFront4, c3, c2, c1);
            BeginColorAnimation(ref txtFront5, c1, c3, c2);
            BeginColorAnimation(ref txtFront6, c3, c1, c2);
            BeginColorAnimation(ref txtFront7, c1, c2, c3);

            BeginColorAnimation(ref txtBack0, c1, c3, c2);
            BeginColorAnimation(ref txtBack1, c3, c1, c2);
            BeginColorAnimation(ref txtBack2, c1, c2, c3);
            BeginColorAnimation(ref txtBack3, c2, c3, c1);
            BeginColorAnimation(ref txtBack4, c3, c2, c1);
            BeginColorAnimation(ref txtBack5, c1, c3, c2);
            BeginColorAnimation(ref txtBack6, c3, c1, c2);
            BeginColorAnimation(ref txtBack7, c1, c2, c3);
        }

        private void BeginColorAnimation(ref TextBlock txt, Color c0, Color c1, Color c2)
        {
            SolidColorBrush brushSC = new SolidColorBrush();

            ColorAnimationUsingKeyFrames caKeyFrames = new ColorAnimationUsingKeyFrames();

            ColorKeyFrameCollection keyFrames0 = caKeyFrames.KeyFrames;
            keyFrames0.Add(new LinearColorKeyFrame(c0, TimeSpan.FromSeconds(0)));
            keyFrames0.Add(new LinearColorKeyFrame(c1, TimeSpan.FromSeconds(2)));
            keyFrames0.Add(new LinearColorKeyFrame(c2, TimeSpan.FromSeconds(4)));
            keyFrames0.Add(new LinearColorKeyFrame(c0, TimeSpan.FromSeconds(6)));
            caKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
            caKeyFrames.AutoReverse = true;

            brushSC.BeginAnimation(SolidColorBrush.ColorProperty, caKeyFrames, HandoffBehavior.Compose);

            txt.Foreground = brushSC;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Manage.WindowLoading = this;
            MainWindow wnd = new MainWindow();
            wnd.Show();
        }
    }
}
