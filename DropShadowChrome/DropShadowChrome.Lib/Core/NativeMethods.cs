/*
The MIT License (MIT)

Copyright (c) 2015 Abdelkarim Sellamna

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Runtime.InteropServices;

namespace DropShadowChrome.Lib.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// 
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. 
        /// These windows are ordered according to their appearance on the screen. 
        /// The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window. 
        /// The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
        /// </summary>
        /// <param name="hwnd">A handle to the window.</param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bRevert"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// Displays a shortcut menu at the specified location and tracks the selection of items
        /// on the shortcut menu. The shortcut menu can appear anywhere on the screen.
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern uint TrackPopupMenuEx(IntPtr hMenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that
        /// created the specified window and returns without waiting for the thread to process the message.
        /// </summary>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        /// <summary>
        /// Deletes an item from the specified menu.
        /// If the menu item opens a menu or submenu,
        /// this function destroys the handle to the menu or submenu and
        /// frees the memory used by the menu or submenu.
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        /// <summary>
        /// Appends a new item to the end of the specified menu bar,
        /// drop-down menu, submenu, or shortcut menu.
        /// You can use this function to specify the content, appearance,
        /// and behavior of the menu item.
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool AppendMenu(IntPtr hMenu, MF uFlags, uint uIDNewItem, string lpNewItem);

        /// <summary>
        /// Changes information about a menu item.
        /// </summary>
        /// <param name="hMenu">A handle to the menu that contains the menu item.</param>
        /// <param name="uItem">The identifier or position of the menu item to change.
        /// The meaning of this parameter depends on the value of <paramref name="fByPosition"/>.
        /// </param>
        /// <param name="fByPosition">
        /// The meaning of <paramref name="uItem"/>.
        ///  If this parameter is <c>false</c>, <paramref name="uItem"/> is a menu item identifier. Otherwise, it is a menu item position.
        /// </param>
        /// <param name="lpmii">
        /// a MENUITEMINFO structure that contains information about the menu item and specifies which menu item attributes to change.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, use the GetLastError function.
        /// </returns>
        [DllImport("user32.dll")]
        internal static extern bool SetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);


        /// <summary>
        /// Retrieves information about a menu item.
        /// </summary>
        /// <param name="hMenu">
        /// A handle to the menu that contains the menu item.
        /// </param>
        /// <param name="uItem">
        /// The identifier or position of the menu item to get information about. The meaning of this parameter depends on the value of fByPosition.
        /// </param>
        /// <param name="fByPosition">
        /// If this parameter is FALSE, uItem is a menu item identifier. Otherwise, it is a menu item position.
        /// </param>
        /// <param name="lpmii">
        /// A pointer to a MENUITEMINFO structure that specifies the information to retrieve and receives information about the menu item. Note that you must set the cbSize member to sizeof(MENUITEMINFO) before calling this function.
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);


        /// <summary>
        /// Changes an existing menu item.
        /// This function is used to specify the content, appearance, and behavior of the menu item.
        /// </summary>
        /// <param name="hMnu">
        /// A handle to the menu to be changed.
        /// </param>
        /// <param name="uPosition">
        /// The menu item to be changed, as determined by the uFlags parameter.
        /// </param>
        /// <param name="uFlags"></param>
        /// <param name="uIDNewItem"></param>
        /// <param name="lpNewItem"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool ModifyMenu(IntPtr hMnu, uint uPosition, uint uFlags, IntPtr uIDNewItem, string lpNewItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="uIDEnableItem"></param>
        /// <param name="uEnable"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
    }
}
