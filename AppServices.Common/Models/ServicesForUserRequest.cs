using System;
using System.ComponentModel.DataAnnotations;

namespace AppServices.Common.Models
{
    public class ServicesForUserRequest
    {
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public Guid UserId { get; set; }

        [Required]
        public string CultureInfo { get; set; }
    }
}
