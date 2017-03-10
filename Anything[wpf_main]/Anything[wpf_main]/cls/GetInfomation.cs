using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anything;
using Microsoft.VisualBasic;
namespace Anything
{
    class GetInfomation
    {
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
            double rtnValue = 0.6;

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
        #endregion

    }
}
