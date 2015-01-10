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
