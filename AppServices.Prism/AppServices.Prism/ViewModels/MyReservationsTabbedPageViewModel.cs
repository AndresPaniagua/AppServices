using AppServices.Prism.Helpers;
using Prism.Navigation;

namespace AppServices.Prism.ViewModels
{
    public class MyReservationsTabbedPageViewModel : ViewModelBase
    {
        public MyReservationsTabbedPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Languages.MyReservations;
        }
    }
}
