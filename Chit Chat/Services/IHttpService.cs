using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Services
{
    public interface IHttpService
    {
        Task PostMessageDataAsync(string jsonContent);
        Task<UserModel> PostUserDataAsync(string endPoint, string jsonCredentials);
        Task<HttpResponseMessage> GetDataAsync(string endponit);
    }
}
