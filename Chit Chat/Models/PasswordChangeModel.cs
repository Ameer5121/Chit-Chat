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
        public PasswordChangeModel(string password, string email)
        {
            Password = password;
            Email = email;
        }
    }
}
