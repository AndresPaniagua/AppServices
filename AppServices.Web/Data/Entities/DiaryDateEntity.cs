using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppServices.Web.Data.Entities
{
    public class DiaryDateEntity
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }

        public ICollection<DiaryHoursEntity> Hours { get; set; }
    }
}
