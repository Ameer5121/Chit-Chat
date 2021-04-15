using ChitChat.Models;
using ChitChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for PrivateChatView.xaml
    /// </summary>
    public partial class PrivateChatView : UserControl
    {
        public PrivateChatView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ChatViewModel).OnMessageSent += ClearPrivateTextBox;
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
            foreach (Paragraph paragraph in PublicChatTextBox.Document.Blocks.ToList())
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

        private void OnPrivateSend(object sender, EventArgs e)
        {
            (DataContext as ChatViewModel).CurrentPrivateMessage = PrivateChatTextBox.Document;
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this) as ChatView;
            parentWindow.PrivateChatTransitioner.SelectedIndex = 1;
        }

        private void ClearPrivateTextBox(object sender, EventArgs e)
        {
            PrivateChatTextBox.Document.Blocks.Clear();
        }
    }
}
