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
using Microsoft.AspNetCore.SignalR.Client;
using ChitChat.Events;
using Newtonsoft.Json;
using System.Security;
using System.Runtime.InteropServices;

namespace ChitChat.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        private bool _isConnecting = false;
        private string _status;
        private string _currentUserName;
        private SecureString _password;
        private HubConnection connection;
        private UserModel _currentUser;
        private HttpClient _httpClient;
        public EventHandler<ConnectionEventArgs> OnSuccessfulConnect;

        public HomeViewModel()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
            _httpClient.BaseAddress = new Uri("http://79.180.212.252:5001");
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
        public string UserName
        {
            get => _currentUserName;
            set => SetPropertyValue(ref _currentUserName, value);
        }
        public SecureString Password
        {
            get => _password;
            set => SetPropertyValue(ref _password, value);
        }

        public ICommand Register => new RelayCommand(RegisterAccount);
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
                    var credentials = new UserCredentials(_currentUserName, _password);
                    var user = await SendUser(credentials);

                    _currentUser = new UserModel { DisplayName = user.DisplayName };

                    connection = new HubConnectionBuilder()
                      .WithUrl("http://79.180.212.252:5001/chathub")
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

        private async Task RegisterAccount()
        {
           
        }

        private void CreateHandlers()
        {
            connection.On<DataModel>("Connected", (data) =>
            {
                // Invoke the handler from the UI thread.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    
                    OnSuccessfulConnect?.Invoke(this, new ConnectionEventArgs
                    {
                       ChatViewModelContext = new ChatViewModel(data, _currentUser, connection)                       
                    });
                });
                connection.Remove("Connected");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">The user to send to the server</param>
        /// <param name="isExternal">Determines whether the connection is external or internal</param>
        /// <returns></returns>
        private async Task<UserModel> SendUser(UserCredentials credentials)
        {
            IsConnecting = true;          
            credentials = new UserCredentials(_currentUserName, Decrypt(credentials.EncryptedPassword));
            var jsonData = JsonConvert.SerializeObject(credentials);
            var response = await _httpClient.PostAsync("/api/chat/LoginUser",
                 new StringContent(jsonData, Encoding.UTF8, "application/json"));

            var data = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(data);
            if (string.IsNullOrEmpty(userModel.DisplayName))
            {
                throw new LoginException("Username or Password Incorrect!");
            }
            return userModel;
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
