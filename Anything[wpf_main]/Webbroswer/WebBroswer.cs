using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webbroswer
{
    public class WebBroswer
    {
        public void AnythingPluginMain()
        {
            wndMain wnd = new wndMain();
            string Url = "";

            if (string.IsNullOrEmpty((string)Argument))
                Url = "http://www.baidu.com/";
            else
                Url = (string)Argument;

            //wnd.broswer.Source = new Uri(Url);
            wnd.broswer.Navigate(new Uri(Url));

            wnd.Show();

            //wnd.Focus();
        }

        public string MdlName { get;} = "Web Broswer";

        public string ManageOperation { get; set; } = "Web";

        public bool NeedArgument { get; } = true;

        public object Argument { get; set; } = null;

        public bool ApplyForHotKey { get; set; } = true;

        public string HotKey { get; set; } = "Ctrl+Alt+D0";
    }
}
