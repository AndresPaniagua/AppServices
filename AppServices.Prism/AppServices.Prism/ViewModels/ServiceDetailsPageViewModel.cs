using AppServices.Common.Models;
using Prism.Navigation;

namespace AppServices.Prism.ViewModels
{
    public class ServiceDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private bool _isLogin;
        private ServiceResponse _service;

        public ServiceDetailsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Details";
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("service"))
            {
                Service = parameters.GetValue<ServiceResponse>("service");
                Title = Service.ServicesName;
            }

        }
    }
}
