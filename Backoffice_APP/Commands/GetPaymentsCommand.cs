using Backoffice_APP.Models.Responses;
using Backoffice_APP.Services;
using Backoffice_APP.ViewModels;
using LiveCharts.Defaults;
using StarterKitMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
                    DateTime date = dateToday.AddDays(-i);
                    double creditSumForTheDay = response.Message.Where(payment => payment.Date.Date == date.Date
                                                                               && payment.Status.ToLower() == "success"
                                                                               && payment.Type.ToLower() == "credit")
                                                             .Sum(payment => payment.Amount);

                    double debitSumForTheDay = response.Message.Where(payment => payment.Date.Date == date.Date
                                                                              && payment.Status.ToLower() == "success"
                                                                              && payment.Type.ToLower() == "debit")
                                                            .Sum(payment => payment.Amount);

                    sumList.Add(new ObservableValue(creditSumForTheDay - debitSumForTheDay));
                }

                _dashboardViewModel.TodayProfit = sumList.Last();
                sumList.Remove(_dashboardViewModel.TodayProfit);

                _dashboardViewModel.ProfitValues.Clear();
                _dashboardViewModel.ProfitValues.AddRange(sumList);
                _dashboardViewModel.ProfitValues.Add(_dashboardViewModel.TodayProfit);

                DateTime dateThirtyDaysAgo = DateTime.Now.AddDays(-30);

                double creditSum = response.Message.Where(payment => payment.Date >= dateThirtyDaysAgo
                                                          && payment.Status.ToLower() == "success"
                                                          && payment.Type.ToLower() == "credit")
                                        .Sum(payment => payment.Amount);

                double debitSum = response.Message.Where(payment => payment.Date >= dateThirtyDaysAgo
                                                         && payment.Status.ToLower() == "success"
                                                         && payment.Type.ToLower() == "debit")
                                       .Sum(payment => payment.Amount);

                _dashboardViewModel.LastMonthIncomesSum.Clear();
                _dashboardViewModel.LastMonthIncomesSum.Add(new ObservableValue(creditSum));

                _dashboardViewModel.LastMonthOutcomesSum.Clear();
                _dashboardViewModel.LastMonthOutcomesSum.Add(new ObservableValue(debitSum));

                _dashboardViewModel.LastMonthIncome = creditSum - debitSum;

                _dashboardViewModel.ErrorMessage = string.Empty;
            }
            catch (Exception e)
            {
                _dashboardViewModel.ErrorMessage = e.Message;
            }
        }
    }
}
