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
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        //自定义标识符
        public const int HOTKEYID_ = 0x3C52;

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
    }
}
