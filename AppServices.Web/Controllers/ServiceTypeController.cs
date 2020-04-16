using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppServices.Web.Data;
using AppServices.Web.Data.Entities;

namespace AppServices.Web.Controllers
{
    public class ServiceTypeController : Controller
    {
        private readonly DataContext _context;

        public ServiceTypeController(DataContext context)
        {
            _context = context;
        }

        // GET: ServiceType
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceTypes.ToListAsync());
        }

        // GET: ServiceType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceTypeEntity = await _context.ServiceTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceTypeEntity == null)
            {
                return NotFound();
            }

            return View(serviceTypeEntity);
        }

        // GET: ServiceType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ServiceTypeEntity serviceTypeEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceTypeEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceTypeEntity);
        }

        // GET: ServiceType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceTypeEntity = await _context.ServiceTypes.FindAsync(id);
            if (serviceTypeEntity == null)
            {
                return NotFound();
            }
            return View(serviceTypeEntity);
        }

        // POST: ServiceType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: ServiceType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceTypeEntity = await _context.ServiceTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceTypeEntity == null)
            {
                return NotFound();
            }

            return View(serviceTypeEntity);
        }

        // POST: ServiceType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceTypeEntity = await _context.ServiceTypes.FindAsync(id);
            _context.ServiceTypes.Remove(serviceTypeEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceTypeEntityExists(int id)
        {
            return _context.ServiceTypes.Any(e => e.Id == id);
        }
    }
}
