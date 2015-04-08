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

        #region ShadowBrush

        /// <summary>
        /// Identifies the <see cref="ShadowBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowBrushProperty = DependencyProperty.Register(
            "ShadowBrush",
            typeof(Brush),
            typeof(DropShadowChrome),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the ShadowBrush property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public Brush ShadowBrush
        {
            get { return (Brush)GetValue(ShadowBrushProperty); }
            set { SetValue(ShadowBrushProperty, value); }
        }

        #endregion

        #region Density

        /// <summary>
        /// Identifies the <see cref="Density"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DensityProperty = DependencyProperty.Register(
            "Density",
            typeof(double),
            typeof(DropShadowChrome),
            new FrameworkPropertyMetadata(0.6));

        /// <summary>
        /// Gets or sets the Density property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public double Density
        {
            get { return (double)GetValue(DensityProperty); }
            set { SetValue(DensityProperty, value); }
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
