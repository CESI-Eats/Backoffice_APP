using LiveCharts.Defaults;
using LiveCharts;
using StarterKitMvvm;
using System;
using System.Collections.ObjectModel;
using Backoffice_APP.Models.Responses;
using System.Windows.Input;
using Backoffice_APP.Commands;
using Backoffice_APP.Services;

namespace Backoffice_APP.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public string[] Labels { get; set; }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public ChartValues<ObservableValue> OrdersValue { get; }

        private ObservableValue _todayOrdersValue;

        public ObservableValue TodayOrdersValue
        {
            get { return _todayOrdersValue; }
            set { 
                _todayOrdersValue = value;
                OnPropertyChanged(nameof(TodayOrdersValue));
            }
        }

        public ChartValues<ObservableValue> LastMonthIncomesSum { get; set; }
        public ChartValues<ObservableValue> LastMonthOutcomesSum { get; set; }

        private double _lastMonthIncome;
        public double LastMonthIncome
        {
            get { return _lastMonthIncome; }
            set { _lastMonthIncome = value; OnPropertyChanged(nameof(LastMonthIncome)); }
        }


        public ObservableCollection<Order> Orders { get; }

        public Func<double, string> Formatter { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        public ICommand GetOrdersCommand { get; }
        public ICommand GetPaymentsCommand { get; }

        public DashboardViewModel()
        {
            OrdersValue = new ChartValues<ObservableValue>();

            LastMonthIncomesSum = new ChartValues<ObservableValue>();
            LastMonthOutcomesSum = new ChartValues<ObservableValue>();

            Orders = new ObservableCollection<Order>();
            GetOrdersCommand = new GetOrdersCommand(this, new GetOrdersService());
            GetOrdersCommand.Execute(null);

            GetPaymentsCommand = new GetPaymentsCommand(this, new GetPaymentsService());
            GetPaymentsCommand.Execute(null);

            Labels = new[] { "D-6", "D-5", "D-4", "D-3", "D-2", "D-1", "Today"};

            Formatter = value => value.ToString("N");
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, (chartPoint.Participation));


        }
    }
}
