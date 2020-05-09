using AppServices.Common.Models;
using AppServices.Prism.Views;
using Prism.Navigation;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppServices.Prism.ViewModels
{
    public class ServiceDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private bool _isLogin;
        private ServiceResponse _service;
        private Position _position;
        private Map _map;

        public ServiceDetailsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Details";
            Map = new Map();
        }

        public bool IsLogin
        {
            get => _isLogin;
            set => SetProperty(ref _isLogin, value);
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
                ServiceDetailsPage.GetInstance().DrawMap(Service);
            }

        }
    }
}
