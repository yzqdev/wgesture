using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;
using Win32;
using Screen = WGestures.Common.OsSpecific.Windows.Screen;
using ThreadState = System.Diagnostics.ThreadState;
using WindowsInput.Events;

namespace WGestures.Core.Commands.Impl
{
    /// <summary>
    /// 执行快捷键
    /// </summary>
    [Named("执行快捷键"), Serializable]
    public class HotKeyCommand : AbstractCommand, IGestureContextAware
    {
        public HotKeyCommand()
        {
            Modifiers = new List<KeyCode>();
            Keys = new List<KeyCode>();
        }

        public List<KeyCode> Modifiers { get; set; }

        public List<KeyCode> Keys { get; set; }

        /// <summary>
        /// 执行快捷键
        /// </summary>
        public override void Execute()
        {

           
            if (Keys.Count + Modifiers.Count == 0) return;
            //锁定windows(锁屏)
            if (Keys.Count == 1 && (Keys[0] == KeyCode.L) &&
                Modifiers.Count == 1 && (Modifiers[0] == KeyCode.LWin || Modifiers[0] == KeyCode.RWin))
            {
                User32.LockWorkStation();
                return;
            }


            //活动进程 未必 是活动root窗口进程, 就像clover
            var fgWindow = Native.GetForegroundWindow();
            var rootWindow = IntPtr.Zero;

            Debug.WriteLine(string.Format("FGWindow: {0:X}", fgWindow.ToInt64()));

            //如果没有前台窗口，或者前台窗口是任务栏，则使用鼠标指针下方的窗口？
            /*var useCursorWindow = false;
            if (fgWindow != IntPtr.Zero)
            {
                var className = new StringBuilder(32);
                Native.GetClassName(fgWindow, className, className.Capacity);

                //如果是任务栏 或者 窗口处于最小化状态
                if (className.ToString() == "Shell_TrayWnd")
                {
                    useCursorWindow = true;
                } //如果活动窗口与鼠标指针不在同一个屏幕
                else if (!IsCursorAndWindowSameScreen(fgWindow))
                {
                    useCursorWindow = true;
                }
                else
                {
                    rootWindow = Native.GetAncestor(fgWindow, Native.GetAncestorFlags.GetRoot);
                    if (IsWindowMinimized(rootWindow))
                    {
                        Debug.WriteLine("Use Cursor Window Cuz rootWindow is Minimized.");
                        useCursorWindow = true;
                    }
                }
            }
            else
            {
                useCursorWindow = true;
            }*/


            //if (useCursorWindow)
            {
                //Debug.WriteLine("* * Why Is fgWindow NULL?");

                if(Context != null) //触发角将不会注入此字段
                {
                    fgWindow = Native.WindowFromPoint(new Native.POINT(){x = Context.StartPoint.X, y = Context.StartPoint.Y});
                    Debug.WriteLine(string.Format("WinforFromPoint={0:x}", fgWindow.ToInt64()));
                    if (fgWindow == IntPtr.Zero)
                        return;
                }
                
            }

            if (rootWindow == IntPtr.Zero)
                rootWindow = Native.GetAncestor(fgWindow, Native.GetAncestorFlags.GetRoot);

            //User32.SetForegroundWindow(fgWindow);
            //ForceWindowIntoForeground(fgWindow);
            uint pid;
            var fgThread = Native.GetWindowThreadProcessId(fgWindow, out pid);
            Debug.WriteLine("pid=" + pid);

            //失败可能原因之一：被杀毒软件或系统拦截
            Debug.WriteLine("第一个按键=" + String.Join(",", Modifiers.ToArray()));
            Debug.WriteLine("第二个按键=" + String.Join(",", Keys.ToArray()));
            try
            {
                //先keydown(hold按下)
                foreach (var k in Modifiers)
                {
                    Debug.WriteLine("modifier"+k);
                    PerformKey(pid, fgThread, k);
                }

                foreach (var k in Keys)
                {
                    Debug.WriteLine("keys="+k);
                    PerformKey(pid, fgThread, k);
                }

                //再松开 keyup(release)
                foreach (var k in Keys)
                {
                    Debug.Write(k + " Up:");

                    PerformKey(pid, fgThread, k, true);
                }

                foreach (var k in Modifiers)
                {
                    Debug.WriteLine(k + " Up:");

                    PerformKey(pid, fgThread, k, true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("发送按键的时候发生异常： " + ex);
                Native.TryResetKeys(Keys, Modifiers);
#if TEST
                throw;
#endif
            }

            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);



        }

        private static bool IsWindowMinimized(IntPtr hwnd)
        {
            int style = User32.GetWindowLong(hwnd, User32.GWL.GWL_STYLE);

            return (int)User32.WS.WS_MINIMIZE == (style & (int)User32.WS.WS_MINIMIZE);
        }
        /// <summary>
        /// 执行按键命令
        /// </summary>
        /// <param name="pid">process id</param>
        /// <param name="tid"></param>
        /// <param name="key">按下的键的code</param>
        /// <param name="isUp"></param>
        private void PerformKey(uint pid, uint tid, KeyCode key, bool isUp = false)
        {

            //Native.WaitForInputIdle(pid, tid, 100);
            Thread.Sleep(10);
            if (!isUp)
            {
                Sim.KeyDown(key);

            }
            else
            {
                Sim.KeyUp(key);
            }

            //Native.WaitForInputIdle(pid, tid, 20);

        }


        private static bool IsCursorAndWindowSameScreen(IntPtr win)
        {
            Native.POINT pt;
            Native.GetCursorPos(out pt);

            var fgWinScreen = Screen.FromHandle(win);
            var cursorScreen = Screen.FromPoint(pt.ToPoint());

            return fgWinScreen.Equals(cursorScreen);

        }

        public override string Description()
        {
            return HotKeyToString(Modifiers, Keys);
        }

        public static void ForceWindowIntoForeground(IntPtr window)
        {
            const uint LSFW_LOCK = 1;
            const uint LSFW_UNLOCK = 2;
            const int ASFW_ANY = -1; // by MSDN

            uint currentThread = Native.GetCurrentThreadId();

            IntPtr activeWindow = User32.GetForegroundWindow();
            //uint activeProcess;
            uint activeThread = User32.GetWindowThreadProcessId(activeWindow, IntPtr.Zero);

            uint windowProcess;
            uint windowThread = User32.GetWindowThreadProcessId(window, IntPtr.Zero);

            if (currentThread != activeThread)
                User32.AttachThreadInput(currentThread, activeThread, true);
            if (windowThread != currentThread)
                User32.AttachThreadInput(windowThread, currentThread, true);

            uint oldTimeout = 0, newTimeout = 0;
            User32.SystemParametersInfo(User32.SPI_GETFOREGROUNDLOCKTIMEOUT, 0, ref oldTimeout, 0);
            User32.SystemParametersInfo(User32.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, ref newTimeout, 0);
            User32.LockSetForegroundWindow(LSFW_UNLOCK);
            User32.AllowSetForegroundWindow(ASFW_ANY);

            User32.SetForegroundWindow(window);
            User32.ShowWindow(window, User32.SW.SW_RESTORE);
            User32.SetFocus(window);

            User32.SystemParametersInfo(User32.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, ref oldTimeout, 0);

            if (currentThread != activeThread)
                User32.AttachThreadInput(currentThread, activeThread, false);
            if (windowThread != currentThread)
                User32.AttachThreadInput(windowThread, currentThread, false);
        }
        /// <summary>
        /// 这里传入modifier和key(入口)
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string HotKeyToString(ICollection<KeyCode> modifiers, ICollection<KeyCode> keys)
        {
            if (keys.Count != 0 || modifiers.Count != 0)
            {
                var sb = new StringBuilder(32);
                foreach (var k in modifiers)
                {
                    string str = "";
                    switch (k)
                    {
                        case KeyCode.Menu:
                        case KeyCode.RMenu:
                        case KeyCode.LMenu:
                            str = "Alt";
                            break;
                        case KeyCode.LControl:
                        case KeyCode.RControl:
                        case KeyCode.Control:
                            str = "Ctrl";
                            break;
                        case KeyCode.RWin:
                        case KeyCode.LWin:
                            str = "Win";
                            break;
                        case KeyCode.Shift:
                        case KeyCode.LShift:
                        case KeyCode.RShift:
                            str = "Shift";
                            break;
                        default:
                            str = k.ToString();
                            break;
                    }
                    if(sb.Length > 0) sb.Append('-');
                    sb.Append(str);
                }

                if(sb.Length > 0) sb.Append(" + ");

                foreach (var k in keys)
                {
                    string str = k.ToString();
                    if (str.StartsWith("VK_")) str = str.Substring(3);

                    sb.Append(str);
                    sb.Append(" + ");
                }


                sb.Remove(sb.Length - 3, 3);
                return sb.ToString();
            }

            return "";
        }


        public GestureContext Context { set; private get; }
    }
}