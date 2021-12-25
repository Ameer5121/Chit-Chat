using ChitChat.Helper.Extensions;
using ChitChat.Helper.Exceptions;
using ChitChat.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Models.Markers;

namespace ChitChat.Services
{
    class HttpService : IHttpService
    {
        private static readonly HttpService _httpService = new HttpService();
        private static readonly HttpClient _httpClient = new HttpClient();
        static HttpService()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.BaseAddress = new Uri("https://localhost:44358");
        }
        private HttpService() { }
        public static HttpService HttpServiceInstance
        {
            get => _httpService;            
        }

        public async Task<HttpResponseMessage> PostDataAsync(string endPoint, DataTransferObject dataTransferObject)
        {
            return await _httpClient.PostAsync($"/api/chat/{endPoint}",
               new StringContent(SerializeModel(dataTransferObject), Encoding.UTF8, "application/json"));
        }

        
        public async Task<UserModel> PostUserDataAsync(string endPoint, UserCredentials userCredentials)
        {
            
            var response = await _httpClient.PostAsync($"{endPoint}",
               new StringContent(SerializeModel(userCredentials), Encoding.UTF8, "application/json"));
            
            var userResponse = await ValidateResponseCodeAsync(response);
            return userResponse.Payload != null ? userResponse.Payload : null;
        }
        public async Task<HttpResponseMessage> GetDataAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return response;
        }

        private async Task<UserResponseModel> ValidateResponseCodeAsync(HttpResponseMessage httpResponseMessage)
        {
            var userResponse = await httpResponseMessage.GetDeserializedData();
            if (userResponse.ResponseCode == HttpStatusCode.NotFound)
            {
                throw new LoginException(userResponse.Message);
            }
            else if (userResponse.ResponseCode == HttpStatusCode.BadRequest)
            {
                throw new RegistrationException(userResponse.Message);
            }
            return userResponse;
        }

        private string SerializeModel(DataTransferObject dataTransferObject)
        {
            return JsonConvert.SerializeObject(dataTransferObject);
        }
        private string SerializeModel(UserCredentials userCredentials)
        {
            return JsonConvert.SerializeObject(userCredentials);
        }
    }
}
