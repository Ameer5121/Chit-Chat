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
        Task<HttpResponseMessage> PostDataAsync(string endPoint, object data);
        Task<UserModel> PostUserCredentialsAsync(string endPoint, UserCredentials userCredentials);
        Task PostEmailAsync(string email);
        Task<bool> PostCodeAsync(int code);
        Task<HttpResponseMessage> GetDataAsync(string endponit);
    }
}
