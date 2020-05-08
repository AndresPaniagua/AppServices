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
            SyncfusionLicenseProvider.RegisterLicense("MjM0NDgzQDMxMzgyZTMxMmUzME5HWDhDY1h2OTJGUjZaOEN5Q0VIVlVKV1h1b1NFYW5hZFdIMlhtQzVuQlE9");

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
        }
    }
}
