using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class NameChangeModel
    {
        public UserModel User { get; set; }
        public string NewName { get; set; }

        public NameChangeModel(UserModel user)
        {
            User = user;
        }
    }
}
