
//This file contains the common Win32 API of the desktop Windows and the Windows CE/Mobile.

//Created by Warren Tang on 8/8/2008

//迁移状态（Vanara 底层化重构）：
// - 已由 Vanara 提供底层实现：BitBlt、GetDeviceCaps、ReleaseDC、DeleteDC、DeleteObject。
//   传入的 IntPtr 以 non-owning 方式转成 Vanara 句柄（ownsHandle=false），
//   避免 Vanara 的 SafeHandle 终结器调用 DeleteDC/DeleteObject 释放业务代码仍在使用的句柄。
// - 保留手写 DllImport：GetDC/GetWindowDC/CreateCompatibleDC/CreateDC/SelectObject/CreateCompatibleBitmap。
//   原因：这些 API 返回需要调用方管理的 GDI 句柄，Vanara 返回 owning 的 SafeHandle
//   （HDC 释放时 DeleteDC，HGDIOBJ 释放时 DeleteObject），包装后临时 SafeHandle 被 GC 回收
//   会把业务代码仍持有的句柄删掉，行为有风险。
// - 死代码已删除：StretchBlt、GetWindowDC、TextOut（无任何调用方）。

using System;
using System.Runtime.InteropServices;
using VanaraGdi32 = Vanara.PInvoke.Gdi32;
using VanaraUser32 = Vanara.PInvoke.User32;

namespace Win32;

public static partial class GDI32
{
    private const string Gdi32Dll = "gdi32.dll";

    #region BitBlt
    public static int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
        IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop)
    {
        var res = VanaraGdi32.BitBlt((Vanara.PInvoke.HDC)hdcDest, nXDest, nYDest, nWidth, nHeight,
            (Vanara.PInvoke.HDC)hdcSrc, nXSrc, nYSrc,
            (VanaraGdi32.RasterOperationMode)dwRop);
        return res ? 1 : 0;
    }

    public enum TernaryRasterOperations : uint
    {
        SRCCOPY = 0x00CC0020, /* dest = source*/
        SRCPAINT = 0x00EE0086, /* dest = source OR dest*/
        SRCAND = 0x008800C6, /* dest = source AND dest*/
        SRCINVERT = 0x00660046, /* dest = source XOR dest*/
        SRCERASE = 0x00440328, /* dest = source AND (NOT dest )*/
        NOTSRCCOPY = 0x00330008, /* dest = (NOT source)*/
        NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
        MERGECOPY = 0x00C000CA, /* dest = (source AND pattern)*/
        MERGEPAINT = 0x00BB0226, /* dest = (NOT source) OR dest*/
        PATCOPY = 0x00F00021, /* dest = pattern*/
        PATPAINT = 0x00FB0A09, /* dest = DPSnoo*/
        PATINVERT = 0x005A0049, /* dest = pattern XOR dest*/
        DSTINVERT = 0x00550009, /* dest = (NOT dest)*/
        BLACKNESS = 0x00000042, /* dest = BLACK*/
        WHITENESS = 0x00FF0062, /* dest = WHITE*/
    }

    public enum PRF
    {
        PRF_CHECKVISIBLE = 1,
        PRF_NONCLIENT = 2,
        PRF_CLIENT = 4,
        PRF_ERASEBKGND = 8,
        PRF_CHILDREN = 16,
        PRF_OWNED = 32
    }

    #endregion

    #region Device Context
    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    public static int ReleaseDC(IntPtr hWnd, IntPtr hDC)
    {
        return VanaraUser32.ReleaseDC((Vanara.PInvoke.HWND)hWnd, (Vanara.PInvoke.HDC)hDC) ? 1 : 0;
    }

    public static int GetDeviceCaps(IntPtr hdc, int nIndex)
    {
        return VanaraGdi32.GetDeviceCaps((Vanara.PInvoke.HDC)hdc, (VanaraGdi32.DeviceCap)nIndex);
    }

    public enum DeviceCap
    {
        /// <summary>
        /// Logical pixels inch in X
        /// </summary>
        LOGPIXELSX = 88,
        /// <summary>
        /// Logical pixels inch in Y
        /// </summary>
        LOGPIXELSY = 90

        // Other constants may be founded on pinvoke.net
    }

    #endregion


    #region structs

    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public bool fErase;
        public RECT rcPaint;
        public bool fRestore;
        public bool fIncUpdate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        int x;
        int y;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT:IEquatable<RECT>
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left_, int top_, int right_, int bottom_)
        {
            Left = left_;
            Top = top_;
            Right = right_;
            Bottom = bottom_;
        }

        public void Shift(int x, int y)
        {
            Left += x;
            Right += x;
            Top += y;
            Bottom += y;
        }

        public bool Equals(RECT other)
        {
            return Left == other.Left &&
                   Top == other.Top &&
                   Right == other.Right &&
                   Bottom == other.Bottom;
        }

        public override bool Equals(object obj)
        {
            return obj is RECT other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Left * 397) ^ (Top * 31) ^ (Right * 17) ^ Bottom;
            }
        }

        public static bool operator==(RECT thiz, RECT other)
        {
            return thiz.Equals(other);
        }

        public static bool operator !=(RECT thiz, RECT other)
        {
            return !(thiz == other);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", Left, Top, Right, Bottom);
        }
    }
    #endregion

    #region Color
    public enum COLOR
    {
        COLOR_WINDOW = 5,
        COLOR_WINDOWFRAME = 6,
        COLOR_WINDOWTEXT = 8
    }
    #endregion

    #region Imports
    public static bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, RasterOperation dwRop)
    {
        return VanaraGdi32.BitBlt((Vanara.PInvoke.HDC)hdc, nXDest, nYDest, nWidth, nHeight,
            (Vanara.PInvoke.HDC)hdcSrc, nXSrc, nYSrc,
            (VanaraGdi32.RasterOperationMode)dwRop);
    }

    public static bool DeleteDC(IntPtr hdc)
    {
        return VanaraGdi32.DeleteDC((Vanara.PInvoke.HDC)hdc);
    }

    [DllImport(Gdi32Dll,  SetLastError = true)]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    public static bool DeleteObject(IntPtr hObject)
    {
        return VanaraGdi32.DeleteObject((Vanara.PInvoke.HGDIOBJ)hObject);
    }

    [DllImport(Gdi32Dll,  SetLastError = true)]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    #endregion

    #region RasterOperation
    public enum RasterOperation : uint
    {
        SRCCOPY = 0x00CC0020,
        SRCPAINT = 0x00EE0086,
        SRCAND = 0x008800C6,
        SRCINVERT = 0x00660046,
        SRCERASE = 0x00440328,
        NOTSRCCOPY = 0x00330008,
        NOTSRCERASE = 0x001100A6,
        MERGECOPY = 0x00C000CA,
        MERGEPAINT = 0x00BB0226,
        PATCOPY = 0x00F00021,
        PATPAINT = 0x00FB0A09,
        PATINVERT = 0x005A0049,
        DSTINVERT = 0x00550009,
        BLACKNESS = 0x00000042,
        WHITENESS = 0x00FF0062
    }
    #endregion

    [DllImport(Gdi32Dll, SetLastError = true, EntryPoint = "CreateDC", CharSet = CharSet.Auto)]
    public static extern IntPtr CreateDC(string lpszDriver, string lpszDeviceName, string lpszOutput, HandleRef devMode);


    public const Int32 ULW_COLORKEY = 0x00000001;
    public const Int32 ULW_ALPHA = 0x00000002;
    public const Int32 ULW_OPAQUE = 0x00000004;

    public const byte AC_SRC_OVER = 0x00;
    public const byte AC_SRC_ALPHA = 0x01;
}
