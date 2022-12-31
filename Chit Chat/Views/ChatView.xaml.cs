using ChitChat.Events;
using ChitChat.Helper;
using ChitChat.Helper.Extensions;
using ChitChat.Models;
using ChitChat.ViewModels;
using ChitChat.Helper.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Globalization;

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
            _chatVM.CurrentPublicMessage = PublicChatTextBox.Document;
            _chatVM.Disconnect += ChangeToHomeWindow;
            _chatVM.MessageSent += ClearPublicTextBox;
            _chatVM.MessageReceived += CheckMessageTheme;
            _chatVM.EmojiClick += SetEmoji;
            _chatVM.ThemeChange += ChangeTheme;
            _chatVM.PrivateChatClick += OnPrivateChatEnter;
            _chatVM.PictureSelected += SetPictureMessage;
            _chatVM.MessageDisplayChange += ChangeMessageDisplay;
            _chatVM.MessageDeleted += ClearPublicChat;
            //Set the correct color for messages upon logging in
            ChangeMessagesColor(_chatVM.CurrentTheme, _chatVM.AllMessages);
        }


        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            _chatVM.Disconnect -= ChangeToHomeWindow;
            _chatVM.MessageSent -= ClearPublicTextBox;
            _chatVM.MessageReceived -= CheckMessageTheme;
            _chatVM.EmojiClick -= SetEmoji;
            _chatVM.ThemeChange -= ChangeTheme;
            _chatVM.PrivateChatClick -= OnPrivateChatEnter;
            _chatVM.PictureSelected -= SetPictureMessage;
            _chatVM.MessageDisplayChange -= ChangeMessageDisplay;
            _chatVM.MessageDeleted -= ClearPublicChat;
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

        private void Emoji_Click(object sender, RoutedEventArgs e) => EmojiTransitioner.SelectedIndex = 0;

        private void LanguageChangeClick(object sender, RoutedEventArgs e) => LanguageTransitioner.SelectedIndex = 0;

        private void ClearPublicTextBox(object sender, EventArgs e) => PublicChatTextBox.Document.Blocks.Clear();

        private void ClearPublicChat(object sender, EventArgs e)
        {
            foreach (MessageModel message in PublicChat.Items)
            {
                FlowDocument document = message.Message;
                FlowDocumentScrollViewer flowDocumentScrollViewer = document.Parent as FlowDocumentScrollViewer;
                flowDocumentScrollViewer.Document = null;
            }
        }


        private void SetEmoji(object sender, EmojiEventArgs e)
        {
            if (e.ForPrivateChat == false)
            {
                PublicChatTextBox.SetEmoji(e.EmojiName);
            }
        }

        private void OnPrivateChatEnter(object sender, EventArgs e)
        {
            PrivateChatView privateChatView = new PrivateChatView(_chatVM);
            privateChatView.Show();
        }

        private void ChangeTheme(object sender, ThemeEventArgs e)
        {
            var app = (App)Application.Current;
            if (_chatVM.CurrentTheme != e.NewTheme)
            {
                app.ChangeTheme(e.NewTheme);

                ChangeMessagesColor(e.NewTheme, _chatVM.AllMessages);
            }
        }

        private void ChangeMessagesColor(Theme? currentTheme, ObservableCollection<MessageModel> messages)
        {
            var app = (App)Application.Current;
            foreach (MessageModel messageModel in messages)
            {
                Inline inline = (messageModel.Message.Blocks.FirstBlock as Paragraph).Inlines.FirstInline;
                if (currentTheme == Theme.Light) inline.Foreground = (SolidColorBrush)app.Resources["TextLightTheme"];
                else inline.Foreground = (SolidColorBrush)app.Resources["TextDarkTheme"];
            }
        }

        private void ChangeMessagesColor(Theme? theme, Inline message, App app)
        {
            if (theme == Theme.Light) message.Foreground = (SolidColorBrush)app.Resources["TextLightTheme"];
            else message.Foreground = (SolidColorBrush)app.Resources["TextDarkTheme"];
        }
        private void CheckMessageTheme(object sender, MessageEventArgs e)
        {
            var app = (App)Application.Current;
            Inline inline = (e.MessageModel.Message.Blocks.FirstBlock as Paragraph).Inlines.FirstInline;
            if (e.CurrentTheme == Theme.Light)
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

        private void PublicChatTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _chatVM.PublicMessageLength = _chatVM.CurrentPublicMessage.GetDocumentString().Length;
        }

        private void SetPictureMessage(object sender, UploadEventArgs e)
        {
            FlowDocument flowDocument = new FlowDocument();
            Image image = new Image();
            image.Source = e.Image;
            flowDocument.Blocks.Add(new Paragraph(new InlineUIContainer(image)));
            if (e.IsPrivate) _chatVM.CurrentPrivateMessage = flowDocument;
            _chatVM.CurrentPublicMessage = flowDocument;
        }

        private void ChangeMessageDisplay(object sender, MessageDisplayEventArgs e)
        {
            if (_chatVM.CurrentMessageDisplay != e.NewMessageDisplay)
            {
                foreach (MessageModel messageModel in _chatVM.PublicMessages)
                {
                    var documentScrollViewer = messageModel.Message.Parent as FlowDocumentScrollViewer;
                    documentScrollViewer.Document = null;
                }
                PublicChat.ItemTemplate = (DataTemplate)Application.Current.Resources[Enum.GetName(typeof(MessageDisplay), e.NewMessageDisplay)];
            }
        }
    }
}
