using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class UserResponseModel
    {
        public HttpStatusCode ResponseCode { get; set; }
        public string Message { get; set; }
        public UserModel Payload { get; set; }
    }
}
