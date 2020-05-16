using System;
using System.Collections.Generic;

namespace AppServices.Common.Models
{
    public class ServiceResponse
    {
        public int Id { get; set; }

        public string ServicesName { get; set; }

        public string Phone { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public DateTime FinishDate { get; set; }

        public DateTime FinishDateLocal => FinishDate.ToLocalTime();

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PhotoPath { get; set; }

        public string PhotoFullPath => string.IsNullOrEmpty(PhotoPath)
            ? "https://appservicesweb.azurewebsites.net//images/noimage.png"
            : $"https://appservicesweb.azurewebsites.net{PhotoPath.Substring(1)}";

        public ServiceTypeResponse ServiceType { get; set; }

        public UserResponse User { get; set; }

        public List<ReservationResponse> Reservations { get; set; }

        public StatusResponse Status { get; set; }
    }
}
