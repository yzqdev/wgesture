
//This file contains the common Win32 API of the desktop Windows and the Windows CE/Mobile.

//Created by Warren Tang on 8/8/2008

//迁移状态（Vanara 底层化重构）：
// - CE/移动端遗留的电源、电池 API（GetSystemPowerStatusEx*、SetPowerRequirement 等）在桌面端无调用方，已删除。
// - GetModuleHandle 保留手写 DllImport：Vanara 返回 owning 的 HINSTANCE SafeHandle（释放时 FreeLibrary），
//   包装后临时 SafeHandle 被 GC 回收会触发对模块句柄的 FreeLibrary，行为有风险，故不迁移。

using System;
using System.Runtime.InteropServices;

namespace Win32;

public static partial class Kernel32
{
    private const string Kernel32Dll = "kernel32.dll";

    [DllImport(Kernel32Dll, CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
}
