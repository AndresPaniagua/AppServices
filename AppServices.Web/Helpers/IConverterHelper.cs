using AppServices.Common.Models;
using AppServices.Web.Data.Entities;
using AppServices.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<ServiceEntity> ToServiceEntityAsync(ServiceViewModel service, bool isNew);

        ServiceViewModel ToServiceViewModel(ServiceEntity service);

        List<ServiceResponse> ToServiceResponse(List<ServiceEntity> service);

        ServiceResponse ToServiceResponse(ServiceEntity service);

        ServiceTypeResponse ToServiceTypeResponse(ServiceTypeEntity service);

        UserResponse ToUserResponse(UserEntity user);

        List<ReservationResponse> ToReservationResponse(List<ReservationEntity> reservations);

        List<ReservationsForUserResponse> ToReservationsForUserResponse(List<ReservationEntity> reservations);

        StatusEntity ToStatusEntity(StatusResponse status);

    }
}
