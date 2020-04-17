using AppServices.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly DataContext _context;

        public ServiceController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(_context.Services
                .Include(s => s.User)
                .Include(s => s.ServiceType)
                .OrderBy(s => s.StartDate)
                .ThenBy(s => s.Price));
        }
    }
}
