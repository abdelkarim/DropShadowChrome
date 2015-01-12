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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using DropShadowChrome.Lib.Core;

namespace DropShadowChrome.Lib
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_IconPlaceHolder", Type = typeof(Border))]
    public class XWindow : Window
    {
        #region "Fields"

        private Border _iconPlaceHolder;
        private bool _isSystemMenuOpen;

        #endregion

        #region "Constructors"

        /// <summary>
        /// Initializes static members of the <see cref="XWindow"/> class.
        /// </summary>
        static XWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (XWindow),
                                                     new FrameworkPropertyMetadata(typeof (XWindow)));
            InitializeCommands();
        }

        #endregion

        #region "Methods"

        private static void InitializeCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(XWindow),
                new CommandBinding(WindowCommands.CloseCommand,
                    (sender, args) =>
                    {
                        var wnd = (XWindow)sender;
                        wnd.Close();
                    }));

            CommandManager.RegisterClassCommandBinding(typeof(XWindow),
                new CommandBinding(WindowCommands.MinimizeCommand,
                    (sender, args) =>
                    {
                        var wnd = (XWindow)sender;
                        wnd.WindowState = WindowState.Minimized;
                    }));

            CommandManager.RegisterClassCommandBinding(typeof(XWindow),
                new CommandBinding(WindowCommands.MaximizeRestoreCommand,
                    (sender, args) =>
                    {
                        var wnd = (XWindow)sender;
                        if (wnd.WindowState == WindowState.Normal)
                            wnd.WindowState = WindowState.Maximized;
                        else if (wnd.WindowState == WindowState.Maximized)
                            wnd.WindowState = WindowState.Normal;
                    }));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_iconPlaceHolder != null)
            {
                _iconPlaceHolder.MouseLeftButtonDown -= OnIconMouseLeftButtonDown;
            }

            _iconPlaceHolder = GetTemplateChild("PART_IconPlaceHolder") as Border;
            if (_iconPlaceHolder != null)
                _iconPlaceHolder.MouseLeftButtonDown += OnIconMouseLeftButtonDown;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(MessageHandle));
            }
        }

        private IntPtr MessageHandle(IntPtr hwnd,
                                     int msg,
                                     IntPtr wParam,
                                     IntPtr lParam,
                                     ref bool handled)
        {
            WM message = (WM) msg;
            if (message == WM.UNINITMENUPOPUP)
            {
                var systemMenuHandle = NativeMethods.GetSystemMenu(hwnd, false);

                if (systemMenuHandle == wParam &&
                    _isSystemMenuOpen &&
                    !IsMouseOverIcon())
                {
                    _isSystemMenuOpen = false;
                }
            }

            return IntPtr.Zero;
        }

        private bool IsMouseOverIcon()
        {
            if (_iconPlaceHolder == null)
                return false;

            POINT screenPoint;
            NativeMethods.GetCursorPos(out screenPoint);
            var mousePos = _iconPlaceHolder.PointFromScreen(screenPoint);
            var rect = LayoutInformation.GetLayoutSlot(_iconPlaceHolder);
            return rect.Contains(mousePos);
        }

        private void OnIconMouseLeftButtonDown(object sender,
                                               MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                this.Close();
                return;
            }

            var wndHandle = new WindowInteropHelper(this).Handle;
            var hMenu = NativeMethods.GetSystemMenu(wndHandle, false);

            if (_isSystemMenuOpen)
            {
                _isSystemMenuOpen = false;
                NativeMethods.ShowWindow(hMenu, (int) SW.HIDE);
                return;
            }

            var screenPoint = _iconPlaceHolder.PointToScreen(new Point(8, _iconPlaceHolder.ActualHeight + 3));
            _isSystemMenuOpen = true;
            var selectedCommand = NativeMethods.TrackPopupMenuEx(hMenu,
                                                                 (uint) (TPM.LEFTALIGN | TPM.RETURNCMD),
                                                                 (int) screenPoint.X,
                                                                 (int) screenPoint.Y,
                                                                 wndHandle,
                                                                 IntPtr.Zero);
            
            // if the user did select and menu item.
            if (selectedCommand != 0)
                NativeMethods.PostMessage(wndHandle, (uint) WM.SYSCOMMAND, new IntPtr(selectedCommand), IntPtr.Zero);
        }

        #endregion

        #region "Properties"

        #region RightAdditionalContent

        /// <summary>
        /// Identifies the <see cref="RightAdditionalContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RightAdditionalContentProperty = DependencyProperty.Register(
            "RightAdditionalContent",
            typeof(object),
            typeof(XWindow),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the RightAdditionalContent property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public object RightAdditionalContent
        {
            get { return (object)GetValue(RightAdditionalContentProperty); }
            set { SetValue(RightAdditionalContentProperty, value); }
        }

        #endregion

        #region LeftAdditionalContent

        /// <summary>
        /// Identifies the <see cref="LeftAdditionalContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LeftAdditionalContentProperty = DependencyProperty.Register(
            "LeftAdditionalContent",
            typeof(object),
            typeof(XWindow),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the LeftAdditionalContent property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public object LeftAdditionalContent
        {
            get { return (object)GetValue(LeftAdditionalContentProperty); }
            set { SetValue(LeftAdditionalContentProperty, value); }
        }

        #endregion

        #endregion
    }
}
