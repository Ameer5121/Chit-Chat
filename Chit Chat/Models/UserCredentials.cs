using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace ChitChat.Models
{
    sealed class UserCredentials
    {

        public readonly string UserName;
        public readonly string DisplayName;
        public readonly string Email;
        public readonly string DecryptedPassword;
        public UserCredentials(string userName, string password)
        {
            UserName = userName;
            DecryptedPassword = password;
        }
        public UserCredentials(string userName, string password, string email, string displayName)
        {
            UserName = userName;
            DecryptedPassword = password;
            Email = email;
            DisplayName = displayName;
        }

    }
}
