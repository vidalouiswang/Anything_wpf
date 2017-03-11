using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Anything_wpf_main_
{
    #region 图片转换
    [System.Windows.Data.ValueConversion(typeof(byte[]), typeof(ImageSource))]
    public class ConvertToRecipesImageInfo : System.Windows.Data.IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            byte[] binaryimagedata = value as byte[];
            if (binaryimagedata == null) return "";
            using (Stream imageStreamSource = new MemoryStream(binaryimagedata, false))
            {

                JpegBitmapDecoder jpeDecoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                ImageSource imageSource = jpeDecoder.Frames[0];
                return imageSource;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    #endregion
}
