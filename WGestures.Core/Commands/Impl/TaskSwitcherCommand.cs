using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;
using Win32;
using WindowsInput.Events;

namespace WGestures.Core.Commands.Impl
{
    [Named("任务切换"), Serializable]
    public class TaskSwitcherCommand : AbstractCommand, IGestureModifiersAware
    {

        public override void Execute()
        {

            try
            {
                Sim.KeyDown(KeyCode.LMenu);

                //Thread.Sleep(50);
                Sim.KeyDown(KeyCode.Tab);

                //如果不延迟，foxitreader等ctrl+C可能失效！
                Thread.Sleep(20);

                Sim.KeyUp(KeyCode.Tab);

                Sim.KeyUp(KeyCode.LMenu);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("发送alt + tab失败: " + ex);
                TryRecoverAltTab();
#if TEST
                throw;
#endif
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
          
        }


        public void GestureRecognized(out GestureModifier observeModifiers)
        {
            //直接交由系统的任务切换机制处理，不需要订阅任何事件
            observeModifiers = GestureModifier.None;

            try
            {
                Sim.KeyDown(KeyCode.LMenu);
                Sim.KeyPress(KeyCode.Tab);


            }
            catch (Exception ex) 
            {
                Debug.WriteLine("发送按键失败: " + ex);
                TryRecoverAltTab();
            }

            
        }

        public void ModifierTriggered(GestureModifier modifier)
        {
            
        }

        public void GestureEnded()
        {
            try
            {
                Sim.KeyUp(KeyCode.LMenu);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("发送按键失败： "+ex);
                TryRecoverAltTab();
            }
        }

        public event Action<string> ReportStatus;

        private void TryRecoverAltTab()
        {
            Debug.WriteLine("尝试恢复Alt Tab的状态...");
            Native.TryResetKeys(new []{KeyCode.LMenu, KeyCode.Tab});
        }
    }
}
