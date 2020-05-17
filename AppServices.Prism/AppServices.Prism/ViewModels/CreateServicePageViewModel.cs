using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Helpers;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppServices.Prism.ViewModels
{
    public class CreateServicePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private DelegateCommand _registerCommand;
        private DelegateCommand _modifyImageCommand;
        private bool _isEnabled;
        private bool _isRunning;
        private ImageSource _image;
        private MediaFile _file;
        private ServiceTypeResponse _serviceType;
        private ObservableCollection<ServiceTypeResponse> _serviceTypes;
        private ServiceRequest _service;
        private DateTime _today;

        public CreateServicePageViewModel(INavigationService navigationService, IApiService apiService, IFilesHelper filesHelper)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Title = Languages.CreateService;
            Image = "Silueta.png";
            Service = new ServiceRequest();
            IsEnabled = true;
            Today = DateTime.Today;
            LoadServiceType();
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public DelegateCommand ModifyImageCommand => _modifyImageCommand ?? (_modifyImageCommand = new DelegateCommand(ChangeImageAsync));

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

        public ServiceTypeResponse ServiceType
        {
            get => _serviceType;
            set => SetProperty(ref _serviceType, value);
        }

        public ObservableCollection<ServiceTypeResponse> ServiceTypes
        {
            get => _serviceTypes;
            set => SetProperty(ref _serviceTypes, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public ServiceRequest Service
        {
            get => _service;
            set => SetProperty(ref _service, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void RegisterAsync()
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

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }

            Service.PhotoArray = imageArray;

            Service.CultureInfo = "en";
            Service.IdType = ServiceType.Id;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            Service.IdUser = Guid.Parse(user.Id);

            Response response = await _apiService.RegisterServiceAsync(url, "/api", "/Service", Service, "bearer", token.Token);
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

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.PictureSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.FromCamera);

            if (source == Languages.Cancel)
            {
                _file = null;
                return;
            }

            if (source == Languages.FromCamera)
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small
                });
            }

            if (_file != null)
            {
                Image = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Service.ServicesName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ServicesNameError, Languages.Accept);
                return false;
            }

            if (string.IsNullOrEmpty(Service.Phone))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PhoneError, Languages.Accept);
                return false;
            }


            if (string.IsNullOrEmpty(Service.Description))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DescriptionError, Languages.Accept);
                return false;
            }


            if (ServiceType == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ServiceTypeError, Languages.Accept);
                return false;
            }

            return true;
        }

        private async void LoadServiceType()
        {
            IsRunning = true;
            IsEnabled = false;
            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }



            Response response = await _apiService.GetListAsync<ServiceTypeResponse>(url, "/api", "/ServiceType");
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            List<ServiceTypeResponse> list = (List<ServiceTypeResponse>)response.Result;
            ServiceTypes = new ObservableCollection<ServiceTypeResponse>(list.OrderBy(t => t.Name));
        }

    }
}
