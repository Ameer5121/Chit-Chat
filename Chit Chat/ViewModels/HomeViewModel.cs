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
using System.Net.Security;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private bool _isConnecting = false;
        private bool _isRegistering = false;
        private string _currentUserName;
        private string _email;
        private string _displayName;
        private string _passwordConstraintMessage;
        private bool _saveCrednentials;
        private SecureString _password;
        private HubConnection connection;
        private UserModel _currentUser;
        private IHttpService _httpService;
        public EventHandler<ConnectionEventArgs> SuccessfulConnect;
        public EventHandler Register;
        public EventHandler<string> CredentialLoad;

        public HomeViewModel(IHttpService httpService)
        {
            _httpService = httpService;
            HomeLogger = new Logger();
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
        public bool InRegisterScreen { get; set; }
        public string UserName
        {
            get => _currentUserName;
            set => SetPropertyValue(ref _currentUserName, value.Trim());
        }
        public bool SaveCredentials
        {
            get => _saveCrednentials;
            set => SetPropertyValue(ref _saveCrednentials, value);
        }
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                if (InRegisterScreen)
                    if (value.Length < 6) PasswordConstraintMessage = "Password must be 6 or longer in length!";
                    else if (value.PasswordIsWeak()) PasswordConstraintMessage = "Password is too weak or common to use!";
                    else PasswordConstraintMessage = default;
            }
        }

        public string PasswordConstraintMessage
        {
            get => _passwordConstraintMessage;
            set => SetPropertyValue(ref _passwordConstraintMessage, value);
        }
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
        public ICommand SaveCredentialsCommand => new RelayCommand(SaveCredentialsToFile, CanSaveCredentials);

        private bool CanLogin() => !string.IsNullOrEmpty(_currentUserName) && Password?.Length >= 6 && !_isConnecting;

        private async Task LoginToServerAsync()
        {
            SetConnectingToTrue();
            UserModel currentUser = null;
            _ = HomeLogger.LogMessage("Connecting...");
            try
            {
                currentUser = await GetUser();
            }
            catch (HttpRequestException e)
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
            SetCurrentUser(currentUser);
            BuildConnection();
            CreateHandlers();
            await connection.StartAsync();
        }

        private async Task<UserModel> GetUser()
        {
            return await _httpService.PostLoginCredentialsAsync(new UserCredentials(_currentUserName, Password.DecryptPassword()));
        }
        private void SetCurrentUser(UserModel user) => _currentUser = user;
        private void SetConnectingToTrue() => IsConnecting = true;

        private void BuildConnection()
        {
            connection = new HubConnectionBuilder()
                      .WithUrl("https://localhost:5001/chathub")
                        .Build();
        }

        private bool CanRegisterAccount() => !string.IsNullOrEmpty(UserName) &&
                Password?.Length >= 6 && !Password.PasswordIsWeak() &&
                !string.IsNullOrEmpty(Email) &&
                !string.IsNullOrEmpty(DisplayName) &&
                !_isRegistering;

        private async Task RegisterAccountAsync()
        {
            IsRegistering = true;
            try
            {
                Email.Validate();
                await _httpService.PostRegisterCredentialsAsync(new UserCredentials(UserName, Password.DecryptPassword(), Email, DisplayName));
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
            PasswordConstraintMessage = "";
            Password?.Clear();
        }
        private bool CanSaveCredentials() => !string.IsNullOrEmpty(_currentUserName) && Password?.Length >= 6 && !_isConnecting;
        private void SaveCredentialsToFile()
        {
            if (!SaveCredentials)
            {
                if (CredentialsSaveService.CredentialsFileExist()) CredentialsSaveService.RemoveCredentialsFile();
                return;
            }
            CredentialsSaveService.SaveCredentials(new UserCredentials(UserName, Password.DecryptPassword(), true));
        }
        public void LoadCredentials()
        {
            var credentialFileExists = CredentialsSaveService.CredentialsFileExist();
            if (credentialFileExists)
            {
                UserCredentials credentials = CredentialsSaveService.LoadCredentials();
                UserName = credentials.UserName;
                SaveCredentials = credentials.SavedLocally;
                CredentialLoad?.Invoke(this, credentials.DecryptedPassword);
            }
        }
        private void CreateHandlers()
        {
            connection.On<DataModel>("Connected", (data) =>
            {

                // Invoke the handler from the UI thread.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SetConnectionID(_currentUser);
                    data.Messages.ConvertRTFToFlowDocument();
                    SuccessfulConnect?.Invoke(this, new ConnectionEventArgs
                    {
                        ChatViewModelContext = new ChatViewModel(data, _currentUser, connection, _httpService)
                    });
                });
                RemoveHandler();
            });
        }
        private void SetConnectionID(UserModel user) => user.ConnectionID = connection.ConnectionId;
        private void RemoveHandler() => connection.Remove("Connected");
    }
}
