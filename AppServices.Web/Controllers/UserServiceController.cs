using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Helpers;
using AppServices.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers
{
    public class UserServiceController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converter;

        public UserServiceController(DataContext dataContext,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IImageHelper imageHelper,
            IConverterHelper converter)
        {
            _context = dataContext;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _imageHelper = imageHelper;
            _converter = converter;
        }

        public async Task<IActionResult> Index()
        {
            return View(_context.Services
                .Include(s => s.User)
                .Include(s => s.ServiceType)
                .Include(s => s.Status)
                .Where(s => s.User.Email == User.Identity.Name)
                .OrderBy(s => s.StartDate)
                .ThenBy(s => s.Price));
        }

        public IActionResult Create()
        {
            ServiceViewModel service = new ServiceViewModel
            {
                ServicesType = _combosHelper.GetComboTypes()
            };
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                if (model.PhotoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PhotoFile, "Services");
                }

                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                model.User = user;
                model.PhotoPath = path;

                ServiceEntity service = await _converter.ToServiceEntityAsync(model, true);
                service.Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Active");

                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.ServicesType = _combosHelper.GetComboTypes();
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceEntity serviceEntity = await _context.Services.Include(s => s.ServiceType)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (serviceEntity == null)
            {
                return NotFound();
            }

            return View(_converter.ToServiceViewModel(serviceEntity));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = model.PhotoPath;
                if (model.PhotoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PhotoFile, "Services");
                }

                UserEntity user = await _userHelper.GetUserAsync(User.Identity.Name);
                model.User = user;
                model.PhotoPath = path;

                ServiceEntity service = await _converter.ToServiceEntityAsync(model, false);

                _context.Update(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.ServicesType = _combosHelper.GetComboTypes();
            return View(model);
        }

    }
}
