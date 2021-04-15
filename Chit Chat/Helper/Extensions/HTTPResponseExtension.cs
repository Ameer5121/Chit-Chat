using ChitChat.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Extensions
{
    static class HTTPResponseExtension
    {
        public static async Task<UserResponseModel> GetDeserializedData(this HttpResponseMessage response)
        {
            var jsonResponseData = await response.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserResponseModel>(jsonResponseData);
            return userResponse;
        }
    }
}
