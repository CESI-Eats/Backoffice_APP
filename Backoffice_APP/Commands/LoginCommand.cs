﻿using Backoffice_APP.Services;
using Backoffice_APP.ViewModels;
using StarterKitMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backoffice_APP.Commands
{
    public class LoginCommand : AsyncBaseCommand
    {
        private readonly LoginViewModel _viewModel;
        private readonly AppUser _appUser;
        private readonly LoginService _loginService;

        public LoginCommand(LoginViewModel viewModel, AppUser appUser)
        {
            _viewModel = viewModel;
            _appUser = appUser;
            _loginService = new LoginService();
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _viewModel.IsLoading = true;
            try
            {
                await _loginService.Login(_viewModel.Mail, _viewModel.Password, _appUser);
                _viewModel.OnLoginSuccessful();
            }
            catch (Exception e)
            {
                _viewModel.ErrorMessage = e.Message;
            }
            _viewModel.IsLoading = false;
        }
    }
}