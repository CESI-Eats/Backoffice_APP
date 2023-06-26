using Backoffice_APP.Models.Responses;
using Backoffice_APP.Services;
using Backoffice_APP.ViewModels;
using StarterKitMvvm;
using System;
using System.Threading.Tasks;

namespace Backoffice_APP.Commands
{
    public class GetOrdersCommand : AsyncBaseCommand
    {
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly GetOrdersService _getCommandService;

        public GetOrdersCommand(DashboardViewModel dashboardViewModel, GetOrdersService getCommandService)
        {
            _dashboardViewModel = dashboardViewModel;
            _getCommandService = getCommandService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                OrderResponse response = await _getCommandService.GetCommands();
                foreach (Order order in response.Message)
                {
                    _dashboardViewModel.Orders.Add(order);
                }
            }
            catch (Exception e)
            {
                _dashboardViewModel.ErrorMessage = e.Message;
            }
        }
    }
}
