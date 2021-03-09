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
        public readonly SecureString EncryptedPassword;

        public UserCredentials(string userName, SecureString password)
        {
            UserName = userName;
            EncryptedPassword = password;
        }
        public UserCredentials(string userName, string password)
        {
            UserName = userName;
            DecryptedPassword = password;
        }
        public UserCredentials(string userName, SecureString password, string email, string displayName)
        {
            UserName = userName;
            EncryptedPassword = password;
            Email = email;
            DisplayName = displayName;
        }
    }
}
