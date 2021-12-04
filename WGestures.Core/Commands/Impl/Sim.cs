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
         

        public static void KeyDown(KeyCode k)
        {
            EventBuilder.Create().Release(k).Invoke();
        }

        public static void KeyUp(KeyCode k)
        {
            EventBuilder.Create().Release(k).Invoke();
        }

        public static void KeyPress(KeyCode k)
        {
            EventBuilder.Create().Click(k).Invoke();
        }

        public static void TextEntry(string s)
        {
            EventBuilder.Create().Click(s).Invoke();
        }
    }
}
