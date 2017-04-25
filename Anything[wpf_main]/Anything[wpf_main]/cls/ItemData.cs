using System;
using Anything;
using System.Windows.Media;
using System.IO;
using System.Diagnostics;
using IWshRuntimeLibrary;

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
            //标签名
            public string tagName;
            //热键
            public HotKeyItem HotKey;
            //是否启用热键
            public bool EnableHotKey;

            public DataST(string name,
                string path,
                string id,
                ImageSource _is = null,
                byte[] icon = null,
                string arguments = "",
                int isuesd = 0,
                int runas = 0,
                int autorun = 0,
                int levels = 0,
                string workingdirectory = "",
                string tagname = "",
                HotKeyItem hotkey = null,
                bool enablehotkey=false)

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
                tagName = tagname;
                HotKey = hotkey;
                EnableHotKey = enablehotkey;
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

        private bool iconChanged = false;

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
        public ItemData(Anoicess.Anoicess.Anoicess objDB)
        {
            if (objDB != null)
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
            objDB = Manage.mMAIN.CreateChildDB(data.ID, true);
            objDB.Insert("ID", data.ID);
            objDB.Insert("Name", data.Name);
            objDB.Insert("Icon", data.Icon.Length.ToString());
            objDB.Insert("Path", data.Path);
            objDB.Insert("Arguments", data.Arguments);
            objDB.Insert("Runas", data.RunAs.ToString());
            objDB.Insert("Autorun", data.AutoRun.ToString());
            objDB.Insert("Levels", data.Levels.ToString());
            objDB.Insert("WorkingDirectory", data.WorkingDirectory);
            objDB.Insert("TagName", data.tagName);
            if (data.HotKey==null)
            {
                data.HotKey = new HotKeyItem(this, System.Windows.Forms.Keys.None, 0, 0, HotKeyItem.HotKeyParentType.Item);
            }

            if (data.HotKey.KeyValue!= System.Windows.Forms.Keys.None)
                objDB.Insert("HotKey", data.HotKey.KeyValue.ToString() + "," + data.HotKey.ModifiersValue.ToString() + "," + data.HotKey.ID.ToString());
            else
                objDB.Insert("HotKey", ",,");

            objDB.Insert("EnableHotKey", data.EnableHotKey.ToString());

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
                int Count = Convert.ToInt32(objDB.ReadFirstByName("Icon"));

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
                this.data.RunAs = Convert.ToInt32(objDB.ReadFirstByName("Runas"));
                this.data.AutoRun = Convert.ToInt32(objDB.ReadFirstByName("Autorun"));
                this.data.Levels = Convert.ToInt32(objDB.ReadFirstByName("Levels"));
                this.data.WorkingDirectory = objDB.ReadFirstByName("WorkingDirectory");
                this.data.tagName = objDB.ReadFirstByName("TagName");

                string strHKI = objDB.ReadFirstByName("HotKey");

                if (strHKI != null)
                {
                    if (!string.IsNullOrEmpty(strHKI.Replace(",", "").Replace(" ", "")))
                    {
                        string[] HKISplit;
                        try
                        {
                            HKISplit = strHKI.Split(',');
                        }
                        catch
                        {
                            HKISplit = new string[] { };
                        }
                        if (HKISplit.Length >= 3)
                        {
                            System.Windows.Forms.KeysConverter keyCvt = new System.Windows.Forms.KeysConverter();

                            this.data.HotKey = new HotKeyItem(this, (System.Windows.Forms.Keys)keyCvt.ConvertFromString(HKISplit[0]), Convert.ToUInt32(HKISplit[1]), Convert.ToInt32(HKISplit[2]), HotKeyItem.HotKeyParentType.Item);

                            if (Manage.CheckAndRegisterHotKey(data.HotKey))
                            {
                                Anything_wpf_main_.cls.HotKey.CurrentID++;
                                data.EnableHotKey = true;
                            }
                        }
                        else
                        {
                            this.data.HotKey = new HotKeyItem(null,System.Windows.Forms.Keys.None,0,0, HotKeyItem.HotKeyParentType.Item);
                        }
                    }
                    else
                        data.HotKey = new HotKeyItem(this, System.Windows.Forms.Keys.None, 0, 0, HotKeyItem.HotKeyParentType.Item);
                }

                return 0;
            }
            return -1;
        }

        private string GetPath()
        {
            int i = data.Path.Length - 1;
            while (i > 0)
            {
                if (data.Path.Substring(i, 1) == "\\")
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

                if (data.ID != OldID)
                {
                    foreach (ItemData inListdata in Manage.listOfInnerData)
                    {
                        if (inListdata.ID == data.ID)
                        {
                            Manage.listOfInnerData.Remove(inListdata);
                            break;
                        }
                    }

                    Manage.mMAIN.RemoveChild(OldID);

                    objDB = null;

                    CreateDB();

                    Manage.listOfInnerData.Add(this);
                }
                
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

        public String TagName
        {
            get
            {
                return data.tagName;
            }
            set
            {
                data.tagName = value;
                objDB.Insert("TagName", data.tagName);
            }
        }

        public bool IconChanged
        {
            get
            {
                return iconChanged;
            }

            set
            {
                iconChanged = value;
            }
        }

        public HotKeyItem HotKey
        {
            get
            {
                return data.HotKey;
            }
            set
            {
                data.HotKey = value;
            }
        }

        public bool EnableHotKey
        {
            get
            {
                return data.EnableHotKey;
            }
            set
            {
                data.EnableHotKey = value;
            }
        }

        #endregion

        #region 其他

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="tempRun"></param>
        /// <returns></returns>
        public int Execute(int Runas = 0, bool Default = false)
        {
            try
            {
                //如果是系统引用
                if (Manage.reSysRef.IsMatch(data.Path))
                {
                    //DO NOTHING
                }
                //如果是网址(URL正则有时也会匹配到程序全路径所以再加个文件存在性检测)
                else if (Manage.reURL.IsMatch(data.Path) && !System.IO.File.Exists(data.Path))
                {
                    //如果接管了浏览器
                    if (Manage.MOWeb.IsUsed)
                    {
                        Plugins.Run(Manage.MOWeb.MdlName, data.Path);
                    }
                }
                //文件或目录
                else
                {
                    //若文件不存在
                    if (FileOperation.IsFile(data.Path) == 1)
                    {
                        if (!System.IO.File.Exists(data.Path))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        //若目录不存在
                        if (!System.IO.Directory.Exists(data.Path))
                        {
                            return -1;
                        }
                        else
                        {
                            if (Manage.MOFolder.IsUsed)
                            {
                                //TODO:使用插件
                            }
                        }
                    }

                    //正常流程
                    //实例化
                    StartInfo = new ProcessStartInfo();

                    //填充信息
                    StartInfo.FileName = data.Path;
                    StartInfo.Arguments = data.Arguments;
                    StartInfo.WorkingDirectory = data.WorkingDirectory;

                    //确定是否使用管理员权限执行
                    if (Runas == 1 && Default)
                        data.RunAs = 1;
                    else if (RunAs == 1 && Default == false)
                        StartInfo.Verb = "runas";

                    //执行
                    Process.Start(StartInfo);

                    //清空主窗体上的检索框
                    Manage.WindowMain.ClearSearch();

                    //给出提示
                    Manage.TipPublic.ShowFixed(Manage.WindowMain, "Go!");
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType()== typeof(System.ComponentModel.Win32Exception))
                {
                    //DO NOTHING
                }
                else
                {
                    return -2;
                }
            }
            return 0;
        }

        /// <summary>
        /// 定位文件
        /// </summary>
        /// <returns></returns>
        public int FindLocation()
        {
            try
            {
                if (Manage.MOFolder.IsUsed)
                {
                    //////////使用插件
                }
                else
                {
                    Process.Start("explorer.exe", " /select," + data.Path);
                }
            }
            catch
            {

            }
            return 0;
        }

        /// <summary>
        /// 在桌面上创建快捷方式
        /// </summary>
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
        /// 保存图标数据
        /// </summary>
        public void SaveIcon()
        {
            if (IconChanged)
            {
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
        }

        public void AddHotKey(UserControls.HotKeyVisualItem HKVI)
        {

            int Set = -1;
            if (!EnableHotKey)
            {
                Set = 0;
            }
            else
            {
                if (data.HotKey.KeyValue!=HKVI.KeyValue || data.HotKey.ModifiersValue!=HKVI.ModifiersValue)
                {
                    Set = 1;
                }
            }
            if (Set > 0)
            {
                int id = 0;
                if (Set == 0)
                {
                    id = ++Anything_wpf_main_.cls.HotKey.CurrentID;
                }
                else if (Set == 1)
                {
                    id = this.data.HotKey.ID;
                }

                HotKeyItem hki = new HotKeyItem(this, HKVI.KeyValue, HKVI.ModifiersValue, id, HotKeyItem.HotKeyParentType.Item);

                this.HotKey = hki;

                if (Manage.CheckAndRegisterHotKey(hki))
                {
                    objDB.Insert("HotKey", data.HotKey.KeyValue.ToString() + "," + data.HotKey.ModifiersValue.ToString() + "," + data.HotKey.ID.ToString());
                }
                EnableHotKey = true;
            }
            
        }


        public void RemoveHotKey()
        {
            int id = data.HotKey.ID;

            this.HotKey.ID = 0;
            this.HotKey.KeyValue = System.Windows.Forms.Keys.None;
            this.HotKey.ModifiersValue = 0;

            Manage.UnregisterHotKey(id);

            objDB.Insert("HotKey", "");

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
