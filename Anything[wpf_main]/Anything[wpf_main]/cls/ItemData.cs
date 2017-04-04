using System;
using System.Drawing;
using Anything;
using System.Windows.Media;
using System.IO;
using System.Diagnostics;
using IWshRuntimeLibrary;
using System.Windows;

namespace Anything_wpf_main_.cls
{
     public class ItemData : IDisposable
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
            //热度
            public int Levels;
            //工作目录
            public string WorkingDirectory;

            public DataST(string name,
                string path,
                string id,
                ImageSource _is=null,
                byte[] icon=null,
                string arguments="",
                int isuesd=0,
                int runas=0,
                int autorun=0,
                int levels=0,
                string workingdirectory="")

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
                Levels = levels;
                WorkingDirectory = workingdirectory;
            }

        }

        #endregion

        #region 成员变量

        public bool IsInit;
        public String DataStr;
        private DataST data;
        public bool Rename { get; set; }

        private Anoicess.Anoicess.Anoicess objDB = null;

        private ProcessStartInfo StartInfo = new ProcessStartInfo();

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
            objDB = Manage.MAIN.CreateChildDB(data.ID, true);
            objDB.Insert("ID", data.ID);
            objDB.Insert("Name", data.Name);
            objDB.Insert("Icon", data.Icon.Length.ToString());
            objDB.Insert("Path", data.Path);
            objDB.Insert("Arguments", data.Arguments);
            objDB.Insert("Runas", data.RunAs.ToString());
            objDB.Insert("Autorun", data.AutoRun.ToString());
            objDB.Insert("Levels", data.Levels.ToString());
            objDB.Insert("WorkingDirectory", data.WorkingDirectory);

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
                this.data.Name = objDB.ReadFirstByName("Name") as string;
                this.data.ID = objDB.ReadFirstByName("ID") as string;
                int Count = Convert.ToInt32( objDB.ReadFirstByName("Icon"));

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

                this.data.Path = objDB.ReadFirstByName("Path") as string;
                this.data.Arguments = objDB.ReadFirstByName("Arguments") as string;
                this.data.RunAs = Convert.ToInt32( objDB.ReadFirstByName("Runas"));
                this.data.AutoRun = Convert.ToInt32(objDB.ReadFirstByName("Autorun"));
                this.data.Levels = Convert.ToInt32(objDB.ReadFirstByName("Levels"));
                this.data.WorkingDirectory = objDB.ReadFirstByName("WorkingDirectory");

                return 0;
            }
            return -1;
        }

        private string GetPath()
        {
            int i = data.Path.Length-1;
            while (i>0)
            {
                if (data.Path.Substring(i,1)=="\\")
                {
                    break;
                }
                else
                {
                    i--;
                }
            }
            return data.Path.Substring(0, i);
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
                objDB.Insert("Arguments", data.Arguments);
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
                string OldID = data.ID;
                data.Name = value;

                data.ID = ClsMD5.ClsMD5.Encrypt(data.Name + data.Path);

                //if (Rename)
                //{

                foreach (ItemData inListdata in Manage.listData)
                {
                    if (inListdata.ID == data.ID)
                    {
                        Manage.listData.Remove(inListdata);
                        break;
                    }
                }

                Manage.MAIN.RemoveChild(OldID);

                objDB = null;

                CreateDB();

                Manage.listData.Add(this);

                Rename = false;

                //}

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
                objDB.Insert("Runas", data.RunAs.ToString());
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
                objDB.Insert("Autorun", data.AutoRun.ToString());
            }
        }

        public int Levels
        {
            get
            {
                return data.Levels;
            }
            set
            {
                data.Levels = value;
            }
        }

        public String Path
        {
            get
            {
                return data.Path;
            }
            set
            {
                data.Path = value;
            }
        }

        public String WorkingDirectory
        {
            get
            {
                return data.WorkingDirectory;
            }
            set
            {
                data.WorkingDirectory = value;
            }
        }

        #endregion

        #region 其他

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="tempRun"></param>
        /// <returns></returns>
        public int Execute(int Runas=0,bool Default=false )
        {
            try
            {
                StartInfo = new ProcessStartInfo();
                StartInfo.FileName = data.Path;
                StartInfo.Arguments = data.Arguments;
                StartInfo.WorkingDirectory = data.WorkingDirectory;

                if (Runas == 1 && Default)
                    data.RunAs = 1;
                else if (RunAs == 1 && Default == false)
                    StartInfo.Verb = "runas";
 
                Process.Start(StartInfo);

                Manage.WindowMain.ClearSearch();

                Manage.TipPublic.ShowFixed(Manage.WindowMain, "Go!");

            }
            catch
            {
                
            }
            return 0;
        }

        public int FindLocation()
        {
            try
            {
                Process.Start("explorer.exe", " /select," + data.Path);
            }
            catch
            {

            }
            return 0;
        }

        public void CreateShortcut()
        {
            WshShell wsh = new WshShell();

            string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";

            IWshShortcut iss = (IWshShortcut)wsh.CreateShortcut(DesktopPath + data.Name + ".lnk");

            iss.Description = data.Name;
            iss.TargetPath = data.Path;
            iss.WindowStyle = 1;
            iss.WorkingDirectory = "";
            iss.Save();

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
