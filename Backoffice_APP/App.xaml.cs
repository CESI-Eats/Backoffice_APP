using Backoffice_APP.ViewModels;
using StarterKitMvvm;
using System.Windows;

namespace Backoffice_APP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly AppUser appUser = new();

        private readonly LoginWindow _loginWindow;

        private readonly CustomMainViewModel _mainViewModel;
        private readonly LoginViewModel _loginViewModel;

        public App()
        {
            _loginWindow = new LoginWindow();

            _mainViewModel = new CustomMainViewModel();
            _loginViewModel = new LoginViewModel(appUser);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _loginViewModel.LoginSuccessful += OnLoginSuccessful;

            _loginWindow.DataContext = _loginViewModel;
            _loginWindow.Show();
            OnLoginSuccessful();
            base.OnStartup(e);
        }

        private void OnLoginSuccessful()
        {
            _loginViewModel.LoginSuccessful -= OnLoginSuccessful;

            MainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };

            _loginWindow.Close();
            MainWindow.Show();
        }
    }
}
