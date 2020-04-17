using AppServices.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppServices.Web.Models
{
    public class ServiceViewModel : ServiceEntity
    {
        [Display(Name = "Photo")]
        public IFormFile PhotoFile { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a service.")]
        public int ServiceTypeId { get; set; }

        public IEnumerable<SelectListItem> ServicesType { get; set; }
    }
}
