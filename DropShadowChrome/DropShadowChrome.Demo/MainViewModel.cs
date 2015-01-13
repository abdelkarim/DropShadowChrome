using System.Windows;
using System.Windows.Input;

namespace DropShadowChrome.Demo
{
    public class MainViewModel
    {
        private ICommand _aboutCommand;

        public ICommand AboutCommand
        {
            get
            {
                return _aboutCommand ?? (_aboutCommand = new RelayCommand(() =>
                {
                    MessageBox.Show("About Command Executing...");
                }));
            }
        }
    }
}
