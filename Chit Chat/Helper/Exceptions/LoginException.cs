using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Exceptions
{
    public class LoginException : Exception
    {

        public LoginException(string message) : base (message)
        {
        }
    }
}
