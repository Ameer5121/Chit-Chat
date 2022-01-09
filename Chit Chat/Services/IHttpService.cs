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
        Task<UserModel> PostLoginCredentialsAsync(UserCredentials userCredentials);
        Task PostRegisterCredentialsAsync(UserCredentials userCredentials);
        Task PostRecoveryDataAsync(string endPoint, object data);
        Task<string> PostProfileImage(ImageUploadDataModel imageUploadDataModel);
        Task GetHeartBeat();
    }
}
