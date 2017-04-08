﻿using Anything;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using IWshRuntimeLibrary;
using System.Windows.Media.Imaging;
using System.IO;
using Anything_wpf_main_.Form;

namespace Anything_wpf_main_.cls
{
    class Manage
    {

        public Manage() { }

        #region 成员变量

        //指示当前路径
        public static string CurrentPath = Environment.CurrentDirectory + "\\";

        //图标文件的存放路径
        public static string IconPath = CurrentPath + "icon\\";

        //数据类对象数据集合
        public static List<ItemData> listData = new List<ItemData>();

        //主库，存储其他子库的信息
        public static Anoicess.Anoicess.Anoicess MAIN = new Anoicess.Anoicess.Anoicess("mData");

        public static Anoicess.Anoicess.Anoicess mRecentList = new Anoicess.Anoicess.Anoicess("mRecentList");

        public static Anoicess.Anoicess.Anoicess mSE = new Anoicess.Anoicess.Anoicess("mSE");

        public static List<Anoicess.Anoicess.Anoicess._Content> SEList = new List<Anoicess.Anoicess.Anoicess._Content>();

        public static List<string> RecentList = new List<string>();

        //用于延迟移除项目的timer
        public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        //待删除项目的集合
        public static List<Item> RemoveList = new List<Item>();

        //主窗体引用
        public static MainWindow WindowMain;

        //Lnk数据结构，仅主要信息
        public struct _Link
        {
             public string Name;
             public string WorkingDirectory;
             public string TargetPath;
             public string Arguments;
        }

        //lnk文件信息
        private static _Link lnkInfo=new _Link();

        //提示窗体
        public static wndTip TipPublic = new wndTip();

        //参数窗体
        public static wndArguments WindowArgs = new wndArguments();

        //手动添加窗体
        public static wndAdd WindowAdd = new wndAdd();

        public static RECT WindowMainRect = new RECT();

        //系统引用
        public const string MyComputer = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
        public const string MyDocument= "::{450D8FBA-AD25-11D0-98A8-0800361B1103}";
        public const string ControlPanel = "::{21EC2020-3AEA-1069-A2DD-08002B30309D}";
        public const string RecycleBin = "::{645FF040-5081-101B-9F08-00AA002F954E}";
        public const string NetworkNeighborhood = "::{208D2C60-3AEA-1069-A2D7-08002B30309D}";


        #endregion

        #region 外部函数

        /// <summary>
        /// 打开手动添加窗体
        /// </summary>
        /// <returns></returns>
        public static int OpenAddWindow()
        {
            WindowAdd = new wndAdd();
            if (WindowAdd != null)
            {
                WindowAdd.ShowDialog();
                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 打开参数窗体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static int OpenArgumentsWindow(ItemData itemdata)
        {

            if (itemdata==null)
                return -1;

            WindowArgs = new wndArguments();
            if (WindowArgs != null)
            {
                WindowArgs.It = itemdata;
                WindowArgs.ItemName = itemdata.Name;
                WindowArgs.Arguments = itemdata.Arguments;
                WindowArgs.ShowDialog();
            }
            else
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// 通过名称和路径检索
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Path"></param>
        /// <param name="wp"></param>
        /// <returns></returns>
        public static List<Item> GetObjsByNameAndPath(string Name,string Path ,ref  System.Windows.Controls.WrapPanel wp)
        {
            List<Item> rtnValue = new List<Item>();

            Name = Name.ToLower();

            foreach (Item i in wp.Children)
            {
                if( (i.Name_Property.ToLower().IndexOf(Name)>=0) || (i.Path.ToLower().IndexOf(Path)>=0))
                {
                    rtnValue.Add(i);
                }
            }

            if (rtnValue.Count > 0)
                return rtnValue;
            else
                return null;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="wnd_"></param>
        /// <param name="wp"></param>
        public static void InitializeData(MainWindow wnd_ ,ref System.Windows.Controls.WrapPanel wp)
        {
            WindowMain = wnd_;
            WindowMainRect.left = (int)wnd_.Left;
            WindowMainRect.right = (int)(wnd_.Left + wnd_.ActualWidth);
            WindowMainRect.top = (int)wnd_.Top;
            WindowMainRect.bottom = (int)(wnd_.Top + wnd_.ActualHeight);

            foreach (Anoicess.Anoicess.Anoicess adf in MAIN.GetAllChild())
            {
                ItemData itemdata = new ItemData(adf);

                listData.Add(itemdata);

                Item item = new Item(itemdata.ID, itemdata.Name, itemdata.Icon_imagesource);

                item.Path = itemdata.Path;

                item.RefItemData = itemdata;

                item.Margin = new System.Windows.Thickness(5);

                item.Click += Item_Click;

                wp.Children.Add(item);

                List<string> tmp= mRecentList.ReadAllString();

                if (tmp != null)
                    RecentList = tmp;

                List<Anoicess.Anoicess.Anoicess._Content> t = mSE.ReadAllContent();
                if (t!=null)
                {
                    if (t.Count > 0)
                        SEList = t;
                }

            }
            TipPublic.Show();
        }

        public static void RefreshSingle(Item item,ItemData itemdata)
        {
            WindowMain.Recent.Children.Remove(item);
            Item newOne = new Item(itemdata.ID, itemdata.Name, itemdata.Icon_imagesource);
            newOne.Path = itemdata.Path;
            newOne.Margin = new System.Windows.Thickness(5);
            newOne.RefItemData = itemdata;
            newOne.Click += Item_Click;
            WindowMain.Recent.Children.Add(newOne);
        }

        /// <summary>
        /// 添加新项目
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Name"></param>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        public static Item AddItem(String Path ,string Name ="",string Arguments="")
        {
            //try
            {
                //检查路径
                CheckPath(Path);

                string subPath = "";

                if (!string.IsNullOrEmpty(lnkInfo.Name))
                {
                    Path = lnkInfo.TargetPath;
                    if (string.IsNullOrEmpty(Name.Trim()))
                    {
                        Name = lnkInfo.Name;
                        if (string.IsNullOrEmpty(Arguments.Trim()))
                            Arguments = lnkInfo.Arguments;
                        
                        if (string.IsNullOrEmpty(lnkInfo.WorkingDirectory.Trim()))
                        {
                            subPath = GetSubPath(Path);
                        }
                        else
                        {
                            subPath = lnkInfo.WorkingDirectory;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Name.Trim()))
                        Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(Path));

                    subPath = GetSubPath(Path);

                    if (string.IsNullOrEmpty(Arguments.Trim()))
                        Arguments = GetArgumentsFromFullPath(Path);
                }

                

                //获取图标
                byte[] b;

                if (FileOperation.IsFile(Path)==-1)
                {
                    b = GetResourcesIcon("folder.png");
                }
                else
                {
                    //系统引用
                    if (Path.IndexOf("{")>0 && Path.IndexOf("}")>0)
                    {
                        switch (Path)
                        {
                            case MyComputer:
                                b = GetResourcesIcon("computer.png");
                                break;
                            case ControlPanel:
                                b = GetResourcesIcon("controlpanel.png");
                                break;
                            case MyDocument:
                                b = GetResourcesIcon("mydocument.png");
                                break;
                            case RecycleBin:
                                b = GetResourcesIcon("recyclebin.png");
                                break;
                            case NetworkNeighborhood:
                                b = GetResourcesIcon("networkneighborhood.png");
                                break;
                            default:
                                b = GetIcon.GetIconByteArray(Path);
                                break;
                        }
                    }
                    else
                    {
                        b = GetIcon.GetIconByteArray(Path);
                    }
                }

                string id = ClsMD5.ClsMD5.Encrypt(Name + Path);

                ImageSource IS = GetIcon.ByteArrayToIS(b);

                //构造UI对象
                Item item = new Item(id,Name, IS);

                item.Path = Path;

                //构造itemdata类对象
                ItemData itemdata = new ItemData(new ItemData.DataST(Name,Path,id,IS,b,"",1,0,0));

                //有参数则填充参数
                if (!string.IsNullOrEmpty(Arguments.Trim()))
                    itemdata.Arguments = Arguments;

                //填充工作路径
                if (!string.IsNullOrEmpty(subPath.Trim()))
                    itemdata.WorkingDirectory = subPath;


                Manage.listData.Add(itemdata);

                item.RefItemData = itemdata;

                item.Margin = new System.Windows.Thickness(5);

                item.Click += Item_Click;

                return item;
            }
           
        }

        /// <summary>
        /// 移除项目
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="ID"></param>
        public static void RemoveItem(object Parent, String ID)
        {
            System.Windows.Controls.WrapPanel wp = (System.Windows.Controls.WrapPanel)Parent;

            Item tmp = null;

            foreach (Item i in wp.Children)
            {
                if (i.ID==ID)
                {
                    wp.Children.Remove(i);
                    MAIN.RemoveChild(i.Name_Property);
                    i.RefItemData.Dispose();
                    listData.Remove(i.RefItemData);
                    i.Dispose();
                    tmp = i;
                    break;
                }
            }
            tmp = null;
        }


        public static Item AddComputer()
        {
            return AddItem(MyComputer, "My Computer");
        }
        public static Item AddControlPanel()
        {
            return AddItem(ControlPanel, "Control Panel");
        }
        public static Item AddRecycleBin()
        {
            return AddItem(RecycleBin, "Recycle Bin");
        }
        public static Item AddMyDocument()
        {
            return AddItem(MyDocument, "My Document");
        }
        public static Item AddNetworkNeighbor()
        {
            return AddItem(NetworkNeighborhood, "Network Neighborhood");
        }

        public static void SaveKeyword( string str)
        {
            if (!string.IsNullOrEmpty(str) && str!="Use keyword to search")
            {
                RecentList.Add(str);
                mRecentList.Insert(str, str);
            }
        }
        #endregion

        #region 内部函数

        /// <summary>
        /// 检查提供的路径，判断是否lnk文件
        /// </summary>
        /// <param name="path"></param>
        private static void CheckPath(string path)
        {
            if (path.IndexOf(".lnk")>=0)
            {
                WshShell shell = new WshShell();
                IWshShortcut iss = (IWshShortcut)shell.CreateShortcut(path);
                lnkInfo.Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(path));
                lnkInfo.TargetPath = iss.TargetPath;
                lnkInfo.WorkingDirectory = iss.WorkingDirectory;
                lnkInfo.Arguments = iss.Arguments;
            }
            else
            {
                lnkInfo.Name = "";
                lnkInfo.TargetPath = path;
                lnkInfo.WorkingDirectory = "";
                lnkInfo.Arguments = "";
            }
            
        }

        /// <summary>
        /// 处理Item的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Item_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Item tmp = (Item)sender;
            int errCode = 0;
            if ( (errCode=tmp.RefItemData.Execute())<0)
            {
                if (errCode == -1)
                    TipPublic.ShowFixed(WindowMain, "File or folder doesn't exist.");
                else
                    TipPublic.ShowFixed(WindowMain, "Unknown error.");
            }

            SaveKeyword(WindowMain.txtMain.Text);

            WindowMain.txtMain.Text = "";
        }

        /// <summary>
        /// 从资源中获取指定的图片
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private static byte[] GetResourcesIcon(string Name)
        {
            byte[] b;
            System.Windows.Resources.StreamResourceInfo sri = System.Windows.Application.GetResourceStream(new Uri("/Resources/" + Name, UriKind.Relative));

            BinaryReader br = new BinaryReader(sri.Stream);

            b = br.ReadBytes((int)sri.Stream.Length);

            br.Close();
            br = null;
            return b;

        }

        /// <summary>
        /// 获取子路径
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private static string GetSubPath(string Path)
        {
            int LastPos = Path.LastIndexOf("\\");
            if (LastPos > 0)
            {
                return Path.Substring(0, LastPos+1);
            }
            else
                return Path;
        }

        /// <summary>
        /// 从路径获取参数
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private static string GetArgumentsFromFullPath(string Path)
        {
            string FileNameWithExtension = FileOperation.GetName(Path);
            if (!string.IsNullOrEmpty(FileNameWithExtension.Trim()))
            {
                int LastPos = Path.LastIndexOf(FileNameWithExtension);
                if (LastPos >= 0)
                {
                    int spaceIndex = FileNameWithExtension.LastIndexOf(" ");
                    int MinusIndex = FileNameWithExtension.LastIndexOf("-");
                    int speatorIndex = FileNameWithExtension.LastIndexOf("/");

                    if (spaceIndex>=0)
                    {
                        if (MinusIndex>=0)
                        {
                            LastPos += MinusIndex;
                        }
                        else
                        {
                            LastPos += spaceIndex;
                        }
                    }
                    else if (MinusIndex>=0)
                    {
                        LastPos += MinusIndex;
                    }
                    else if (speatorIndex>=0)
                    {
                        LastPos += speatorIndex;
                    }
                    
                    return Path.Substring(LastPos, Path.Length - LastPos);
                }
                else return "";
            }
            else return "";
        }
        #endregion

        //public static ItemData GetObjByID(String ID)
        //{
        //    foreach (ItemData it in listData)
        //    {
        //        if (ID == it.ID)
        //        {
        //            return it;
        //        }
        //    }
        //    return null;
        //}

        //public static int GetIndexByPath(String Path)
        //{
        //    try
        //    {

        //    }
        //    catch
        //    {

        //    }
        //    return -1;
        //}
    }
}
