using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers
{
    public class UserServiceController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public UserServiceController(DataContext dataContext, IUserHelper userHelper)
        {
            _context = dataContext;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(_context.Services
                .Include(s => s.User)
                .Include(s => s.ServiceType)
                .Where(s => s.User.Email == User.Identity.Name)
                .OrderBy(s => s.StartDate)
                .ThenBy(s => s.Price));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceEntity model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);

                ServiceEntity travel = new ServiceEntity
                {
                    ServicesName = model.ServicesName,
                    ServiceType = model.ServiceType
                };

                _context.Add(travel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceEntity service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            ReservationEntity[] reservations = await _context.Reservations.Where(t => t.Service.Id == id).ToArrayAsync();
            _context.Reservations.RemoveRange(reservations);

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }

}
