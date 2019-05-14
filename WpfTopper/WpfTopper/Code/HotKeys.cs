/****************************** Module HotKeys ******************************\
* Module Name:  HotKeys.cs
* Project:      wpfTopper
* Copyright (c) Vikram Singh Saini.
* 
* Provide way for registering keyboard hotkeys.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace WpfTopper.Code
{
    public sealed class KeyboardHook : IDisposable
    {
        private int _currentId;
        private readonly Window _window = new Window();

        public KeyboardHook()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate(object sender, KeyPressedEventArgs args)
                                      {
                                          if (KeyPressed != null)
                                              KeyPressed(this, args);
                                      };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            // increment the counter.
            _currentId = _currentId + 1;

            try
            {
                // register the hot key.
                WinApi.RegisterHotKey(_window.Handle, _currentId, modifier, key);
            }
            catch (Exception exc)
            {
                
            }
        }

        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public void Dispose()
        {
            // unregister all the registered hot keys.
            for (var i = _currentId; i > 0; i--)
            {
                WinApi.UnRegisterHotKey(_window.Handle, i);
            }

            // dispose the inner native window.
            _window.Dispose();
        }


        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region Window that is used internally to get the messages

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private sealed class Window : NativeWindow, IDisposable
        {
            private const int WmHotkey = 0x0312;

            [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
            public Window()
            {
                // create the handle for the window.
                CreateHandle(new CreateParams());
            }

            #region IDisposable Members

            public void Dispose()
            {
                DestroyHandle();
            }

            #endregion

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg != WmHotkey) return;
                // get the keys.
                var key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);

                // invoke the event to notify the parent.
                if (KeyPressed != null)
                    KeyPressed(this, new KeyPressedEventArgs(key));
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;
        }

        #endregion
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private readonly Keys _key;

        internal KeyPressedEventArgs(Keys key)
        {
            //Modifier = modifier;
            _key = key;
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : uint
    {
        None = 0,
        Control = 2,
        Alt = 1,
        Shift = 4,
        Win = 8
    }
}