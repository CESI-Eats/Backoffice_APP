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
                    var sumForTheDay = response.Message.Where(payment => payment.Date.Date == date.Date && payment.Status.ToLower() == "success")
                                               .Sum(payment => payment.Amount);
                    sumList.Add(new ObservableValue(sumForTheDay));
                }

                _dashboardViewModel.TodayOrdersValue = sumList.Last();
                sumList.Remove(_dashboardViewModel.TodayOrdersValue);

                _dashboardViewModel.OrdersValue.Clear();
                _dashboardViewModel.OrdersValue.AddRange(sumList);
                _dashboardViewModel.OrdersValue.Add(_dashboardViewModel.TodayOrdersValue);

                DateTime dateThirtyDaysAgo = DateTime.Now.AddDays(-30);

                double creditSum = response.Message.Where(payment => payment.Date >= dateThirtyDaysAgo
                                                          && payment.Type.ToLower() == "credit")
                                        .Sum(payment => payment.Amount);

                double debitSum = response.Message.Where(payment => payment.Date >= dateThirtyDaysAgo
                                                         && payment.Type.ToLower() == "debit")
                                       .Sum(payment => payment.Amount);

                _dashboardViewModel.LastMonthIncomesSum.Clear();
                _dashboardViewModel.LastMonthIncomesSum.Add(creditSum);

                _dashboardViewModel.LastMonthOutcomesSum.Clear();
                _dashboardViewModel.LastMonthOutcomesSum.Add(debitSum);

                _dashboardViewModel.LastMonthIncome = creditSum - debitSum;
            }
            catch (Exception e)
            {
                _dashboardViewModel.ErrorMessage = e.Message;
            }
        }
    }
}
