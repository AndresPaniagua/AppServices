using AppServices.Common.Interfaces;
using AppServices.Prism.Resources;
using System.Globalization;
using Xamarin.Forms;

namespace AppServices.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string ChangePhotoNoAppServiceUser => Resource.ChangePhotoNoAppServiceUser;

        public static string LoginFacebook => Resource.LoginFacebook;

        public static string Culture { get; set; }

        public static string Error => Resource.Error;

        public static string Services => Resource.Services;

        public static string ConnectionError => Resource.ConnectionError;

        public static string Accept => Resource.Accept;

        public static string CreateService => Resource.CreateService;

        public static string MyReservations => Resource.MyReservations;

        public static string MyServices => Resource.MyServices;

        public static string MyAgenda => Resource.MyAgenda;

        public static string ModifyUser => Resource.ModifyUser;

        public static string Logout => Resource.Logout;

        public static string Login => Resource.Login;

        public static string Ok => Resource.Ok;

        public static string HourError => Resource.HourError;

        public static string EmailError => Resource.EmailError;

        public static string DocumentError => Resource.DocumentError;

        public static string FullNameError => Resource.FullNameError;

        public static string AddressError => Resource.AddressError;

        public static string PhoneError => Resource.PhoneError;

        public static string PasswordError => Resource.PasswordError;

        public static string PasswordConfirmError1 => Resource.PasswordConfirmError1;

        public static string PasswordConfirmError2 => Resource.PasswordConfirmError2;

        public static string UserUpdated => Resource.UserUpdated;

        public static string LoginError => Resource.LoginError;

        public static string PictureSource => Resource.PictureSource;

        public static string Cancel => Resource.Cancel;

        public static string FromGallery => Resource.FromGallery;

        public static string FromCamera => Resource.FromCamera;

        public static string ServicesNameError => Resource.ServicesNameError;

        public static string DescriptionError => Resource.DescriptionError;

        public static string ServiceTypeError => Resource.ServiceTypeError;

        public static string ChangePassword => Resource.ChangePassword;

        public static string CurrentPasswordError => Resource.CurrentPasswordError;

        public static string NewPasswordError => Resource.NewPasswordError;

        public static string ConfirmNewPasswordError => Resource.ConfirmNewPasswordError;

        public static string Details => Resource.Details;

        public static string Reservation => Resource.Reservation;

        public static string RecoverPassword => Resource.RecoverPassword;

        public static string Register => Resource.Register;

        public static string Loading => Resource.Loading;

        public static string ServicenamePlaceholder => Resource.ServicenamePlaceholder;

        public static string LoginSeeInfo => Resource.LoginSeeInfo;

        public static string ToLogin => Resource.ToLogin;

        public static string Reserve => Resource.Reserve;

        public static string Description => Resource.Description;

        public static string MapTitle => Resource.MapTitle;

        public static string Price => Resource.Price;

        public static string Information => Resource.Information;

        public static string Date => Resource.Date;

        public static string Hour => Resource.Hour;

        public static string Reserved => Resource.Reserved;

        public static string Email => Resource.Email;

        public static string Recover => Resource.Recover;

        public static string Document => Resource.Document;

        public static string Fullname => Resource.Fullname;

        public static string Address => Resource.Address;

        public static string Phone => Resource.Phone;

        public static string Password => Resource.Password;

        public static string ConfirmPassword => Resource.ConfirmPassword;

        public static string Save => Resource.Save;

        public static string ForgotPassword => Resource.ForgotPassword;

        public static string NewAccount => Resource.NewAccount;

        public static string StartDate => Resource.StartDate;

        public static string FinishDate => Resource.FinishDate;

        public static string TypeService => Resource.TypeService;

        public static string CurrentPassword => Resource.CurrentPassword;

        public static string NewPassword => Resource.NewPassword;

        public static string SelectHour => Resource.SelectHour;

    }
}
