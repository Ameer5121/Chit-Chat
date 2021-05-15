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
        private bool _isDisconnecting;
        private bool _controlsEnabled = true;
        private bool _isPrivateChatting = false;
        private string _errorMessage;
        private Themes _currentTheme;
        private FlowDocument _currentPublicMessage;
        private FlowDocument _currentPrivateMessage;
        private HubConnection _connection;
        public event EventHandler OnDisconnect;
        public event EventHandler OnPublicEnterKey;
        public event EventHandler OnPrivateEnterKey;
        public event EventHandler OnMessageSent;
        public event EventHandler<EmojiEventArgs> OnEmojiClick;
        public event EventHandler<ThemeEventArgs> OnThemeChange;
        public ChatViewModel(DataModel data, UserModel currentuser, HubConnection connection, IHttpService httpService)
        {
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

        public ICommand Send => new RelayCommand(SendMessage);
        public ICommand SetEmojiCommand => new RelayCommand(SetEmoji);
        public ICommand Disconnect => new RelayCommand(DisconnectFromServer);
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

        public bool IsDisconnecting
        {
            get => _isDisconnecting;
            set => SetPropertyValue(ref _isDisconnecting, value);
        }

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set => SetPropertyValue(ref _controlsEnabled, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetPropertyValue(ref _errorMessage, value);
        }

        public Array Themes { get; } = Enum.GetValues(typeof(Themes));
        public Themes CurrentTheme
        {
            get => _currentTheme;
            set => OnThemeChange?.Invoke(this, new ThemeEventArgs { CurrentTheme = value });
        }
        private bool CanSendMessage()
        {
            return string.IsNullOrEmpty(CurrentPublicMessage?.GetDocumentString()) && string.IsNullOrEmpty(CurrentPrivateMessage?.GetDocumentString()) ? false : true;
        }
        public async Task SendMessage(object destinationUser)
        {
            if (destinationUser == null)
            {
                // Gets the FlowDocument value from the view's textbox.
                OnPublicEnterKey?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnPrivateEnterKey?.Invoke(this, EventArgs.Empty);
            }
            if (!CanSendMessage())
            {
                return;
            }
            MessageModel messagetoSend = null;
            if (destinationUser == null)
            {
                messagetoSend = new MessageModel
                {
                    RTFData = CurrentPublicMessage.GetRTFData(),
                    User = _currentUser,
                    MessageDate = DateTime.Now
                };
            }
            else
            {
                messagetoSend = new MessageModel
                {
                    RTFData = CurrentPrivateMessage.GetRTFData(),
                    User = _currentUser,
                    DestinationUser = destinationUser as UserModel,
                    MessageDate = DateTime.Now
                };
            }
            try
            {
                await _httpService.PostMessageDataAsync(JsonConvert.SerializeObject(messagetoSend));
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "Could not send message!";
            }
            finally
            {
                //Clear the view's textbox value.
                OnMessageSent?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SetEmoji(string emojiName)
        {
            if (_isPrivateChatting)
            {
                OnEmojiClick?.Invoke(this, new EmojiEventArgs { EmojiName = emojiName, ForPrivateChat = true });
            }
            else
            {
                OnEmojiClick?.Invoke(this, new EmojiEventArgs { EmojiName = emojiName, ForPrivateChat = false });
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

        private void ReceiveData(DataModel data)
        {
            if (data.Users.Count != _users.Count)
            {
                Users = data.Users;
            }
            else if (data.Messages.Count != _messages.Count)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    data.Messages.LastOrDefault().ConvertRTFToFlowDocument();

                    _messages.Add(data.Messages.LastOrDefault());
                });
            }
        }

        private void CreateHandlers()
        {
            _connection.On<DataModel>("ReceiveData", ReceiveData);
        }

        private async Task DisconnectFromServer()
        {
            IsDisconnecting = true;
            //UnSubscribe and dispose.
            _connection.Remove("ReceiveData");
            _heartbeatToken.Cancel();
            await _connection.DisposeAsync();
            OnDisconnect?.Invoke(this, EventArgs.Empty);
        }
    }
}
