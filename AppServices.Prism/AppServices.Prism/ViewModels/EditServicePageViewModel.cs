using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppServices.Prism.ViewModels
{
    public class EditServicePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ServiceResponse _service;
        private DelegateCommand _saveCommand;
        private DelegateCommand _modifyImageCommand;
        private bool _isEnabled;
        private bool _isRunning;
        private ImageSource _image;
        private MediaFile _file;
        private List<ServiceTypeResponse> _serviceTypes;
        private ServiceTypeResponse _serviceType;
        private DateTime _today;

        public EditServicePageViewModel(INavigationService navigationService,
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Edit";
            Image = "Silueta.png";
            IsEnabled = true;
            Today = DateTime.Today;
            LoadServiceType();
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ModifyImageCommand => _modifyImageCommand ?? (_modifyImageCommand = new DelegateCommand(ChangeImageAsync));

        public ServiceResponse Service
        {
            get => _service;
            set => SetProperty(ref _service, value);
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

        public ServiceTypeResponse ServiceType
        {
            get => _serviceType;
            set => SetProperty(ref _serviceType, value);
        }

        public List<ServiceTypeResponse> ServiceTypes
        {
            get => _serviceTypes;
            set => SetProperty(ref _serviceTypes, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
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
                Title = $"{Service.ServicesName}";
                Image = Service.PhotoFullPath;
            }

        }

        private void SaveAsync()
        {
            throw new NotImplementedException();
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

            ServiceTypes = (List<ServiceTypeResponse>)response.Result;
            ServiceType = ServiceTypes.FirstOrDefault(st => st.Id == Service.ServiceType.Id);
        }

    }
}
