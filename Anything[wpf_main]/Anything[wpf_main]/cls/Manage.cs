using Anything;
using System;
using System.Collections.Generic;
using System.Windows.Media;

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


        #endregion

        #region 外部函数

        //public static List<int> GetIndicesByName(String Name)
        //{
        //    List<int> rtnValue = new List<int>();

        //    try
        //    {
        //        foreach (ItemData idata in listData)
        //        {
        //            if (idata.GetName()==Name)
        //            {

        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
            
        //}

        public static void InitializeData(ref System.Windows.Controls.WrapPanel wp)
        { 
            foreach (Anoicess.Anoicess.Anoicess adf in MAIN.GetAllChild())
            {
                ItemData itemdata = new ItemData(adf);

                Item item = new Item(itemdata.ID, itemdata.Name, itemdata.Icon_imagesource);

                item.Margin = new System.Windows.Thickness(5);

                wp.Children.Add(item);
            }
        }


        public static Item AddItem(String Path )
        {
            try
            {
                //获取图标
                byte[] b = GetIcon.GetIconByteArray(Path);

                string Name = FileOperation.GetNameWithoutExtension( FileOperation.GetName(Path));

                string id = ClsMD5.ClsMD5.Encrypt(Name + Path);

                ImageSource IS = GetIcon.ByteArrayToIS(b);

                //构造UI对象
                Item item = new Item(id,Name, IS);

                //构造itemdata类对象
                ItemData itemdata = new ItemData(new ItemData.DataST(Name,Path,id,IS,b,"",1,0,0));

                Manage.listData.Add(itemdata);

                item.Margin = new System.Windows.Thickness(5);

                return item;
            }
            catch
            {
                throw new Exception("Add Error");
            }
        }

        public static ItemData GetIndexByID(String ID)
        {
            try
            {
                foreach (ItemData it in listData)
                {
                    if (ID==it.ID)
                    {
                        return it;
                    }
                }
            }
            catch
            {

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


        public void RemoveItem(String ID)
        {

        }
        #endregion

        #region 内部函数
        private void InitializeList()
        {

        }
        #endregion
    }
}
