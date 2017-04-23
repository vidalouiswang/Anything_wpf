using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Anything_wpf_main_.cls
{
    class HotKey
    {
        //导入API
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint Modifiers, uint vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern bool SetActiveWindow(IntPtr hWnd);

        //自定义标识符
        public const int QUICK_SEARCH_HOTKEY_ID = 0x3C52;

        public static int CurrentID = QUICK_SEARCH_HOTKEY_ID;

        //热键消息常亮
        public const int WM_HOTKEY = 0x0312;

        //控制键
        public enum KeyModifiers
        {
            None=0,
            Alt=1,
            Ctrl=2,
            Shift=4,
            WindowsKey=8
        }

        /// <summary>
        /// 测试热键的可用性
        /// </summary>
        /// <param name="Modifiers"></param>
        /// <param name="Key_"></param>
        /// <returns></returns>
        public static bool TestHotKey(uint Modifiers, System.Windows.Forms.Keys key,int id=0,bool Unregister=true)
        {
            if (id==0)
                id = HotKey.QUICK_SEARCH_HOTKEY_ID - 1;

            bool Result = false;

            if (RegisterHotKey(Manage.WindowMainHandle,id,Modifiers,(uint)key))
            {
                Result = true;
            }

            if (Unregister)
                UnregisterHotKey(Manage.WindowMainHandle, id);

            return Result;
        }
    }
}
