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
using System.Windows.Media.Effects;

namespace DropShadowChrome.Lib.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class GlowHwndSource : HwndSource, INotifyPropertyChanged
    {
        #region "Fields"

        private Color _shadowColor;
        private Brush _borderBrush;
        private double _glowSize;

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

        #region "Properties"

        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set
            {
                if (_borderBrush == value)
                    return;

                _borderBrush = value;
                RaisePropertyChanged();
            }
        }

        public Color ShadowColor
        {
            get { return _shadowColor; }
            set
            {
                if (_shadowColor == value)
                    return;

                _shadowColor = value;
                RaisePropertyChanged();
            }
        }

        public double GlowSize
        {
            get { return _glowSize; }
            set
            {
                if (_glowSize == value)
                    return;

                _glowSize = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region "Methods"

        internal void InitializeRootVisual(Dock dock)
        {
            var root = new Border
            {
                SnapsToDevicePixels = true,
                ClipToBounds = true
            };

            var borderBrushBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath("BorderBrush"),
                Mode = BindingMode.OneWay
            };
            var element = new Border
            {
                SnapsToDevicePixels = true,
                Effect = GetShadowEffect()
            };

            switch (dock)
            {
                case Dock.Right:
                case Dock.Left:
                    {
                        element.Width = 1;
                        element.Margin = new Thickness(0, 1, 0, 1);
                        root.Width = GlowSize;
                        break;
                    }
                case Dock.Bottom:
                case Dock.Top:
                    {
                        element.Height = 1;
                        element.Margin = new Thickness(3, 0, 3, 0);
                        root.Height = GlowSize;
                        break;
                    }
            }

            switch (dock)
            {
                case Dock.Left:
                    element.HorizontalAlignment = HorizontalAlignment.Right;
                    break;
                case Dock.Top:
                    element.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case Dock.Right:
                    element.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case Dock.Bottom:
                    element.VerticalAlignment = VerticalAlignment.Top;
                    break;
            }

            element.SetBinding(Border.BackgroundProperty, borderBrushBinding);

            root.Child = element;
            RootVisual = root;
        }

        private DropShadowEffect GetShadowEffect()
        {
            var dropShadowColorBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath("ShadowColor"),
                Mode = BindingMode.OneWay
            };
            var dropShadowEffect = new DropShadowEffect
            {
                ShadowDepth = 0,
                BlurRadius = 8
            };
            BindingOperations.SetBinding(dropShadowEffect, DropShadowEffect.ColorProperty, dropShadowColorBinding);

            return dropShadowEffect;
        }

        #endregion

        #region "INotifyPropertyChanged"

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
