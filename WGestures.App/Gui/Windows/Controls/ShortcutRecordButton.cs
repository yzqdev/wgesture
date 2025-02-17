﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WGestures.Common.OsSpecific.Windows;
using Win32;
using WindowsInput;
using WindowsInput.Events;
using WindowsInput.Native;

namespace WGestures.App.Gui.Windows.Controls
{
    class ShortcutRecordButton : Button
    {
        internal class ShortcutRecordEventArgs : EventArgs
        {
            public IList<KeyCode> Modifiers { get; set; }
            public IList<KeyCode> Keys { get; set; }
        }

        private static readonly KeyCode[] modifierKeys = { KeyCode.Control, KeyCode.LControl,KeyCode.RControl,
                                                                    KeyCode.Menu, KeyCode.LMenu,KeyCode.RMenu,
                                                                    KeyCode.Shift, KeyCode.LShift, KeyCode.RShift,
                                                                    KeyCode.RWin, KeyCode.LWin};

        public event EventHandler BeginRecord;
        public event EventHandler<ShortcutRecordEventArgs> EndRecord;

        private GlobalKeyboardHook hook = new GlobalKeyboardHook();

        private List<KeyCode> _keys = new List<KeyCode>();
        private List<KeyCode> _modifiers = new List<KeyCode>();
        private HashSet<KeyCode> _pressedKeys = new HashSet<KeyCode>();

        private bool _isRecording;
        private string _oldText;

        //避免委托被回收
        private KeyEventHandler _keyboardHookOnKeyDown;
        private KeyEventHandler _keyboardhookOnKeyUp;
        

        public ShortcutRecordButton()
        {
            _keyboardHookOnKeyDown = KeyboardHookOnKeyDown;
            _keyboardhookOnKeyUp = KeyboardHookOnKeyUp;

            hook.KeyDown += _keyboardHookOnKeyDown;
            hook.KeyUp += _keyboardhookOnKeyUp;

            Click += btn_recordHotkey_Click;
        }

        private void btn_recordHotkey_Click(object sender, EventArgs e)
        {
            if (!_isRecording)
            {
                _StartRecord();
            }
            else
            {
                _EndRecord();
            }
        }

        private int _lastKey = -1;
        private void KeyboardHookOnKeyDown(object sender, KeyEventArgs args)
        {
            args.Handled = true;

            if (!Enum.IsDefined(typeof(KeyCode),  (ushort)args.KeyValue)) return;

            var key = (KeyCode)args.KeyValue;
            if (!_pressedKeys.Add(key)) return;

            if (modifierKeys.Contains(key) && !_modifiers.Contains(key))
            {
                _modifiers.Add(key);
            }
            else if (!_keys.Contains(key))
            {
                _keys.Add(key);
            }
        }

        private void KeyboardHookOnKeyUp(object sender, KeyEventArgs args)
        {
            args.Handled = true;
            if (args.KeyValue == _lastKey) _lastKey = -1;

            if (Enum.IsDefined(typeof(KeyCode), (ushort)args.KeyValue))
            {
                var key = (KeyCode)args.KeyValue;

                if (_modifiers.Contains(key) || _keys.Contains(key))
                {
                    _pressedKeys.Remove(key);
                }
            }

            if (_pressedKeys.Count == 0)
            {
                this.PerformClick();
            }

        }

        private void _StartRecord()
        {
            _isRecording = true;
            _lastKey = -1;
            _pressedKeys.Clear();

            _modifiers.Clear();
            _keys.Clear();

            BackColor = Color.OrangeRed;
            ForeColor = Color.White;
            _oldText = Text;
            Text = "...";

            hook.hook();

            if(BeginRecord != null)
            {
                BeginRecord(this, EventArgs.Empty);
            }
        }

        private void _EndRecord()
        {
            hook.unhook();

            BackColor = SystemColors.Control;
            ForeColor = Color.Black;
            Text = _oldText;

            _isRecording = false;

            if (EndRecord != null)
            {
                EndRecord(this, new ShortcutRecordEventArgs() { Keys = _keys.ToList(), Modifiers = _modifiers.ToList() });
            }

        }

        public static string HotKeyToString(IList<KeyCode> modifiers, IList<KeyCode> keys)
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

                    if (sb.Length > 0) sb.Append('-');
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


        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                hook.Dispose();
                hook = null;

                _keyboardHookOnKeyDown = null;
                _keyboardhookOnKeyUp = null;
            }

            base.Dispose(disposing);
        }
    }
}
