using Anything;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using IWshRuntimeLibrary;
using System.Windows.Media.Imaging;
using System.IO;

namespace Anything_wpf_main_.cls
{
    class Manage
    {

        public Manage() { }

        #region 成员变量

        public static string CurrentPath = Environment.CurrentDirectory + "\\";

        public static string IconPath = CurrentPath + "icon\\";

        //数据集合
        public static List<ItemData> listData = new List<ItemData>();

        public static Anoicess.Anoicess.Anoicess MAIN = new Anoicess.Anoicess.Anoicess("mData");

        public static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public static List<Item> RemoveList = new List<Item>();

        public struct _Link
        {
             public string Name;
             public string TargetPath;
        }
        private static _Link lnkInfo=new _Link();



        #endregion

        #region 外部函数

        public static List<string> GetObjsByNameAndPath(string Name,string Path ,ref  System.Windows.Controls.WrapPanel wp)
        {
            List<string> rtnValue = new List<string>();

            Name = Name.ToLower();

            foreach (ItemData i in listData)
            {
                if( (i.Name.ToLower().IndexOf(Name)>=0) || (i.Path.ToLower().IndexOf(Path)>=0))
                {
                    rtnValue.Add(i.Name);
                }
            }

            if (rtnValue.Count > 0)
                return rtnValue;
            else
                return null;
        }

        public static void InitializeData(ref System.Windows.Controls.WrapPanel wp)
        { 
            foreach (Anoicess.Anoicess.Anoicess adf in MAIN.GetAllChild())
            {
                ItemData itemdata = new ItemData(adf);

                listData.Add(itemdata);

                Item item = new Item(itemdata.ID, itemdata.Name, itemdata.Icon_imagesource);

                item.Margin = new System.Windows.Thickness(5);

                item.DoubleClick += Item_DoubleClick;

                wp.Children.Add(item);

                //foreach (Item i in wp.Children)
                //{
                    
                //}

            }
        }

        private static void Item_DoubleClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Item tmp = (Item)sender;
            ItemData idTmp = GetObjByID(tmp.ID);
            if (idTmp != null)
            {
                idTmp.Execute();
            }
        }

        public static Item AddItem(String Path )
        {
            //try
            {
                //检查路径
                CheckPath(Path);

                string Name = "";

                if (!string.IsNullOrEmpty(lnkInfo.Name))
                {
                    Path = lnkInfo.TargetPath;
                    Name = lnkInfo.Name;
                }
                else
                {
                    Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(Path));
                }

                //获取图标
                byte[] b;

                if (FileOperation.IsFile(Path)==-1)
                {
                    System.Windows.Resources.StreamResourceInfo sri = System.Windows.Application.GetResourceStream(new Uri("/Resources/folder.png", UriKind.Relative));

                    BinaryReader br = new BinaryReader(sri.Stream);
                    
                    b = br.ReadBytes((int)sri.Stream.Length);

                    br.Close();
                    br = null;                
                }
                else
                {
                     b = GetIcon.GetIconByteArray(Path);
                }

                

                string id = ClsMD5.ClsMD5.Encrypt(Name + Path);

                ImageSource IS = GetIcon.ByteArrayToIS(b);

                //构造UI对象
                Item item = new Item(id,Name, IS);

                //构造itemdata类对象
                ItemData itemdata = new ItemData(new ItemData.DataST(Name,Path,id,IS,b,"",1,0,0));

                Manage.listData.Add(itemdata);

                item.Margin = new System.Windows.Thickness(5);

                item.DoubleClick += Item_DoubleClick;

                return item;
            }
           
        }

        public static ItemData GetObjByID(String ID)
        {
            foreach (ItemData it in listData)
            {
                if (ID == it.ID)
                {
                    return it;
                }
            }
            return null;
        }

        public static int GetIndexByPath(String Path)
        {
            try
            {

            }
            catch
            {

            }
            return -1;
        }


        public static void RemoveItem(object Parent, String ID)
        {
            System.Windows.Controls.WrapPanel wp = (System.Windows.Controls.WrapPanel)Parent;

            foreach (Item i in wp.Children)
            {
                if (i.ID==ID)
                {
                    wp.Children.Remove(i);
                    MAIN.RemoveChild(i.Name_Property);
                    foreach(ItemData j in listData)
                    {
                        if (j.ID==ID)
                        {
                            listData.Remove(j);
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region 内部函数
        private void InitializeList()
        {

        }

        private static void CheckPath(string path)
        {
            if (path.IndexOf(".lnk")>=0)
            {
                WshShell shell = new WshShell();
                IWshShortcut iss = (IWshShortcut)shell.CreateShortcut(path);
                lnkInfo.Name = FileOperation.GetNameWithoutExtension(FileOperation.GetName(path));
                lnkInfo.TargetPath = iss.TargetPath;
            }
            else
            {
                lnkInfo.Name = "";
                lnkInfo.TargetPath = path;
                
            }
            
        }
        #endregion
    }
}
