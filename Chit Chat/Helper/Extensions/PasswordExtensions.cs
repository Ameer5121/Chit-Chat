using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Extensions
{
    public static class PasswordExtensions
    {
        public static bool PasswordIsWeak(this SecureString password)
        {
            var decryptedPassword = password.DecryptPassword();
            if (int.TryParse(decryptedPassword, out _) && decryptedPassword.Length < 10) return true;
            else return false;
        }
    }
}
