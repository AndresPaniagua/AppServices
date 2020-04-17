using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AppServices.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboTypes();
    }
}
