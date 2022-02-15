using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Newtonsoft.Json;

namespace ChitChat.Models
{
    public class MessageModel
    {
        public UserModel Sender { get; set; }
        public byte[]  RTFData { get; set; }
        [JsonIgnore]
        public FlowDocument Message { get; set; }
        public UserModel DestinationUser { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
