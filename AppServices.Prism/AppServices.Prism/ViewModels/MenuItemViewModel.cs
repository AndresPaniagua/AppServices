﻿using AppServices.Common.Helpers;
using AppServices.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppServices.Prism.ViewModels 
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            if (PageName == "LoginPage" && Settings.IsLogin)
            {
                Settings.IsLogin = false;
                Settings.User = null;
                Settings.Token = null;
            }

            if (PageName == "LoginPage")
            {
                await _navigationService.NavigateAsync($"/AppServicesMasterDetailPage/NavigationPage/{PageName}");
                //await _navigationService.NavigateAsync($"{PageName}");
            }
            else
            {
                await _navigationService.NavigateAsync($"/AppServicesMasterDetailPage/NavigationPage/{PageName}");
            }
        }
    }
}
