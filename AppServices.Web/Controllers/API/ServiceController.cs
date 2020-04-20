using AppServices.Common.Models;
using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public ServiceController(DataContext context, IUserHelper userHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }        

        [HttpGet]
        public async Task<IActionResult> GetServicesAsync()
        {
            List<ServiceEntity> services = await _context.Services
               .Include(t => t.User)
               .Include(s => s.ServiceType)
               .ToListAsync();

            return Ok(_converterHelper.ToServiceResponse(services));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetServicesForUser")]
        public async Task<IActionResult> GetServicesForUser([FromBody] ServicesForUserRequest request)
        {
            List<ServiceEntity> services = await _context.Services
              .Include(t => t.User)
              .Include(s => s.ServiceType)
              .Where(u => u.User.Id == request.UserId.ToString())
              .ToListAsync();

            return Ok(_converterHelper.ToServiceResponse(services));

        }

    }
}
