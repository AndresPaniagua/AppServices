namespace AppServices.Common.Models
{
    public class ReservationsForUserResponse
    {
        public int Id { get; set; }

        public UserResponse User { get; set; }

        public ServiceResponse Service { get; set; }

        public DiaryDateResponse DiaryDate { get; set; }

        public StatusResponse Status { get; set; }

    }
}
