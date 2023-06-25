using LiveCharts.Defaults;
using LiveCharts;
using StarterKitMvvm;
using System;
using System.Collections.ObjectModel;
using Backoffice_APP.Models.Responses;

namespace Backoffice_APP.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public string[] Labels { get; set; }

        public ChartValues<ObservableValue> Incomes { get; }
        public ObservableValue TodayIncome { get; set;}


        private ChartValues<int> _successfulPaymentsCount;
        public ChartValues<int> SuccessfulPaymentsCount
        {
            get { return _successfulPaymentsCount; }
            set { _successfulPaymentsCount = value; OnPropertyChanged(nameof(SuccessfulPaymentsCount)); }
        }
        private ChartValues<int> _failedPaymentsCount;
        public ChartValues<int> FailedPaymentsCount
        {
            get { return _failedPaymentsCount; }
            set { _failedPaymentsCount = value; OnPropertyChanged(nameof(FailedPaymentsCount)); }
        }


        public Func<double, string> Formatter { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        public ObservableCollection<OrderResponse> Orders { get; }

        public DashboardViewModel()
        {
            Incomes = new ChartValues<ObservableValue>();
            TodayIncome = new ObservableValue(50);
            Incomes.AddRange(new[]
            {
                new ObservableValue(10),
                new ObservableValue(20),
                new ObservableValue(80),
                TodayIncome
            });

            SuccessfulPaymentsCount = new ChartValues<int>();
            FailedPaymentsCount = new ChartValues<int>();

            SuccessfulPaymentsCount.Add(8);
            FailedPaymentsCount.Add(2);

            Orders = new ObservableCollection<OrderResponse>();

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");

            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, (chartPoint.Participation));
        }
    }
}
