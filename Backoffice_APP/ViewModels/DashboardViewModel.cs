using LiveCharts.Defaults;
using LiveCharts;
using StarterKitMvvm;
using System;
using System.Collections.ObjectModel;
using Backoffice_APP.Models.Responses;
using System.Windows.Input;
using Backoffice_APP.Commands;
using Backoffice_APP.Services;
using System.Configuration;
using SocketIOClient;
using Newtonsoft.Json;
using System.Linq;

namespace Backoffice_APP.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private SocketIO socket;
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

        public ChartValues<ObservableValue> ProfitValues { get; }

        private ObservableValue _todayProfit;

        public ObservableValue TodayProfit
        {
            get { return _todayProfit; }
            set { 
                _todayProfit = value;
                OnPropertyChanged(nameof(TodayProfit));
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
            ProfitValues = new ChartValues<ObservableValue>();

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
            ConnectSocket();
        }

        private async void ConnectSocket()
        {
            socket = new SocketIO(ConfigurationManager.AppSettings["socket_url"]);
            socket.OnConnected += async (sender, e) =>
            {
                await socket.EmitAsync("setClientId", AppUser.Token);
            };
            socket.On("payment.created", response =>
            {
                Payment payment = JsonConvert.DeserializeObject<Payment>(response.GetValue().ToString());
                if (payment.Status.ToLower() == "success")
                {
                    var newTodayProfit = new ObservableValue(payment.Type == "credit" ? TodayProfit.Value + payment.Amount : TodayProfit.Value - payment.Amount);
                    ProfitValues.Remove(TodayProfit);
                    ProfitValues.Add(newTodayProfit);
                    TodayProfit = newTodayProfit;

                    if (payment.Type == "credit")
                    {
                        ObservableValue newLastMonthIncomeSum = new ObservableValue(LastMonthIncomesSum.FirstOrDefault().Value + payment.Amount);
                        LastMonthIncomesSum.Clear();
                        LastMonthIncomesSum.Add(newLastMonthIncomeSum);
                    }
                    else
                    {
                        ObservableValue newLastMonthOutcomeSum = new ObservableValue(LastMonthOutcomesSum.FirstOrDefault().Value + payment.Amount);
                        LastMonthOutcomesSum.Clear();
                        LastMonthOutcomesSum.Add(newLastMonthOutcomeSum);
                    }
                    LastMonthIncome = LastMonthIncomesSum.FirstOrDefault().Value - LastMonthOutcomesSum.FirstOrDefault().Value;
                }
            });
            try
            {
                await socket.ConnectAsync();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        public override void Dispose()
        {
            socket.DisconnectAsync();
            base.Dispose();
        }
    }
}
