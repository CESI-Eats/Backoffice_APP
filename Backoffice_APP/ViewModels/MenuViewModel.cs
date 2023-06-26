﻿using StarterKitMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Backoffice_APP.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        public ICommand DashboardNavigationCommand { get; set; }
        public MenuViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            DashboardNavigationCommand = new NavigationCommand(new NavigationService(_navigationStore, CreateDashboardViewModel));
            DashboardNavigationCommand.Execute(null);
        }

        private BaseViewModel CreateDashboardViewModel()
        {
            return new DashboardViewModel();
        }
    }
}