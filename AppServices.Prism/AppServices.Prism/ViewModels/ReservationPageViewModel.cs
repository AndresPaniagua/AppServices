using AppServices.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace AppServices.Prism.ViewModels
{
    public class ReservationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ServiceResponse _service;
        private DelegateCommand _reservedCommand;
        private DateTime _today;
        private List<string> _hours;
        private bool _isRunning;
        private string _hour;

        public ReservationPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Reservation";
            Today = DateTime.Now;
            LoadHourList();
        }

        public DelegateCommand ReservedCommand => _reservedCommand ?? (_reservedCommand = new DelegateCommand(ReservedAsync));

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

        private void ReservedAsync()
        {
            /**/
        }

    }
}
