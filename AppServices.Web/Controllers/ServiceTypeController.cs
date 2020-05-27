using AppServices.Common.Constants;
using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers
{
    public class ServiceTypeController : Controller
    {
        private readonly DataContext _context;

        public ServiceTypeController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceTypes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceTypeEntity serviceTypeEntity = await _context.ServiceTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceTypeEntity == null)
            {
                return NotFound();
            }

            return View(serviceTypeEntity);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ServiceTypeEntity serviceTypeEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceTypeEntity);
                await _context.SaveChangesAsync();
                await SendNotificationAsync(serviceTypeEntity);
                return RedirectToAction(nameof(Index));
            }
            return View(serviceTypeEntity);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceTypeEntity serviceTypeEntity = await _context.ServiceTypes.FindAsync(id);
            if (serviceTypeEntity == null)
            {
                return NotFound();
            }
            return View(serviceTypeEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ServiceTypeEntity serviceTypeEntity)
        {
            if (id != serviceTypeEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceTypeEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceTypeEntityExists(serviceTypeEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceTypeEntity);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceTypeEntity serviceTypeEntity = await _context.ServiceTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceTypeEntity == null)
            {
                return NotFound();
            }

            _context.ServiceTypes.Remove(serviceTypeEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceTypeEntityExists(int id)
        {
            return _context.ServiceTypes.Any(e => e.Id == id);
        }
        
        private async Task SendNotificationAsync(ServiceTypeEntity model)
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(
                AppConstants.ListenConnectionString,
                AppConstants.NotificationHubName);

            Dictionary<string, string> templateParameters = new Dictionary<string, string>();

            foreach (string tag in AppConstants.SubscriptionTags)
            {
                templateParameters["messageParam"] = $"New Type: A new type of service was created ({model.Name}), what are you waiting to publish yours!";
                try
                {
                    await hub.SendTemplateNotificationAsync(templateParameters, tag);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }

    }
}
