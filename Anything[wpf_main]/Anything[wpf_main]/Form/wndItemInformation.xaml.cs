using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Anything;
using Anything_wpf_main_.cls;

namespace Anything_wpf_main_.Form
{
    /// <summary>
    /// wndItemInformation.xaml 的交互逻辑
    /// </summary>
    public partial class wndItemInformation : Window
    {

        private Item item = null;
        private ItemData itemdata = null;

        public string  ItemName
        {
            get { return (string )GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }
        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string ), typeof(wndItemInformation), new PropertyMetadata(""));

        public string  Path
        {
            get { return (string )GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string ), typeof(wndItemInformation), new PropertyMetadata(""));

        public string  Arguments
        {
            get { return (string )GetValue(ArgumentsProperty); }
            set { SetValue(ArgumentsProperty, value); }
        }
        public static readonly DependencyProperty ArgumentsProperty =
            DependencyProperty.Register("Arguments", typeof(string ), typeof(wndItemInformation), new PropertyMetadata(""));


        public string TagName
        {
            get { return (string)GetValue(TagNameProperty); }
            set { SetValue(TagNameProperty, value); }
        }
        public static readonly DependencyProperty TagNameProperty =
            DependencyProperty.Register("TagName", typeof(string), typeof(wndItemInformation), new PropertyMetadata(""));

        public ImageSource ItemIcon
        {
            get { return (ImageSource)GetValue(ItemIconProperty); }
            set { SetValue(ItemIconProperty, value); }
        }
        public static readonly DependencyProperty ItemIconProperty =
            DependencyProperty.Register("ItemIcon", typeof(ImageSource), typeof(wndItemInformation), new PropertyMetadata(null));


        public string WorkingDirectory
        {
            get { return (string)GetValue(WorkingDirectoryProperty); }
            set { SetValue(WorkingDirectoryProperty, value); }
        }
        public static readonly DependencyProperty WorkingDirectoryProperty =
            DependencyProperty.Register("WorkingDirectory", typeof(string), typeof(wndItemInformation), new PropertyMetadata(""));


        public ItemData Itemdata
        {
            get
            {
                return itemdata;
            }

            set
            {
                itemdata = value;
            }
        }

        public Item Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
            }
        }

        public wndItemInformation(Item item)
        {
            InitializeComponent();

            //填充信息
            this.Item = item;
            this.Itemdata = item.refItemData;
            this.ItemName = item.Name_Property;
            this.Path = item.refItemData.Path;
            this.Arguments = item.refItemData.Arguments;
            this.ItemIcon = item.refItemData.Icon_imagesource;
            this.WorkingDirectory = item.refItemData.WorkingDirectory;
            this.Itemdata = item.refItemData;
            this.TagName = item.TagName;
            this.txtHotKey.InputKeyString(itemdata.HotKey);

            this.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ItemName.Trim()))
                itemdata.Name = this.ItemName;

            if (!string.IsNullOrEmpty(this.Path.Trim()))
                itemdata.Path = this.Path;

            if (!string.IsNullOrEmpty(this.Arguments.Trim()))
                itemdata.Arguments = this.Arguments;

            if (!string.IsNullOrEmpty(this.WorkingDirectory.Trim()))
                itemdata.WorkingDirectory = this.WorkingDirectory;

            if (this.ItemIcon != null && this.itemdata.IconChanged)
            {
                itemdata.Icon_imagesource = this.ItemIcon;
                itemdata.Icon_byte = GetIcon.ImageSourceToByteArray(this.ItemIcon);
            }

            if (!string.IsNullOrEmpty(this.TagName.Trim()))
            {
                itemdata.TagName = TagName;
            }

            if (this.txtHotKey.Available)
            {
                this.itemdata.AddHotKey(this.txtHotKey);
            }
            else
            {
                this.itemdata.RemoveHotKey();
            }

            Manage.RefreshSingle(item, itemdata);
                            
            this.Close();

        }

        private void btnChangeIcon_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "PNG文件|*.png";
            ofd.ShowDialog();
            

            string FileName = ofd.FileName;
            if(System.IO.File.Exists(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.Open);

                byte[] b = null;
                fs.Position = 0;
                using (BinaryReader br = new BinaryReader(fs))
                {
                        b = br.ReadBytes((int)fs.Length);
                }

                this.imgIcon.Source = GetIcon.ByteArrayToIS(b);
                itemdata.IconChanged = true;
            }
            else
            {
                Manage.TipPublic.ShowFixed(this, "File doesn't exists.");
            }
        }
    }
}
