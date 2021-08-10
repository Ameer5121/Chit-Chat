using ChitChat.Events;
using ChitChat.Helper.Extensions;
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
        private ChatViewModel _chatVM;
        public PrivateChatView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnLoaded;
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {          
            _chatVM.MessageSent -= ClearPrivateTextBox;
            _chatVM.EmojiClick -= SetEmoji;
            Loaded -= OnLoaded;
            Unloaded -= OnUnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _chatVM = DataContext as ChatViewModel;
            _chatVM.CurrentPrivateMessage = PrivateChatTextBox.Document;
            _chatVM.MessageSent += ClearPrivateTextBox;
            _chatVM.EmojiClick += SetEmoji;
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
        private void SetEmoji(object sender, EmojiEventArgs e)
        {
            if (e.ForPrivateChat == true)
            {
                PrivateChatTextBox.SetEmoji(e.EmojiName);
            }
        }
        private void Emoji_Click(object sender, RoutedEventArgs e)
        {
            EmojiTransitioner.SelectedIndex = 0;
        }

    }
}
