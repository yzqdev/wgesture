using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace WGestures.Common.OsSpecific.Windows;

public static class ScreenHelper {
    public static bool IsAnyAppFullScreen()
    {
        // 1. 获取当前处于最前端的窗口句柄 (HWND)
        HWND hwnd = GetForegroundWindow();
        if (hwnd.IsNull)
        {
            Debug.WriteLine("[FullScreenCheck] 当前最前端窗口句柄为空(NULL)。");
            return false;
        }

        // 获取窗口标题，方便在日志中识别是哪个软件
        const int maxTitleLength = 256;
        var titleBuilder = new StringBuilder(maxTitleLength);
        GetWindowText(hwnd, titleBuilder, maxTitleLength);
        string windowTitle = titleBuilder.ToString();

        // 获取窗口类名以排除桌面、任务栏
        const int maxClassName = 256;
        var classNameBuilder = new StringBuilder(maxClassName);
        GetClassName(hwnd, classNameBuilder, maxClassName);
        string className = classNameBuilder.ToString();

        Debug.WriteLine($"[FullScreenCheck] 激活窗口 -> 标题: '{windowTitle}', 类名: '{className}', 句柄: {hwnd.DangerousGetHandle()}");

        // 2. 排除桌面和任务栏
        HWND desktopHwnd = GetDesktopWindow();
        HWND shellHwnd = GetShellWindow();

        if (hwnd == desktopHwnd || hwnd == shellHwnd)
        {
            Debug.WriteLine("[FullScreenCheck] 结果: False (当前窗口是桌面或Shell窗口)");
            return false;
        }

        if (className == "Shell_TrayWnd" || className == "WorkerW")
        {
            Debug.WriteLine("[FullScreenCheck] 结果: False (当前窗口是任务栏或桌面背景WorkerW)");
            return false;
        }

        // 3. 使用 Vanara 封装的方法获取窗口矩形 (RECT)
        if (GetWindowRect(hwnd, out RECT rect))
        {
            int windowWidth = rect.right - rect.left;
            int windowHeight = rect.bottom - rect.top;
            Debug.WriteLine($"[FullScreenCheck] 窗口尺寸 -> L:{rect.left}, T:{rect.top}, R:{rect.right}, B:{rect.bottom} | 宽度: {windowWidth}, 高度: {windowHeight}");

            // 4. 获取当前最前端窗口所在的屏幕分辨率
            // 修正：Vanara 中 MonitorFromWindow 直接支持 HWND 强类型
            var hMonitor = MonitorFromWindow(hwnd, MonitorFlags.MONITOR_DEFAULTTONEAREST);

            // 使用 Vanara 获取显示器信息
            MONITORINFO monitorInfo = new MONITORINFO();
            monitorInfo.cbSize = (uint)Marshal.SizeOf(monitorInfo);

            if (GetMonitorInfo(hMonitor, ref monitorInfo))
            {
                // rcMonitor 是包含任务栏的完整屏幕大小
                int screenWidth = monitorInfo.rcMonitor.right - monitorInfo.rcMonitor.left;
                int screenHeight = monitorInfo.rcMonitor.bottom - monitorInfo.rcMonitor.top;
                Debug.WriteLine($"[FullScreenCheck] 屏幕尺寸 -> L:{monitorInfo.rcMonitor.left}, T:{monitorInfo.rcMonitor.top}, R:{monitorInfo.rcMonitor.right}, B:{monitorInfo.rcMonitor.bottom} | 宽度: {screenWidth}, 高度: {screenHeight}");

                // 5. 比对大小：如果窗口大小大于等于屏幕大小，说明全屏
                if (windowWidth >= screenWidth && windowHeight >= screenHeight)
                {
                    Debug.WriteLine("[FullScreenCheck] 结果: True (窗口大小 >= 屏幕大小，判定为全屏)");
                    return true;
                }
                else
                {
                    Debug.WriteLine("[FullScreenCheck] 结果: False (窗口未达到全屏尺寸)");
                }
            }
            else
            {
                Debug.WriteLine("[FullScreenCheck] 错误: 获取显示器信息(GetMonitorInfo)失败。");
            }
        }
        else
        {
            Debug.WriteLine("[FullScreenCheck] 错误: 获取窗口矩形(GetWindowRect)失败。");
        }

        return false;
    }
}