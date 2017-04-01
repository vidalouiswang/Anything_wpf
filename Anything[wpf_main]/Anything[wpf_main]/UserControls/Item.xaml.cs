using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Anything_wpf_main_.cls;

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

        //事件辅助
        private DateTime OnDown=new DateTime(0);
        private DateTime Clicked=new DateTime(0);

        private string OldName = "";



        //热度    
        public int Levels
        {
            get { return (int)GetValue(LevelsProperty); }
            set { SetValue(LevelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Levels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LevelsProperty =
            DependencyProperty.Register("Levels", typeof(int), typeof(Item), new PropertyMetadata(0));



        #region 事件

        #region 单击
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Item));

        public event RoutedEventHandler Click
        {
            add
            {
                base.AddHandler(Item.ClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(Item.ClickEvent, value);
            }
        }
        #endregion

        #region 双击
        public static readonly RoutedEvent DoubleClickEvent = EventManager.RegisterRoutedEvent("DoubleClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Item));

        public event RoutedEventHandler DoubleClick
        {
            add
            {
                base.AddHandler(Item.DoubleClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(Item.DoubleClickEvent, value);
            }
        }
        #endregion

        #endregion

        #region 属性

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

        #endregion

        #region public
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        #endregion

        #region 事件响应

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
            OldName = Txt.Text;
            this.Txt.Visibility = Visibility.Hidden;
            this.TxtWrite.Visibility = Visibility.Visible;
            this.TxtWrite.Focus();
        }

        private void DoneName()
        {
            this.Name_Property = this.TxtWrite.Text;
            this.TxtWrite.Visibility = Visibility.Hidden;
            this.Txt.Visibility = Visibility.Visible;

            if (!String.IsNullOrEmpty(this.TxtWrite.Text) && !String.IsNullOrEmpty(this.iD))
            {
                if (OldName != Name_Property)
                {
                    string path = Manage.GetObjByID(this.ID).Path;
                    Manage.GetObjByID(this.iD).Rename = true;
                    Manage.GetObjByID(this.iD).Name = this.TxtWrite.Text;

                    this.ID = ClsMD5.ClsMD5.Encrypt(this.name_Property + path);

                }
            }
        }

        /// <summary>
        /// 丢失焦点时保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtWrite_LostFocus(object sender, RoutedEventArgs e)
        {
            DoneName();
        }

        /// <summary>
        /// 销毁数据
        /// </summary>
        public void Dispose()
        {
            this.iD = null;
            this.img_Property = null;
            this.name_Property = null;
            this.length = 0;
            GC.Collect();
        }

        private void dock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnDown = DateTime.Now;
            
        }

        private void dock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RoutedEventArgs ee;

            DateTime n = DateTime.Now;

            double DownUpInterval = (n - OnDown).TotalMilliseconds;
            double ClickInterval = (n - Clicked).TotalMilliseconds;

            if (DownUpInterval<500)
            {
                if (ClickInterval<1300)
                {
                    //双击
                    ee = new RoutedEventArgs(Item.DoubleClickEvent, this);
                    base.RaiseEvent(ee);
                }
                else
                {

                    //普通单击
                    ee = new RoutedEventArgs(Item.ClickEvent, this);
                    base.RaiseEvent(ee);

                }
                //记录信息
                Clicked = n;
            }
        }
        #endregion


        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.GetObjByID(this.ID).Execute();
        }

        private void AdminOpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.GetObjByID(this.iD).Execute(1, true);
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {

            this.Visibility = Visibility.Collapsed;
            Manage.RemoveList.Add(this);
            Manage.timer.Start();
        }


        private void ReNameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SetName();
        }

        private void TxtWrite_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==System.Windows.Input.Key.Enter)
                DoneName();
        }

        private void LocationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.GetObjByID(ID).FindLocation();
        }

        private void CreateShortcutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.GetObjByID(ID).CreateShortcut();
        }
    }
}
