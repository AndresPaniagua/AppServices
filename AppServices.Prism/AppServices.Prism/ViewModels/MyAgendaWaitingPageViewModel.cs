using AppServices.Common.Helpers;
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
    public class MyAgendaWaitingPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static MyAgendaWaitingPageViewModel _instance;
        private List<ServiceResponse> _services;
        private List<ReservationItemViewModel> _reservations;
        private bool _isRunning;
        private bool _isEmpty;

        public MyAgendaWaitingPageViewModel(INavigationService navigationService,
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = Languages.Waiting;
            LoadServicesAsync();
        }

        public List<ServiceResponse> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public List<ReservationItemViewModel> Reservations
        {
            get => _reservations;
            set => SetProperty(ref _reservations, value);
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

        public static MyAgendaWaitingPageViewModel GetInstance()
        {
            return _instance;
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
            aux = aux.OrderByDescending(r => r.DiaryDate.Date).Where(r => r.Status.Name != "Active").ToList();
            await ChangeReservations(aux);
        }

        private async Task ChangeReservations(List<ReservationResponse> list)
        {
            List<ReservationItemViewModel> aux = new List<ReservationItemViewModel>();
            foreach (ReservationResponse item in list)
            {
                aux.Add(new ReservationItemViewModel
                {
                    Id = item.Id,
                    DiaryDate = item.DiaryDate,
                    User = item.User,
                    Status = item.Status
                });
            }

            Reservations = aux;
            IsEmpty = Reservations.Count <= 0;
        }

        public async Task AcceptReserveAsync(ReservationModel request)
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

            Response response = await _apiService.AceptedReservation(
                url,
                "/api",
                "/Reservations/AceptedReservation",
                request,
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                IsRunning = false;
                return;
            }

            LoadServicesAsync();
            await App.Current.MainPage.DisplayAlert(Languages.Ok, response.Message, Languages.Accept);
            IsRunning = false;
        }

    }
}
