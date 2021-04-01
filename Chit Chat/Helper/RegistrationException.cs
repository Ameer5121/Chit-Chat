using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper
{
    class RegistrationException : Exception
    {
        public string Message { get; set; }
        public RegistrationException(string message)
        {
            Message = message;
        }
    }
}
