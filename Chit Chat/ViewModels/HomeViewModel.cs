using System;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using ChitChat.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ChitChat.Commands;
using ChitChat.Helper.Extensions;
using ChitChat.Helper.Exceptions;
using ChitChat.Helper;
using System.Windows;
using System.Net.Mail;
using Microsoft.AspNetCore.SignalR.Client;
using ChitChat.Events;
using Newtonsoft.Json;
using System.Security;
using System.Runtime.InteropServices;
using ChitChat.Services;

namespace ChitChat.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private bool _isConnecting = false;
        private bool _isRegistering = false;
        private string _currentUserName;
        private string _email;
        private string _displayName;
        private HubConnection connection;
        private UserModel _currentUser;
        private IHttpService _httpService;
        public EventHandler<ConnectionEventArgs> SuccessfulConnect;
        public EventHandler Register;

        public HomeViewModel(IHttpService httpService, Logger logger)
        {
            _httpService = httpService;
            HomeLogger = logger;
            Password = new SecureString();
        }

        public Logger HomeLogger { get; }
        public bool IsConnecting
        {
            get => _isConnecting;
            set => SetPropertyValue(ref _isConnecting, value);
        }
        public bool IsRegistering
        {
            get => _isRegistering;
            set => SetPropertyValue(ref _isRegistering, value);
        }
        public string UserName
        {
            get => _currentUserName;
            set => SetPropertyValue(ref _currentUserName, value.Trim());
        }
        public SecureString Password { get; set; }

        public string Email
        {
            get => _email;
            set => SetPropertyValue(ref _email, value.Trim());
        }
        public string DisplayName
        {
            get => _displayName;
            set
            {
                if (value.Length > 20)
                    return;
                SetPropertyValue(ref _displayName, value);                    
            }
        }

        public ICommand RegisterCommand => new RelayCommand(RegisterAccountAsync, CanRegisterAccount);
        public ICommand LoginCommand => new RelayCommand(LoginToServerAsync, CanLogin);


        private bool CanLogin() => !string.IsNullOrEmpty(_currentUserName) && Password.Length > 0 && !_isConnecting;

        private async Task LoginToServerAsync()
        {
            IsConnecting = true;
            UserModel currentUser = null;
            _ = HomeLogger.LogMessage("Connecting...");
            try
            {        
                await Task.Run(async () =>
                {
                    currentUser = await _httpService.PostUserDataAsync("/api/chat/Login", new UserCredentials(_currentUserName, Password.DecryptPassword()));                       
                });
            }
            catch (HttpRequestException)
            {
                _ = HomeLogger.LogMessage($"Could not connect to the server.");
                IsConnecting = false;
                return;
            }
            catch (TaskCanceledException)
            {
                _ = HomeLogger.LogMessage($"Could not connect to the server.");
                IsConnecting = false;
                return;
            }
            catch (LoginException e)
            {
                _ = HomeLogger.LogMessage(e.Message);
                IsConnecting = false;
                return;
            }
            _currentUser = new UserModel { DisplayName = currentUser.DisplayName, ProfilePicture = currentUser.ProfilePicture };
            BuildConnection();
            CreateHandlers();
            await connection.StartAsync();
        }

        private void BuildConnection()
        {
            connection = new HubConnectionBuilder()
                      .WithUrl("https://localhost:44358/chathub")
                      .Build();
        }

        private bool CanRegisterAccount() => !string.IsNullOrEmpty(UserName) &&
                Password.Length > 0 &&
                !string.IsNullOrEmpty(Email) &&
                !string.IsNullOrEmpty(DisplayName) &&
                !_isRegistering;

        private async Task RegisterAccountAsync()
        {
            IsRegistering = true;
            try
            {
                Email.Validate();
                await _httpService.PostUserDataAsync("/api/chat/PostUser", new UserCredentials(UserName, Password.DecryptPassword(), Email, DisplayName));             
            }
            catch (FormatException)
            {
                _ = HomeLogger.LogMessage("Email Address is Invalid");
                IsRegistering = false;
                return;
            }
            catch (TaskCanceledException)
            {
                _ = HomeLogger.LogMessage("Could not connect to the server.");
                IsRegistering = false;
                return;
            }
            catch (HttpRequestException)
            {
                _ = HomeLogger.LogMessage("Could not connect to the server.");
                IsRegistering = false;
                return;
            }
            catch (RegistrationException e)
            {
                _ = HomeLogger.LogMessage(e.Message);
                IsRegistering = false;
                return;
            }
            _ = HomeLogger.LogMessage("Successfully Registered!");
            IsRegistering = false;
            ClearCredentials();
            Register?.Invoke(this, EventArgs.Empty);
        }

        public void ClearCredentials()
        {
            UserName = "";
            Email = "";
            DisplayName = "";
            Password.Clear();         
        }
        private void CreateHandlers()
        {
            connection.On<DataModel>("Connected", (data) =>
            {

                // Invoke the handler from the UI thread.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SetConnectionID(_currentUser);
                    ConvertRTFDataToMessages(data.Messages);
                    SuccessfulConnect?.Invoke(this, new ConnectionEventArgs
                    {
                        ChatViewModelContext = new ChatViewModel(data, _currentUser, connection, _httpService)
                    });
                });
                RemoveHandlers();
            });
        }

        private void ConvertRTFDataToMessages(IEnumerable<MessageModel> data)
        {
            data.ConvertRTFToFlowDocument();
        }

        private void SetConnectionID(UserModel user)
        {
            user.ConnectionID = connection.ConnectionId;
        }

        private void RemoveHandlers()
        {
           connection.Remove("Connected");
        }
    }
}
