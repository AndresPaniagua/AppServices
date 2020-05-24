using AppServices.Common.Enums;
using AppServices.Web.Data.Entities;
using AppServices.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            await CheckStatusAsync();
            await CheckServicesTypesAsync();

            await CheckUserAsync("1010", "Juan Zuluaga", "jzuluaga55@gmail.com", "350 634 2747", "Cl. 85 #95-46, Medellín, Antioquía, Colombia", UserType.Admin);
            await CheckUserAsync("2020", "Andres Paniagua", "andresfelipep.l14@gmail.com", "304 636 5116", "Cl. 85 #95-46, Medellín, Antioquía, Colombia", UserType.Admin);
            await CheckUserAsync("2030", "Andres Betancur", "verificacion32@gmail.com", "311 686 9854", "Cl. 85 #95-46, Medellín, Antioquía, Colombia", UserType.Admin);
            await CheckUserAsync("3030", "Andres Lema", "andrespaniagua250958@gmail.com", "304 636 5116", "Cl. 85 #95-46, Medellín, Antioquía, Colombia", UserType.User);
            await CheckUserAsync("4040", "Felipe Paniagua", "andrespaniagua250958@correo.itm.edu.co", "350 634 2747", "Cl. 85 #95-46, Medellín, Antioquía, Colombia", UserType.User);
            await CheckUserAsync("5050", "Felipe Betancur", "andresbt10@hotmail.com", "345 784 6514", "Cl. 85 #95-46, Medellín, Antioquía, Colombia", UserType.User);
            await CheckUserAsync("5050", "Andres Betancur", "andresBetancur250047@correo.itm.edu.co", "385 784 6514", "Cl. 85 #95-46, Medellín, Antioquía, Colombia", UserType.User);

            await CheckServicesPlumbingAsync("andresbt10@hotmail.com");
            await CheckServicesComputingAsync("andrespaniagua250958@gmail.com");
            await CheckReservationPlumbingAsync("andrespaniagua250958@correo.itm.edu.co", "andresbt10@hotmail.com");
            await CheckReservationComputingAsync("andresBetancur250047@correo.itm.edu.co", "andrespaniagua250958@gmail.com");
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckStatusAsync()
        {
            await _context.Statuses.AddAsync(new StatusEntity { Name = "Active" });
            await _context.Statuses.AddAsync(new StatusEntity { Name = "Inactive" });
            await _context.Statuses.AddAsync(new StatusEntity { Name = "Waiting" });

            await _context.SaveChangesAsync();
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
                    UserType = userType,
                    LoginType = LoginType.AppService
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }

        private async Task CheckServicesTypesAsync()
        {
            if (!_context.ServiceTypes.Any())
            {
                AddServiceType("Plumbing");
                AddServiceType("Computer maintenance");
                AddServiceType("Clothes designer");
                AddServiceType("Driver");
                AddServiceType("Building");
                AddServiceType("Health");
                await _context.SaveChangesAsync();
            }
        }

        private void AddServiceType(string name)
        {
            _context.ServiceTypes.Add(new ServiceTypeEntity
            {
                Name = name
            });
        }

        private async Task CheckServicesPlumbingAsync(string email)
        {
            ServiceEntity service = _context.Services.FirstOrDefault(s => s.ServicesName == "Plomeria");

            if (service == null)
            {
                //Id, ServicesName, Phone, startdate, finishDate, Description, price, photoPath, ServiceType, User
                DateTime startDate = DateTime.Today.AddDays(6).ToUniversalTime();
                DateTime finishDate = DateTime.Today.AddMonths(10).ToUniversalTime();

                _context.Services.Add(new ServiceEntity
                {
                    ServicesName = "Plomeria",
                    Phone = "3255486995",
                    StartDate = startDate,
                    FinishDate = finishDate,
                    Description = "Se hace plomeria donde seaaa",
                    Price = 36542,
                    PhotoPath = $"~/images/Services/Plomeria.jpg",
                    ServiceType = _context.ServiceTypes.FirstOrDefault(s => s.Name == "Plumbing"),
                    User = _context.Users.Where(u => u.Email == email).FirstOrDefault(),
                    Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Active")
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckServicesComputingAsync(string email)
        {

            ServiceEntity service = _context.Services.FirstOrDefault(s => s.ServicesName == "Mantenimiento de PCs");

            if (service == null)
            {
                //Id, ServicesName, Phone, startdate, finishDate, Description, price, photoPath, ServiceType, User
                DateTime startDate = DateTime.Today.AddDays(6).ToUniversalTime();
                DateTime finishDate = DateTime.Today.AddMonths(10).ToUniversalTime();

                _context.Services.Add(new ServiceEntity
                {
                    ServicesName = "Mantenimiento de PCs",
                    Phone = "643515485",
                    StartDate = startDate,
                    FinishDate = finishDate,
                    Description = "software and hardware",
                    Price = 36542,
                    PhotoPath = $"~/images/Services/Computadores.jpg",
                    ServiceType = _context.ServiceTypes.FirstOrDefault(s => s.Name == "Computer maintenance"),
                    User = _context.Users.Where(u => u.Email == email).FirstOrDefault(),
                    Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Active")
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckReservationPlumbingAsync(string email, string emailService)
        {

            ReservationEntity reservation = _context.Reservations.FirstOrDefault(r => r.User.Email == email);

            if (reservation == null)
            {
                DateTime reservationDate = DateTime.Today.AddDays(15).ToUniversalTime();

                _context.Reservations.Add(new ReservationEntity
                {
                    DiaryDate = new DiaryDateEntity
                    {
                        Date = reservationDate
                    },
                    User = _context.Users.Where(u => u.Email == email).FirstOrDefault(),
                    Service = _context.Services.Where(s => s.User.Email == emailService).FirstOrDefault(),
                    Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Active")
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckReservationComputingAsync(string email, string emailService)
        {
            ReservationEntity reservation = _context.Reservations.FirstOrDefault(r => r.User.Email == email);

            if (reservation == null)
            {
                DateTime reservationDate = DateTime.Today.AddDays(25).ToUniversalTime();

                _context.Reservations.Add(new ReservationEntity
                {
                    DiaryDate = new DiaryDateEntity
                    {
                        Date = reservationDate
                    },
                    User = _context.Users.Where(u => u.Email == email).FirstOrDefault(),
                    Service = _context.Services.Where(s => s.User.Email == emailService).FirstOrDefault(),
                    Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Waiting")
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
