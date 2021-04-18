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
    public static class DocumentExtensions
    {
        public static string GetDocumentString(this FlowDocument document)
        {
            TextRange textRange = new TextRange(document.ContentStart, document.ContentEnd);
            return textRange.Text.Trim();
        }

        public static byte[] GetRTFData(this FlowDocument document)
        {
            TextRange textRange = new TextRange(document.ContentStart, document.ContentEnd);
            byte[] RTFdata;
            using (var stream = new MemoryStream())
            {
                textRange.Save(stream, DataFormats.Rtf);
                stream.Position = 0;
                RTFdata = new byte[stream.Length];
                stream.Read(RTFdata, 0, RTFdata.Length);
            };
            return RTFdata;
        }
    }
}
