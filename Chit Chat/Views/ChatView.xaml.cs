using ChitChat.Events;
using ChitChat.Helper;
using ChitChat.Helper.Extensions;
using ChitChat.Models;
using ChitChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ChitChat.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Window
    {
        private ChatViewModel _chatVM;
        public ChatView(ChatViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            Loaded += OnLoaded;
            Unloaded += OnUnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _chatVM = (DataContext as ChatViewModel);            
            _chatVM.OnDisconnect += ChangeToHomeWindow;
            _chatVM.OnPublicEnterKey += SendFlowDocumentValue;
            _chatVM.OnMessageSent += ClearPublicTextBox;
            _chatVM.OnMessageReceived += CheckMessageTheme;
            _chatVM.OnEmojiClick += SetEmoji;
            _chatVM.OnThemeChange += ChangeTheme;
            //Set the correct color for messages upon logging in
            ChangeMessagesColor(_chatVM.CurrentTheme, _chatVM.AllMessages);
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            _chatVM.OnDisconnect -= ChangeToHomeWindow;
            _chatVM.OnPublicEnterKey -= SendFlowDocumentValue;
            _chatVM.OnMessageSent -= ClearPublicTextBox;
            _chatVM.OnMessageReceived -= CheckMessageTheme;
            _chatVM.OnEmojiClick -= SetEmoji;
            _chatVM.OnThemeChange -= ChangeTheme;
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
            _chatVM.CurrentPublicMessage = PublicChatTextBox.Document;
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
        private void ChangeTheme(object sender, ThemeEventArgs e)
        {
            var app = (App)Application.Current;
            if(_chatVM.CurrentTheme != e.NewTheme)
            {
                app.ChangeTheme(e.NewTheme);
                ChangeMessagesColor(e.NewTheme, _chatVM.AllMessages);
            }          
        }

        private void ChangeMessagesColor(Themes currentTheme, ObservableCollection<MessageModel> messages)
        {
            var app = (App)Application.Current;
            foreach (MessageModel messageModel in messages)
            {
                Inline inline = (messageModel.Message.Blocks.FirstBlock as Paragraph).Inlines.FirstInline;
                if (currentTheme == Themes.Light)
                {
                    inline.Foreground = (SolidColorBrush)app.Resources["TextLightTheme"];
                }
                else
                {
                    inline.Foreground = (SolidColorBrush)app.Resources["TextDarkTheme"];
                }
            }
        }

        private void ChangeMessagesColor(Themes theme, Inline message, App app)
        {
            if (theme == Themes.Light)
            {
                message.Foreground = (SolidColorBrush)app.Resources["TextLightTheme"];
            }
            else
            {
                message.Foreground = (SolidColorBrush)app.Resources["TextDarkTheme"];
            }
        }
        private void CheckMessageTheme(object sender, MessageEventArgs e)
        {
            var app = (App)Application.Current;
            Inline inline = (e.MessageModel.Message.Blocks.FirstBlock as Paragraph).Inlines.FirstInline;
            if (e.CurrentTheme == Themes.Light)
            {              
                if (inline.Background == app.Resources["TextLightTheme"]) return;

                ChangeMessagesColor(e.CurrentTheme, inline, app);
            }
            else
            {
                if (inline.Background == app.Resources["TextDarkTheme"]) return;
                ChangeMessagesColor(e.CurrentTheme, inline, app);
            }
        }

    }
}
