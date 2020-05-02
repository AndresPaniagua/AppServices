using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly DataContext _context;

        public ServiceTypeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ServiceTypeEntity> GetServiceTypes()
        {
            return _context.ServiceTypes;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceTypeEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServiceTypeEntity serviceTypeEntity = await _context.ServiceTypes.FindAsync(id);

            if (serviceTypeEntity == null)
            {
                return NotFound();
            }

            return Ok(serviceTypeEntity);
        }
    }
}