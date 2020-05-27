using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly DataContext _context;

        public ReservationsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(_context.Reservations
              .Include(u => u.User)
              .Include(s => s.Service)
              .Include(dd => dd.DiaryDate)
              .ThenInclude(dh => dh.Hours)
              .Include(r => r.Status)
              .Where(u => u.User.Email == User.Identity.Name));
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceEntity service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            ReservationViewModel reservation = new ReservationViewModel
            {
                ServiceId = service.Id
            };

            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            ReservationEntity reservation = new ReservationEntity
            {
                User = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault(),
                Service = await _context.Services.FirstOrDefaultAsync(s => s.Id == model.ServiceId),
                DiaryDate = model.DiaryDate
            };

            _context.Reservations.Add(reservation);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Service");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReservationEntity reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
