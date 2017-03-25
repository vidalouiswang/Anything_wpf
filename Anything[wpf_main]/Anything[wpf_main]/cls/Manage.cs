using Anything;
using System;
using System.Collections.Generic;

namespace Anything_wpf_main_.cls
{
    class Manage
    {

        public Manage() { }

        #region 成员变量
        //数据集合
        public static List<ItemData> listData = new List<ItemData>();

        #endregion

        #region 外部函数

        public static int GetIndexByName(String Name)
        {
            try
            {
                //todo
            }
            catch
            {

            }
            return -1;
        }

        public static int GetIndexByID(String ID)
        {
            try
            {
                //todo
            }
            catch
            {

            }
            return -1;
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

        public static Item AddItem(String Path )
        {
            try
            {
                //获取图标
                byte[] b = GetIcon.GetIconByteArray(Path);

                string Name = FileOperation.GetName(Path);
                //构造UI对象
                Item item = new Item("ID",
                    Name, 
                    GetIcon.ByteArrayToIS(b));

                //构造ID类对象
                ItemData itemdata = new ItemData(new ItemData.DataST(Name, Path, "auto"));

                Manage.listData.Add(itemdata);

                item.Margin = new System.Windows.Thickness(5);

                return item;
            }
            catch
            {
                throw new Exception("Add Error");
            }
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
