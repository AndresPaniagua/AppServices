using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace AppServices.Prism.ViewModels
{
    public class MyReservationsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<ReservationsForUserResponse> _reservations;
        private List<ServiceResponse> _services;
        private bool _isRunning;
        private bool _isEmpty;

        public MyReservationsPageViewModel(INavigationService navigationService,
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.MyReservations;
            LoadReservationsAsync();
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value);
        }

        public List<ServiceResponse> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public List<ReservationsForUserResponse> Reservations
        {
            get => _reservations;
            set => SetProperty(ref _reservations, value);
        }

        private async void LoadReservationsAsync()
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
                CultureInfo = Languages.Culture
            };

            Response response = await _apiService.GetListAsync<ReservationsForUserResponse>(
                url,
                "/api",
                "/Reservations/GetReservationsForUser",
                servicesForUser,
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            Reservations = (List<ReservationsForUserResponse>)response.Result;
            IsRunning = false;
            IsEmpty = Reservations.Count <= 0;
        }
    }
}
