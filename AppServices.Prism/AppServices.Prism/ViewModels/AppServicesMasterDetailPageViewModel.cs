using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppServices.Prism.ViewModels
{
    public class AppServicesMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private static AppServicesMasterDetailPageViewModel _instance;

        public AppServicesMasterDetailPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        public static AppServicesMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        private async void ModifyUserAsync()
        {
            await _navigationService.NavigateAsync($"/AppServicesMasterDetailPage/NavigationPage/{nameof(ModifyUserPage)}");
        }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "newservice",
                    PageName = "CreateServicePage",
                    Title = "Create Service",
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "services",
                    PageName = "CreateServicePage",
                    Title = "Services"
                },
                new Menu
                {
                    Icon = "myservice",
                    PageName = "CreateServicePage",
                    Title = "My services" 
                },
                 new Menu
                {
                    Icon = "edit",
                    PageName = "ModifyUserPage",
                    Title = "Modify user",
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "login",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? "Logout" : "Login"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                    IsLoginRequired = m.IsLoginRequired
                }).ToList());
        }

        public async void ReloadUser()
        {
            string url = App.Current.Resources["UrlAPI"].ToString();

            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            EmailRequest emailRequest = new EmailRequest
            {
                CultureInfo = "en",
                Email = user.Email
            };

            Response response = await _apiService.GetUserByEmail(url, "api", "/Account/GetUserByEmail", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response.Result;
            Settings.User = JsonConvert.SerializeObject(userResponse);
            LoadUser();
        }
    }
}
