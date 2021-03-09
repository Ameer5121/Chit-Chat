using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper
{
    class LoginException : Exception
    {
        public string Message { get; set; }
        public LoginException(string message)
        {
            Message = message;
        }
    }
}
