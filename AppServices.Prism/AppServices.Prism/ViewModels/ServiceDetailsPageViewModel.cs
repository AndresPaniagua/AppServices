using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Maps;

namespace AppServices.Prism.ViewModels
{
    public class ServiceDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ServiceResponse _service;
        private Map _map;
        private DelegateCommand _reservedCommand;
        private Position _position;
        private bool _isLogin;
        private bool _notLogin;

        public ServiceDetailsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Details";
            Map = new Map();
        }

        public DelegateCommand ReservedCommand => _reservedCommand ?? (_reservedCommand = new DelegateCommand(ReservationAsync));

        public bool IsLogin
        {
            get => _isLogin;
            set => SetProperty(ref _isLogin, value);
        }

        public bool NotLogin
        {
            get => _notLogin;
            set => SetProperty(ref _notLogin, value);
        }

        public ServiceResponse Service
        {
            get => _service;
            set => SetProperty(ref _service, value);
        }

        public Position Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("service"))
            {
                Service = parameters.GetValue<ServiceResponse>("service");
                Title = Service.ServicesName;
                if (Settings.IsLogin)
                {
                    IsLogin = true;
                    NotLogin = false;
                    ServiceDetailsPage.GetInstance().DrawMap(Service);
                }
                else
                {
                    IsLogin = false;
                    NotLogin = true;
                }
            }

        }

        private async void ReservationAsync()
        {
            if (Settings.IsLogin)
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { "service", Service }
                };

                await _navigationService.NavigateAsync(nameof(ReservationPage), parameters);
            }
            else
            {
                await _navigationService.NavigateAsync($"/AppServicesMasterDetailPage/NavigationPage/LoginPage");
            }

        }

    }
}
