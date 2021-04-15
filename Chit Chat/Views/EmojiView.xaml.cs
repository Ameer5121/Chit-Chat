using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChitChat.Views
{
    /// <summary>
    /// Interaction logic for EmojiView.xaml
    /// </summary>
    public partial class EmojiView : UserControl
    {
        public EmojiView()
        {
            InitializeComponent();
        }

        private void SetEmoji(object sender, RoutedEventArgs e)
        {
            var parentWindow = GetWindow();
            var emojiName = GetSender(sender);
            var PublicChatTextBox = parentWindow.PublicChatTextBox;
            Image image = new Image();
            image.Width = 20;
            image.Height = 20;
            image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/Emojis/{emojiName}.png", UriKind.Absolute));
            InlineUIContainer container = new InlineUIContainer(image);
            foreach(Paragraph paragraph in PublicChatTextBox.Document.Blocks.ToList())
            {
                paragraph.Inlines.Add(container);
            }
        }

        private ChatView GetWindow()
        {
            return Window.GetWindow(this) as ChatView;
        }

        private string GetSender(object sender)
        {
            var emojiName = (sender as Button).Name;
            return emojiName;
        }
    }
}
