using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Helpers;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppServices.Prism.ViewModels
{
    public class ServicesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static ServicesPageViewModel _instance;
        private DelegateCommand _searchCommand;
        private DelegateCommand _filterCommand;
        private ObservableCollection<ServiceItemViewModel> _services;
        private ObservableCollection<ServiceTypeModel> _typeServices;
        private List<ServiceItemViewModel> _myServices;
        private bool _isRunning;
        private string _search;
        private bool _isNotEnable;
        private bool _notFound;

        public ServicesPageViewModel(INavigationService navigationService,
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = Languages.Services;
            LoadTypesAsync();
            LoadServicesAsync();
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowServices));

        public DelegateCommand FilterCommand => _filterCommand ?? (_filterCommand = new DelegateCommand(FilterServices));

        public ObservableCollection<ServiceItemViewModel> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public ObservableCollection<ServiceTypeModel> TypeServices
        {
            get => _typeServices;
            set => SetProperty(ref _typeServices, value);
        }

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

        public bool IsNotEnable
        {
            get => _isNotEnable;
            set => SetProperty(ref _isNotEnable, value);
        }

        public bool NotFound
        {
            get => _notFound;
            set => SetProperty(ref _notFound, value);
        }

        public static ServicesPageViewModel GetInstance()
        {
            return _instance;
        }

        public void AddMessage(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (message.StartsWith("Someone wants"))
                {
                    message = $"{message.Substring(7)}, {Languages.CheckAgenda}";
                }
                App.Current.MainPage.DisplayAlert(Languages.Notification, message, Languages.Accept);
            });
        }

        private async void LoadServicesAsync()
        {
            IsRunning = true;
            IsNotEnable = false;
            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            Response response = await _apiService.GetListAsync<ServiceResponse>(
                url,
                "/api",
                "/Service");

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }
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
                Status = a.Status,
                ServiceType = a.ServiceType,
                User = a.User
            }).Where(s => s?.Status?.Name == "Active").OrderBy(s => s.ServiceType.Name).ToList();

            IsRunning = false;
            IsNotEnable = true;

            ShowServices();
        }

        private async void LoadTypesAsync()
        {
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            Response response = await _apiService.GetListAsync<ServiceTypeModel>(url, "/api", "/ServiceType");

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            List<ServiceTypeModel> list = (List<ServiceTypeModel>)response.Result;
            foreach (ServiceTypeModel item in list)
            {
                item.IsCheck = true;
            }
            TypeServices = new ObservableCollection<ServiceTypeModel>(list.OrderBy(t => t.Name));
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
            NotFound = Services.Count < 1;
        }

        private void FilterServices()
        {
            IsRunning = true;
            IsNotEnable = false;
            List<ServiceItemViewModel> aux = new List<ServiceItemViewModel>();
            foreach (ServiceTypeModel type in TypeServices)
            {
                if (type.IsCheck)
                {
                    aux = aux.Concat(_myServices.Where(p => p.ServiceType.Name.ToUpper().Contains(type.Name.ToUpper()) ||
                                                        p.ServiceType.Name.ToUpper().Contains(type.Name.ToUpper()))).ToList();
                }
            }
            Services = new ObservableCollection<ServiceItemViewModel>(aux);
            IsRunning = false;
            IsNotEnable = true;
            NotFound = Services.Count < 1;
        }

    }
}
