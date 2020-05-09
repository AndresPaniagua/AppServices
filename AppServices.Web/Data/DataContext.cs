using AppServices.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppServices.Web.Data
{
    public class DataContext : IdentityDbContext<UserEntity>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
    
        public DbSet<ServiceEntity> Services { get; set; }

        public DbSet<ServiceTypeEntity> ServiceTypes { get; set; }

        public DbSet<ReservationEntity> Reservations { get; set; }

        public DbSet<StatusEntity> Statuses { get; set; }

        public DbSet<DiaryDateEntity> DiaryDates { get; set; }

        public DbSet<DiaryHoursEntity> DiaryHours { get; set; }
    }
}
