using ChitChat.Helper.Enums;
using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Events
{
    public class MessageEventArgs
    {
        public MessageModel MessageModel { get; set; }
        public Theme? CurrentTheme { get; set; }
    }
}
