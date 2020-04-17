using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ServiceController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
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
