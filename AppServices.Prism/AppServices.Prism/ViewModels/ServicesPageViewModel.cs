﻿using AppServices.Common.Models;
using AppServices.Common.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace AppServices.Prism.ViewModels
{
    public class ServicesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _searchCommand;
        private ObservableCollection<ServiceItemViewModel> _services;
        private List<ServiceItemViewModel> _myServices;
        private bool _isRunning;
        private string _search;

        public ServicesPageViewModel(INavigationService navigationService,
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Services";
            LoadServicesAsync();
        }

        public ObservableCollection<ServiceItemViewModel> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowServices));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowServices();
            }
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
            _myServices = services.Select(a => new ServiceItemViewModel(_navigationService)
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
            ShowServices();
        }

        private void ShowServices()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Services = new ObservableCollection<ServiceItemViewModel>(_myServices);
            }
            else
            {
                Services = new ObservableCollection<ServiceItemViewModel>(
                    _myServices.Where(p => p.ServicesName.ToUpper().Contains(Search.ToUpper()) ||
                                            p.ServicesName.ToUpper().Contains(Search.ToUpper())));
            }
        }

    }
}