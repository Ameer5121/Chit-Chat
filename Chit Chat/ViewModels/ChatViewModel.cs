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
using ChitChat.Helper;
using System.ComponentModel;
using System.Windows.Data;

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
        private string _currentPublicMessage;
        private string _currentPrivateMessage;
        private HubConnection _connection;
        public event EventHandler OnDisconnect;
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

        public ICommand Send => new RelayCommand(SendMessage, CanSendMessage);
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

        public string CurrentPublicMessage
        {
            get => _currentPublicMessage;
            set => SetPropertyValue(ref _currentPublicMessage, value);
        }
        public string CurrentPrivateMessage
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

        private bool CanSendMessage()
        {
            return string.IsNullOrEmpty(CurrentPublicMessage) && string.IsNullOrEmpty(CurrentPrivateMessage) ? false : true;
        }
        private async Task SendMessage(object destinationUser)
        {
            MessageModel messagetoSend = null;
            if (destinationUser == null)
            {
                messagetoSend = new MessageModel 
                { 
                    Message = CurrentPublicMessage,
                    User = _currentUser, 
                    MessageDate = DateTime.Now                
                };
                CurrentPublicMessage = default;
            }
            else
            {
                messagetoSend = new MessageModel
                {
                    Message = CurrentPrivateMessage,
                    User = _currentUser,
                    DestinationUser = destinationUser as UserModel,
                    MessageDate = DateTime.Now
                };
                CurrentPrivateMessage = default;
            }
             try
             {
                await _httpService.PostMessageDataAsync(JsonConvert.SerializeObject(messagetoSend));
             }
             catch (HttpRequestException)
             {
                _messages.Add(new MessageModel
                {
                    Message = "Could not send message!",
                    User = new UserModel
                    {
                        DisplayName = "System"
                    }
                });            
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
        }
        private void EnableControls()
        {
            ControlsEnabled = true;
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
            else if(data.Messages.Count != _messages.Count)
            {
               Application.Current.Dispatcher.Invoke(() =>
               {
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
