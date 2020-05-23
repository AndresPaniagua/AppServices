using AppServices.Common.Models;
using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServices.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combos;

        public ConverterHelper(DataContext context,
            ICombosHelper combos)
        {
            _context = context;
            _combos = combos;
        }

        public async Task<ServiceEntity> ToServiceEntityAsync(ServiceViewModel service, bool isNew)
        {
            ServiceEntity serviceEntity = new ServiceEntity
            {
                Id = isNew ? 0 : service.Id,
                ServicesName = service.ServicesName,
                StartDate = service.StartDate,
                FinishDate = service.FinishDate,
                Description = service.Description,
                Price = service.Price,
                PhotoPath = service.PhotoPath,
                Phone = service.Phone,
                ServiceType = await _context.ServiceTypes.FindAsync(service.ServiceTypeId),
                User = service.User
            };
            return serviceEntity;
        }
    
        public ServiceViewModel ToServiceViewModel(ServiceEntity service)
        {
            return new ServiceViewModel
            {
                ServicesName = service.ServicesName,
                Phone = service.Phone,
                Description = service.Description,
                PhotoPath = service.PhotoPath,
                StartDate = service.StartDate,
                FinishDate = service.FinishDate,
                Price = service.Price,
                ServicesType = _combos.GetComboTypes(),
                User = service.User,
                ServiceTypeId = service.ServiceType.Id
            };
        }

        public List<ServiceResponse> ToServiceResponse(List<ServiceEntity> serviceEntities)
        {
            List<ServiceResponse> list = new List<ServiceResponse>();
            foreach (ServiceEntity serviceEntity in serviceEntities)
            {
                list.Add(ToServiceResponse(serviceEntity));
            }

            return list;
        }

        public ServiceResponse ToServiceResponse(ServiceEntity serviceEntity)
        {
            return new ServiceResponse
            {
                Id = serviceEntity.Id,
                ServicesName = serviceEntity.ServicesName,
                Phone = serviceEntity.Phone,
                Description = serviceEntity.Description,
                Price = serviceEntity.Price,
                FinishDate = serviceEntity.FinishDate,
                StartDate = serviceEntity.StartDate,
                PhotoPath = serviceEntity.PhotoPath,
                ServiceType = ToServiceTypeResponse(serviceEntity.ServiceType),
                User = ToUserResponse(serviceEntity.User),
                Reservations = serviceEntity.Reservations?.Select(h => ToReservationResponse(h)).ToList(),
                Status = ToStatusResponse(serviceEntity.Status)
            };
        }

        public List<ReservationResponse> ToReservationResponse(List<ReservationEntity> reservationEntities)
        {
            List<ReservationResponse> list = new List<ReservationResponse>();
            foreach (ReservationEntity reservationEntity in reservationEntities)
            {
                list.Add(ToReservationResponse(reservationEntity));
            }

            return list;
        }

        public ReservationResponse ToReservationResponse(ReservationEntity reservation)
        {
            return new ReservationResponse
            {
                Id = reservation.Id,
                DiaryDate = ToDiaryDateResponse(reservation.DiaryDate),
                User = ToUserResponse(reservation.User)
            };
        }

        public StatusResponse ToStatusResponse(StatusEntity status)
        {
            if (status != null)
            {
                return new StatusResponse
                {
                    Id = status.Id,
                    Name = status.Name
                };
            }
            return null;
        }

        public DiaryDateResponse ToDiaryDateResponse(DiaryDateEntity diaryDate)
        {
            return new DiaryDateResponse
            {
                Id = diaryDate.Id,
                Date = diaryDate.Date,
                Hours = diaryDate.Hours?.Select(h => new DiaryHoursResponse
                {
                    Id = h.Id,
                    Hour = h.Hour
                }).ToList()
            };
        }
        
        public ServiceTypeResponse ToServiceTypeResponse(ServiceTypeEntity service)
        {
            if (service == null)
            {
                return null;
            }

            return new ServiceTypeResponse
            {
                Id = service.Id,
                Name = service.Name
            };
        }

        public UserResponse ToUserResponse(UserEntity user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Document = user.Document,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserType = user.UserType,
                Address = user.Address,
                LoginType = user.LoginType
            };
        }

        public List<ReservationsForUserResponse> ToReservationsForUserResponse(List<ReservationEntity> reservationEntities)
        {
            List<ReservationsForUserResponse> list = new List<ReservationsForUserResponse>();
            foreach (ReservationEntity reservationEntity in reservationEntities)
            {
                list.Add(ToReservationsForUserResponse(reservationEntity));
            }

            return list;
        }

        public ReservationsForUserResponse ToReservationsForUserResponse(ReservationEntity reservation)
        {
            return new ReservationsForUserResponse
            {
                Id = reservation.Id,
                DiaryDate = ToDiaryDateResponse(reservation.DiaryDate),
                User = ToUserResponse(reservation.User),
                Service = ToServiceForUserResponse(reservation.Service)
            };
        }

        public ServiceResponse ToServiceForUserResponse(ServiceEntity serviceEntity)
        {
            return new ServiceResponse
            {
                Id = serviceEntity.Id,
                ServicesName = serviceEntity.ServicesName,
                Phone = serviceEntity.Phone,
                Description = serviceEntity.Description,
                Price = serviceEntity.Price,
                FinishDate = serviceEntity.FinishDate,
                StartDate = serviceEntity.StartDate,
                PhotoPath = serviceEntity.PhotoPath,
                ServiceType = ToServiceTypeResponse(serviceEntity.ServiceType),
                User = ToUserResponse(serviceEntity.User),
                Status = ToStatusResponse(serviceEntity.Status)
            };
        }

        public StatusEntity ToStatusEntity(StatusResponse status)
        {
            if (status != null)
            {
                return new StatusEntity
                {
                    Id = status.Id,
                    Name = status.Name
                };
            }
            return null;
        }

    }
}
