using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using DropShadowChrome.Lib.Core;

namespace DropShadowChrome.Lib
{
    public class SystemMenuItem : SystemMenuBase, ICommandSource
    {
        #region "Constructors"

        /// <summary>
        /// Initializes static members of the <see cref="SystemMenuItem"/> class.
        /// </summary>
        static SystemMenuItem()
        {
            IsEnabledProperty.OverrideMetadata(typeof(SystemMenuItem), new FrameworkPropertyMetadata(OnIsEnabledChanged));
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var systemMenuItem = (SystemMenuItem) d;
            systemMenuItem.InvalidateState();
        }

        #endregion

        #region "Properties"

        internal bool IsMenuItemInitialized
        {
            get { return WindowHandle != IntPtr.Zero; }
        }

        /// <summary>
        /// The handle of the hosting window.
        /// </summary>
        public IntPtr WindowHandle { get; set; }

        #region Header

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof (string),
            typeof (SystemMenuItem),
            new FrameworkPropertyMetadata
                (null, OnHeaderChanged));

        private static void OnHeaderChanged(DependencyObject d,
                                            DependencyPropertyChangedEventArgs e)
        {
            var menuItem = (SystemMenuItem) d;
            if (!menuItem.IsMenuItemInitialized)
                return;

            menuItem.InvalidateSystemMenuItemHeader();
        }

        /// <summary>
        /// Gets or sets the Header property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        #endregion

        public string Shortcut { get; set; }

        #region Command

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof (ICommand),
            typeof (SystemMenuItem),
            new FrameworkPropertyMetadata(null, OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var systemMenuItem = (SystemMenuItem) d;
            systemMenuItem.InvalidateState();
        }

        /// <summary>
        /// Gets or sets the Command property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region CommandParameter

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof (object),
            typeof (SystemMenuItem),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the CommandParameter property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #region CommandTarget

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            "CommandTarget",
            typeof (IInputElement),
            typeof (SystemMenuItem),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the CommandTarget property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public IInputElement CommandTarget
        {
            get { return (IInputElement) GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        #endregion

        #endregion

        #region Events

        #region Click

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(SystemMenuItem));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        /// <summary>
        /// A helper method to raise the Click event.
        /// </summary>
        internal void RaiseClickEvent()
        {
            RaiseClickEvent(this);
            RaiseCommand();
        }

        /// <summary>
        /// A static helper method to raise the Click event on a target element.
        /// </summary>
        /// <param name="target">UIElement or ContentElement on which to raise the event</param>
        internal static RoutedEventArgs RaiseClickEvent(FrameworkElement target)
        {
            if (target == null) return null;
            RoutedEventArgs args = new RoutedEventArgs {RoutedEvent = ClickEvent};
            target.RaiseEvent(args);
            return args;
        }

        #endregion

        #endregion

        private void InvalidateSystemMenuItemHeader()
        {
            var sysMenuHandle = NativeMethods.GetSystemMenu(WindowHandle, false);
            NativeMethods.ModifyMenu(sysMenuHandle, Id, (uint) (MF.BYCOMMAND | MF.STRING), new IntPtr(Id), Header);
        }

        internal static string EscapeHeader(string header)
        {
            return Regex.Replace(header, "_", "&", RegexOptions.IgnoreCase);
        }

        internal override void InvalidateState()
        {
            if (!IsMenuItemInitialized)
                return;

            var systemMenuHandle = NativeMethods.GetSystemMenu(WindowHandle, false);
            if (!IsEnabled)
            {
                NativeMethods.EnableMenuItem(systemMenuHandle, Id, (uint) (MF.BYCOMMAND | MF.DISABLED));
                return;
            }

            var command = this.Command;
            if (command != null)
            {
                bool isEnabled = command.CanExecute(CommandParameter);
                NativeMethods.EnableMenuItem(systemMenuHandle, Id,
                    (uint) (MF.BYCOMMAND | (isEnabled ? MF.ENABLED : MF.DISABLED)));
            }
            else
            {
                NativeMethods.EnableMenuItem(systemMenuHandle, Id, (uint)(MF.BYCOMMAND | MF.ENABLED));
            }
        }

        private void RaiseCommand()
        {
            var command = this.Command;
            if (command != null)
            {
                var parameter = CommandParameter;
                var target = this.CommandTarget;

                var routedCommand = command as RoutedCommand;
                if (routedCommand != null)
                {
                    if (target == null)
                        target = this;

                    if (routedCommand.CanExecute(parameter, target))
                        routedCommand.Execute(parameter, target);
                }
                else if (command.CanExecute(parameter))
                    command.Execute(parameter);
            }
        }
    }
}