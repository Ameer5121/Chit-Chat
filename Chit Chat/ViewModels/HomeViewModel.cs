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
        private string _status;
        private string _currentUserName;
        private SecureString _password;
        private string _email;
        private string _displayName;
        private HubConnection connection;
        private UserModel _currentUser;
        private IHttpService _httpService;
        public EventHandler<ConnectionEventArgs> OnSuccessfulConnect;
        public EventHandler OnRegister;

        public HomeViewModel(IHttpService httpService)
        {
            _httpService = httpService;         
            _password = new SecureString();
        }
        public string Status
        {
            get => _status;
            set => SetPropertyValue(ref _status, value);       
        }
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
            set => SetPropertyValue(ref _currentUserName, value);
        }
        public SecureString Password
        {
            get => _password;
            set => _password = value; 
        }
        public string Email
        {
            get => _email;
            set => SetPropertyValue(ref _email, value);
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
            return string.IsNullOrEmpty(_currentUserName) || Password.Length == 0 ||  _isConnecting ? false : true; 
        }
        private async Task LoginToServer()
        {
            try
            {           
                await Task.Run(async () =>
                {
                    _ = LogStatus("Connecting...");
                    var user = await SendUser(new UserCredentials(_currentUserName, Decrypt(_password)));
                    _currentUser = new UserModel { DisplayName = user.DisplayName };

                    connection = new HubConnectionBuilder()
                      .WithUrl("http://localhost:5001/chathub")
                      .Build();
                    CreateHandlers();
                    await connection.StartAsync();
                });
            }
            catch(HttpRequestException)
            {
                _ = LogStatus($"Could not connect to the server.");
                IsConnecting = false;
            }
            catch (TaskCanceledException)
            {
                _ = LogStatus($"Could not connect to the server.");
                IsConnecting = false;
            }
            catch(LoginException e)
            {
                _ = LogStatus(e.Message);
                IsConnecting = false;
            }
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
                    //Validate email
                    var email = new MailAddress(Email);

                    var credentials = new UserCredentials(UserName, Decrypt(Password), Email, DisplayName);
                    var jsonData = JsonConvert.SerializeObject(credentials);
                    var response = await _httpService.PostData("/api/chat/PostUser", jsonData);                    
                    var userResponse = await response.GetDeserializedData();                 
                    if (userResponse.ResponseCode == HttpStatusCode.BadRequest)
                    {
                        _ = LogStatus(userResponse.Message);
                        IsRegistering = false;
                    }
                    else
                    {
                        _ = LogStatus(userResponse.Message);
                        UserName = "";
                        Email = "";
                        DisplayName = "";
                        IsRegistering = false;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            OnRegister?.Invoke(this, EventArgs.Empty);
                        });
                    }
                });
            }
            catch (FormatException)
            {
                _ = LogStatus("Email Address is Invalid");
                IsRegistering = false;
            }
            catch (TaskCanceledException)
            {
                _ = LogStatus("Could not connect to the server.");
                IsRegistering = false;
            }
            catch (HttpRequestException)
            {
                _ = LogStatus("Could not connect to the server.");
                IsRegistering = false;
            }
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

       /// <summary>
       /// Sends the user credentials to the server and returns a UserModel upon a successful connection.
       /// </summary>
       /// <param name="credentials"></param>
       /// <returns>UserModel</returns>
        private async Task<UserModel> SendUser(UserCredentials credentials)
        {
            IsConnecting = true;          
            var jsonData = JsonConvert.SerializeObject(credentials);
            var response = await _httpService.PostData("/api/chat/Login", jsonData);               
            var userResponse = await response.GetDeserializedData();
            if (userResponse.ResponseCode == HttpStatusCode.NotFound)
            {
                throw new LoginException(userResponse.Message);
            }          
            return userResponse.Payload;
        }

        private string Decrypt(SecureString password)
        {
            IntPtr valuePtr = IntPtr.Zero;
            string decryptedpassword;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(password);
                decryptedpassword = Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
            _password.Dispose();
            return decryptedpassword;
        }
        private async Task LogStatus(string message)
        {
            Status = message;
            await Task.Delay(2000);
            Status = default;
        }
    }
}
