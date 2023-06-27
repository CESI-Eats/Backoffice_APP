using StarterKitMvvm;

namespace Backoffice_APP.ViewModels
{
    public class CustomMainViewModel : MainViewModel
    {
        public MenuViewModel MenuViewModel { get; set; }
        public CustomMainViewModel()
        {
            MenuViewModel = new MenuViewModel(_navigationStore);
        }
    }
}
