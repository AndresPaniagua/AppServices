﻿namespace AppServices.Web.Data.Entities
{
    public class ReservationEntity
    {
        public int Id { get; set; }

        public DiaryDateEntity DiaryDate { get; set; }

        public UserEntity User { get; set; }

        public ServiceEntity Service { get; set; }

        public StatusEntity Status { get; set; }
    }
}
