using System;
using System.Drawing;
using Anything;
using System.Windows.Media;
using System.IO;

namespace Anything_wpf_main_.cls
{
    class ItemData : IDisposable
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

        private Anoicess.Anoicess.Anoicess objDB = null;

        

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

            CreateDB();
            //初始化成功
            this.IsInit = true;
        }

        /// <summary>
        /// 从给定的数据文件路径构造对象
        /// </summary>
        /// <param name="path"></param>
        public  ItemData(Anoicess.Anoicess.Anoicess objDB)
        {
            if (objDB!=null)
            {
                this.objDB = objDB;
                ConnectDB();
            }
            else
            {
                Dispose();
            }
        }

        #endregion

        #region 内部函数

        private void CreateDB()
        {
            objDB = Manage.MAIN.CreateChildDB(data.Name, true);
            objDB.Insert("ID", data.ID);
            objDB.Insert("Name", data.Name);
            objDB.Insert("Icon", data.Icon.Length.ToString());
            objDB.Insert("Path", data.Path);
            objDB.Insert("Arguments", data.Arguments);
            objDB.Insert("Runas", data.RunAs.ToString());
            objDB.Insert("Autorun", data.AutoRun.ToString());

            if (!Directory.Exists(Manage.IconPath))
            {
                Directory.CreateDirectory(Manage.IconPath);
            }

            try
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(Manage.IconPath + data.ID + ".ib", FileMode.Create)))
                {
                    bw.Write(data.Icon);
                    bw.Flush();
                    bw.Close();
                }
            }
            catch
            {

            }

            
            

        }


        /// <summary>
        /// 从数据文件路径初始化内部数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int ConnectDB()
        {
            if (objDB != null)
            {
                this.data.Name = objDB.Read("Name") as string;
                this.data.ID = objDB.Read("ID") as string;
                int Count = Convert.ToInt32( objDB.Read("Icon"));

                try
                {
                    using (BinaryReader br = new BinaryReader(new FileStream(Manage.IconPath + data.ID + ".ib", FileMode.Open)))
                    {
                        data.Icon = br.ReadBytes(Count);
                    }

                    data.IS = GetIcon.ByteArrayToIS(data.Icon);
                }
                catch
                {

                }

                this.data.Path = objDB.Read("Path") as string;
                this.data.Arguments = objDB.Read("Arguments") as string;
                this.data.RunAs = Convert.ToInt32( objDB.Read("Runas"));
                this.data.AutoRun = Convert.ToInt32(objDB.Read("Autorun"));

                return 0;
            }
            return -1;
        }

        #endregion

        #region 外部函数

        #region 属性
        
        public ImageSource Icon_imagesource
        {
            get
            {
                return this.data.IS;
            }
            set
            {
                this.data.IS = value;
            }
        }

        public byte[] Icon_byte
        {
            get
            {
                return this.data.Icon;
            }
            set
            {
                this.data.Icon = value;
            }
        }

        public String Arguments
        {
            get
            {
                return this.data.Arguments;
            }
            set
            {
                this.data.Arguments = value;
            }

        }

        public String Name
        {
            get
            {
                return this.data.Name;
            }
            set
            {
                this.data.Name = value;
            }
        }

        public String ID
        {
            get
            {
                return this.data.ID;
            }

        }

        public int RunAs
        {
            get
            {
                return this.data.RunAs; 
            }
            set
            {
                this.data.RunAs = value;
            }
        }

        public int AutoRun
        {
            get
            {
                return this.data.AutoRun;
            }
            set
            {
                this.data.AutoRun = value;
            }
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

        /// <summary>
        /// 销毁数据
        /// </summary>
        public void Dispose()
        {
            
        }
        #endregion

        #endregion



    }
}
