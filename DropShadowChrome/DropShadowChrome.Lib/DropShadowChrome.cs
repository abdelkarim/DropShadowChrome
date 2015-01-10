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

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using DropShadowChrome.Lib.Core;

namespace DropShadowChrome.Lib
{
    /// <summary>
    /// 
    /// </summary>
    public class DropShadowChrome : Freezable
    {
        #region "Properties"

        #region DropShadowChrome

        public static readonly DependencyProperty DropShadowChromeProperty = DependencyProperty.RegisterAttached(
            "DropShadowChrome",
            typeof(DropShadowChrome),
            typeof(DropShadowChrome),
            new FrameworkPropertyMetadata(null, OnDropShadowChromeChanged));

        public static DropShadowChrome GetDropShadowChrome(DependencyObject d)
        {
            return (DropShadowChrome)d.GetValue(DropShadowChromeProperty);
        }

        public static void SetDropShadowChrome(DependencyObject d, DropShadowChrome value)
        {
            d.SetValue(DropShadowChromeProperty, value);
        }

        private static void OnDropShadowChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // no support for design-time
            if (DesignerProperties.GetIsInDesignMode(d))
                return;

            var window = (Window)d;
            var newChrome = (DropShadowChrome)e.NewValue;

            var shadowWorder = DropShadowWorker.GetDropShadowWorker(window);
            if (shadowWorder == null)
            {
                shadowWorder = new DropShadowWorker();
                DropShadowWorker.SetDropShadowWorker(window, shadowWorder);
            }

            shadowWorder.SetWindowShadow(newChrome);
        }

        #endregion

        #region DropShadowColor

        /// <summary>
        /// Identifies the <see cref="DropShadowColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DropShadowColorProperty = DependencyProperty.Register(
            "DropShadowColor",
            typeof(Color),
            typeof(DropShadowChrome),
            new FrameworkPropertyMetadata(Colors.Transparent));

        /// <summary>
        /// Gets or sets the DropShadowColor property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public Color DropShadowColor
        {
            get { return (Color)GetValue(DropShadowColorProperty); }
            set { SetValue(DropShadowColorProperty, value); }
        }

        #endregion

        #region BorderBrush

        /// <summary>
        /// Identifies the <see cref="BorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush",
            typeof(Brush),
            typeof(DropShadowChrome),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BorderBrush property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        #endregion

        #endregion

        #region "Methods"

        protected override Freezable CreateInstanceCore()
        {
            return new DropShadowChrome();
        }

        #endregion
    }
}
