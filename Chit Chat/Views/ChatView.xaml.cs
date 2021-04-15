using System;
using System.Windows;
using ChitChat.ViewModels;
using ChitChat.Models;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;

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
            (DataContext as ChatViewModel).OnDisconnect += ChangeToHomeWindow;
            (DataContext as ChatViewModel).OnPublicEnter += OnPublicSend;
            (DataContext as ChatViewModel).OnMessageSent += ClearPublicTextBox;
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
            (DataContext as ChatViewModel).OnDisconnect -= ChangeToHomeWindow;
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

        private void OnPublicSend(object sender, EventArgs e)
        {
            (DataContext as ChatViewModel).CurrentPublicMessage = PublicChatTextBox.Document;
        }

        private void ClearPublicTextBox(object sender, EventArgs e)
        {
            PublicChatTextBox.Document.Blocks.Clear();
        }
    }
}
