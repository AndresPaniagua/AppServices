using System;
using System.Collections.Generic;

namespace AppServices.Web.Data.Entities
{
    public class DiaryDateEntity
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public ICollection<DiaryHoursEntity> Hours { get; set; }
    }
}
