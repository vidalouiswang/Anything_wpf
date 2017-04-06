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

        // Using a DependencyProperty as the backing store for ItemName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string ), typeof(wndItemInformation), new PropertyMetadata(""));




        public string  Path
        {
            get { return (string )GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string ), typeof(wndItemInformation), new PropertyMetadata(""));



        public string  Arguments
        {
            get { return (string )GetValue(ArgumentsProperty); }
            set { SetValue(ArgumentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Arguments.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArgumentsProperty =
            DependencyProperty.Register("Arguments", typeof(string ), typeof(wndItemInformation), new PropertyMetadata(""));




        public string TagName
        {
            get { return (string)GetValue(TagNameProperty); }
            set { SetValue(TagNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TagName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TagNameProperty =
            DependencyProperty.Register("TagName", typeof(string), typeof(wndItemInformation), new PropertyMetadata(""));







        public ImageSource ItemIcon
        {
            get { return (ImageSource)GetValue(ItemIconProperty); }
            set { SetValue(ItemIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemIconProperty =
            DependencyProperty.Register("ItemIcon", typeof(ImageSource), typeof(wndItemInformation), new PropertyMetadata(null));





        public string WorkingDirectory
        {
            get { return (string)GetValue(WorkingDirectoryProperty); }
            set { SetValue(WorkingDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkingDirectory.  This enables animation, styling, binding, etc...
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


        public wndItemInformation()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ItemName.Trim()))
                itemdata.Name = this.ItemName;

            if (!string.IsNullOrEmpty(this.Path.Trim()))
                itemdata.Path = this.Path;

            if (!string.IsNullOrEmpty(this.Arguments))
                itemdata.Arguments = this.Arguments;

            if (!string.IsNullOrEmpty(this.WorkingDirectory))
                itemdata.WorkingDirectory = this.WorkingDirectory;

            if (this.ItemIcon != null)
            {
                itemdata.Icon_imagesource = this.ItemIcon;
                itemdata.Icon_byte = GetIcon.ImageSourceToByteArray(this.ItemIcon);
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
