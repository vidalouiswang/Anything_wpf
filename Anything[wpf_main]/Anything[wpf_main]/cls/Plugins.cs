using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Controls;

namespace Anything_wpf_main_.cls
{
   public class Plugins
    {
        public static List<object> plugins = new List<object>();

        public static void GetPlugins()
        {
            if (Directory.Exists(Manage.Plugins))
            {
                //获取所有文件
                string[] listFiles = Directory.GetFiles(Manage.Plugins);

                //遍历
                foreach (string s in listFiles)
                {
                    //找到类库
                    if (s.ToUpper().EndsWith(".DLL"))
                    {
                        //加载
                        Assembly asm = Assembly.LoadFrom(s);
                        
                        if (asm != null)
                        {
                            //获取类型名集合
                            Type[] types = asm.GetTypes();
                            foreach (Type t in types)
                            {
                                //找到类型名内含有入口方法的类型
                                if (t.GetMethod("AnythingPluginMain") != null)
                                {
                                    //创建对象
                                    object obj = asm.CreateInstance(t.FullName);

                                    //添加到集合
                                    plugins.Add(obj);

                                    //创建对应的菜单项
                                    MenuItem menuitem = new MenuItem();

                                    //写菜单项名称
                                    menuitem.Header = t.GetProperty("MdlName").GetValue(obj,null).ToString();

                                    //检查是否要接管内部操作
                                    if (t.GetProperty("ManageOperation").GetValue(obj, null).ToString() != "")
                                    {
                                        //接管网络浏览器
                                        if (t.GetProperty("ManageOperation").GetValue(obj, null).ToString() == "Web")
                                        {
                                            Manage.MOWeb.IsUsed = true;
                                            Manage.MOWeb.Name = t.GetProperty("MdlName").GetValue(obj, null).ToString();
                                        }
                                        //接管文件夹浏览
                                        else if (t.GetProperty("ManageOperation").GetValue(obj, null).ToString() == "Folder")
                                        {
                                            Manage.MOFolder.IsUsed = true;
                                            Manage.MOFolder.Name = t.GetProperty("MdlName").GetValue(obj, null).ToString();
                                        }
                                    }

                                    //菜单项添加事件
                                    menuitem.Click += Menuitem_Click;

                                    //添加菜单项
                                    Manage.WindowMain.Plugins.Items.Add(menuitem);
                                }
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 菜单项的响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Menuitem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            Run(mi.Header.ToString());
        }

        /// <summary>
        /// 执行插件主方法
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Argument"></param>
        public static void Run(string Name,object Argument=null)
        {
            foreach (object obj in plugins)
            {
                Type t = obj.GetType();
                if (t.GetProperty("MdlName").GetValue(obj,null).ToString()==Name)
                {
                    if ((bool)t.GetProperty("NeedArgument").GetValue(obj, null))
                    {
                        t.GetProperty("Argument").SetValue(obj, Argument, null);
                    }

                    t.GetMethod("AnythingPluginMain").Invoke(obj, null);
                    break;
                }
            }
        }
    }
}
