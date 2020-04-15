using AppServices.Common.Enums;
using AppServices.Web.Data.Entities;
using AppServices.Web.Helpers;
using System.Threading.Tasks;

namespace AppServices.Web.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDB(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            //

            await CheckUserAsync("1010", "Juan Zuluaga", "jzuluaga55@gmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.Admin);
            await CheckUserAsync("2020", "Andres Paniagua", "andresfelipep.l14@gmail.com", "304 636 5116", "Calle Luna Calle Sol", UserType.Admin);
            await CheckUserAsync("2030", "Andres Betancur", "verificacion32@gmail.com", "311 686 9854", "Calle Luna Calle Sol", UserType.Admin);
            await CheckUserAsync("3030", "Andres Lema", "andrespaniagua250958@gmail.com", "304 636 5116", "Calle Luna Calle Sol", UserType.User);
            await CheckUserAsync("4040", "Felipe Paniagua", "andrespaniagua250958@correo.itm.edu.co", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            await CheckUserAsync("5050", "Felipe Betancur", "andresbt10@hotmail.com", "345 784 6514", "Calle Luna Calle Sol", UserType.User);

        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<UserEntity> CheckUserAsync(
            string document,
            string fullName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            UserEntity user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    FullName = fullName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }


        private async Task CheckServicesAsync(string email)
        {
            //Id, ServicesName, Phone, startdate, finishDate, Description, price, photoPath, ServiceType, User
        }
    }
}
