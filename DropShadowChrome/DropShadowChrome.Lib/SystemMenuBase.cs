using System.Windows;

namespace DropShadowChrome.Lib
{
    public abstract class SystemMenuBase : FrameworkElement
    {
        internal virtual void InvalidateState()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        internal uint Id { get; set; }
    }
}