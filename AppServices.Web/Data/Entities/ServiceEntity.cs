using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppServices.Web.Data.Entities
{
    public class ServiceEntity
    {
        public int Id { get; set; }

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
        public string PhotoPath { get; set; }

        [Display(Name = "Photo")]
        public string PhotoFullPath => string.IsNullOrEmpty(PhotoPath)
            ? "https://appservicesweb.azurewebsites.net//images/noimage.png"
            : $"https://appservicesweb.azurewebsites.net{PhotoPath.Substring(1)}";

        public ServiceTypeEntity ServiceType { get; set; }

        public UserEntity User { get; set; }

        public ICollection<DiaryDateEntity> DiaryDate { get; set; }

        public StatusEntity Status { get; set; }
    }
}
