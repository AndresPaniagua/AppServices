using AppServices.Common.Constants;
using AppServices.Common.Models;
using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Helpers;
using AppServices.Web.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public ReservationsController(DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> PostReservation([FromBody] ReservationRequest request)
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

            ServiceEntity service = await _context.Services
                .Include(r => r.Reservations)
                .ThenInclude(d => d.DiaryDate)
                .ThenInclude(h => h.Hours)
                .FirstOrDefaultAsync(st => st.Id == request.IdService);

            if (service == null)
            {
                return BadRequest(Resource.ServiceDoesntExists);
            }


            ReservationEntity result = service.Reservations.ToList().FirstOrDefault(dd => dd.DiaryDate.Date == request.Date);

            if (result != null)
            {
                DiaryHoursEntity result2 = result.DiaryDate.Hours.ToList().FirstOrDefault(dd => dd.Hour == request.Hour);

                if (result2 != null)
                {
                    return BadRequest(Resource.ReservationExists);
                }

                result.DiaryDate.Hours.Add(new DiaryHoursEntity
                {
                    Hour = request.Hour
                });

                _context.Reservations.Add(new ReservationEntity
                {
                    DiaryDate = result.DiaryDate,
                    User = userEntity,
                    Service = service,
                    Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Waiting")
                });
            }
            else
            {

                _context.Reservations.Add(new ReservationEntity
                {
                    DiaryDate = new DiaryDateEntity
                    {
                        Date = request.Date,
                        Hours = new List<DiaryHoursEntity>
                        {
                            new DiaryHoursEntity
                            {
                                Hour = request.Hour
                            }
                        }
                    },
                    User = userEntity,
                    Service = service,
                    Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Waiting")
                });

            }
            await SendNotificationAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetReservationsForUser")]
        public async Task<IActionResult> GetReservationsForUser([FromBody] ServicesForUserRequest request)
        {
            List<ReservationEntity> reservations = await _context.Reservations
              .Include(u => u.User)
              .Include(s => s.Service)
              .Include(dd => dd.DiaryDate)
              .ThenInclude(dh => dh.Hours)
              .Include(r => r.Status)
              .Where(u => u.User.Id.ToString() == request.UserId.ToString())
              .ToListAsync();

            return Ok(_converterHelper.ToReservationsForUserResponse(reservations));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("AceptedReservation")]
        public async Task<IActionResult> AceptedReservation([FromBody] ReservationModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            ReservationEntity result = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == request.IdReservation);

            if (result != null)
            {
                result.Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Active");
                await _context.SaveChangesAsync();
                return Ok(Resource.ReservationStatus);
            }
            return BadRequest(Resource.ReservationDoesnExists);
        }

        private async Task SendNotificationAsync()
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(
                AppConstants.ListenConnectionString,
                AppConstants.NotificationHubName);

            Dictionary<string, string> templateParameters = new Dictionary<string, string>();

            foreach (string tag in AppConstants.SubscriptionTags)
            {
                templateParameters["messageParam"] = "Someone wants to book your services";
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
