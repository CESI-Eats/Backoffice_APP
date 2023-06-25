using Backoffice_APP.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        private readonly MainViewModel _mainViewModel;
        private readonly LoginViewModel _loginViewModel;

        public App()
        {
            _loginWindow = new LoginWindow();

            _mainViewModel = new MainViewModel();
            _loginViewModel = new LoginViewModel(appUser);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _loginViewModel.LoginSuccessful += OnLoginSuccessful;

            _loginWindow.DataContext = _loginViewModel;
            _loginWindow.Show();
            base.OnStartup(e);
        }

        private void OnLoginSuccessful()
        {
            _loginViewModel.LoginSuccessful -= OnLoginSuccessful;
            _mainViewModel.Dashboard = new DashboardViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };

            _loginWindow.Close();
            MainWindow.Show();
        }
    }
}
