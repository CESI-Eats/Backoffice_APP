using StarterKitMvvm;

namespace Backoffice_APP.ViewModels
{
    public class CustomMainViewModel : MainViewModel
    {
        public DashboardViewModel DashboardViewModel { get; set; }
        public CustomMainViewModel()
        {
            DashboardViewModel = new DashboardViewModel();
        }
    }
}
