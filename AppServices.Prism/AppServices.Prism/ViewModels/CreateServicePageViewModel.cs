using Prism.Navigation;
using Xamarin.Forms;

namespace AppServices.Prism.ViewModels
{
    public class CreateServicePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ImageSource _image;

        public CreateServicePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Image = "Silueta.png";
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

    }
}
