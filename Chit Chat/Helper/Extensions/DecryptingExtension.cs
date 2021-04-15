using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Extensions
{
    public static class DecryptingExtension
    {
        public static string DecryptPassword(this SecureString password)
        {
            IntPtr valuePtr = IntPtr.Zero;
            string decryptedpassword;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(password);
                decryptedpassword = Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
            return decryptedpassword;
        }
    }
}
