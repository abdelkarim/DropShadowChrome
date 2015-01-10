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

using System.Windows.Input;

namespace DropShadowChrome.Lib
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowCommands
    {
        private static RoutedUICommand _closeCommand;
        private static RoutedUICommand _maximizeRestoreCommand;
        private static RoutedUICommand _minimizeCommand;

        /// <summary>
        /// Close the window.
        /// </summary>
        public static RoutedUICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RoutedUICommand("Close",
                    "Close",
                    typeof(WindowCommands)));
            }
        }

        /// <summary>
        /// Maximize/restore the window.
        /// </summary>
        public static RoutedUICommand MaximizeRestoreCommand
        {
            get
            {
                return _maximizeRestoreCommand ?? (_maximizeRestoreCommand = new RoutedUICommand("MaximizeRestore",
                    "MaximizeRestore",
                    typeof(WindowCommands)));
            }
        }

        /// <summary>
        /// Minimize the Window to the taskbar.
        /// </summary>
        public static RoutedUICommand MinimizeCommand
        {
            get
            {
                return _minimizeCommand ?? (_minimizeCommand = new RoutedUICommand("Minimize",
                    "Minimize",
                    typeof(WindowCommands)));
            }
        }
    }
}
