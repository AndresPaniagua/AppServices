using System;
using System.Collections.Generic;
using System.Text;

namespace AppServices.Common.Models
{
    public class ReservationResponse
    {
        public int Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public UserResponse User { get; set; }

        public ServiceResponse Service { get; set; }
    }
}
