using Prism.Navigation;

namespace AppServices.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        public RegisterPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Register";
        }
    }
}
