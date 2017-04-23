using Anything;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using IWshRuntimeLibrary;
using System.IO;
using Anything_wpf_main_.Form;
using System.Text.RegularExpressions;
using Anything_wpf_main_.UserControls;

namespace Anything_wpf_main_.cls
{
    class Manage
    {
        //不允许实例化使用
        private Manage() { }

        #region 成员变量

        #region Path

        //指示当前路径
        public static string CurrentPath = Environment.CurrentDirectory + "\\";

        //图标文件的存放路径
        public static string IconPath = CurrentPath + "icon\\";

        //存放插件的目录
        public static string Plugins = CurrentPath + "Plugins\\";

        #endregion

        #region Anoicess

        //主库，存储其他子库的信息
        public static Anoicess.Anoicess.Anoicess mMAIN = new Anoicess.Anoicess.Anoicess("mData");

        //用于保存近期搜索关键字的库
        public static Anoicess.Anoicess.Anoicess mKeywordRecent = new Anoicess.Anoicess.Anoicess("mKeywordRecent");

        //用于保存搜索引擎的库
        public static Anoicess.Anoicess.Anoicess mSearchEngine = new Anoicess.Anoicess.Anoicess("mSearchEngine");

        //用于保存热键的库
        public static Anoicess.Anoicess.Anoicess mHoyKey = new Anoicess.Anoicess.Anoicess("mHotKey");

        #endregion

        #region List

        //数据类对象数据集合
        public static List<ItemData> listOfInnerData = new List<ItemData>();

        //用于保存搜索引擎的内部列表
        public static List<Anoicess.Anoicess.Anoicess._Content> listOfSearchEngineInnerData = new List<Anoicess.Anoicess.Anoicess._Content>();

        //用于保存近期搜索关键字的内部列表
        public static List<string> listOfRecentKeyword = new List<string>();

        //待删除项目的集合
        public static List<Item> listOfRemoveItem = new List<Item>();

        //存储搜索引擎可视化资源的集合
        public static List<SearchEngineItem> listOfSearchEnginesVisualElement = new List<SearchEngineItem>();

        //存储应用程序申请的全局热键
        public static List<HotKeyItem> listOfApplicationHotKey = new List<HotKeyItem>();

        #endregion

        #region MO
        //接管信息结构
        public struct ManagedOperation
        {
            public bool IsUsed;
            public string MdlName;
            public ManagedOperation(bool used, string name)
            {
                IsUsed = used;
                MdlName = name;
            }
        }

        //是否接管网络浏览器功能
        public static ManagedOperation MOWeb = new ManagedOperation(false,"");

        //是否接管文件夹浏览功能
        public static ManagedOperation MOFolder = new ManagedOperation(false, "");

        #endregion

        #region lnk
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

        #endregion

        #region Window
        //提示窗体
        public static wndTip TipPublic = new wndTip();

        //参数窗体
        public static wndArguments WindowArgs = new wndArguments();

        //手动添加窗体
        public static wndAdd WindowAdd = new wndAdd();

        //主窗体引用
        public static MainWindow WindowMain;
        public static IntPtr WindowMainHandle = IntPtr.Zero;


        //加载窗体引用
        public static wndLoading WindowLoading;

        #endregion

        #region Others

        //用于延迟移除项目的timer
        public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        //用于保存主窗体的位置信息
        public static RECT WindowMainRect = new RECT();

        //匹配网址的正则
        public static Regex reURL = new Regex(@"^((http|https|ftp)://)?\S+?\S+\.\S+.+$", RegexOptions.IgnoreCase);

        //匹配系统引用
        public static Regex reSysRef = new Regex(@"::\{\S+\}");

        #endregion

        #region System Reference
        //系统引用
        public const string MyComputer = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
        public const string MyDocument= "::{450D8FBA-AD25-11D0-98A8-0800361B1103}";
        public const string ControlPanel = "::{21EC2020-3AEA-1069-A2DD-08002B30309D}";
        public const string RecycleBin = "::{645FF040-5081-101B-9F08-00AA002F954E}";
        public const string NetworkNeighborhood = "::{208D2C60-3AEA-1069-A2D7-08002B30309D}";

        #endregion

        #endregion

        #region public


        public static int FindAndInsert(Item item)
        {
            //检查tagName
            bool IsAdded = false;
            if (!string.IsNullOrEmpty(item.TagName))
            {
                foreach (object obj in WindowMain.Recent.Children)
                {
                    //查找子元素
                    if (obj is ExpanderEx)
                    {
                        //获取到expander
                        ExpanderEx exTmp = (ExpanderEx)obj;

                        exTmp.IsExpanded = true;

                        //若标签名相同
                        if (exTmp.tagName == item.TagName)
                        {
                            System.Windows.Controls.WrapPanel wpTmp = (System.Windows.Controls.WrapPanel)exTmp.Content;

                            wpTmp.Children.Add(item);

                            IsAdded = true;

                        }
                    }
                }

                //如果未找到相同标签名的分组
                if (!IsAdded)
                {
                    ExpanderEx ex = new ExpanderEx(item.TagName);

                    ex.tagName = item.TagName;

                    ex.IsExpanded = true;

                    System.Windows.Controls.WrapPanel wp = new System.Windows.Controls.WrapPanel();

                    wp.Children.Add(item);

                    ex.Content = wp;

                    WindowMain.Recent.Children.Add(ex);
                }
            }
            else
            {
                WindowMain.Recent.Children.Add(item);
            }
            return 0;
        }


        /// <summary>
        /// 反注册所有的热键
        /// </summary>
        /// <returns></returns>
        public static int UnregisterAllHotKeys()
        {
            //卸载主要热键
            HotKey.UnregisterHotKey(WindowMainHandle, HotKey.QUICK_SEARCH_HOTKEY_ID);

            int UnregisteredCount = 0;
            
            //反注册其他热键
            foreach (HotKeyItem i in listOfApplicationHotKey)
            {
                if (HotKey.UnregisterHotKey(WindowMainHandle,i.ID))
                {
                    UnregisteredCount++;
                }
            }

            return UnregisteredCount;

        }

        /// <summary>
        /// 反注册某一热键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int UnregisterHotKey(int id)
        {
            foreach (HotKeyItem item in listOfApplicationHotKey)
            {
                if (item.ID == id)
                {
                    HotKey.UnregisterHotKey(WindowMainHandle, id);
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 检查待注册的热键是否冲突，若不冲突则注册，否则返回false
        /// </summary>
        /// <param name="HKVI">HotKeyVisualItem</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckAndRegisterHotKey(HotKeyVisualItem HKVI,object iParent, int id=-65536)
        {
            bool StatusAvailable = true;
            if (HKVI.Available)
            {
                if (id == -65536)
                    id = ++HotKey.CurrentID;

                HotKeyItem hki = new HotKeyItem(iParent,HKVI.KeyValue, HKVI.ModifiersValue, id,HotKeyItem.HotKeyParentType.Item);

                foreach (HotKeyItem h in listOfApplicationHotKey)
                {
                    if (hki.ID == h.ID)
                    {
                        StatusAvailable = false;
                        break;
                    }
                }

                if (StatusAvailable)
                {
                    listOfApplicationHotKey.Add(hki);
                    HotKey.RegisterHotKey(new System.Windows.Interop.WindowInteropHelper(WindowMain).Handle, hki.ID, hki.ModifiersValue, (uint)hki.KeyValue);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 检测是否冲突并注册热键
        /// </summary>
        /// <param name="HKI">HotKeyItem</param>
        /// <returns></returns>
        public static bool CheckAndRegisterHotKey(HotKeyItem HKI)
        {
            if (HKI.ID==0x0000)
            {
                HKI.ID = ++HotKey.CurrentID;
            }

            if (HotKey.TestHotKey(HKI.ModifiersValue,HKI.KeyValue, HKI.ID,false))
            {
                listOfApplicationHotKey.Add(HKI);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查找都应的快捷键响应
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool FindHotKeyAndExecute(int id)
        {
            foreach (HotKeyItem i in listOfApplicationHotKey)
            {
                if (id==i.ID)
                {
                    if (i.IParent is ItemData)
                    {
                        (i.IParent as ItemData).Execute();
                        break;
                    }
                    else if (i.IParent is string)
                    {
                        Anything_wpf_main_.cls.Plugins.Run((string)i.IParent);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 使用搜索引擎搜索
        /// </summary>
        /// <param name="Keyword"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static int SearchOnWeb(string Keyword,string URL)
        {
            //检查参数的正确性
            if (string.IsNullOrEmpty(Keyword) || string.IsNullOrEmpty(URL))
                return -1;

            if (URL.IndexOf("%keyword%") < 0)
                return -2;

            //执行搜索
            if (!Manage.MOWeb.IsUsed)
            {
                System.Diagnostics.Process.Start(URL.Replace("%keyword%", Keyword));
            }
            else
            {
                Anything_wpf_main_.cls.Plugins.Run(MOWeb.MdlName, URL.Replace("%keyword%", Keyword));
            }

            return 0;
        }

        /// <summary>
        /// 打开属性窗口
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int OpenAttributeWindow(Item item)
        {
            wndItemInformation wndInfo = new wndItemInformation(item);
            
            return 0;
        }

        /// <summary>
        /// 打开搜索引擎选择窗口
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public static int OpenSearchWindow(string Keyword)
        {
            wndSE wndse = new wndSE(Keyword);
            wndse.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //wndse.ShowActivated = true;
            wndse.ShowDialog();
            return 0; 
        }

        /// <summary>
        /// 用于设置搜索框的显示相关
        /// </summary>
        /// <param name="txtMain"></param>
        /// <param name="Show"></param>
        /// <param name="FillText"></param>
        /// <returns></returns>
        public static int ClearOrFillText(ref System.Windows.Controls.TextBox txtMain, bool Show,string FillText="Use keyword to search")
        {
            if (txtMain != null)
            {
                if (Show)
                {
                    if (txtMain.Text.Trim() == "Use keyword to search")
                        txtMain.Text = "";
                }
                else
                {
                    if (string.IsNullOrEmpty(txtMain.Text.Trim()))
                        txtMain.Text = FillText;
                }
            }
            else
                return -1;
            return 0;
        }

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
        public static List<Item> GetObjsByNameAndPathAndTag(string Name,string Path,string TagName,bool Union =false)
        {
            List<Item> rtnValue = new List<Item>();

            Name = Name.ToLower();

            Path = Path.ToLower();

            TagName = TagName.ToLower();

            bool emptyName = string.IsNullOrEmpty(Name);
            bool emptyPath = string.IsNullOrEmpty(Path);
            bool emptyTagName = string.IsNullOrEmpty(TagName);

            foreach (object obj in WindowMain.Recent.Children)
            {
                if (obj is ExpanderEx)
                {
                    foreach (Item i in ((System.Windows.Controls.WrapPanel)(((ExpanderEx)obj).Content)).Children)
                    {
                        if (i != null && i.Name_Property != null && i.Path != null && i.TagName != null)
                        {
                            if (!Union)
                            {
                                if ((i.Name_Property.ToLower().IndexOf(Name) >= 0) || (i.Path.ToLower().IndexOf(Path) >= 0) || (i.TagName.ToLower().IndexOf(TagName) >= 0))
                                {
                                    rtnValue.Add(i);
                                }
                            }
                            else
                            {
                                if (!emptyName && !emptyPath && !emptyTagName) //三字段全有
                                {
                                    if ((i.Name_Property.ToLower().IndexOf(Name) >= 0) && (i.Path.ToLower().IndexOf(Path) >= 0) && (i.TagName.ToLower().IndexOf(TagName) >= 0))
                                    {
                                        rtnValue.Add(i);
                                    }
                                }
                                else if (!emptyName && !emptyPath && emptyTagName) //没有标签
                                {
                                    if ((i.Name_Property.ToLower().IndexOf(Name) >= 0) && (i.Path.ToLower().IndexOf(Path) >= 0))
                                    {
                                        rtnValue.Add(i);
                                    }
                                }
                                else if (!emptyName && emptyPath & emptyTagName) //没有标签和路径
                                {
                                    if ((i.Name_Property.ToLower().IndexOf(Name) >= 0))
                                    {
                                        rtnValue.Add(i);
                                    }
                                }
                                else if (emptyTagName && !emptyPath && !emptyTagName) //没有名
                                {
                                    if ((i.Path.ToLower().IndexOf(Path) >= 0) && (i.TagName.ToLower().IndexOf(TagName) >= 0))
                                    {
                                        rtnValue.Add(i);
                                    }
                                }
                                else if (emptyName && emptyPath && !emptyTagName) //只有标签
                                {
                                    if ((i.TagName.ToLower().IndexOf(TagName) >= 0))
                                    {
                                        rtnValue.Add(i);
                                    }
                                }
                                else if (emptyName && !emptyPath && emptyTagName) //只有路径
                                {
                                    if ((i.Path.ToLower().IndexOf(Path) >= 0))
                                    {
                                        rtnValue.Add(i);
                                    }
                                }
                                else if (!emptyName && emptyPath && !emptyTagName) //有名和标签
                                {
                                    if ((i.Name_Property.ToLower().IndexOf(Name) >= 0) && (i.TagName.ToLower().IndexOf(TagName) >= 0))
                                    {
                                        rtnValue.Add(i);
                                    }
                                }
                            }
                        }
                    }
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
        public static void InitializeData(MainWindow wnd_)
        {
            //保存主窗体相关信息
            WindowMain = wnd_;
            WindowMainRect.left = (int)wnd_.Left;
            WindowMainRect.right = (int)(wnd_.Left + wnd_.ActualWidth);
            WindowMainRect.top = (int)wnd_.Top;
            WindowMainRect.bottom = (int)(wnd_.Top + wnd_.ActualHeight);

            //创建进度窗体实例
            wndProgressBar wndpb = new wndProgressBar("Loading data","Please wait...",mMAIN.GetAllChild().Count);

            //wnd_.Opacity = 0.000001;
            wnd_.WindowState = System.Windows.WindowState.Normal;

            int ChildCount = mMAIN.GetAllChild().Count;

            //获取项目尺寸
            double ItemSize = ApplicationInformations.Anything.AppInfoOperations.GetItemSize();

            //开始加载数据
            for (int i =0;i< ChildCount; i++)
            {

                ItemData itemdata = new ItemData(mMAIN.GetAllChild()[i]);

                listOfInnerData.Add(itemdata);

                Item item = new Item(itemdata.ID, itemdata.Name, itemdata.Icon_imagesource, ItemSize,itemdata.TagName);

                item.Path = itemdata.Path;

                item.RefItemData = itemdata;

                item.Margin = new System.Windows.Thickness(5);

                item.Click += Item_Click;

                //wp.Children.Add(item);

                FindAndInsert(item);
                
                wndpb.Increase();

                //wnd_.Opacity = (double)((double)i / (double)ChildCount);

            }




            wnd_.Opacity = ApplicationInformations.Anything.AppInfoOperations.GetMaxOpacity();


            List<string> tmp = mKeywordRecent.ReadAllString();

            if (tmp != null)
                listOfRecentKeyword = tmp;

            LoadSE();

            TipPublic.Show();

            wndpb.Increase();
        }

        /// <summary>
        /// 加载搜索引擎资源
        /// </summary>
        public static void LoadSE()
        {

            listOfSearchEngineInnerData.Clear();
            listOfSearchEnginesVisualElement.Clear();
            List<Anoicess.Anoicess.Anoicess._Content> t = mSearchEngine.ReadAllContent();
            if (t != null)
            {
                if (t.Count > 0)
                    listOfSearchEngineInnerData = t;

                foreach (Anoicess.Anoicess.Anoicess._Content item in t)
                {
                    if (item.IsUsed == 1)
                    {
                        listOfSearchEnginesVisualElement.Add(new SearchEngineItem(item.Name, item.Content, ""));
                    }
                }
            }

        }

        /// <summary>
        /// 用于刷新项目的图标
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemdata"></param>
        public static void RefreshSingle(Item item,ItemData itemdata)
        {
            //从原有集合中删除
            ((System.Windows.Controls.WrapPanel)item.Parent).Children.Remove(item);

            //构造新的Item对象
            Item newOne = new Item(itemdata.ID, itemdata.Name, itemdata.Icon_imagesource,ApplicationInformations.Anything.AppInfoOperations.GetItemSize(),itemdata.TagName);

            //填充基本信息
            newOne.Path = itemdata.Path;
            newOne.Margin = new System.Windows.Thickness(5);
            newOne.RefItemData = itemdata;

            //添加消息处理
            newOne.Click += Item_Click;

            FindAndInsert(newOne);
            
        }

        /// <summary>
        /// 添加新项目
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Name"></param>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        public static Item AddItem(String Path ,string Name ="",string Arguments="",string tagName="")
        {
            //try
            {
                //检查路径
                CheckPath(Path);

                string subPath = "";

                if (!string.IsNullOrEmpty(lnkInfo.Name))
                {
                    Path = lnkInfo.TargetPath;
                    if (string.IsNullOrEmpty(Name))
                    {
                        Name = lnkInfo.Name;
                        if (string.IsNullOrEmpty(Arguments))
                            Arguments = lnkInfo.Arguments;
                        
                        if (string.IsNullOrEmpty(lnkInfo.WorkingDirectory))
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
                    if (string.IsNullOrEmpty(Name))
                        Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(Path));

                    subPath = GetSubPath(Path);

                    if (string.IsNullOrEmpty(Arguments))
                    {
                        if (!new Regex("::\\{.+\\}").IsMatch(Path))
                        {
                            Arguments = GetArgumentsFromFullPath(Path); 
                        }
                    }
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
                Item item = new Item(id, Name, IS, ApplicationInformations.Anything.AppInfoOperations.GetItemSize(),tagName);

                item.Path = Path;

                //构造itemdata类对象
                ItemData itemdata = new ItemData(new ItemData.DataST(Name,Path,id,IS,b,"",1,0,0));

                if (!string.IsNullOrEmpty(tagName))
                    itemdata.TagName = tagName;

                //有参数则填充参数
                if (!string.IsNullOrEmpty(Arguments.Trim()))
                    itemdata.Arguments = Arguments;

                //填充工作路径
                if (!string.IsNullOrEmpty(subPath.Trim()))
                    itemdata.WorkingDirectory = subPath;


                Manage.listOfInnerData.Add(itemdata);

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
                    mMAIN.RemoveChild(i.Name_Property);
                    i.RefItemData.Dispose();
                    listOfInnerData.Remove(i.RefItemData);
                    i.Dispose();
                    tmp = i;
                    break;
                }
            }
            tmp = null;
        }

        /// <summary>
        /// 添加我的电脑
        /// </summary>
        /// <returns></returns>
        public static Item AddComputer()
        {
            return AddItem(MyComputer, "My Computer");
        }

        /// <summary>
        /// 添加控制面板
        /// </summary>
        /// <returns></returns>
        public static Item AddControlPanel()
        {
            return AddItem(ControlPanel, "Control Panel");
        }

        /// <summary>
        /// 添加回收站
        /// </summary>
        /// <returns></returns>
        public static Item AddRecycleBin()
        {
            return AddItem(RecycleBin, "Recycle Bin");
        }

        /// <summary>
        /// 添加家我的文档
        /// </summary>
        /// <returns></returns>
        public static Item AddMyDocument()
        {
            return AddItem(MyDocument, "My Document");
        }

        /// <summary>
        /// 添加网络邻居
        /// </summary>
        /// <returns></returns>
        public static Item AddNetworkNeighbor()
        {
            return AddItem(NetworkNeighborhood, "Network Neighborhood");
        }

        /// <summary>
        /// 用于保存搜索关键字，暂未完成
        /// </summary>
        /// <param name="str"></param>
        public static void SaveKeyword( string str)
        {
            if (!string.IsNullOrEmpty(str) && str!="Use keyword to search")
            {
                listOfRecentKeyword.Add(str);
                mKeywordRecent.Insert(str, str);
            }
        }
        #endregion

        #region private

        /// <summary>
        /// 检查提供的路径，判断是否lnk文件
        /// </summary>
        /// <param name="path"></param>
        private static void CheckPath(string path)
        {
            if (path.ToLower().IndexOf(".lnk")>=0)
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
            else
                WindowMain.txtMain.Text = "";

            SaveKeyword(WindowMain.txtMain.Text);

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

    }
}
