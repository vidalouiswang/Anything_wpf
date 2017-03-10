using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public FileOperation() { }

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
    }
}
