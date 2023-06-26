using Backoffice_APP.Models.Responses;
using Backoffice_APP.Services;
using Backoffice_APP.ViewModels;
using LiveCharts.Defaults;
using StarterKitMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice_APP.Commands
{
    public class GetPaymentsCommand : AsyncBaseCommand
    {
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly GetPaymentsService _getPaymentService;

        public GetPaymentsCommand(DashboardViewModel dashboardViewModel, GetPaymentsService getPaymentService)
        {
            _dashboardViewModel = dashboardViewModel;
            _getPaymentService = getPaymentService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                PaymentResponse response = await _getPaymentService.GetPayments();

                DateTime dateToday = DateTime.Now;
                List<ObservableValue> sumList = new List<ObservableValue>();

                for (int i = 6; i >= 0; i--)
                {
                    var date = dateToday.AddDays(-i);
                    var sumForTheDay = response.Message.Where(payment => payment.Date.Date == date.Date)
                                               .Sum(payment => payment.Amount);
                    sumList.Add(new ObservableValue(sumForTheDay));
                }

                _dashboardViewModel.TodayIncome = sumList.Last();
                sumList.Remove(_dashboardViewModel.TodayIncome);

                _dashboardViewModel.Incomes.Clear();
                _dashboardViewModel.Incomes.AddRange(sumList);
                _dashboardViewModel.Incomes.Add(_dashboardViewModel.TodayIncome);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
