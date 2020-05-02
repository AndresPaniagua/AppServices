using Prism;
using Prism.Ioc;
using AppServices.Prism.ViewModels;
using AppServices.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppServices.Common.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppServices.Prism
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("/AppServicesMasterDetailPage/NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RecoverPasswordPage, RecoverPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateServicePage, CreateServicePageViewModel>();
            containerRegistry.RegisterForNavigation<AppServicesMasterDetailPage, AppServicesMasterDetailPageViewModel>();
        }
    }
}
