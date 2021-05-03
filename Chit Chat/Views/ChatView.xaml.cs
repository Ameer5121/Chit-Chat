using ChitChat.Events;
using ChitChat.Helper.Extensions;
using ChitChat.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace ChitChat.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Window
    {
        public ChatView(ChatViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            Loaded += OnLoaded;          
            Unloaded += OnUnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ChatViewModel).OnDisconnect += ChangeToHomeWindow;
            (DataContext as ChatViewModel).OnPublicEnterKey += SendFlowDocumentValue;
            (DataContext as ChatViewModel).OnMessageSent += ClearPublicTextBox;
            (DataContext as ChatViewModel).OnEmojiClick += SetEmoji;
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ChatViewModel).OnDisconnect -= ChangeToHomeWindow;
            (DataContext as ChatViewModel).OnPublicEnterKey -= SendFlowDocumentValue;
            (DataContext as ChatViewModel).OnMessageSent -= ClearPublicTextBox;
            (DataContext as ChatViewModel).OnEmojiClick -= SetEmoji;
            Loaded -= OnLoaded;
            Unloaded -= OnUnLoaded;
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
                this.DragMove();
        }

        private void ChangeToHomeWindow(object sender, EventArgs e)
        {
            HomeView home = new HomeView();
            home.Show();
            this.Close();
        }

        private void PrivateChat_Click(object sender, RoutedEventArgs e)
        {
            PrivateChatTransitioner.SelectedIndex = 0;
        }

        private void Emoji_Click(object sender, RoutedEventArgs e)
        {
            EmojiTransitioner.SelectedIndex = 0;
        }

        private void SendFlowDocumentValue(object sender, EventArgs e)
        {
            (DataContext as ChatViewModel).CurrentPublicMessage = PublicChatTextBox.Document;
        }

        private void ClearPublicTextBox(object sender, EventArgs e)
        {
            PublicChatTextBox.Document.Blocks.Clear();
        }

        private void SetEmoji(object sender, EmojiEventArgs e)
        {
            if (e.ForPrivateChat == false)
            {
                PublicChatTextBox.SetEmoji(e.EmojiName);
            }
        }
    }
}
