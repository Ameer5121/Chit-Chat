using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public static void RemoveParent(this FlowDocumentScrollViewer document)
        {
            document.Document = null;
        }

        public static bool HasImage(this FlowDocument document)
        {
            foreach (Block block in document.Blocks)
            {
                if (block.GetType() == typeof(BlockUIContainer))
                {
                    BlockUIContainer blockUIContainer = block as BlockUIContainer;
                    if (blockUIContainer.Child.GetType() == typeof(Image))
                    {
                        var image = blockUIContainer.Child as Image;
                        return image.Source.ToString().Contains("Emojis") ? false : true;
                    }
                }
                else
                {
                    Paragraph paragraph = block as Paragraph;
                    foreach(Inline inline in paragraph.Inlines)
                    {
                        if (inline.GetType() == typeof(InlineUIContainer))
                        {
                            InlineUIContainer inlineUIContainer = inline as InlineUIContainer;
                            if (inlineUIContainer.Child.GetType() == typeof(Image))
                            {
                                var image = inlineUIContainer.Child as Image;
                                return image.Source.ToString().Contains("Emojis") ? false : true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
