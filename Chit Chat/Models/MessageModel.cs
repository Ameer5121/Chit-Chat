using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ChitChat.Models
{
    public class MessageModel
    {
        public UserModel User { get; set; }
        public byte[]  RTFData { get; set; }
        public FlowDocument Message { get; set; }
        public UserModel DestinationUser { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
