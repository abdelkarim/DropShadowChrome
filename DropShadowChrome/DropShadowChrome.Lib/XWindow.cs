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

using System.Windows;
using System.Windows.Input;

namespace DropShadowChrome.Lib
{
    /// <summary>
    /// 
    /// </summary>
    public class XWindow : Window
    {
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

        #endregion
    }
}
