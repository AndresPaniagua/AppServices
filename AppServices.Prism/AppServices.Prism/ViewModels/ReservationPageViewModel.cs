using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AppServices.Prism.ViewModels
{
    public class ReservationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly ReservationRequest _reservation;
        private ServiceResponse _service;
        private DelegateCommand _reservedCommand;
        private DateTime _today;
        private List<string> _hours;
        private bool _isRunning;
        private bool _isEnabled;
        private string _hour;
        private DateTime _date;

        public ReservationPageViewModel(INavigationService navigationService, IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Reservation;
            Today = DateTime.Today;
            _reservation = new ReservationRequest();
            LoadHourList();
        }

        public DelegateCommand ReservedCommand => _reservedCommand ?? (_reservedCommand = new DelegateCommand(ReservedAsync));

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public ServiceResponse Service
        {
            get => _service;
            set => SetProperty(ref _service, value);
        }

        public List<string> Hours
        {
            get => _hours;
            set => SetProperty(ref _hours, value);
        }

        public string Hour
        {
            get => _hour;
            set => SetProperty(ref _hour, value);
        }

        public DateTime Today
        {
            get => _today;
            set => SetProperty(ref _today, value);
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("service"))
            {
                Service = parameters.GetValue<ServiceResponse>("service");
                Title = Service.ServicesName;
            }
        }

        private void LoadHourList()
        {
            Hours = new List<string>
            {
                "8:00 - 9:00",
                "9:00 - 10:00",
                "10:00 - 11:00",
                "11:00 - 12:00",
                "12:00 - 13:00",
                "13:00 - 14:00",
                "14:00 - 15:00",
                "15:00 - 16:00",
                "16:00 - 17:00",
                "17:00 - 18:00",
                "18:00 - 19:00",
                "19:00 - 20:00"
            };
        }

        private async void ReservedAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;
            string url = App.Current.Resources["UrlAPI"].ToString();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            if (Date == DateTime.Parse("01/01/0001"))
            {
                Date = DateTime.Today;
            }

            _reservation.CultureInfo = "en";
            _reservation.IdService = Service.Id;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            _reservation.IdUser = Guid.Parse(user.Id);
            _reservation.Hour = Hour;
            _reservation.Date = Date;

            Response response = await _apiService.ReservationAsync(url, "/api", "/Reservations", _reservation, "bearer", token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, response.Message, Languages.Accept);
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Hour))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.HourError, Languages.Accept);
                return false;
            }

            return true;
        }
    }
}
