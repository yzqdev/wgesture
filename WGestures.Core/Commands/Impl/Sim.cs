using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using WindowsInput.Events;
using WindowsInput.Native;

namespace WGestures.Core.Commands.Impl
{
    static internal class Sim
    {
         
        /// <summary>
        /// hold==keydown
        /// release==keyup
        /// hold and release == click
        /// </summary>
        /// <param name="k"></param>
        public static void KeyDown(KeyCode k)
        {
            Simulate.Events().Hold(k).Wait(100).Invoke();
        }

        public static void KeyUp(KeyCode k)
        {
            Simulate.Events().Release(k).Wait(100).Invoke();
        }

        public static void KeyPress(KeyCode k)
        {
            Simulate.Events().Click(k).Wait(1000).Invoke();
        }

        public static void TextEntry(string s)
        {
            Simulate.Events().Click(s).Invoke();
        }
    }
}
