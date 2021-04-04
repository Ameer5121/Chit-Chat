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
        public EventHandler<ConnectionEventArgs> OnSuccessfulConnect;
        public EventHandler OnRegister;

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

        public ICommand Register => new RelayCommand(RegisterAccount, CanRegisterAccount);
        public ICommand Login => new RelayCommand(LoginToServer, CanLogin);


        private bool CanLogin()
        {
            return string.IsNullOrEmpty(_currentUserName) || Password.Length == 0 || _isConnecting ? false : true;
        }

        private async Task LoginToServer()
        {
            IsConnecting = true;
            _ = HomeLogger.LogMessage("Connecting...");
            try
            {
                await Task.Run(async () =>
                {
                    var user = await _httpService.PostUserData("/api/chat/Login",
                        JsonConvert.SerializeObject(new UserCredentials(_currentUserName, Password.DecryptPassword())));

                    _currentUser = new UserModel { DisplayName = user.DisplayName };
                    BuildConnection();
                    CreateHandlers();
                    await connection.StartAsync();
                });
            }
            catch (HttpRequestException)
            {
                _ = HomeLogger.LogMessage($"Could not connect to the server.");
                IsConnecting = false;
            }
            catch (TaskCanceledException)
            {
                _ = HomeLogger.LogMessage($"Could not connect to the server.");
                IsConnecting = false;
            }
            catch (LoginException e)
            {
                _ = HomeLogger.LogMessage(e.Message);
                IsConnecting = false;
            }
         
        }

        private void BuildConnection()
        {
            connection = new HubConnectionBuilder()
                      .WithUrl("http://localhost:5001/chathub")
                      .Build();
            
        }

        private bool CanRegisterAccount()
        {
            return String.IsNullOrEmpty(UserName) ||
                Password.Length <= 0 ||
                String.IsNullOrEmpty(Email) ||
                String.IsNullOrEmpty(DisplayName) ||
                _isRegistering ? false : true;
        }

        private async Task RegisterAccount()
        {
            try
            {
                await Task.Run(async () =>
                {
                    IsRegistering = true;
                    Email.Validate();

                    await _httpService.PostUserData("/api/chat/PostUser",
                        JsonConvert.SerializeObject(new UserCredentials(UserName, Password.DecryptPassword(), Email, DisplayName)));


                    _ = HomeLogger.LogMessage("Successfully Registered!");
                    ClearCredentials();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OnRegister?.Invoke(this, EventArgs.Empty);
                    });
                });
            }
            catch (FormatException)
            {
                _ = HomeLogger.LogMessage("Email Address is Invalid");
                IsRegistering = false;
            }
            catch (TaskCanceledException)
            {
                _ = HomeLogger.LogMessage("Could not connect to the server.");
                IsRegistering = false;
            }
            catch (HttpRequestException)
            {
                _ = HomeLogger.LogMessage("Could not connect to the server.");
                IsRegistering = false;
            }
            catch (RegistrationException e)
            {
                _ = HomeLogger.LogMessage(e.Message);
                IsRegistering = false;
            }
        }

        private void ClearCredentials()
        {
            UserName = "";
            Email = "";
            DisplayName = "";
            Password.Clear();
            IsRegistering = false;
        }
        private void CreateHandlers()
        {
            connection.On<DataModel>("Connected", (data) =>
            {
                // Invoke the handler from the UI thread.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _currentUser.ConnectionID = connection.ConnectionId;
                    OnSuccessfulConnect?.Invoke(this, new ConnectionEventArgs
                    {
                        ChatViewModelContext = new ChatViewModel(data, _currentUser, connection, _httpService)
                    });
                });
                connection.Remove("Connected");
            });
        }
    }
}
