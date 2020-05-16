using AppServices.Common.Models;
using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Helpers;
using AppServices.Web.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IImageHelper _imageHelper;

        public ServiceController(DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetServicesAsync()
        {
            List<ServiceEntity> services = await _context.Services
               .Include(t => t.User)
               .Include(s => s.ServiceType)
               .Include(r => r.Reservations)
               .ThenInclude(s => s.DiaryDate)
               .ThenInclude(dd => dd.Hours)
               .ToListAsync();

            return Ok(_converterHelper.ToServiceResponse(services));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetServicesForUser")]
        public async Task<IActionResult> GetServicesForUser([FromBody] ServicesForUserRequest request)
        {
            List<ServiceEntity> services = await _context.Services
              .Include(s => s.User)
              .Include(u => u.ServiceType)
              .Include(st => st.Reservations)
              .ThenInclude(r => r.User)
              .Include(st => st.Reservations)
              .ThenInclude(r => r.DiaryDate)
              .ThenInclude(dd => dd.Hours)
              .Where(u => u.User.Id == request.UserId.ToString())
              .ToListAsync();

            return Ok(_converterHelper.ToServiceResponse(services));

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> PostService([FromBody] ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            UserEntity userEntity = await _userHelper.GetUserAsync(request.IdUser);
            if (userEntity == null)
            {
                return BadRequest(Resource.UserDoesntExists);
            }

            ServiceTypeEntity serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(st => st.Id == request.IdType);
            if (serviceType == null)
            {
                return BadRequest(Resource.UserDoesntExists);
            }

            string picturePath = string.Empty;
            if (request.PhotoArray != null && request.PhotoArray.Length > 0)
            {
                picturePath = _imageHelper.UploadImage(request.PhotoArray, "Services");
            }

            ServiceEntity serviceEntity = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.IdService);

            if (serviceEntity == null)
            {
                serviceEntity = new ServiceEntity
                {
                    ServicesName = request.ServicesName,
                    Phone = request.Phone,
                    PhotoPath = picturePath,
                    StartDate = request.StartDate,
                    FinishDate = request.FinishDate,
                    Description = request.Description,
                    Price = request.Price,
                    User = userEntity,
                    ServiceType = serviceType
                };

                _context.Services.Add(serviceEntity);
            }
            else
            {
                if (string.IsNullOrEmpty(picturePath))
                {
                    picturePath = serviceEntity.PhotoPath;
                }

                serviceEntity.FinishDate = request.FinishDate;
                serviceEntity.Description = request.Description;
                serviceEntity.Price = request.Price;
                serviceEntity.PhotoPath = picturePath;
                serviceEntity.Phone = request.Phone;
                _context.Services.Update(serviceEntity);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
