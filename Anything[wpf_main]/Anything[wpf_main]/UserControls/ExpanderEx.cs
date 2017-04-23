using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anything_wpf_main_.UserControls
{
    class ExpanderEx: System.Windows.Controls.Expander
    {
        public ExpanderEx(string TagName)
        {
            this.tagName = TagName;
            this.Header = tagName;

            this.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0xff, 0xff, 0xff));

            this.FontSize = 16;
            this.Content = new System.Windows.Controls.WrapPanel();
            this.Margin = new System.Windows.Thickness(3);

        }

        public string tagName { get; set; } = "";
    }
}
