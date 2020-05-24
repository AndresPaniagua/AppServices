using AppServices.Prism.Helpers;
using Prism.Navigation;

namespace AppServices.Prism.ViewModels
{
    public class MyAgendaTabbedPageViewModel : ViewModelBase
    {
        public MyAgendaTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Languages.MyAgenda;
        }

    }
}
