using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Drawing;

namespace Anything_wpf_main_
{
    /// <summary>
    /// Item.xaml 的交互逻辑
    /// </summary>
    public partial class Item : UserControl
    {
        public Item(){InitializeComponent();}

        public Item(String ID,String Name,ImageSource IS)
        {
            InitializeComponent();
            this.ID = ID;
            this.name_Property = Name;
            this.img_Property = IS;
            this.Img.Source = IS;
        }
        private ImageSource img_Property = null;
        private String name_Property = "";
        private String iD = "";


        public string Name_Property
        {
            get
            {
                return name_Property;
            }

            set
            {
                name_Property = value;
                this.Txt.Text = this.name_Property;
            }
        }

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
    }
}
