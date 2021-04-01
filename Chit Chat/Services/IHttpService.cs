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
        Task PostMessageData(string jsonContent);
        Task<UserModel> PostUserData(string endPoint, string jsonCredentials);
        Task<HttpResponseMessage> GetData(string endponit);
    }
}
