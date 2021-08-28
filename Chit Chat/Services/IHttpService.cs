using ChitChat.Models;
using ChitChat.Models.Markers;
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
        Task<HttpResponseMessage> PostDataAsync(string endPoint, DataTransferObject dataTransferObject);
        Task<UserModel> PostUserDataAsync(string endPoint, UserCredentials userCredentials);
        Task<HttpResponseMessage> GetDataAsync(string endponit);
    }
}
