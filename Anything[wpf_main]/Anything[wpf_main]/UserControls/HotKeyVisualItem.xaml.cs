using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Anything_wpf_main_.cls;

namespace Anything_wpf_main_.UserControls
{
    /// <summary>
    /// HotKeyVisualItem.xaml 的交互逻辑
    /// </summary>
    public partial class HotKeyVisualItem : UserControl
    {
        public HotKeyVisualItem()
        {
            InitializeComponent();
        }

        public HotKeyVisualItem(HotKeyItem HotKey=null)
        {
            InitializeComponent();
            if (HotKey!=null)
                InputKeyString(HotKey);
        }

        public void InputKeyString(HotKeyItem HotKey)
        {
            switch (HotKey.ModifiersValue)
            {
                case 1:
                    HotKeysString = "Alt+" + HotKey.KeyValue.ToString();
                    ModifiersValue = HotKey.ModifiersValue;
                    KeyValue = HotKey.KeyValue;
                    this.Available = true;
                    break;
                case 2:
                    HotKeysString = "Ctrl+" + HotKey.KeyValue.ToString();
                    ModifiersValue = HotKey.ModifiersValue;
                    KeyValue = HotKey.KeyValue;
                    this.Available = true;
                    break;
                case 3:
                    HotKeysString = "Ctrl+Alt+" + HotKey.KeyValue.ToString();
                    ModifiersValue = HotKey.ModifiersValue;
                    KeyValue = HotKey.KeyValue;
                    this.Available = true;
                    break;
                case 4:
                    HotKeysString = "Shift+" + HotKey.KeyValue.ToString();
                    ModifiersValue = HotKey.ModifiersValue;
                    KeyValue = HotKey.KeyValue;
                    this.Available = true;
                    break;
                case 6:
                    HotKeysString = "Ctrl+Shift+" + HotKey.KeyValue.ToString();
                    ModifiersValue = HotKey.ModifiersValue;
                    KeyValue = HotKey.KeyValue;
                    this.Available = true;
                    break;
                case 8:
                    HotKeysString = "Shift+" + HotKey.KeyValue.ToString();
                    ModifiersValue = HotKey.ModifiersValue;
                    KeyValue = HotKey.KeyValue;
                    this.Available = true;
                    break;
                default:
                    break;
            }
            
        }

        public string HotKeysString
        {
            get { return (string)GetValue(HotKeysStringProperty); }
            set { SetValue(HotKeysStringProperty, value); }
        }
        public static readonly DependencyProperty HotKeysStringProperty =
            DependencyProperty.Register("HotKeysString", typeof(string), typeof(HotKeyVisualItem), new PropertyMetadata("", PropertyChanged));

        public System.Windows.Forms.Keys KeyValue { get; set; }

        public uint ModifiersValue { get; set; }

        public bool Available { get; set; } = false;

        private static void PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            HotKeyVisualItem Base = (HotKeyVisualItem)sender;
            Base.GetKeys();
        }

        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {

            string strVisual = "";
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                strVisual += "Ctrl+";

            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                strVisual += "Alt+";

            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                strVisual += "Shift+";

            if (e.Key == Key.None)
                strVisual = "";
            else
            {
                string strTmp=e.Key.ToString();

                strTmp = strTmp.Replace("LeftCtrl", "");
                strTmp = strTmp.Replace("RightCtrl", "");
                strTmp = strTmp.Replace("LeftShift", "");
                strTmp = strTmp.Replace("RightShift", "");
                strTmp = strTmp.Replace("LeftAlt", "");
                strTmp = strTmp.Replace("RightAlt", "");
                strTmp = strTmp.Replace("System", "");
                strTmp = strTmp.Replace("LWin", "");
                strTmp = strTmp.Replace("RWin", "");
                strTmp = strTmp.Replace("++", "+");

                strVisual += strTmp;
            }

            this.txtKey.Text = strVisual;

            e.Handled = true;

            //GetKeys();

        }

        private int GetKeys()
        {
            //获得组合键值string
            string strKeys=this.txtKey.Text.Trim();

            //检查是否为空
            if (!string.IsNullOrEmpty(strKeys))
            {

                System.Windows.Forms.KeysConverter keyCvt = new System.Windows.Forms.KeysConverter();

                //合法标志置否
                this.Available = false;

                //转全小写
                strKeys = strKeys.ToLower();

                //声明键状态
                bool Ctrl = false;
                bool Alt = false;
                bool Shift = false;
                System.Windows.Forms.Keys key = System.Windows.Forms.Keys.None;

                //检查是否包含Ctrl
                if (strKeys.IndexOf("ctrl") >= 0)
                {
                    Ctrl = true;
                    strKeys = strKeys.Replace("ctrl+", "");
                }

                //检查是否包含Alt
                if (strKeys.IndexOf("alt") >= 0)
                {
                    Alt = true;
                    strKeys = strKeys.Replace("alt+", "");
                }

                //检查是否包含Shift
                if (strKeys.IndexOf("shift") >= 0)
                {
                    Shift = true;
                    strKeys = strKeys.Replace("shift+", "");
                }

                //确保字符串不包含空格
                strKeys = strKeys.Trim();

                //检查是否为空，此步骤非必要，为了健壮加上
                if (!string.IsNullOrEmpty(strKeys))
                {
                    //从字符串转换键值
                    
                    key = (System.Windows.Forms.Keys)keyCvt.ConvertFromString(strKeys.ToUpper());
                }

                //最后检查合法性
                //至少一个Key + 一种控制键
                if (key != System.Windows.Forms.Keys.None && (Ctrl || Alt || Shift))
                {
                    this.KeyValue = key;

                    if (Ctrl && !Alt && !Shift) //Ctrl + Key
                    {
                        this.ModifiersValue = (uint)HotKey.KeyModifiers.Ctrl;
                    }
                    else if (Ctrl && Alt && !Shift) //Ctrl + Alt + Key
                    {
                        this.ModifiersValue = (uint)HotKey.KeyModifiers.Ctrl | (uint)HotKey.KeyModifiers.Alt;
                    }
                    else if (Ctrl && Alt && Shift) //Ctrl + Alt + Shift + Key
                    {
                        this.ModifiersValue = (uint)HotKey.KeyModifiers.Ctrl | (uint)HotKey.KeyModifiers.Alt | (uint)HotKey.KeyModifiers.Shift;
                    }
                    else if (!Ctrl && Alt && Shift) // Alt + Shift + Key
                    {
                        this.ModifiersValue = (uint)HotKey.KeyModifiers.Alt | (uint)HotKey.KeyModifiers.Shift;
                    }
                    else if (!Ctrl && !Alt && Shift) //Shift + Key
                    {
                        this.ModifiersValue = (uint)HotKey.KeyModifiers.Shift;
                    }
                    else //非法组合件
                    {
                        this.Available = false;
                        this.KeyValue = System.Windows.Forms.Keys.None;
                        this.ModifiersValue = 0;
                        this.bdrAC.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                        return -1;
                    }
                }
                else
                {
                    //非法
                    this.Available = false;
                    this.bdrAC.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    this.KeyValue = System.Windows.Forms.Keys.None;
                    this.ModifiersValue = 0;
                    return -1;
                }

                this.Available = true;

                if (HotKey.TestHotKey(this.ModifiersValue, this.KeyValue))
                {
                    this.bdrAC.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                }
                else
                {
                    this.bdrAC.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                }
                return 0;
            }
            else
                return -1;
        }

        private void txtKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                this.txtKey.Text = "";
                this.bdrAC.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            }
            else
                GetKeys();
        }

        private void bdrAC_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.HotKeysString = "";
            this.KeyValue = System.Windows.Forms.Keys.None;
            this.ModifiersValue = 0;
            this.HotKeysString = "";
            this.Available = false; ;
        }
    }
}
