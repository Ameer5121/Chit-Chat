using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public struct VoiceChatData
    {
        public readonly string ConnectionID;
        public readonly byte[] Data;

        public VoiceChatData(string connectionID, byte[] data)
        {
            ConnectionID = connectionID;
            Data = data;
        }
    }
}
