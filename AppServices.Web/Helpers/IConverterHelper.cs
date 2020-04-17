using AppServices.Web.Data.Entities;
using AppServices.Web.Models;
using System.Threading.Tasks;

namespace AppServices.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<ServiceEntity> ToServiceEntityAsync(ServiceViewModel service, bool isNew);

        ServiceViewModel ToServiceViewModel(ServiceEntity service);
    }
}
