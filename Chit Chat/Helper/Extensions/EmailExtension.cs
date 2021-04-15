using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Extensions
{
    public static class EmailExtension
    {
        public static void Validate(this string email)
        {
            new MailAddress(email);
        }
    }
}
