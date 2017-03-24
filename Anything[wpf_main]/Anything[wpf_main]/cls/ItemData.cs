using System;
using System.Drawing;
using Anything;
using System.Windows.Media;

namespace Anything_wpf_main_.cls
{
    class ItemData
    {
        #region 结构

        /// <summary>
        /// Item的组成
        /// </summary>
        public struct DataST
        {
            //名称
            public String Name;
            //图标byte array
            public byte[] Icon;
            //图标image source
            public ImageSource IS;
            //路径
            public String Path;
            //ID
            public String ID;
            //参数
            public String Arguments;
            //是否使用
            public int IsUesd;
            //执行权限
            public int RunAs;
            //自动运行
            public int AutoRun;

            public DataST(string name,
                string path,
                string id,
                ImageSource _is=null,
                byte[] icon=null,
                string arguments="",
                int isuesd=0,
                int runas=0,
                int autorun=0)

            {
                Name = name;
                Path = path;
                ID = id;
                IS = _is;
                Icon = icon;
                Arguments = arguments;
                IsUesd = isuesd;
                RunAs = runas;
                AutoRun = autorun;
            }

        }

        /// <summary>
        /// 自动运行方式
        /// </summary>
        public enum Run
        {
            //自动运行
            AutoRun,
            //正常
            Normal,
        }

        /// <summary>
        /// 运行权限
        /// </summary>
        public enum RunWay
        {
            //管理员
            Administrator,
            //默认
            AsInvoker,
        }

        #endregion

        #region 成员变量

        public bool IsInit;
        public String DataStr;
        private DataST data;

        internal DataST Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }


        #endregion

        #region 构造

        /// <summary>
        /// 从指定的Data构造对象
        /// </summary>
        /// <param name="data"></param>
        public ItemData(DataST data)
        {
            //填充数据
            this.data = data;

            //初始化成功
            this.IsInit = true;
        }

        /// <summary>
        /// 从给定的数据文件路径构造对象
        /// </summary>
        /// <param name="path"></param>
        public ItemData(String path)
        {
            //判断给定路径的合法性
            if (!String.IsNullOrEmpty(path))
            {
                //调用内部函数初始化数据
                if (this.InitFromStrPath(path) == 0)
                {
                    //初始化成功
                    this.IsInit = true;
                }
                else
                    //初始化失败
                    throw new Exception("load error");
            }
        }

        #endregion

        #region 内部函数

        /// <summary>
        /// 从数据文件路径初始化内部数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int InitFromStrPath(String path)
        {
            this.DataStr = FileOperation.ReadTextFile(path);
            String[] strSplit = DataStr.Split(new char[6] { 'A', '0', 'C', '0', 'F', '0' });
            if (strSplit.Length > 0)
            {
                //todo
            }
            return 0;
        }

        #endregion

        #region 外部函数

        #region 读
        /// <summary>
        /// 获取图标数据
        /// </summary>
        /// <returns></returns>
        
        public ImageSource GetIcon()
        {
            ImageSource imgsrc = null;
            
            try
            {
                //todo
            }
            catch
            {

            }

            return imgsrc;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public String GetArguments()
        {

            String rtnStr = null;
            try
            {
                //todo
            }
            catch
            {

            }

            return rtnStr;
        }

        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public String GetName()
        {
            String rtnName = null;
            try
            {

            }
            catch
            {

            }
            return rtnName;
        }
        #endregion

        #region 写

        /// <summary>
        /// 更改图标从字节数组
        /// </summary>
        /// <param name="Icon"></param>
        /// <returns></returns>
        public int ChangeIcon(byte[] Icon)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }

        /// <summary>
        /// 更改图标从Image对象
        /// </summary>
        /// <param name="Icon"></param>
        /// <returns></returns>
        public int ChangeIcon(Image Icon)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }

        /// <summary>
        /// 更改图标从Bitmap对象
        /// </summary>
        /// <param name="Icon"></param>
        /// <returns></returns>
        public int ChangeIcon(Bitmap Icon)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }

        /// <summary>
        /// 更改名称
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int ChangeName(String Name)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }

        /// <summary>
        /// 更改路径
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public int ChangePath(String Path)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }

        /// <summary>
        /// 更改参数
        /// </summary>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        public int ChangeArguments(String Arguments)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }

        /// <summary>
        /// 设置自动启动
        /// </summary>
        /// <param name="run"></param>
        /// <returns></returns>
        public int SetAutoRun(Run run)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }

        /// <summary>
        /// 设置运行权限
        /// </summary>
        /// <param name="runway"></param>
        /// <returns></returns>
        public int SetRunAs(RunWay runway)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }

        #endregion

        #region 其他

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="tempRun"></param>
        /// <returns></returns>
        public int Execute(RunWay tempRun)
        {
            try
            {
                //todo
            }
            catch
            {
                throw new Exception("Error");
            }
            return 0;
        }
        #endregion

        #endregion



    }
}
