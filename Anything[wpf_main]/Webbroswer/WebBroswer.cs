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
            if (Argument == null)
                Argument = "";
            wnd.broswer.Source = new Uri((string)Argument);
            wnd.Show();
        }

        public string MdlName { get;} = "Web Broswer";

        public string ManageOperation { get; set; } = "Web";

        public bool NeedArgument { get; } = true;

        public object Argument { get; set; } = null;
    }
}
