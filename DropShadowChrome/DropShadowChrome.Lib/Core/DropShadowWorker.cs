using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace DropShadowChrome.Lib.Core
{
    internal class DropShadowWorker
    {
        #region "Fields"

        private Window _window;
        private DropShadowChrome _dropShadow;
        private IntPtr _wndHandle;
        private const int GlowSize = 6;
        private GlowHwndSource[] _glowWindows = new GlowHwndSource[4];
        private GlowHwndSource _topGlowWnd;
        private GlowHwndSource _leftGlowWnd;
        private GlowHwndSource _bottomGlowWnd;
        private GlowHwndSource _rightGlowWnd;
        private bool _isGlowWindowsInitialized;

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
                return;

            var newRect = GetWindowRect();
            var uFlags = SWP.NOACTIVATE;

            NativeMethods.SetWindowPos(_topGlowWnd.Handle,
                                       IntPtr.Zero,
                                       (int)(newRect.Left - 4),
                                       (int)(newRect.Top - GlowSize),
                                       (int)(newRect.Width + 8),
                                       GlowSize,
                                       uFlags);

            NativeMethods.SetWindowPos(_bottomGlowWnd.Handle,
                                       IntPtr.Zero,
                                       (int)(newRect.Left - 4),
                                       (int)(newRect.Top + newRect.Height),
                                       (int)(newRect.Width + 8),
                                       GlowSize,
                                       uFlags);

            NativeMethods.SetWindowPos(_leftGlowWnd.Handle,
                                       IntPtr.Zero,
                                       (int)(newRect.Left - GlowSize),
                                       (int)(newRect.Top - 2)
                                       , GlowSize
                                       , (int)(newRect.Height + 4),
                                       uFlags);

            NativeMethods.SetWindowPos(_rightGlowWnd.Handle,
                                       IntPtr.Zero,
                                       (int)(newRect.Left + newRect.Width),
                                       (int)(newRect.Top - 2),
                                       GlowSize,
                                       (int)(newRect.Height + 4),
                                       uFlags);
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

            ShowGlowWindows();
        }

        private void InvalidateGlowWindows()
        {
            var newShadow = _dropShadow;
            foreach (var glowHwndSource in _glowWindows)
            {
                glowHwndSource.BorderBrush = newShadow.BorderBrush;
                glowHwndSource.ShadowColor = newShadow.DropShadowColor;
            }
        }

        private void InitializeGlowWindows()
        {
            var rect = GetWindowRect();

            _topGlowWnd = _glowWindows[0] = CreateGlowWindow((int)(rect.Width + 8),
                                           GlowSize,
                                           (int)(rect.Left - 4),
                                           (int)(rect.Top - GlowSize),
                                           Dock.Top);

            _bottomGlowWnd = _glowWindows[1] = CreateGlowWindow((int)(rect.Width + 8),
                                              GlowSize,
                                              (int)(rect.Left - 4),
                                              (int)(rect.Top + rect.Height),
                                              Dock.Bottom);

            _leftGlowWnd = _glowWindows[2] = CreateGlowWindow(GlowSize,
                                            (int)(rect.Height + 4),
                                            (int)(rect.Left - GlowSize),
                                            (int)(rect.Top - 2),
                                            Dock.Left);

            _rightGlowWnd = _glowWindows[3] = CreateGlowWindow(GlowSize,
                                             (int)(rect.Height + 4),
                                             (int)(rect.Left + rect.Width),
                                             (int)(rect.Top - 2),
                                             Dock.Right);
            _isGlowWindowsInitialized = true;
        }

        private Rect GetWindowRect()
        {
            RECT nativeRect;
            NativeMethods.GetWindowRect(_wndHandle, out nativeRect);
            return new Rect(new Point(nativeRect.Left, nativeRect.Top), new Point(nativeRect.Right, nativeRect.Bottom));

            /*return new Rect(_window.PointToScreen(new Point()),
                            new Size(_window.ActualWidth, _window.ActualHeight));*/
        }

        private GlowHwndSource CreateGlowWindow(int width, int height, int x, int y, Dock dock)
        {
            var windowStyle = (uint)(WS.POPUP | WS.CLIPCHILDREN | WS.CLIPCHILDREN | WS.DISABLED);
            var parameters = new HwndSourceParameters("GlowWindow", width, height)
            {
                ParentWindow = _wndHandle,
                UsesPerPixelOpacity = true,
                PositionX = x,
                PositionY = y,
                WindowStyle = (int)windowStyle
            };
            var hwndSource = new GlowHwndSource(parameters)
            {
                GlowSize = GlowSize,
                BorderBrush = _dropShadow.BorderBrush,
                ShadowColor = _dropShadow.DropShadowColor
            };
            hwndSource.InitializeRootVisual(dock);

            return hwndSource;
        }

        private void ShowGlowWindows()
        {
            foreach (var hwndSource in _glowWindows)
                NativeMethods.ShowWindow(hwndSource.Handle, (int)SW.SHOW);
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
