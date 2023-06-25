using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using StarterKitMvvm;
using System;

namespace Backoffice_APP.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel()
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<ObservableValue>
                    { new ObservableValue(10),
                      new ObservableValue(20),
                      new ObservableValue(80),
                      new ObservableValue(50)
                    }
                }
            };

            //also adding values updates and animates the chart automatically
            //SeriesCollection[0].Values.Add(48d);

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");
        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
