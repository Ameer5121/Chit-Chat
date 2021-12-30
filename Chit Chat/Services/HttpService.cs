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
        public static HttpService HttpServiceInstance => _httpService;

        public async Task<HttpResponseMessage> GetDataAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return response;
        }

        public async Task<HttpResponseMessage> PostDataAsync(string endPoint, object data)
        {
            return await _httpClient.PostAsync($"/api/chat/{endPoint}",
               new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
        }

        public async Task<UserModel> PostUserCredentialsAsync(string endPoint, UserCredentials userCredentials)
        {           
            var response = await _httpClient.PostAsync($"/api/chat/{endPoint}",
               new StringContent(JsonConvert.SerializeObject(userCredentials), Encoding.UTF8, "application/json"));

            var userResponseModel = await GetDeserializedUserResponseModel(response);
            if (userResponseModel.ResponseCode == HttpStatusCode.NotFound) throw new LoginException(userResponseModel.Message);
            else if (userResponseModel.ResponseCode == HttpStatusCode.BadRequest) throw new RegistrationException(userResponseModel.Message);
            return userResponseModel.Payload;           
        }
        public async Task PostEmailAsync(string email)
        {
            var response = await _httpClient.PostAsync("/api/chat/PostEmail",
               new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json"));
            if (response.StatusCode == HttpStatusCode.NotFound) throw new RecoveryException("Email not found");
        }
        private async Task<UserResponseModel> GetDeserializedUserResponseModel(HttpResponseMessage response)
        {
            var jsonResponseData = await response.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserResponseModel>(jsonResponseData);
            return userResponse;
        }
    }
}
