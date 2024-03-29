﻿using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
using AppServices.Prism.Helpers;
using AppServices.Prism.Views;
using Newtonsoft.Json;
using Prism.Navigation;
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

        public static AppServicesMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
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
                    Icon = "Services",
                    PageName = "ServicesPage",
                    Title = Languages.Services
                },
                new Menu
                {
                    Icon = "NewService",
                    PageName = "CreateServicePage",
                    Title = Languages.CreateService,
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "My_Reservations",
                    PageName = "MyReservationsTabbedPage",
                    Title = Languages.MyReservations,
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "MyServices",
                    PageName = "MyServicesPage",
                    Title = Languages.MyServices,
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "My_Agenda",
                    PageName = "MyAgendaTabbedPage",
                    Title = Languages.MyAgenda,
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "Edit",
                    PageName = "ModifyUserPage",
                    Title = Languages.ModifyUser,
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "Login",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? Languages.Logout : Languages.Login
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
