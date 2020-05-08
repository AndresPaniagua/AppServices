using AppServices.Common.Models;
using AppServices.Common.Services;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace AppServices.Prism.ViewModels
{
    public class ServicesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<ServiceItemViewModel> _services;
        private bool _isRunning;

        public ServicesPageViewModel(INavigationService navigationService,
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Services";
            LoadServicesAsync();
        }

        public List<ServiceItemViewModel> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private async void LoadServicesAsync()
        {
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.ConnectionError", "Languages.Accept");
                return;
            }

            Response response = await _apiService.GetListAsync<ServiceResponse>(
                url,
                "/api",
                "/Service");
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", response.Message, "Languages.Accept");
                return;
            }
            ServiceItemViewModel.Pos = 1;
            List<ServiceResponse> services = (List<ServiceResponse>)response.Result;
            Services = services.Select(a => new ServiceItemViewModel(_navigationService)
            {
                Id = a.Id,
                ServicesName = a.ServicesName,
                Description = a.Description,
                Phone = a.Phone,
                StartDate = a.StartDate,
                PhotoPath = a.PhotoPath,
                FinishDate = a.FinishDate,
                Price = a.Price,
                ServiceType = a.ServiceType,
                User = a.User
            }).ToList();
        }

    }
}
