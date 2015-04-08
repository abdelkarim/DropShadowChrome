using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DropShadowChrome.Lib.Controls
{
    public class GlowControl : Control
    {
        #region "Constructors"

        /// <summary>
        /// Initializes static members of the <see cref="GlowControl"/> class.
        /// </summary>
        static GlowControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowControl), new FrameworkPropertyMetadata(typeof(GlowControl)));
            FocusableProperty.OverrideMetadata(typeof(GlowControl), new FrameworkPropertyMetadata(false));
        }

        #endregion

        #region "Properties"

        #region Dock

        /// <summary>
        /// Identifies the <see cref="Dock"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DockProperty = DependencyProperty.Register(
            "Dock",
            typeof(Dock),
            typeof(GlowControl),
            new FrameworkPropertyMetadata(Dock.Bottom));

        /// <summary>
        /// Gets or sets the Dock property. This is a dependency property.
        /// </summary>
        /// <value>
        ///
        /// </value>
        [Bindable(true)]
        public Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockProperty, value); }
        }

        #endregion

        #endregion
    }
}
