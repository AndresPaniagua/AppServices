namespace AppServices.Web.Data.Entities
{
    public class DiaryHoursEntity
    {
        public int Id { get; set; }

        public string Hour { get; set; }

        public DiaryDateEntity DiaryDate { get; set; }
    }
}
