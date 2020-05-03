using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Essentials;

namespace AppServices.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _registerCommand;
        private DelegateCommand _loginCommand;
        private DelegateCommand _forgotPasswordCommand;
        private bool _isRunning;
        private bool _isEnabled;
        private string _password;

        public LoginPageViewModel(INavigationService navigationService, IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));
        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));
        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(RecoverPasswordAsync));

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

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void LoginAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "EmailError", "Accept");
                // Languages.Error,
                // Languages.EmailError,
                // Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "PasswordError", "Accept");
                /*Languages.Error,
                    Languages.PasswordError,
                    Languages.Accept);*/
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();

            //if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            //{
            //    IsRunning = true;
            //    IsEnabled = false;
            //    await App.Current.MainPage.DisplayAlert("Error", "ConnectionError", "Accept");
            //    //Languages.Error, Languages.ConnectionError, Languages.Accept);
            //    return;
            //}
            //MUCHACHOS ESTO NO ME DEJA AVANZAR.. ME SALE QUE DEBO TENER UNOS PERMISOS, PERO YA LOGUEA 
            TokenRequest request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            Response response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "LoginError", "Accept");
                //Languages.Error, Languages.LoginError, Languages.Accept);
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            EmailRequest request2 = new EmailRequest
            {
                CultureInfo = "es",
                Email = Email
            };

            Response response2 = await _apiService.GetUserByEmail(url, "api", "/Account/GetUserByEmail", "bearer", token.Token, request2);
            UserResponse userResponse = (UserResponse)response2.Result;

            Settings.User = JsonConvert.SerializeObject(userResponse);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync("/AppServicesMasterDetailPage/NavigationPage/CreateServicePage");
            Password = string.Empty;
        }
        private async void RecoverPasswordAsync()
        {
            await _navigationService.NavigateAsync(nameof(RecoverPasswordPage));
        }

        private async void RegisterAsync()
        {
            await _navigationService.NavigateAsync(nameof(RegisterPage));
        }

    }
}
