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
            return messages.Where(
                             x => x.DestinationUser?.DisplayName == toUser.DisplayName
                            || x.User.DisplayName == toUser.DisplayName
                            && x.DestinationUser?.DisplayName == fromUser.DisplayName);
        }
        public static IEnumerable<UnLoadedMessagesIntervalModel> TakePrivateIntervals(this IEnumerable<UnLoadedMessagesIntervalModel> intervals, UserModel fromUser, UserModel toUser)
        {
            return intervals.Where(
                             x => x.To?.DisplayName == toUser.DisplayName
                            || x.From.DisplayName == toUser.DisplayName
                            && x.To?.DisplayName == fromUser.DisplayName);
        }

        public static bool HasPrivateIntervals(this IEnumerable<UnLoadedMessagesIntervalModel> intervals, UserModel fromUser, UserModel toUser)
        {
            return intervals.Any(x => x.To?.DisplayName == toUser.DisplayName
                         || x.From.DisplayName == toUser.DisplayName
                         && x.To?.DisplayName == fromUser.DisplayName);
        }
    }
}
