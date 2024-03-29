﻿using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AppServices.Prism.ViewModels
{
    public class MyAgendaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static MyAgendaPageViewModel _instance;
        private List<ServiceResponse> _services;
        private List<ReservationResponse> _reservations;
        private bool _isRunning;
        private bool _isEmpty;

        public MyAgendaPageViewModel(INavigationService navigationService,
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = Languages.Acepted;
            LoadServicesAsync();
        }

        public List<ServiceResponse> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public List<ReservationResponse> Reservations
        {
            get => _reservations;
            set => SetProperty(ref _reservations, value);
        }

        public static MyAgendaPageViewModel GetInstance()
        {
            return _instance;
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

        private async void LoadServicesAsync()
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
                return;
            }

            Services = (List<ServiceResponse>)response.Result;
            LoadReservations();
            IsRunning = false;
        }

        private async void LoadReservations()
        {
            List<ReservationResponse> aux = new List<ReservationResponse>();
            foreach (ServiceResponse service in Services)
            {
                aux.AddRange(service.Reservations);
            }
            aux = aux.OrderByDescending(r => r.DiaryDate.Date).Where(r => r.Status.Name == "Active").ToList();
            Reservations = aux;

            IsEmpty = Reservations.Count <= 0;
        }

        public async Task ReloadAgenda()
        {
            LoadServicesAsync();
        }

    }
}
