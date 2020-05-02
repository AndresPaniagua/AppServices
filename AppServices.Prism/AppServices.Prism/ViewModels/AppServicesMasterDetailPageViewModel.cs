using AppServices.Common.Helpers;
using AppServices.Common.Models;
using AppServices.Common.Services;
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

        public AppServicesMasterDetailPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "plane",
                    PageName = "CreateServicePage",
                    Title = "Create Service"
                },
                new Menu
                {
                    Icon = "addtravel",
                    PageName = "CreateServicePage",
                    Title = "Services"
                },
                new Menu
                {
                    Icon = "otro",
                    PageName = "CreateServicePage",
                    Title = "My services"
                },
                new Menu
                {
                    Icon = "exit",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? "Logout" : "Login"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}
