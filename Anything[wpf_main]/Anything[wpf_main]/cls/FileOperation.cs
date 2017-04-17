using System;
using Microsoft.VisualBasic.Devices;
using System.IO;

namespace Anything
{
    /*说明
        名称空间：     Anything
        类名：         FileOperation
        作用：         封装的文件操作
        构造函数：     无
        注意：         静态调用
        作者：         Vida Wang
     */

    class FileOperation
    {

        private static Computer objComputer = new Computer();

        /// <summary>
        /// 构造函数
        /// </summary>
        private FileOperation() { }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static String ReadTextFile(String FileName)
        {
            if (File.Exists(FileName))
            {
                return objComputer.FileSystem.ReadAllText(FileName);
            }
            else
                return null;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Text"></param>
        /// <param name="Append"></param>
        /// <returns></returns>
        public static int WriteTextFile(String FileName, String Text, bool Append = false)
        {
            if (File.Exists(FileName))
            {
                try
                {
                    objComputer.FileSystem.WriteAllText(FileName, Text, Append);
                }
                catch
                {
                    return -1;
                }
            }
            return 0;
        }

        public static String GetName(String Path)
        {
            return objComputer.FileSystem.GetName(Path);
        }

        public static String GetNameWithoutExtension(String Name)
        {
            int i = Name.Length-1;

            while (i>0)
            {
                if (Name.Substring(i,1)==".")
                {
                    return Name.Substring(0, i);
                }
                i--;
            }
            return Name;
        }

        public static void RenameFile(string FileName,string NewName)
        {
            if (objComputer.FileSystem.FileExists(FileName))
            {
                try
                {
                    objComputer.FileSystem.RenameFile(FileName, NewName);

                }
                catch
                {

                }
            }
        }

        public static void RenameDirectory(string DirectoryName, string NewName)
        {
            if (objComputer.FileSystem.DirectoryExists(DirectoryName))
            {
                objComputer.FileSystem.RenameFile(DirectoryName, NewName);
            }
        }

        public static void DeleteFile(string FileName )
        {
            if (objComputer.FileSystem.FileExists(FileName))
            {
                try
                {
                objComputer.FileSystem.DeleteFile(FileName);
                }
                catch
                {

                }
            }
        }

        public static int IsFile(string FileName)
        {
            bool folder = false;
            bool file = false;
            if (objComputer.FileSystem.DirectoryExists(FileName))
            {
                folder = true;
            }
            if (objComputer.FileSystem.FileExists(FileName))
            {
                file = true;
            }

            if (folder==true && file ==false)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public static FileInfo  GetFileInfo(string Path)
        {
            return objComputer.FileSystem.GetFileInfo(Path);
        }

    }
}
