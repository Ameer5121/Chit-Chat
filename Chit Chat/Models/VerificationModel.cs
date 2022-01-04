using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class VerificationModel
    {
        public int Code { get; }
        public string Email { get; }
        public VerificationModel(int code, string email)
        {
            Code = code;
            Email = email;
        }
    }
}
