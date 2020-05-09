using System;
using System.Collections.Generic;

namespace AppServices.Common.Models
{
    public class DiaryDateResponse
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public List<DiaryHoursResponse> Hours { get; set; }
    }
}
