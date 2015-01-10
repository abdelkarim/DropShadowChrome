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
