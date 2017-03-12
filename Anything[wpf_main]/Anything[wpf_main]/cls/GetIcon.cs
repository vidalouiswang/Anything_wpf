using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Anything
{
    /*说明
        名称空间：     Anything
        类名：         GetIcon
        作用：         从文件获取资源图标、格式转换
        构造函数：     无
        注意：         静态调用
        作者：         Vida Wang
     */
    class GetIcon
    {
        #region 常量
        const string IID_IImageList = "46EB5926-582E-4017-9FDF-E8998DAA0950";
        const string IID_IImageList2 = "192B9D83-50FC-457B-90A0-2B82A8B5DAE1";
        #endregion

        /// <summary>
        /// 根据图片得到一个图片非透明部分的区域
        /// </summary>
        /// <param name="bckImage"></param>
        /// <returns></returns>
        public static unsafe Region GetRegion(Bitmap bckImage)
        {
            GraphicsPath path = new GraphicsPath();
            int w = bckImage.Width;
            int h = bckImage.Height;
            BitmapData bckdata = null;
            try
            {
                bckdata = bckImage.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                uint* bckInt = (uint*)bckdata.Scan0;
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        if ((*bckInt & 0xff000000) != 0)
                        {
                            path.AddRectangle(new Rectangle(i, j, 1, 1));
                        }
                        bckInt++;
                    }
                }
                bckImage.UnlockBits(bckdata); bckdata = null;
            }
            catch
            {
                if (bckdata != null)
                {
                    bckImage.UnlockBits(bckdata);
                    bckdata = null;
                }
            }
            Region region = new System.Drawing.Region(path);
            path.Dispose(); path = null;
            return region;
        }

        public static Bitmap GetPart(Bitmap bit)
        {
            System.Drawing.Color c = new System.Drawing.Color();
            int count = 0;
            for (int i=0;i<bit.Width;i++)
            {
                for (int j=0;j<bit.Height;j++)
                {
                
                    c = bit.GetPixel(i, j);
                    if (c.R>0 && c.G>0 && c.B>0)
                    {
                        count++;
                        break;
                    }
                    
                }
            }

            if (count > 0 && count <= 16)
                count = 16;
            else if (count > 16 && count <= 24)
                count = 24;
            else if (count > 24 && count <= 32)
                count = 32;
            else if (count > 32 && count <= 36)
                count = 36;
            else if (count > 36 && count <= 48)
                count = 48;
            else if (count > 48 && count <= 64)
                count = 64;
            else if (count > 64 && count <= 72)
                count = 72;
            else if (count > 72 && count <= 96)
                count = 96;
            else if (count > 96 && count <= 128)
                count = 128;
            else if (count > 128 && count <= 256)
                count = 256;

            Rectangle rect = new Rectangle(0, 0, count, count);
            Bitmap bmSmall = new Bitmap(rect.Width, rect.Height, bit.PixelFormat);
            using (Graphics grSmall = Graphics.FromImage(bmSmall))
            {
                grSmall.DrawImage(bit,
                                  new System.Drawing.Rectangle(0, 0, bmSmall.Width, bmSmall.Height),
                                  rect,
                                  GraphicsUnit.Pixel);
                grSmall.Dispose();
            }
            return bmSmall;
        }

        #region 外部函数

        /// <summary>
        /// 获取图标ImageSource格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ImageSource GetIconIS(String path)
        {
            //获得资源句柄
            IntPtr hIcon = GetJumbo(GetIndex(path));
            //获取Icon数据
            Icon icon = (Icon)Icon.FromHandle(hIcon).Clone();
            //转换
            ImageSource IS= Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
                       new Int32Rect(0, 0, icon.Width, icon.Height),
                       BitmapSizeOptions.FromEmptyOptions());
            return IS;
        }

        /// <summary>
        /// 获取图标byte[]格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetIconByteArray(String path)
        {
            //获得资源句柄
            IntPtr hIcon = GetJumbo(GetIndex(path));
            //获取Icon数据
            Icon icon = (Icon)Icon.FromHandle(hIcon).Clone();

            Bitmap bit = GetPart(icon.ToBitmap());
            
            return BitmapToByte(bit);
        }
        #endregion

        #region 转换

        /// <summary>
        /// 从字节数组转换为ImageSource
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static ImageSource ByteArrayToIS(byte[] buffer)
        {
            //创建流
            MemoryStream ms = new MemoryStream(buffer);

            //创建BitmapIamge对象
            BitmapImage rtnValue = new BitmapImage();

            //填充源
            rtnValue.BeginInit();
            rtnValue.StreamSource = ms;
            rtnValue.EndInit();

            return rtnValue;
        }

        /// <summary>
        /// 从ImageSource转换为字节数组
        /// </summary>
        /// <param name="imagesource"></param>
        /// <returns></returns>
        public static byte[] ImageSourceToByteArray(ImageSource imagesource)
        {
            //强制转换，第一次读取图标需要直接读字节数组，否则此强制
            //转换可能报错
            BitmapImage bmp = (BitmapImage)imagesource;

            //声明字节数组
            byte[] b = null;
            try
            {
                //创建流
                Stream stream = bmp.StreamSource;
                //检查流的正确性
                if (stream != null && stream.Length > 0)
                {
                    //设置offset
                    stream.Position = 0;

                    //从流中读取数据
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        b = br.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch
            {
                throw new Exception("IS to byte array error");
            }
            return b;
        }

        /// <summary>
        /// 从Bitmap转换为字节数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private static byte[] BitmapToByte(Bitmap bmp)
        {
            //创建流
            MemoryStream ms = new MemoryStream();

            //填充流
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            bmp.Save("123.png");
            return ms.ToArray();
        }

        /// <summary>
        /// 从字节数组转换为Bitmap
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static Bitmap ByteToBitmap(byte[] b)
        {
            //从字节数组创建流
            MemoryStream ms = new MemoryStream(b);
            try
            {
                return (Bitmap)Bitmap.FromStream(ms);
            }
            catch
            {
                throw new Exception("Byte array to bitmap error");
            }
        }

        #endregion










        #region 内部函数

        /// <summary>
        /// 获取图标索引
        /// </summary>
        /// <param name="pszFile"></param>
        /// <returns></returns>
        private static int GetIndex(string pszFile)
        {
            SHFILEINFO sfi = new SHFILEINFO();
            Shell32.SHGetFileInfo(pszFile,0, ref sfi, (uint)System.Runtime.InteropServices.Marshal.SizeOf(sfi)
                , (uint)(SHGFI.SysIconIndex | SHGFI.LargeIcon | SHGFI.UseFileAttributes));
            return sfi.iIcon;
        }

        /// <summary>
        /// 获取256图标
        /// </summary>
        /// <param name="iImage"></param>
        /// <returns></returns>
        private static IntPtr GetJumbo(int img)
        {
            IImageList spiml = null;
            //获取索引表
            Guid guil = new Guid(IID_IImageList2);//or IID_IImageList
            Shell32.SHGetImageList(Shell32.SHIL_JUMBO, ref guil, ref spiml);
            //填充句柄
            IntPtr hIcon = IntPtr.Zero;
            spiml.GetIcon(img, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);

            return hIcon;
        }

        #endregion


    }


    public static class Shell32
    {

        public const int SHIL_LARGE = 0x0;
        public const int SHIL_SMALL = 0x1;
        public const int SHIL_EXTRALARGE = 0x2;
        public const int SHIL_SYSSMALL = 0x3;
        public const int SHIL_JUMBO = 0x4;
        public const int SHIL_LAST = 0x4;

        public const int ILD_TRANSPARENT = 0x00000001;
        public const int ILD_IMAGE = 0x00000020;

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

        [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("shell32.dll")]
        public static extern uint SHGetIDListFromObject([MarshalAs(UnmanagedType.IUnknown)] object iUnknown, out IntPtr ppidl);

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags
        );
    }
    [Flags]

    #region 数据结构

    enum SHGFI : uint
    {
        /// <summary>get icon</summary>
        Icon = 0x000000100,
        /// <summary>get display name</summary>
        DisplayName = 0x000000200,
        /// <summary>get type name</summary>
        TypeName = 0x000000400,
        /// <summary>get attributes</summary>
        Attributes = 0x000000800,
        /// <summary>get icon location</summary>
        IconLocation = 0x000001000,
        /// <summary>return exe type</summary>
        ExeType = 0x000002000,
        /// <summary>get system icon index</summary>
        SysIconIndex = 0x000004000,
        /// <summary>put a link overlay on icon</summary>
        LinkOverlay = 0x000008000,
        /// <summary>show icon in selected state</summary>
        Selected = 0x000010000,
        /// <summary>get only specified attributes</summary>
        Attr_Specified = 0x000020000,
        /// <summary>get large icon</summary>
        LargeIcon = 0x000000000,
        /// <summary>get small icon</summary>
        SmallIcon = 0x000000001,
        /// <summary>get open icon</summary>
        OpenIcon = 0x000000002,
        /// <summary>get shell size icon</summary>
        ShellIconSize = 0x000000004,
        /// <summary>pszPath is a pidl</summary>
        PIDL = 0x000000008,
        /// <summary>use passed dwFileAttribute</summary>
        UseFileAttributes = 0x000000010,
        /// <summary>apply the appropriate overlays</summary>
        AddOverlays = 0x000000020,
        /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
        OverlayIndex = 0x000000040,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public const int NAMESIZE = 80;
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left, top, right, bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        int x;
        int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGELISTDRAWPARAMS
    {
        public int cbSize;
        public IntPtr himl;
        public int i;
        public IntPtr hdcDst;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int xBitmap;
        public int yBitmap;
        public int rgbBk;
        public int rgbFg;
        public int fStyle;
        public int dwRop;
        public int fState;
        public int Frame;
        public int crEffect;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGEINFO
    {
        public IntPtr hbmImage;
        public IntPtr hbmMask;
        public int Unused1;
        public int Unused2;
        public RECT rcImage;
    }
    [ComImportAttribute()]
    [GuidAttribute("46EB5926-582E-4017-9FDF-E8998DAA0950")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IImageList
    {
        [PreserveSig]
        int Add(
        IntPtr hbmImage,
        IntPtr hbmMask,
        ref int pi);

        [PreserveSig]
        int ReplaceIcon(
        int i,
        IntPtr hicon,
        ref int pi);

        [PreserveSig]
        int SetOverlayImage(
        int iImage,
        int iOverlay);

        [PreserveSig]
        int Replace(
        int i,
        IntPtr hbmImage,
        IntPtr hbmMask);

        [PreserveSig]
        int AddMasked(
        IntPtr hbmImage,
        int crMask,
        ref int pi);

        [PreserveSig]
        int Draw(
        ref IMAGELISTDRAWPARAMS pimldp);

        [PreserveSig]
        int Remove(
    int i);

        [PreserveSig]
        int GetIcon(
        int i,
        int flags,
        ref IntPtr picon);

        [PreserveSig]
        int GetImageInfo(
        int i,
        ref IMAGEINFO pImageInfo);

        [PreserveSig]
        int Copy(
        int iDst,
        IImageList punkSrc,
        int iSrc,
        int uFlags);

        [PreserveSig]
        int Merge(
        int i1,
        IImageList punk2,
        int i2,
        int dx,
        int dy,
        ref Guid riid,
        ref IntPtr ppv);

        [PreserveSig]
        int Clone(
        ref Guid riid,
        ref IntPtr ppv);

        [PreserveSig]
        int GetImageRect(
        int i,
        ref RECT prc);

        [PreserveSig]
        int GetIconSize(
        ref int cx,
        ref int cy);

        [PreserveSig]
        int SetIconSize(
        int cx,
        int cy);

        [PreserveSig]
        int GetImageCount(
    ref int pi);

        [PreserveSig]
        int SetImageCount(
        int uNewCount);

        [PreserveSig]
        int SetBkColor(
        int clrBk,
        ref int pclr);

        [PreserveSig]
        int GetBkColor(
        ref int pclr);

        [PreserveSig]
        int BeginDrag(
        int iTrack,
        int dxHotspot,
        int dyHotspot);

        [PreserveSig]
        int EndDrag();

        [PreserveSig]
        int DragEnter(
        IntPtr hwndLock,
        int x,
        int y);

        [PreserveSig]
        int DragLeave(
        IntPtr hwndLock);

        [PreserveSig]
        int DragMove(
        int x,
        int y);

        [PreserveSig]
        int SetDragCursorImage(
        ref IImageList punk,
        int iDrag,
        int dxHotspot,
        int dyHotspot);

        [PreserveSig]
        int DragShowNolock(
        int fShow);

        [PreserveSig]
        int GetDragImage(
        ref POINT ppt,
        ref POINT pptHotspot,
        ref Guid riid,
        ref IntPtr ppv);

        [PreserveSig]
        int GetItemFlags(
        int i,
        ref int dwFlags);

        [PreserveSig]
        int GetOverlayImage(
        int iOverlay,
        ref int piIndex);
    };
    #endregion

}
