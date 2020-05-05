using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppServices.Prism.ViewModels
{
    public class RememberPasswordPageViewModel : ViewModelBase
    {
        public RememberPasswordPageViewModel(INavigationService navigationService):base(navigationService)
        {
            Title = "Recover Password";
        }
    }
}
