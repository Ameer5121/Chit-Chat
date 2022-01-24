using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Extensions
{
    static class MessageExtensions
    {
        public static IEnumerable<MessageModel> TakePrivateMessages(this IEnumerable<MessageModel> messages, UserModel fromUser, UserModel toUser)
        {
            return messages.TakeWhile(
                             x => x.DestinationUser?.ConnectionID == toUser.ConnectionID
                            || x.User.ConnectionID == toUser.ConnectionID
                            && x.DestinationUser?.ConnectionID == fromUser.ConnectionID);
        }
    }
}
