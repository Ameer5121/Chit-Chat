using System;
using ChitChat.Helper.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Events
{
    public class MessageDisplayEventArgs : EventArgs
    {
        public MessageDisplay? NewMessageDisplay { get; set; }
    }
}
