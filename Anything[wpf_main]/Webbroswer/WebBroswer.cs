﻿using System;
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

            if (Argument == null || Argument.ToString()=="")
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
    }
}
