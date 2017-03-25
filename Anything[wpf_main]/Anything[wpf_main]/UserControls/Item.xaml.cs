using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Anything_wpf_main_
{

    /// <summary>
    /// Item.xaml 的交互逻辑
    /// </summary>
    public partial class Item : UserControl,IDisposable
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public Item()
        {
            InitializeComponent();   
        }

        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="IS"></param>
        public Item(String ID,String Name,ImageSource IS)
        {
            InitializeComponent();
            Init(ID,Name,IS);
        }

        /// <summary>
        /// 初始化过程
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="IS"></param>
        private void Init(String ID, String Name = "Default Name", ImageSource IS = null)
        {
            this.Name_Property = Name;
            this.ID = ID;
            this.Img_Property = IS;
        }

        //项目的可视化资源
        private ImageSource img_Property = null;

        //项目名称属性
        private String name_Property = "";

        //项目的唯一标识符
        private String iD = "";

        //边长
        private double length = 0;

        /// <summary>
        /// Img_Property属性的包装器
        /// </summary>
        public ImageSource Img_Property
        {
            get
            {
                return img_Property;
            }

            set
            {
                img_Property = value;
                this.Img.Source = this.img_Property;
                
            }
        }

        /// <summary>
        /// ID属性的包装器
        /// </summary>
        public string ID
        {
            get
            {
                return iD;
            }

            set
            {
                iD = value;
            }
        }

        /// <summary>
        /// Name属性的包装器
        /// </summary>
        public string Name_Property
        {
            get
            {
                return name_Property;
            }

            set
            {
                name_Property = value;
                this.Txt.Text = value;
                this.TxtWrite.Text = value;
            }
        }

        /// <summary>
        /// 边长属性的包装器
        /// </summary>
        public double Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
                this.Width = value;
                this.Height = value;
            }
        }

        /// <summary>
        /// 尺寸改变时调整字体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Txt.FontSize = this.ActualWidth / 8;
            this.TxtWrite.FontSize = this.ActualWidth / 8;
            this.Length = this.ActualWidth;
        }

        /// <summary>
        /// 命名项目
        /// </summary>
        private void SetName()
        {
            this.Txt.Visibility = Visibility.Hidden;
            this.TxtWrite.Visibility = Visibility.Visible;
            this.TxtWrite.Focus();
        }

        /// <summary>
        /// 丢失焦点时保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtWrite_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Name_Property = this.TxtWrite.Text;
            this.TxtWrite.Visibility = Visibility.Hidden;
            this.Txt.Visibility = Visibility.Visible;
        }

        public void Dispose()
        {
            this.iD = null;
            this.img_Property = null;
            this.name_Property = null;
            this.length = 0;
            GC.Collect();
        }
    }
}
