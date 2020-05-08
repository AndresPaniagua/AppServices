using AppServices.Common.Models;
using AppServices.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace AppServices.Prism.ViewModels
{
    public class ServiceItemViewModel : ServiceResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectServiceCommand;

        public ServiceItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectServiceCommand => _selectServiceCommand ?? (_selectServiceCommand = new DelegateCommand(SelectServiceAsync));

        public static int Pos { get; set; }

        public int Position
        {
            get
            {
                int o = Pos == 0 ? 1 : 0;
                Pos = Pos == 0 ? 1 : 0;
                return o;
            }
        }

        private async void SelectServiceAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "service", this }
            };

            await _navigationService.NavigateAsync(nameof(ServiceDetailsPage), parameters);
        }

    }
}
