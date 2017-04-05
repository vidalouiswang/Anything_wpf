using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Anything_wpf_main_.cls;
using Anything_wpf_main_.Form;

namespace Anything_wpf_main_
{

    /// <summary>
    /// Item.xaml 的交互逻辑
    /// </summary>
    public partial class Item : UserControl ,IDisposable
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
        public Item(String ID,String Name,ImageSource IS,double Length =128)
        {
            InitializeComponent();
            Init(ID,Name,IS);
            this.Length = Length;
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

        //
        private string _Path = "";


        //边长
        private double length = 0;

        //事件辅助
        private DateTime OnDown=new DateTime(0);
        private DateTime Clicked=new DateTime(0);

        private string OldName = "";

        private ItemData refItemData = null;


        //热度    
        public int Levels
        {
            get { return (int)GetValue(LevelsProperty); }
            set { SetValue(LevelsProperty, value); }
        }
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

        public string Path
        {
            get
            {
                return _Path;
            }

            set
            {
                _Path = value;
            }
        }

        public ItemData RefItemData
        {
            get
            {
                return refItemData;
            }

            set
            {
                refItemData = value;
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
        /// 取消冒泡已防止冒泡到parent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtWrite_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 鼠标按下响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Focus();
            e.Handled = true;


            if (this.Parent.ToString().IndexOf("WrapPanel")>0)
            {
                wndDrag drag = new wndDrag();
                drag.Show();
                drag.IParent = this.Parent as WrapPanel;
                (this.Parent as WrapPanel).Children.Remove(this);

                Point ItemPos = System.Windows.Input.Mouse.GetPosition(Manage.WindowMain.Parent as Window);

                drag.Width = this.Length + 18;
                drag.Height = this.Length + 18;

                drag.InnerObj = this;
                
                drag.Left = ItemPos.X -drag.Width/2;
                drag.Top = ItemPos.Y-drag.Height /2;
                
            }
        }


        /// <summary>
        /// 双击响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RoutedEventArgs ee;
            ee = new RoutedEventArgs(Item.ClickEvent, this);
            base.RaiseEvent(ee);

        }

        /// <summary>
        /// 尺寸改变时调整字体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Me_SizeChanged(object sender, SizeChangedEventArgs e)
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
            Manage.WindowMain.NowReName = true;
            OldName = Txt.Text;
            this.Txt.Visibility = Visibility.Hidden;
            this.TxtWrite.Visibility = Visibility.Visible;
            this.TxtWrite.Focus();
        }

        /// <summary>
        /// 命名完成
        /// </summary>
        private void DoneName()
        {
            this.Name_Property = this.TxtWrite.Text;
            this.TxtWrite.Visibility = Visibility.Hidden;
            this.Txt.Visibility = Visibility.Visible;

            if (!String.IsNullOrEmpty(this.TxtWrite.Text) && !String.IsNullOrEmpty(this.iD))
            {
                if (OldName != Name_Property)
                {
                    string path = this.refItemData.Path;
                    this.refItemData.Rename = true;
                    this.refItemData.Name = this.TxtWrite.Text;

                    this.ID = ClsMD5.ClsMD5.Encrypt(this.name_Property + path);

                }
            }
            Manage.WindowMain.NowReName = false;

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

        #endregion

        #region 菜单响应

        private void ArgumentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manage.OpenArgumentsWindow(this.RefItemData);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
           this.refItemData.Execute();
        }

        private void AdminOpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.refItemData.Execute(1, true);
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
            this.refItemData.FindLocation();
        }

        private void CreateShortcutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.refItemData.CreateShortcut();
        }

        #endregion


        private Item Clone()
        {
            Item item = new Item();
            item = this;
            return item;
        }
    }
}
