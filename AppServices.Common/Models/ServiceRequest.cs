using System;
using System.ComponentModel.DataAnnotations;

namespace AppServices.Common.Models
{
    public class ServiceRequest
    {
        public int IdService { get; set; }

        public int IdType { get; set; }

        public Guid IdUser { get; set; }

        [MaxLength(30, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string ServicesName { get; set; }

        [MaxLength(11, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Phone { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartDateLocal => StartDate.ToLocalTime();

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime FinishDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime FinishDateLocal => FinishDate.ToLocalTime();

        [MaxLength(500, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Photo")]
        public byte[] PhotoArray { get; set; }

        public string CultureInfo { get; set; }
    }
}
