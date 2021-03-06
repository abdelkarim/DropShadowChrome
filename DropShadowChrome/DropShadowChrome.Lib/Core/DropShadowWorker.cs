﻿/*
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;

namespace DropShadowChrome.Lib.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class DropShadowWorker
    {
        #region "Fields"

        private Window _window;
        private DropShadowChrome _dropShadow;
        private IntPtr _wndHandle;
        private const int GLOW_SIZE = 8;
        private GlowHwndSource[] _glowWindows = new GlowHwndSource[4];
        private GlowHwndSource _topGlowWnd;
        private GlowHwndSource _leftGlowWnd;
        private GlowHwndSource _bottomGlowWnd;
        private GlowHwndSource _rightGlowWnd;
        private bool _isGlowWindowsInitialized;
        private DispatcherTimer _delayTimer;
        private bool _delay;
        private static IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static SWP WndPlacementFlags = SWP.NOACTIVATE;

        #endregion

        #region "Properties"

        #region DropShadowWorker

        public static readonly DependencyProperty DropShadowWorkerProperty = DependencyProperty.RegisterAttached(
            "DropShadowWorker",
            typeof(DropShadowWorker),
            typeof(DropShadowWorker),
            new FrameworkPropertyMetadata(null, OnDropShadowWorkerChanged));

        private static void OnDropShadowWorkerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (Window)d;
            var worker = (DropShadowWorker)e.NewValue;
            worker.AttachToWindow(window);
        }

        public static DropShadowWorker GetDropShadowWorker(DependencyObject d)
        {
            return (DropShadowWorker)d.GetValue(DropShadowWorkerProperty);
        }

        public static void SetDropShadowWorker(DependencyObject d, DropShadowWorker value)
        {
            d.SetValue(DropShadowWorkerProperty, value);
        }

        #endregion

        #endregion

        #region "Methods"

        private void UnsubscribeWindowEvents()
        {
            if (_window == null)
                return;

            _window.Closed -= OnWindowClosed;
            _window.SizeChanged -= OnSizeChanged;
            _window.LocationChanged -= OnLocationChanged;
            _window.SourceInitialized -= OnSourceInitialized;
        }

        private void AttachToWindow(Window window)
        {
            _window = window;
            _window.Closed += OnWindowClosed;
            _window.SizeChanged += OnSizeChanged;
            _window.LocationChanged += OnLocationChanged;
            _window.StateChanged += OnStateChanged;

            _wndHandle = new WindowInteropHelper(window).Handle;
            if (_wndHandle != IntPtr.Zero)
            {
                // if a dropshadow is specified,
                if (_dropShadow != null)
                    ApplyShadow();
            }
            else
            {
                _window.SourceInitialized += OnSourceInitialized;
            }
        }

        private void OnStateChanged(object sender,
                                    EventArgs eventArgs)
        {
            // in case the window is not yet initialized.
            if (_wndHandle == IntPtr.Zero)
                return;

            if (_window.WindowState == WindowState.Normal)
            {
                _delay = true;
                ShowGlowWindows();
            }
            else
                HideGlowWindows();
        }

        private void OnLocationChanged(object sender,
                                             EventArgs e)
        {
            InvalidateGlowsRect();
        }

        private void OnSizeChanged(object sender,
                                         SizeChangedEventArgs e)
        {
            InvalidateGlowsRect();
        }

        private void DetachFromWindow(Window window)
        {

        }

        private void InvalidateGlowsRect()
        {
            if (_wndHandle == IntPtr.Zero)
            {
                return;
            }

            var newRect = GetWindowRect();

            var left = (int)(newRect.Left - GLOW_SIZE);
            var right = (int)(newRect.Left + newRect.Width);
            var top = (int)(newRect.Top - GLOW_SIZE);
            var bottom = (int)(newRect.Top + newRect.Height);
            var width = (int)(newRect.Width + (GLOW_SIZE * 2));
            var height = (int)(newRect.Height + (GLOW_SIZE * 2));

            NativeMethods.SetWindowPos(_topGlowWnd.Handle,
                HWND_NOTOPMOST, 
                left,
                top,
                width,
                GLOW_SIZE,
                WndPlacementFlags);

            NativeMethods.SetWindowPos(_bottomGlowWnd.Handle,
                HWND_NOTOPMOST,
                left,
                bottom,
                width,
                GLOW_SIZE,
                WndPlacementFlags);

            NativeMethods.SetWindowPos(_leftGlowWnd.Handle,
                HWND_NOTOPMOST,
                left,
                top
                , GLOW_SIZE
                , height,
                WndPlacementFlags);

            NativeMethods.SetWindowPos(_rightGlowWnd.Handle,
                HWND_NOTOPMOST,
                right,
                top,
                GLOW_SIZE,
                height,
                WndPlacementFlags);
        }

        private void OnSourceInitialized(object sender,
                                               EventArgs eventArgs)
        {
            var window = (Window)sender;
            _wndHandle = new WindowInteropHelper(window).Handle;

            if (_dropShadow != null)
            {
                ApplyShadow();
            }
        }

        private void OnWindowClosed(object sender,
                                    EventArgs eventArgs)
        {
            var window = (Window)sender;
            DetachFromWindow(window);
        }

        public void SetWindowShadow(DropShadowChrome dropShadowChrome)
        {
            if (_dropShadow == dropShadowChrome)
                return;

            _dropShadow = dropShadowChrome;
            ApplyShadow();
        }

        private void ApplyShadow()
        {
            // the window might be present but not yet initialized.
            if (_wndHandle == IntPtr.Zero)
                return;

            if (_dropShadow == null)
            {
                RemoveShadowElements();
                return;
            }

            if (!_isGlowWindowsInitialized)
                InitializeGlowWindows();
            else
                InvalidateGlowWindows();

            _delay = true;
            ShowGlowWindows();
        }

        private void InvalidateGlowWindows()
        {
            /*var newShadow = _dropShadow;
            foreach (var glowHwndSource in _glowWindows)
            {
                glowHwndSource.ShadowBrush = newShadow.ShadowBrush;
            }*/
        }

        private void InitializeGlowWindows()
        {
            var rect = GetWindowRect();

            _topGlowWnd = _glowWindows[0] = CreateGlowWindow((int)(rect.Width + (GLOW_SIZE * 2)),
                                           GLOW_SIZE,
                                           (int)(rect.Left - GLOW_SIZE),
                                           (int)(rect.Top - GLOW_SIZE),
                                           Dock.Top);

            _bottomGlowWnd = _glowWindows[1] = CreateGlowWindow((int)(rect.Width + (GLOW_SIZE * 2)),
                                              GLOW_SIZE,
                                              (int)(rect.Left - GLOW_SIZE),
                                              (int)(rect.Top + rect.Height),
                                              Dock.Bottom);

            _leftGlowWnd = _glowWindows[2] = CreateGlowWindow(GLOW_SIZE,
                                            (int)(rect.Height + (GLOW_SIZE * 2)),
                                            (int)(rect.Left - GLOW_SIZE),
                                            (int)(rect.Top - GLOW_SIZE),
                                            Dock.Left);

            _rightGlowWnd = _glowWindows[3] = CreateGlowWindow(GLOW_SIZE,
                                             (int)(rect.Height + (GLOW_SIZE * 2)),
                                             (int)(rect.Left + rect.Width),
                                             (int)(rect.Top - GLOW_SIZE),
                                             Dock.Right);
            _isGlowWindowsInitialized = true;
        }

        private Rect GetWindowRect()
        {
            RECT nativeRect;
            NativeMethods.GetWindowRect(_wndHandle, out nativeRect);
            return new Rect(new Point(nativeRect.Left, nativeRect.Top), new Point(nativeRect.Right, nativeRect.Bottom));
        }

        private GlowHwndSource CreateGlowWindow(int width, int height, int x, int y, Dock dock)
        {
            var windowStyle = (uint)(WS.POPUP | WS.CLIPCHILDREN | WS.CLIPCHILDREN | WS.DISABLED);
            int extendedWindowStyle = (int)(WS_EX.TRANSPARENT);
            var parameters = new HwndSourceParameters("GlowWindow", width, height)
            {
                ParentWindow = _wndHandle,
                UsesPerPixelOpacity = true,
                PositionX = x,
                PositionY = y,
                WindowStyle = (int)windowStyle,
                ExtendedWindowStyle = extendedWindowStyle
            };
            var hwndSource = new GlowHwndSource(parameters);
            hwndSource.InitializeRootVisual(_dropShadow, dock, GLOW_SIZE);
            return hwndSource;
        }

        private void ShowGlowWindows()
        {
            if (_window.WindowState == WindowState.Maximized)
                return;

            if (SystemParameters.MinimizeAnimation && _delay)
            {
                _delayTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(270),
                    DispatcherPriority.Normal,
                    OnDelayShowGlow,
                    Dispatcher.CurrentDispatcher);
                _delayTimer.Start();
                return;
            }

            foreach (var hwndSource in _glowWindows)
            {
                NativeMethods.ShowWindow(hwndSource.Handle, (int)SW.SHOWNOACTIVATE);
            }
        }

        private void OnDelayShowGlow(object sender, EventArgs eventArgs)
        {
            _delayTimer.Stop();
            _delay = false;
            ShowGlowWindows();
        }

        private void HideGlowWindows()
        {
            foreach (var glowWindow in _glowWindows)
                NativeMethods.ShowWindow(glowWindow.Handle, (int)SW.HIDE);
        }

        private void RemoveShadowElements()
        {
            foreach (var hwndSource in _glowWindows)
            {
                NativeMethods.ShowWindow(hwndSource.Handle, (int)SW.HIDE);
                hwndSource.Dispose();
            }

            _glowWindows = new GlowHwndSource[4];
            _topGlowWnd = null;
            _bottomGlowWnd = null;
            _leftGlowWnd = null;
            _rightGlowWnd = null;

            _isGlowWindowsInitialized = false;
        }

        #endregion
    }
}
