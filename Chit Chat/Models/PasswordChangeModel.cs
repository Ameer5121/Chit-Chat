using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class PasswordChangeModel
    {
        public string Password { get; }
        public string Email { get; }
        public int Code { get; }
        public PasswordChangeModel(int code, string password, string email)
        {
            Code = code;
            Password = password;
            Email = email;
        }
    }
}
