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
        private DelegateCommand _editServiceCommand;

        public ServiceItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectServiceCommand => _selectServiceCommand ?? (_selectServiceCommand = new DelegateCommand(SelectServiceAsync));

        public DelegateCommand EditServiceCommand => _editServiceCommand ?? (_editServiceCommand = new DelegateCommand(EditServiceAsync));

        private async void SelectServiceAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "service", this }
            };

            await _navigationService.NavigateAsync(nameof(ServiceDetailsPage), parameters);
        }

        private async void EditServiceAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "service", this }
            };

            await _navigationService.NavigateAsync(nameof(EditServicePage), parameters);
        }

    }
}
