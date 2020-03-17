﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class GlobalHook : IDisposable
    {

        private static class WinAPI
        {
            public static class Kernel32
            {
                [DllImport("kernel32")]
                public static extern IntPtr LoadLibrary(string lpFileName);
            }

            public static class User32
            {

               
                public struct KeyboardHookStruct
                {
                    public uint VKCode;
                    public uint ScanCode;
                    public uint Flags;
                    public uint Time;
                    public IntPtr dwExtraInfo;
                }

                public struct MouseHookStruct
                {
                    public int X;
                    public int Y;
                    public uint MouseData;
                    public uint Flags;
                    public uint Time;
                    public IntPtr dwExtraInfo;
                }

               
                public enum WindowsHook : int
                {
                    KeyboardLowLevel = 13,
                    MouseLowLevel = 14,
                }

                
                public enum WindowsMessage : int
                {
                    KeyDown = 0x100,
                    KeyUp = 0x101,

                    SysKeyDown = 0x104,
                    SysKeyUp = 0x105,

                    MouseMove = 0x200,
                    LeftButtonDown = 0x201,
                    LeftButtonUp = 0x202,

                    RightButtonDown = 0x204,
                    RightButtonUp = 0x205,

                    MiddleButtonDown = 0x207,
                    MiddleButtonUp = 0x208,
                }

                public delegate int KeyboardHookProc(int code,
                    WindowsMessage wParam, ref KeyboardHookStruct lParam);

                public delegate int MouseHookProc(int code,
                    WindowsMessage wParam, ref MouseHookStruct lParam);

                [DllImport("user32")]
                public static extern int CallNextHookEx(IntPtr hHk, int nCode,
                    WindowsMessage wParam, ref KeyboardHookStruct lParam);

                [DllImport("user32")]
                public static extern int CallNextHookEx(IntPtr hHk, int nCode,
                    WindowsMessage wParam, ref MouseHookStruct lParam);

                [DllImport("user32")]
                public static extern IntPtr SetWindowsHookEx(WindowsHook idHook,
                    KeyboardHookProc lpfn, IntPtr hMod, uint dwThreadId);

                [DllImport("user32")]
                public static extern IntPtr SetWindowsHookEx(WindowsHook idHook,
                    MouseHookProc lpfn, IntPtr hMod, uint dwThreadId);

                [DllImport("user32")]
                public static extern bool UnhookWindowsHookEx(IntPtr hHk);
            }
        }

       
        private readonly IntPtr _keyboardHookHandle;
        private readonly IntPtr _mouseHookHandle;

        
        private readonly WinAPI.User32.KeyboardHookProc _keyboardCallback;
        private readonly WinAPI.User32.MouseHookProc _mouseCallback;

        
        public event KeyEventHandler KeyDown = (s, e) => { };
        public event KeyEventHandler KeyUp = (s, e) => { };
        public event MouseEventHandler MouseButtonDown = (s, e) => { };
        public event MouseEventHandler MouseButtonUp = (s, e) => { };
        public event MouseEventHandler MouseMove = (s, e) => { };

        public GlobalHook()
        {
            
            _keyboardCallback = new WinAPI.User32.KeyboardHookProc((int code,
                WinAPI.User32.WindowsMessage wParam, ref WinAPI.User32.KeyboardHookStruct lParam) =>
            {
                
                if (code >= 0)
                {
                    var key = (Keys)lParam.VKCode;
                    var eventArgs = new KeyEventArgs(key);

                    
                    switch (wParam)
                    {
                        case WinAPI.User32.WindowsMessage.KeyDown:
                        case WinAPI.User32.WindowsMessage.SysKeyDown:
                            KeyDown(this, eventArgs);
                            break;

                        case WinAPI.User32.WindowsMessage.KeyUp:
                        case WinAPI.User32.WindowsMessage.SysKeyUp:
                            KeyUp(this, eventArgs);
                            break;
                    }


                    if (eventArgs.Handled)
                        return 1;
                }


                return WinAPI.User32.CallNextHookEx(_keyboardHookHandle, code, wParam, ref lParam);
            });

            _mouseCallback = new WinAPI.User32.MouseHookProc((int code,
                WinAPI.User32.WindowsMessage wParam, ref WinAPI.User32.MouseHookStruct lParam) =>
            {

                if (code >= 0)
                {

                    switch (wParam)
                    {
                        case WinAPI.User32.WindowsMessage.MouseMove:
                            MouseMove(this,
                                new MouseEventArgs(MouseButtons.None, 0, lParam.X, lParam.Y, 0));
                            break;

                        case WinAPI.User32.WindowsMessage.LeftButtonDown:
                            MouseButtonDown(this,
                                new MouseEventArgs(MouseButtons.Left, 0, lParam.X, lParam.Y, 0));
                            break;

                        case WinAPI.User32.WindowsMessage.RightButtonDown:
                            MouseButtonDown(this,
                                new MouseEventArgs(MouseButtons.Right, 0, lParam.X, lParam.Y, 0));
                            break;

                        case WinAPI.User32.WindowsMessage.MiddleButtonDown:
                            MouseButtonDown(this,
                                new MouseEventArgs(MouseButtons.Middle, 0, lParam.X, lParam.Y, 0));
                            break;

                        case WinAPI.User32.WindowsMessage.LeftButtonUp:
                            MouseButtonUp(this,
                                new MouseEventArgs(MouseButtons.Left, 0, lParam.X, lParam.Y, 0));
                            break;

                        case WinAPI.User32.WindowsMessage.RightButtonUp:
                            MouseButtonUp(this,
                                new MouseEventArgs(MouseButtons.Right, 0, lParam.X, lParam.Y, 0));
                            break;

                        case WinAPI.User32.WindowsMessage.MiddleButtonUp:
                            MouseButtonUp(this,
                                new MouseEventArgs(MouseButtons.Middle, 0, lParam.X, lParam.Y, 0));
                            break;
                    }
                }

               
                return WinAPI.User32.CallNextHookEx(_mouseHookHandle, code, wParam, ref lParam);
            });

            IntPtr user32Handle = WinAPI.Kernel32.LoadLibrary("user32");


            _keyboardHookHandle = WinAPI.User32.SetWindowsHookEx(
                WinAPI.User32.WindowsHook.KeyboardLowLevel, _keyboardCallback, user32Handle, 0);

            _mouseHookHandle = WinAPI.User32.SetWindowsHookEx(
                WinAPI.User32.WindowsHook.MouseLowLevel, _mouseCallback, user32Handle, 0);
        }

        #region IDisposable implementation

        private bool _isDisposed = false;

        public bool IsDisposed { get { return _isDisposed; } }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            
            WinAPI.User32.UnhookWindowsHookEx(_keyboardHookHandle);
            WinAPI.User32.UnhookWindowsHookEx(_mouseHookHandle);
        }

        ~GlobalHook()
        {
            Dispose();
        }

        #endregion
    }
}
