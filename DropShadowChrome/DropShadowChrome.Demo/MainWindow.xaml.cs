using System.Windows;
using DropShadowChrome.Lib;

namespace DropShadowChrome.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SystemMenuItem_OnClick(object sender,
                                            RoutedEventArgs e)
        {
            var systemMenuItem = (SystemMenuItem) sender;
            MessageBox.Show(systemMenuItem.Header);
        }
    }
}
