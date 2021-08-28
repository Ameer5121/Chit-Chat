using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace ChitChat.Models
{
    public class UserCredentials
    {

        public readonly string UserName;
        public readonly string DisplayName;
        public readonly string Email;
        public readonly string DecryptedPassword;
        public UserCredentials(string userName, string decryptedPassword)
        {
            UserName = userName;
            DecryptedPassword = decryptedPassword;
        }
        public UserCredentials(string userName, string decryptedPassword, string email, string displayName)
        {
            UserName = userName;
            DecryptedPassword = decryptedPassword;
            Email = email;
            DisplayName = displayName;
        }

    }
}
