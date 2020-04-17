using AppServices.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace AppServices.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboTypes()
        {
            List<SelectListItem> list = _context.ServiceTypes.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = $"{s.Id}"
            }).OrderBy(s => s.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a type]",
                Value = "0"
            });

            return list;
        }
    }
}
