using ChitChat.Events;
using ChitChat.Helper.Extensions;
using ChitChat.Helper.Enums;
using ChitChat.Models;
using ChitChat.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class PrivateChatView : Window
    {
        private ChatViewModel _chatVM;
        public PrivateChatView(ChatViewModel chatViewModel)
        {
            InitializeComponent();
            _chatVM = chatViewModel;
            DataContext = _chatVM;
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            Loaded += OnLoaded;
            Unloaded += OnUnLoaded;
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            _chatVM.MessageSent -= ClearPrivateTextBox;
            _chatVM.EmojiClick -= SetEmoji;
            _chatVM.MessageDisplayChange -= ChangeMessageDisplay;
            Loaded -= OnLoaded;
            Unloaded -= OnUnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _chatVM.CurrentPrivateMessage = PrivateChatTextBox.Document;
            _chatVM.MessageSent += ClearPrivateTextBox;
            _chatVM.EmojiClick += SetEmoji;
            _chatVM.MessageDisplayChange += ChangeMessageDisplay;
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            ClearParent();
            Close();
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

        private void ClearParent()
        {
            foreach (MessageModel messageModel in PrivateChat.Items)
            {
                var flowDocumentScrollViewer = messageModel.Message.Parent as FlowDocumentScrollViewer;
                if (flowDocumentScrollViewer.Document != null) flowDocumentScrollViewer.Document = null;
            }
        }

        private void PrivateChatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _chatVM.PrivateMessageLength = _chatVM.CurrentPrivateMessage.GetDocumentString().Length;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ChangeMessageDisplay(object sender, MessageDisplayEventArgs e)
        {
            if (_chatVM.CurrentMessageDisplay != e.NewMessageDisplay)
            {
                foreach (MessageModel messageModel in _chatVM.PrivateMessages)
                {
                    var documentScrollViewer = messageModel.Message.Parent as FlowDocumentScrollViewer;
                    documentScrollViewer.Document = null;
                }
                PrivateChat.ItemTemplate = (DataTemplate)Application.Current.Resources[Enum.GetName(typeof(MessageDisplay), e.NewMessageDisplay)];
            }
        }
    }
}
