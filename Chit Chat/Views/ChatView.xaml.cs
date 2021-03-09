using System;
using System.Windows;
using ChitChat.ViewModels;
using ChitChat.Models;


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
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
    }
}
