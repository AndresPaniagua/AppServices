using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace AppServices.Prism.ViewModels
{
    public class MyServicesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static MyServicesPageViewModel _instance;
        private List<ServiceItemViewModel> _myServices;
        private bool _isEnabled;
        private bool _isRunning;

        public MyServicesPageViewModel(INavigationService navigationService, IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = Languages.MyServices;
            LoadMyServicesAsync();
        }

        public List<ServiceItemViewModel> MyServices
        {
            get => _myServices;
            set => SetProperty(ref _myServices, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        
        public static MyServicesPageViewModel GetInstance()
        {
            return _instance;
        }

        private async void LoadMyServicesAsync()
        {
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            ServicesForUserRequest servicesForUser = new ServicesForUserRequest
            {
                UserId = Guid.Parse(user.Id),
                CultureInfo = "en"
            };

            Response response = await _apiService.GetListAsync<ServiceResponse>(
                url,
                "/api",
                "/Service/GetServicesForUser",
                servicesForUser,
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                IsRunning = false;
                return;
            }

            List<ServiceResponse> services = (List<ServiceResponse>)response.Result;

            MyServices = services.Select(a => new ServiceItemViewModel(_navigationService)
            {
                Id = a.Id,
                ServicesName = a.ServicesName,
                Description = a.Description,
                Phone = a.Phone,
                PhotoPath = a.PhotoPath,
                FinishDate = a.FinishDate,
                Price = a.Price,
                Status = a.Status,
                StartDate = a.StartDate,
                ServiceType = a.ServiceType
            }).ToList();

            IsRunning = false;
        }

        public async void ReloadServices()
        {
            string url = App.Current.Resources["UrlAPI"].ToString();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            ServicesForUserRequest servicesForUser = new ServicesForUserRequest
            {
                UserId = Guid.Parse(user.Id),
                CultureInfo = "en"
            };

            Response response = await _apiService.GetListAsync<ServiceResponse>(
                url,
                "/api",
                "/Service/GetServicesForUser",
                servicesForUser,
                "bearer",
                token.Token);

            List<ServiceResponse> services = (List<ServiceResponse>)response.Result;

            MyServices = services.Select(a => new ServiceItemViewModel(_navigationService)
            {
                Id = a.Id,
                ServicesName = a.ServicesName,
                Description = a.Description,
                Phone = a.Phone,
                PhotoPath = a.PhotoPath,
                FinishDate = a.FinishDate,
                Price = a.Price,
                Status = a.Status,
                StartDate = a.StartDate,
                ServiceType = a.ServiceType
            }).ToList();

        }

    }
}
