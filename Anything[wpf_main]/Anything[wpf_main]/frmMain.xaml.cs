using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Anything;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.IO;

namespace Anything_wpf_main_
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 成员变量

        //边界颜色
        private Color bdrColor = new Color();

        //窗体效果对象
        private FormEffects effect = null;

        //关闭按钮的计数
        private byte btnCloseOnceClick = 0;

        //最大最小透明度
        private double MinOpacity = 0.03;
        private double MaxOpacity = 0.95;

        //指示窗体是否最大化
        private bool IsMaximized = false;

        public List<Photo> photos = new List<Photo>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();

            //边界颜色定义
            bdrColor = Color.FromArgb(0xff, 0x28, 0x28, 0x28);
            this.bdrMainForm.Background = new SolidColorBrush(bdrColor);


            //this.btnTest.VImage = BitmapFrame.Create(st, BitmapCreateOptions.None, BitmapCacheOption.Default);


        }

        #endregion

        #region 窗体事件响应

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.effect = new FormEffects(this, 1, 1);
            this.StateChanged += new EventHandler(this.effect.Window_StateChanged);
            InitStyles.InitBdrStyle(ref this.bdrMain);


            //this.InitPhoto();

        }

        #endregion

        #region 按钮事件响应

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.btnCloseOnceClick++;
            if (this.btnCloseOnceClick >= 2)
                effect.Hide(true, WindowState.Normal, true);
            else
                this.btnClose.ToolTip = "Click Once";
        }

        /// <summary>
        /// 最大化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                effect.Hide(true, WindowState.Maximized);
            }
            else
            {
                effect.Hide(true, WindowState.Normal);
            }

        }

        /// <summary>
        /// 最小化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            effect.Hide(true);

        }

        /// <summary>
        /// 关闭按钮离开处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnCloseOnceClick = 0;
        }

        #endregion

        private void InitPhoto()
        {


            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            string rootPath = fbd.SelectedPath;
            //MessageBox.Show(rootPath);  
            GetAllImagePath(rootPath);
            lstImgs.ItemsSource = photos;


        }

        public void GetAllImagePath(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);


            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (file.Extension == (".jpg") ||
                        file.Extension == (".png") ||
                        file.Extension == (".bmp") ||
                        file.Extension == (".gif"))
                    {
                        photos.Add(new Photo()
                        {
                            FullPath = file.FullName
                        });
                    }
                }
            }
        }
        public class Photo
        {
            public string FullPath { get; set; }
        }
    }
}
