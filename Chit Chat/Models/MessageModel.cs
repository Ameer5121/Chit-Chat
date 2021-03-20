using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class MessageModel
    {
        public UserModel User { get; set; }
        public string Message { get; set; }
        public UserModel DestinationUser { get; set; }
    }
}
