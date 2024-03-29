﻿using Prism;
using Prism.Ioc;
using AppServices.Prism.ViewModels;
using AppServices.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppServices.Common.Services;
using Syncfusion.Licensing;
using AppServices.Common.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppServices.Prism
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MjYyNDA5QDMxMzgyZTMxMmUzMERDZFNlWXIrVVJTYzRBUDBnK3dGSWpHaldwQkdXamVBeG41TTVHeVo2Q2c9");

            InitializeComponent();

            await NavigationService.NavigateAsync("/AppServicesMasterDetailPage/NavigationPage/ServicesPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateServicePage, CreateServicePageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<AppServicesMasterDetailPage, AppServicesMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateServicePage, CreateServicePageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<RememberPasswordPage, RememberPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ServicesPage, ServicesPageViewModel>();
            containerRegistry.RegisterForNavigation<ServiceDetailsPage, ServiceDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<MyServicesPage, MyServicesPageViewModel>();
            containerRegistry.RegisterForNavigation<ReservationPage, ReservationPageViewModel>();
            containerRegistry.RegisterForNavigation<MyReservationsPage, MyReservationsPageViewModel>();
            containerRegistry.RegisterForNavigation<MyAgendaPage, MyAgendaPageViewModel>();
            containerRegistry.RegisterForNavigation<EditServicePage, EditServicePageViewModel>();
            containerRegistry.RegisterForNavigation<MyReservationsWaitingPage, MyReservationsWaitingPageViewModel>();
            containerRegistry.RegisterForNavigation<MyAgendaWaitingPage, MyAgendaWaitingPageViewModel>();
            containerRegistry.RegisterForNavigation<MyAgendaTabbedPage, MyAgendaTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<MyReservationsTabbedPage, MyReservationsTabbedPageViewModel>();
        }
    }
}
