using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ChitChat.ViewModels;
using ChitChat.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Input;
using ChitChat.Commands;
using System.Windows;
using System.Collections.Specialized;
using System.Threading;
using ChitChat.Services;
using ChitChat.Helper.Extensions;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using ChitChat.Events;
using ChitChat.Helper;
using MaterialDesignThemes.Wpf;
using ChitChat.Helper.Exceptions;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace ChitChat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private UserModel _currentUser;
        private UserModel _selectedUser;
        private ObservableCollection<UserModel> _users;
        private CollectionViewSource _privateMessagesCollectionView;
        private ObservableCollection<MessageModel> _messages;
        private CancellationTokenSource _heartbeatToken;
        private IHttpService _httpService;
        private bool _isUploading;
        private bool _controlsEnabled = true;
        private bool _isPrivateChatting = false;
        private ErrorModel _error;
        private const int _characterLimit = 600;
        private int _publicMessageLength;
        private int _privateMessageLength;
        private static Themes _currentTheme;
        private FlowDocument _currentPublicMessage;
        private FlowDocument _currentPrivateMessage;
        private HubConnection _connection;
        public event EventHandler Disconnect;
        public event EventHandler MessageSent;
        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler<EmojiEventArgs> EmojiClick;
        public event EventHandler<ThemeEventArgs> ThemeChange;
        public ChatViewModel(DataModel data, UserModel currentuser, HubConnection connection, IHttpService httpService)
        {
            _currentPublicMessage = new FlowDocument();
            _currentPrivateMessage = new FlowDocument();
            _currentUser = currentuser;
            _users = data.Users;
            _messages = data.Messages;
            // Set a default value so the binding doesn't fail.
            SelectedUser = new UserModel();

            PublicMessages = CollectionViewSource.GetDefaultView(_messages);
            PublicMessages.Filter = FilterPublicMessages;

            _privateMessagesCollectionView = new CollectionViewSource();
            _privateMessagesCollectionView.Source = _messages;
            PrivateMessages = _privateMessagesCollectionView.View;
            PrivateMessages.Filter = FilterPrivateMessages;

            _connection = connection;
            _httpService = httpService;
            _heartbeatToken = new CancellationTokenSource();
            CreateHandlers();
            SendHeartBeat(_heartbeatToken.Token);
        }

        public ICommand ChooseProfilePictureCommand => new RelayCommand(ChooseProfilePicture);
        public ICommand ConstructPublicMessageCommand => new RelayCommand(ConstructPublicMessage, CanConstructPublicMessage);
        public ICommand ConstructPrivateMessageCommand => new RelayCommand(ConstructPrivateMessage, CanConstructPrivateMessage);
        public ICommand SetEmojiCommand => new RelayCommand(SetEmoji);
        public ICommand DisconnectCommand => new RelayCommand(DisconnectFromServer);
        public ICommand OnPrivateChatEnter => new RelayCommand(SetSelectedUser, RefreshPrivateCollectionView, DisableControls);
        public ICommand OnPrivateChatExit => new RelayCommand(EnableControls);

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set => SetPropertyValue(ref _selectedUser, value);
        }

        public UserModel CurrentUser
        {
            get => _currentUser;
        }

        public ICollectionView PublicMessages { get; }
        public ICollectionView PrivateMessages { get; }
        public ObservableCollection<MessageModel> AllMessages
        {
            get => _messages;
        }
        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set => SetPropertyValue(ref _users, value);
        }

        public FlowDocument CurrentPublicMessage
        {
            get => _currentPublicMessage;
            set => SetPropertyValue(ref _currentPublicMessage, value);
        }
        public FlowDocument CurrentPrivateMessage
        {
            get => _currentPrivateMessage;
            set => SetPropertyValue(ref _currentPrivateMessage, value);
        }

        public bool IsUploading
        {
            get => _isUploading;
            set => SetPropertyValue(ref _isUploading, value);
        }

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set => SetPropertyValue(ref _controlsEnabled, value);
        }
        public ErrorModel Error
        {
            get => _error;
            set => SetPropertyValue(ref _error, value);
        }
        public int CharacterLimit => _characterLimit;
        public int PublicMessageLength
        {
            get => _publicMessageLength;
            set => SetPropertyValue(ref _publicMessageLength, value);
        }
        public int PrivateMessageLength
        {
            get => _privateMessageLength;
            set => SetPropertyValue(ref _privateMessageLength, value);
        }
        public Array Themes { get; } = Enum.GetValues(typeof(Themes));
        public Themes CurrentTheme
        {
            get => _currentTheme;
            set
            {
                ThemeChange?.Invoke(this, new ThemeEventArgs { NewTheme = value });
                _currentTheme = value;
            }
        }
        private bool CanConstructPublicMessage() => !string.IsNullOrEmpty(CurrentPublicMessage?.GetDocumentString());
        private bool CanConstructPrivateMessage() => !string.IsNullOrEmpty(CurrentPrivateMessage?.GetDocumentString());

        private async Task ConstructPublicMessage()
        {
            MessageModel messagetoSend = null;
            messagetoSend = new MessageModel
            {
                RTFData = CurrentPublicMessage.GetRTFData(),
                Message = _currentPublicMessage,
                User = _currentUser,
                MessageDate = DateTime.Now
            };
            try
            {
                await SendMessage(messagetoSend);
            }
            catch (HttpRequestException)
            {
                ConstructError("Connection Error", "Could not send message");
                DisplayError();
            }
            catch (SendException e)
            {
                ConstructError(e.Subject, e.Message);
                DisplayError();
            }
        }
        private async Task ConstructPrivateMessage(object destinationUser)
        {
            MessageModel messagetoSend = null;
            messagetoSend = new MessageModel
            {
                RTFData = CurrentPrivateMessage.GetRTFData(),
                User = _currentUser,
                Message = _currentPrivateMessage,
                DestinationUser = destinationUser as UserModel,
                MessageDate = DateTime.Now
            };
            try
            {
                await SendMessage(messagetoSend);
            }
            catch (HttpRequestException)
            {
                ConstructError("Connection Error", "Could not send message");
                DisplayError();
            }
            catch (SendException e)
            {
                ConstructError(e.Subject, e.Message);
                DisplayError();
            }
        }

        private async Task SendMessage(MessageModel messageModel)
        {
            if (_characterLimit - messageModel.Message.GetDocumentString().Length < 0)
            {
                throw new SendException("Message too large!", $"Please make your message {messageModel.Message.GetDocumentString().Length - _characterLimit} characters shorter!");
            }
            await _httpService.PostDataAsync("PostMessage", messageModel);
            MessageSent?.Invoke(this, EventArgs.Empty);
        }



        private void SetEmoji(string emojiName)
        {
            if (_isPrivateChatting)
            {
                EmojiClick?.Invoke(this, new EmojiEventArgs { EmojiName = emojiName, ForPrivateChat = true });
            }
            else
            {
                EmojiClick?.Invoke(this, new EmojiEventArgs { EmojiName = emojiName, ForPrivateChat = false });
            }
        }

        private bool FilterPrivateMessages(object item)
        {
            MessageModel currentMessage = item as MessageModel;
            if (currentMessage.DestinationUser != null && SelectedUser != null)
            {
                // We want to show messages that are directed to the selected user,
                //and to see the messages that are directed to us from the selected user
                return
                currentMessage.DestinationUser.DisplayName == SelectedUser.DisplayName
               || currentMessage.User.DisplayName == SelectedUser.DisplayName
               && currentMessage.DestinationUser.DisplayName == _currentUser.DisplayName;
            }
            return false;
        }
        private bool FilterPublicMessages(object item)
        {
            MessageModel currentMessage = item as MessageModel;
            return currentMessage.DestinationUser == null;
        }

        private void DisableControls()
        {
            ControlsEnabled = false;
            _isPrivateChatting = true;
        }
        private void EnableControls()
        {
            ControlsEnabled = true;
            _isPrivateChatting = false;
        }
        private void SetSelectedUser(UserModel selectedUser)
        {
            SelectedUser = selectedUser;
        }
        private void RefreshPrivateCollectionView()
        {
            PrivateMessages.Refresh();
        }

        private async Task SendHeartBeat(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;

                await Task.Delay(2000);
                try
                {
                    var response = await _httpService.GetDataAsync("api/chat/GetHeartBeat");
                }
                catch (HttpRequestException)
                {
                    //Server is down.
                    await DisconnectFromServer();
                }
                catch (TaskCanceledException)
                {
                    //Server is down.
                    await DisconnectFromServer();
                }
            }
        }

        private void ReceiveMessages(ObservableCollection<MessageModel> messages)
        {
            if (messages.Count > _messages.Count)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    messages.LastOrDefault().ConvertRTFToFlowDocument();

                    _messages.Add(messages.LastOrDefault());
                    MessageReceived?.Invoke(this, new MessageEventArgs
                    {
                        MessageModel = messages.LastOrDefault(),
                        CurrentTheme = this.CurrentTheme
                    });
                });
            }
        }

        private void ReceiveUsers(ObservableCollection<UserModel> users)
        {
            Users = users;
        }

        private async Task ChooseProfilePicture()
        {
            var openfiledialog = new OpenFileDialog();
            openfiledialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                await UploadImage(new ProfileImageDataModel(openfiledialog.ConvertImageToBase64(), _currentUser));
            }
        }
        private async Task UploadImage(ProfileImageDataModel profileImageDataModel)
        {
            IsUploading = true;
            var response = await _httpService.PostDataAsync("PostImage", profileImageDataModel);
            var imageLink = await response.Content.ReadAsStringAsync();
            ChangeProfilePicture(imageLink);
            IsUploading = false;
        }

        private void ChangeProfilePicture(string profilePictureSource)
        {
            _currentUser.ProfilePicture = profilePictureSource;
        }

        private void CreateHandlers()
        {
            _connection.On<ObservableCollection<UserModel>>("ReceiveUsers", ReceiveUsers);
            _connection.On<ObservableCollection<MessageModel>>("ReceiveMessages", ReceiveMessages);
        }

        private void ConstructError(string errorSubject, string errorMessage)
        {
            Error = new ErrorModel(errorSubject, errorMessage);            
        }
        private void DisplayError()
        {
            DialogHost.OpenDialogCommand.Execute(null, null);
        }
        private async Task DisconnectFromServer()
        {
            //UnSubscribe and dispose.
            _connection.Remove("ReceiveUsers");
            _connection.Remove("ReceiveMessages");
            _heartbeatToken.Cancel();
            await _connection.DisposeAsync();
            Disconnect?.Invoke(this, EventArgs.Empty);
        }
    }
}
