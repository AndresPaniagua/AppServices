using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
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
        private bool _isEnabled;
        private bool _isRunning;
        private List<ServiceResponse> _myServices;

        public MyServicesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "My services";
            LoadMyServicesAsync();
        }

        public List<ServiceResponse> MyServices
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

        private async void LoadMyServicesAsync()
        {
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.ConnectionError", "Languages.Accept");
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
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", response.Message, "Languages.Accept");
                return;
            }
            MyServices = (List<ServiceResponse>)response.Result;           
            
        }

    }
}
