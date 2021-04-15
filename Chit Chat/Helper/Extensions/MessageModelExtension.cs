using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ChitChat.Helper.Extensions
{
    public static class MessageModelExtension
    {
        public static void ConvertRTFToFlowDocument(this MessageModel messageModel)
        {
            messageModel.Message = new FlowDocument();
            var message = messageModel.Message;
            TextRange range = new TextRange(message.ContentStart, message.ContentEnd);
            using (var stream = new MemoryStream())
            {
                stream.Write(messageModel.RTFData, 0, messageModel.RTFData.Length);
                range.Load(stream, DataFormats.Rtf);
            }
        }

        public static void ConvertRTFToFlowDocument(this IEnumerable<MessageModel> messageModels)
        {
            foreach(MessageModel message in messageModels)
            {
                ConvertRTFToFlowDocument(message);
            }
        }
    }
}
