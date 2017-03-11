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
        public Item()
        {
            InitializeComponent();

            
        }
        private BitmapImage vImage = null;
        public Bitmap bitmap=null;



        public BitmapImage VImage
        {
            get
            {
                return vImage;
            }

            set
            {
                vImage = value;
                this.btnMain.Content = vImage;
            }
        }
    }
}
