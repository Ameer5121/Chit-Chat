using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public struct MessageModel
    {
       public string Message { get; set; }
      
       public UserModel User { get; set; }
   
    }
}
