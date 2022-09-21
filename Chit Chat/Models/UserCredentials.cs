using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using Newtonsoft.Json;

namespace ChitChat.Models
{
    public class UserCredentials
    {

        public string UserName { get; }
        public string DisplayName { get; }
        public string Email { get; }
        public string DecryptedPassword { get; }
        public bool SavedLocally { get; set; }

        [JsonConstructor]
        public UserCredentials(string userName, string decryptedPassword, bool savedLocally = false)
        {
            UserName = userName;
            DecryptedPassword = decryptedPassword;
            SavedLocally = savedLocally;
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
