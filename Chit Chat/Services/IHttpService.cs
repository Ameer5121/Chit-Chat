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
        Task<HttpResponseMessage> PostData(string endpoint, string jsonContent);
        Task<UserModel> PostData(string endpoint, UserCredentials userCredientals);
        Task<HttpResponseMessage> GetData(string endponit);
    }
}
