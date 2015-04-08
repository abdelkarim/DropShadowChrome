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
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using DropShadowChrome.Lib.Controls;

namespace DropShadowChrome.Lib.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class GlowHwndSource : HwndSource, INotifyPropertyChanged
    {
        #region "Fields"

        private Brush _shadowBrush;
        private double _glowSize;
        private double _density;

        #endregion

        #region "Constructors"

        public GlowHwndSource(int classStyle,
                          int style,
                          int exStyle,
                          int x,
                          int y,
                          string name,
                          IntPtr parent)
            : base(classStyle, style, exStyle, x, y, name, parent)
        {
        }

        public GlowHwndSource(int classStyle,
                          int style,
                          int exStyle,
                          int x,
                          int y,
                          int width,
                          int height,
                          string name,
                          IntPtr parent,
                          bool adjustSizingForNonClientArea)
            : base(classStyle, style, exStyle, x, y, width, height, name, parent, adjustSizingForNonClientArea)
        {
        }

        public GlowHwndSource(int classStyle,
                          int style,
                          int exStyle,
                          int x,
                          int y,
                          int width,
                          int height,
                          string name,
                          IntPtr parent)
            : base(classStyle, style, exStyle, x, y, width, height, name, parent)
        {
        }

        public GlowHwndSource(HwndSourceParameters parameters)
            : base(parameters)
        {
        }

        #endregion

        #region "Methods"

        internal void InitializeRootVisual(DropShadowChrome chrome, Dock dock, double glowSize)
        {
            var glowControl = new GlowControl
            {
                Dock = dock
            };
            glowControl.SetBinding(Control.BackgroundProperty, new Binding
            {
                Source = chrome,
                Path = new PropertyPath("ShadowBrush"),
                Mode = BindingMode.OneWay
            });

            glowControl.SetBinding(UIElement.OpacityProperty, new Binding
            {
                Source = chrome,
                Path = new PropertyPath("Density"),
                Mode = BindingMode.OneWay
            });

            switch (dock)
            {
                case Dock.Right:
                case Dock.Left:
                    glowControl.Width = glowSize;
                    break;
                case Dock.Top:
                case Dock.Bottom:
                    glowControl.Height = glowSize;
                    break;
            }

            RootVisual = glowControl;
        }

        #endregion

        #region "INotifyPropertyChanged"

        public event PropertyChangedEventHandler PropertyChanged;

        protected internal virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
