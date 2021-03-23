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

namespace ChitChat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private UserModel _currentUser;
        private ObservableCollection<UserModel> _users;
        private ObservableCollection<MessageModel> _messages;
        private CancellationTokenSource _heartbeatToken;
        private IHttpService _httpService;
        private bool _isDisconnecting;
        private string _currentMessage;
        private HubConnection _connection;
        public event EventHandler OnDisconnect;
        public ChatViewModel(DataModel data, UserModel currentuser, HubConnection connection, IHttpService httpService)
        {
            _currentUser = currentuser;
            _users = data.Users;
            _messages = data.Messages;
            _connection = connection;
            _httpService = httpService;
            _heartbeatToken = new CancellationTokenSource();
            CreateHandlers();
            SendHeartBeat(_heartbeatToken.Token);
        }

        public ICommand Send => new RelayCommand(SendMessage, CanSendMessage);
        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set => SetPropertyValue(ref _users, value);
        }
        public ObservableCollection<MessageModel> Messages
        {
            get => _messages;
            set => SetPropertyValue(ref _messages, value);
        }

        public string CurrentMessage
        {
            get => _currentMessage;
            set => SetPropertyValue(ref _currentMessage, value);
        }
        public bool IsDisconnecting
        {
            get => _isDisconnecting;
            set => SetPropertyValue(ref _isDisconnecting, value);
        }

        public ICommand Disconnect => new RelayCommand(DisconnectFromServer);

        private bool CanSendMessage()
        {
            return string.IsNullOrEmpty(CurrentMessage) ? false : true;
        }
        private async Task SendMessage()
        {
            var messagetoSend = new MessageModel { Message = CurrentMessage, User = _currentUser };
             var jsonData = JsonConvert.SerializeObject(messagetoSend);
             try
             {
                await _httpService.PostData("/api/chat/PostMessage", jsonData);
                 CurrentMessage = default;
             }
             catch (HttpRequestException)
             {
                Messages.Add(new MessageModel
                {
                    Message = "Could not send message!",
                    User = new UserModel
                    {
                        DisplayName = "System"
                    }
                });            
             }
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
                    var response = await _httpService.GetData("api/chat/GetHeartBeat");
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

        private void CreateHandlers()
        {
            _connection.On<DataModel>("ReceiveData", ReceiveData);
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
                   Messages.Add(data.Messages.LastOrDefault());
               });
            }               
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
