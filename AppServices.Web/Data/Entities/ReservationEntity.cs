namespace AppServices.Web.Data.Entities
{
    public class ReservationEntity
    {
        public int Id { get; set; }

        public UserEntity User { get; set; }

        public ServiceEntity Service { get; set; }
    }
}
