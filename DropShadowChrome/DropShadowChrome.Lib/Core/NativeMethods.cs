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
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    /// <summary>
    /// ShowWindow options
    /// </summary>
    internal enum SW
    {
        HIDE = 0,
        SHOW = 5,
        SHOWNOACTIVATE = 4
    }

    [Flags]
    internal enum WS : uint
    {
        VISIBLE = 0x10000000,
        CHILD = 0x40000000,
        DISABLED = 0x08000000,
        CLIPSIBLINGS = 0x04000000,
        CLIPCHILDREN = 0x02000000,
        POPUP = 0x80000000
    }

    /// <summary>
    /// SetWindowPos options
    /// </summary>
    [Flags]
    internal enum SWP
    {
        NOACTIVATE = 0x0010
    }
}
