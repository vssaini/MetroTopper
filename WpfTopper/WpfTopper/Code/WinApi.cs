/****************************** Module WinApi ******************************\
* Module Name:  WinApi.cs
* Project:      WpfTopper
* Copyright (c) Vikram Singh Saini
* 
* Provide way for calling Win32 API by safe native methods.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WpfTopper.Code
{
    public static class WinApi
    {
        /// <summary>
        /// Register hotkey with modifier 'Ctrl' or 'None' overriding other system hotkeys.
        /// </summary>
        /// <param name="handle">Register hotkey to specific handle.</param>
        /// <param name="currentId">Maintain ids of hotkeys.</param>
        /// <param name="modifier">Modifier key either 'Ctrl' or 'None'</param>
        /// <param name="key">key as alphabet keys.</param>
        public static void RegisterHotKey(IntPtr handle, int currentId, ModifierKeys modifier, Keys key)
        {
            NativeMethods.RegisterHotKey(handle, currentId, (uint)modifier, (uint)key);
        }

        /// <summary>
        /// Unregister hotkeys registered earlier.
        /// </summary>
        /// <param name="handle">Unregister hotkey to specific handle.</param>
        /// <param name="currentId">Ids of hotkeys to unregister.</param>
        public static void UnRegisterHotKey(IntPtr handle, int currentId)
        {
            NativeMethods.UnregisterHotKey(handle, currentId);
        }
    }

    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}