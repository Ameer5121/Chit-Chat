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
using System.Windows.Media.Imaging;

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
        private static Helper.Theme _currentTheme;
        private FlowDocument _currentPublicMessage;
        private FlowDocument _currentPrivateMessage;
        private HubConnection _connection;
        public event EventHandler Disconnect;
        public event EventHandler MessageSent;
        public event EventHandler Refresh;
        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler<EmojiEventArgs> EmojiClick;
        public event EventHandler<ThemeEventArgs> ThemeChange;
        public EventHandler<UploadEventArgs> PictureSelected;
        public event EventHandler PrivateChatEnter;

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
            SendHeartBeatAsync(_heartbeatToken.Token);
        }

        public ICommand ChooseImageCommand => new RelayCommand(UploadImageAsync);
        public ICommand ChooseProfilePictureCommand => new RelayCommand(UploadProfileImageAsync);
        public ICommand ShowNameChangerDialogCommand => new RelayCommand(ShowNameChangerDialog);
        public ICommand ChangeDisplayNameCommand => new RelayCommand(ChangeDisplayName);
        public ICommand ConstructPublicMessageCommand => new RelayCommand(ConstructPublicMessageAsync, CanConstructPublicMessage);
        public ICommand ConstructPrivateMessageCommand => new RelayCommand(ConstructPrivateMessageAsync, CanConstructPrivateMessage);
        public ICommand SetEmojiCommand => new RelayCommand(SetEmoji);
        public ICommand DisconnectCommand => new RelayCommand(DisconnectFromServer);
        public ICommand PrivateChatEnterCommand => new RelayCommand(ConstructPrivateChat, SetSelectedUser, RefreshPrivateCollectionView, DisableControls);
        public ICommand PrivateChatExitCommand => new RelayCommand(EnableControls);

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
        public ObservableCollection<MessageModel> AllMessages => _messages;

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

        public bool MessageContainsImage { get; set; }

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
        public Array Themes { get; } = Enum.GetValues(typeof(Helper.Theme));
        public Helper.Theme CurrentTheme
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

        private async Task ConstructPublicMessageAsync()
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
                await SendMessageAsync(messagetoSend);
            }
            catch (HttpRequestException)
            {
                ConstructError("Connection Error", "Could not send message");
                await DisplayError();
            }
            catch (SendException e)
            {
                ConstructError(e.Subject, e.Message);
                await DisplayError();
            }
        }
        private async Task ConstructPrivateMessageAsync(UserModel destinationUser)
        {
            MessageModel messagetoSend = null;
            messagetoSend = new MessageModel
            {
                RTFData = CurrentPrivateMessage.GetRTFData(),
                User = _currentUser,
                Message = _currentPrivateMessage,
                DestinationUser = destinationUser,
                MessageDate = DateTime.Now
            };
            try
            {
                await SendMessageAsync(messagetoSend);
            }
            catch (HttpRequestException)
            {
                ConstructError("Connection Error", "Could not send message");
                await DisplayError();
            }
            catch (SendException e)
            {
                ConstructError(e.Subject, e.Message);
                await DisplayError();
            }
        }

        private async Task SendMessageAsync(MessageModel messageModel)
        {
            if (MessageTooLong(messageModel)) throw new SendException("Message too large!", $"Please make your message {messageModel.Message.GetDocumentString().Length - _characterLimit} characters shorter!");
            await _httpService.PostDataAsync("PostMessage", messageModel);
            MessageSent?.Invoke(this, EventArgs.Empty);
        }

        private bool MessageTooLong(MessageModel messageModel) => _characterLimit - messageModel.Message.GetDocumentString().Length < 0;

        private void SetEmoji(string emojiName)
        {
            if (_isPrivateChatting) EmojiClick?.Invoke(this, new EmojiEventArgs { EmojiName = emojiName, ForPrivateChat = true });
            else EmojiClick?.Invoke(this, new EmojiEventArgs { EmojiName = emojiName, ForPrivateChat = false });
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

        private void ConstructPrivateChat()
        {
            PrivateChatEnter?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshPrivateCollectionView()
        {
            Refresh?.Invoke(this, EventArgs.Empty);
            PrivateMessages.Refresh();
        }

        private async Task SendHeartBeatAsync(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;

                await Task.Delay(2000);
                try
                {
                    var response = await _httpService.GetDataAsync("GetHeartBeat");
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

        private BitmapImage ChoosePicture()
        {
            var openfiledialog = new OpenFileDialog();
            BitmapImage image = new BitmapImage();
            openfiledialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(openfiledialog.FileName);
                image.EndInit();
                if (image.IsBiggerThan5MB()) throw new SizeException("Picture is too large!", "Picture cannot be bigger than 5 MB!");
                return image;
            }
            return null;
        }

        private void ShowNameChangerDialog()
        {
            var nameChangeModel = new NameChangeModel(_currentUser);
            DialogHost.Show(nameChangeModel, "ChatDialog");
        }
        private void CloseDialog()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task ChangeDisplayName(NameChangeModel nameChangeModel)
        {
            CloseDialog();
            try
            {
                await _httpService.PostDataAsync("PostName", nameChangeModel);
            }
            catch (HttpRequestException)
            {
                ConstructError("Connection Error", "Could not change name");
                await DisplayError();
            }
            _currentUser.DisplayName = nameChangeModel.NewName;
        }

        private async Task UploadImageAsync(bool isPrivate)
        {
            BitmapImage image = null;
            FlowDocument currentMessageTemp = null;
            try
            {
                image = ChoosePicture();
            }
            catch (SizeException e)
            {
                ConstructError(e.Subject, e.Message);
                await DisplayError();
                return;
            }
            if (image != null)
            {
                if (isPrivate)
                {
                    currentMessageTemp = _currentPrivateMessage;
                    PictureSelected?.Invoke(this, new UploadEventArgs(image, true));
                    await ConstructPrivateMessageAsync(_selectedUser);
                    CurrentPrivateMessage = currentMessageTemp;
                }
                else
                {
                    currentMessageTemp = _currentPublicMessage;
                    PictureSelected?.Invoke(this, new UploadEventArgs(image, false));
                    await ConstructPublicMessageAsync();
                    CurrentPublicMessage = currentMessageTemp;
                }
            }
        }

        private async Task UploadProfileImageAsync()
        {
            BitmapImage image = null;
            try
            {
                image = ChoosePicture();
            }
            catch (SizeException e)
            {
                ConstructError(e.Subject, e.Message);
                await DisplayError();
                return;
            }
            if (image != null)
            {
                ImageUploadDataModel profileImageDataModel = new ImageUploadDataModel(image.ConvertImageToBase64(), _currentUser);
                IsUploading = true;
                var response = await _httpService.PostDataAsync("PostImage", profileImageDataModel);
                var imageLink = await response.Content.ReadAsStringAsync();
                ChangeProfilePicture(imageLink);
                IsUploading = false;
            }
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
        private async Task DisplayError()
        {
            await DialogHost.Show(Error, "ChatDialog");
            Error = null;
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
