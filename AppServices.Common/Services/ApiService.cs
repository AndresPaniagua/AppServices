using AppServices.Common.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppServices.Common.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string tokenType, string accessToken)
        {
            try
            {
                string request = JsonConvert.SerializeObject(changePasswordRequest);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();
                Response obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<bool> CheckConnectionAsync(string url)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            return await CrossConnectivity.Current.IsRemoteReachable(url);
        }

        public async Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                List<T> list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request)
        {
            try
            {
                string requestString = JsonConvert.SerializeObject(request);
                StringContent content = new StringContent(requestString, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = token
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, EmailRequest request)
        {
            try
            {
                string requestString = JsonConvert.SerializeObject(request);
                StringContent content = new StringContent(requestString, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = userResponse
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PutAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest)
        {
            try
            {
                string request = JsonConvert.SerializeObject(emailRequest);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();
                Response obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> RegisterServiceAsync(string urlBase, string servicePrefix, string controller, ServiceRequest service, string tokenType, string accessToken)
        {
            try
            {
                string request = JsonConvert.SerializeObject(service);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                List<ServiceResponse> list = JsonConvert.DeserializeObject<List<ServiceResponse>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest)
        {
            try
            {
                string request = JsonConvert.SerializeObject(userRequest);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }

                Response obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            ServicesForUserRequest model,
            string tokenType,
            string accessToken)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                List<T> list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> ReservationAsync(string urlBase, string servicePrefix, string controller, ReservationRequest reservation, string tokenType, string accessToken)
        {
            try
            {
                string request = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

               
                return new Response
                {
                    IsSuccess = true,
                    Result = "Reserva Hecha"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
