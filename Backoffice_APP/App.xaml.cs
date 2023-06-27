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
        private readonly LoginWindow _loginWindow;

        private CustomMainViewModel _mainViewModel;
        private readonly LoginViewModel _loginViewModel;

        public App()
        {
            _loginWindow = new LoginWindow();

            _loginViewModel = new LoginViewModel();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _loginViewModel.LoginSuccessful += OnLoginSuccessful;

            _loginWindow.DataContext = _loginViewModel;
            _loginWindow.Show();
            //OnLoginSuccessful();
            base.OnStartup(e);
        }

        private void OnLoginSuccessful()
        {
            _mainViewModel = new CustomMainViewModel();

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
