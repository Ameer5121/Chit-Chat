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
        public string Message { get; set; }
        public UserModel Payload { get; set; }

        public UserResponseModel(UserModel payload, string message)
        {
            Payload = payload;  
            Message = message;
        }
    }
}
