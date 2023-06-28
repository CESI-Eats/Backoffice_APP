using Backoffice_APP.Models.Responses;
using Backoffice_APP.Services;
using Backoffice_APP.ViewModels;
using StarterKitMvvm;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Backoffice_APP.Commands
{
    public class GetOrdersCommand : AsyncBaseCommand
    {
        private readonly CommandsViewModel _dashboardViewModel;
        private readonly GetOrdersService _getCommandService;

        public GetOrdersCommand(CommandsViewModel dashboardViewModel, GetOrdersService getCommandService)
        {
            _dashboardViewModel = dashboardViewModel;
            _getCommandService = getCommandService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                OrderResponse response = await _getCommandService.GetCommands();
                _dashboardViewModel.Orders.Clear();
                foreach (Order order in response.Message)
                {
                    _dashboardViewModel.Orders.Add(order);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
