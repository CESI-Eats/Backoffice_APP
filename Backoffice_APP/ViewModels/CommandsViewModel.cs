using Backoffice_APP.Commands;
using Backoffice_APP.Models.Responses;
using Backoffice_APP.Services;
using StarterKitMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Backoffice_APP.ViewModels
{
    public class CommandsViewModel : BaseViewModel
    {
        public ObservableCollection<Order> Orders { get; }

        public ICommand GetOrdersCommand { get; }

        public CommandsViewModel()
        {
            Orders = new ObservableCollection<Order>();

            GetOrdersCommand = new GetOrdersCommand(this, new GetOrdersService());
            GetOrdersCommand.Execute(null);
        }
    }
}
