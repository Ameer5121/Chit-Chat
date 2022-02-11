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
using System.Net.Security;

namespace ChitChat.Services
{
    class HttpService : IHttpService
    {
        private static readonly HttpService _httpService = new HttpService();
        private static readonly HttpClient _httpClient;
        static HttpService()
        {

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    if (policyErrors == SslPolicyErrors.None) return true;
                    if (cert.GetCertHashString() == "6178922209F45C7A6D4F3C321CDA4FD775A6A1CA") return true;
                    return false;
                };
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.BaseAddress = new Uri("https://localhost:5001");
        }
        private HttpService() { }
        public static HttpService HttpServiceInstance => _httpService;

        public async Task GetHeartBeat() => await _httpClient.GetAsync("/api/chat/GetHeartBeat");
        public async Task<HttpResponseMessage> PostDataAsync(string endPoint, object data)
        {
            return await _httpClient.PostAsync($"/api/chat/{endPoint}",
             new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
        }
        public async Task<UserModel> PostLoginCredentialsAsync(UserCredentials userCredentials)
        {
            var response = await PostDataAsync("Login", userCredentials);
            var jsonResponseData = await response.Content.ReadAsStringAsync();
            var userResponseModel = JsonConvert.DeserializeObject<UserResponseModel>(jsonResponseData);
            if (response.StatusCode == HttpStatusCode.NotFound) throw new LoginException(userResponseModel.Message);
            return userResponseModel.Payload;
        }
        public async Task PostRegisterCredentialsAsync(UserCredentials userCredentials)
        {
            var response = await PostDataAsync("PostUser", userCredentials);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new RegistrationException(body);
            }
        }
        public async Task PostRecoveryDataAsync(string endPoint, object data)
        {
            var response = await PostDataAsync(endPoint, data);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var deserializedResponse = await response.Content.ReadAsStringAsync();
                throw new RecoveryException(deserializedResponse);
            }
        }
        public async Task<string> PostProfileImage(ImageUploadDataModel imageUploadDataModel)
        {
            var response = await PostDataAsync("PostImage", imageUploadDataModel);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
