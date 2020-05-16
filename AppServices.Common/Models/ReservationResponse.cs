namespace AppServices.Common.Models
{
    public class ReservationResponse
    {
        public int Id { get; set; }

        public DiaryDateResponse DiaryDate { get; set; }

        public UserResponse User { get; set; }
    }
}
