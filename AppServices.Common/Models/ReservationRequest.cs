using System;

namespace AppServices.Common.Models
{
    public class ReservationRequest
    {
        public string Hour { get; set; }

        public DateTime Date { get; set; }

        public int IdService { get; set; }

        public Guid IdUser { get; set; }

        public string CultureInfo { get; set; }
    }
}
