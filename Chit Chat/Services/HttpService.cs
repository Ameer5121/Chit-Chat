using ChitChat.Helper;
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
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _httpClient.BaseAddress = new Uri("http://localhost:5001");
        }
        private HttpService() { }
        public static HttpService HttpServiceInstance
        {
            get => _httpService;            
        }


        public async Task<HttpResponseMessage> PostData(string endpoint, string jsonContent)
        {
            var response = await _httpClient.PostAsync($"{endpoint}",
               new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            return response;
        }
        public async Task<UserModel> PostData(string endpoint, UserCredentials userCredientals)
        {
            var jsonData = JsonConvert.SerializeObject(userCredientals);
            var response = await PostData(endpoint, jsonData);
            var userResponse = await response.GetDeserializedData();
            if (userResponse.ResponseCode == HttpStatusCode.NotFound)
            {
                throw new LoginException(userResponse.Message);
            }else if (userResponse.ResponseCode == HttpStatusCode.BadRequest)
            {
                throw new RegistrationException(userResponse.Message);
            }
            return userResponse.Payload;
        }

        public async Task<HttpResponseMessage> GetData(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return response;
        }
    }
}
