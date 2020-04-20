using AppServices.Web.Data.Entities;
using System;

namespace AppServices.Web.Models
{
    public class ReservationViewModel : ReservationEntity
    {
        public Guid UserId { get; set; }

        public int ServiceId { get; set; }
    }
}
