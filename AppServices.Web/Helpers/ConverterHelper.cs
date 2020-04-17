using AppServices.Web.Data;
using AppServices.Web.Data.Entities;
using AppServices.Web.Models;
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

        public async Task<ServiceEntity> ToServiceEntityAsync (ServiceViewModel service, bool isNew)
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
    }
}
