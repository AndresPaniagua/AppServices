using AppServices.Common.Models;
using System.Threading.Tasks;

namespace AppServices.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> RegisterServiceAsync(string urlBase, string servicePrefix, string controller, ServiceRequest service, string tokenType, string accessToken);

        Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, EmailRequest request);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);

        Task<bool> CheckConnectionAsync(string url);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);

        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);

        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string tokenType, string accessToken);
    
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller, ServicesForUserRequest model, string tokenType, string accessToken);

        Task<Response> ReservationAsync(string urlBase, string servicePrefix, string controller, ReservationRequest reservation, string tokenType, string accessToken);

    }
}
