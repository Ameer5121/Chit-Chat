using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ChitChat.Helper.Extensions
{
    public static class TextBoxExtensions
    {
        public static void SetEmoji(this RichTextBox richTextBox, string emoji)
        {
            Image image = new Image();
            image.Width = 20;
            image.Height = 20;
            image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/Emojis/{emoji}.png", UriKind.Absolute));
            InlineUIContainer container = new InlineUIContainer(image);
            foreach (Paragraph paragraph in richTextBox.Document.Blocks.ToList())
            {
                paragraph.Inlines.Add(container);
            }
        }
    }
}
