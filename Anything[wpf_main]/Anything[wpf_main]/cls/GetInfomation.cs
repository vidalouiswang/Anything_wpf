using System;
using System.Collections.Generic;

namespace Anything
{
 static class GetInfomation
    {

        public struct INFO
        {
            int Left;
            int Top;
            double MinOpa;
            double MaxOpa;
            double ShowTimeSpan;
            double HideTimeSpan;
            int TimeOut;
        }

        public static INFO info;
        public static string ExePath = System.Windows.Forms.Application.StartupPath + "/";
        public static string SettingsFileName = ExePath + "Settings.vid";

        public static int GetSettings()
        {
            String strSettings = FileOperation.ReadTextFile(SettingsFileName);
            String[] strArr = strSettings.Split(new char[6]{'A','0','C','0','F','0'});
            if (strArr.Length>0)
            {

            }
            return 0;
        }

        #region 基础信息

        /// <summary>
        /// 获取配置信息中的最小透明度参数
        /// </summary>
        /// <returns></returns>
        public static double GetMinOpacity()
        {
            double rtnValue = 0.05;

            return rtnValue;
        }

        /// <summary>
        /// 获取配置信息中的最大透明度
        /// </summary>
        /// <returns></returns>
        public static double GetMaxOpacity()
        {
            double rtnValue = 0.98;

            return rtnValue;
        }

        /// <summary>
        /// 获取显示时长
        /// </summary>
        /// <returns></returns>
        public static double GetShowTimeSpan()
        {
            double rtnValue = 0.3;

            return rtnValue;
        }

        /// <summary>
        /// 获取隐藏时长
        /// </summary>
        /// <returns></returns>
        public static double GetHideTimeSpan()
        {
            double rtnValue = 0.3;

            return rtnValue;
        }

        /// <summary>
        /// 获取超时时间
        /// </summary>
        /// <returns></returns>
        public static double GetTimeoutTimeSpan()
        {
            double rtnValue = 3;

            return rtnValue;
        }


        public static List<string> Split(this string input, string sign)
        {
            List<string> strs = new List<string>();
            int index = input.IndexOf(sign);
            while (index != -1)
            {
                strs.Add(input.Substring(0, index));
                input = input.Substring(index + sign.Length);
                index = input.IndexOf(sign);
            }
            if (!string.IsNullOrEmpty(input))
                strs.Add(input);
            return strs;
        }
        #endregion

    }
}
