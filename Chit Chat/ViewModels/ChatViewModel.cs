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
using ChitChat.Helper.Enums;
using MaterialDesignThemes.Wpf;
using ChitChat.Helper.Exceptions;
using System.Windows.Forms;
using Application = System.Windows.Application;
using System.Windows.Media.Imaging;
using ChitChat.Helper.Language;

namespace ChitChat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private UserModel _currentUser;
        private UserModel _selectedUser;
        private ObservableCollection<UserModel> _users;
        private CollectionViewSource _privateMessagesCollectionView;
        private ObservableCollection<MessageModel> _messages;
        private List<UnLoadedMessagesIntervalModel> _unLoadedMessagesIntervals;
        private CancellationTokenSource _heartbeatToken;
        private IHttpService _httpService;
        private bool _isUploading;
        private bool _controlsEnabled;
        private bool _isPrivateChatting;
        private MessageDisplay? _messageDisplay;
        private ErrorModel _error;
        private const int _characterLimit = 600;
        private int _publicMessageLength;
        private int _privateMessageLength;
        private Array _themes;
        private Array _messageDisplayOptions;
        private static Helper.Enums.Theme? _currentTheme;
        private FlowDocument _currentPublicMessage;
        private FlowDocument _currentPrivateMessage;
        private HubConnection _connection;
        public event EventHandler Disconnect;
        public event EventHandler MessageSent;
        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler<EmojiEventArgs> EmojiClick;
        public event EventHandler<ThemeEventArgs> ThemeChange;
        public event EventHandler<MessageDisplayEventArgs> MessageDisplayChange;
        public EventHandler<UploadEventArgs> PictureSelected;
        public event EventHandler PrivateChatClick;

        public ChatViewModel(DataModel data, UserModel currentuser, HubConnection connection, IHttpService httpService)
        {

            _controlsEnabled = true;
            _isPrivateChatting = false;
            // Set a null value so the ComboBox is empty.
            _messageDisplay = null;

            _currentPublicMessage = new FlowDocument();
            _currentPrivateMessage = new FlowDocument();
            _currentUser = currentuser;
            _users = data.Users;
            _messages = data.Messages;
            _unLoadedMessagesIntervals = data.UnLoadedMessagesIntervalModels;

            // Set a default value so the binding doesn't fail.
            SelectedUser = new UserModel();

            _themes = Enum.GetNames(typeof(Helper.Enums.Theme));
            _messageDisplayOptions = Enum.GetNames(typeof(MessageDisplay));

            PublicMessages = CollectionViewSource.GetDefaultView(_messages) as ListCollectionView;
            PublicMessages.Filter = FilterPublicMessages;
            PublicMessages.SortDescriptions.Add(new SortDescription("MessageDate", ListSortDirection.Ascending));

            _privateMessagesCollectionView = new CollectionViewSource();
            _privateMessagesCollectionView.Source = _messages;
            PrivateMessages = _privateMessagesCollectionView.View;
            PrivateMessages.Filter = FilterPrivateMessages;
            PrivateMessages.SortDescriptions.Add(new SortDescription("MessageDate", ListSortDirection.Ascending));

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
        public ICommand ChangeLanguageCommand => new RelayCommand(ChangeLanguage);
        public ICommand ConstructPublicMessageCommand => new RelayCommand(ConstructPublicMessageAsync, CanConstructPublicMessage);
        public ICommand ConstructPrivateMessageCommand => new RelayCommand(ConstructPrivateMessageAsync, CanConstructPrivateMessage);
        public ICommand SetEmojiCommand => new RelayCommand(SetEmoji);
        public ICommand DisconnectCommand => new RelayCommand(DisconnectFromServer);
        public ICommand PrivateChatEnterCommand => new RelayCommand(SetSelectedUser, RefreshPrivateCollectionView, ConstructPrivateChat, DisableControls);
        public ICommand PrivateChatExitCommand => new RelayCommand(EnableControls);
        public ICommand GetPreviousPublicMessagesCommand => new RelayCommand(GetPreviousPublicMessages, CanGetPreviousPublicMessages);
        public ICommand GetPreviousPrivateMessagesCommand => new RelayCommand(GetPreviousPrivateMessages, CanGetPreviousPrivateMessages);
        public UserModel SelectedUser
        {
            get => _selectedUser;
            set => SetPropertyValue(ref _selectedUser, value);
        }

        public UserModel CurrentUser
        {
            get => _currentUser;
        }

        public ICollectionView PublicMessages { get; set; }
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
        public Array Themes
        {
            get => _themes;
            set => SetPropertyValue(ref _themes, value);
        }
        public Array MessageDisplayOptions
        {
            get => _messageDisplayOptions;
            set => SetPropertyValue(ref _messageDisplayOptions, value);
        }

        public Helper.Enums.Theme? CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (value != null)
                {
                    ThemeChange?.Invoke(this, new ThemeEventArgs { NewTheme = value });
                    _currentTheme = value;
                }
            }
        }

        public MessageDisplay? CurrentMessageDisplay
        {
            get => _messageDisplay;
            set
            {
                if (value != null)
                {
                    MessageDisplayChange?.Invoke(this, new MessageDisplayEventArgs { NewMessageDisplay = value });
                    _messageDisplay = value;
                }
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
        private void SetSelectedUser(UserModel selectedUser) => SelectedUser = selectedUser;
        private void ConstructPrivateChat() => PrivateChatClick?.Invoke(this, EventArgs.Empty);
        private void RefreshPrivateCollectionView() => PrivateMessages.Refresh();

        private async Task SendHeartBeatAsync(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;

                await Task.Delay(2000);
                try
                {
                    await _httpService.GetHeartBeat();
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
                    if (_isPrivateChatting && CanReducePrivateMessages()) ReduceMessages(true);
                    else if (CanReducePublicMessages()) ReduceMessages(false);
                    MessageReceived?.Invoke(this, new MessageEventArgs
                    {
                        MessageModel = messages.LastOrDefault(),
                        CurrentTheme = this.CurrentTheme
                    });
                });
            }
        }

        private bool CanReducePublicMessages() => _messages.TakeWhile(x => x.DestinationUser == null).Count() - 5 == 5;

        private bool CanReducePrivateMessages() => _messages.TakePrivateMessages(_currentUser, SelectedUser).Count() - 5 == 5;

        private void ReduceMessages(bool privateReduce)
        {
            IEnumerable<MessageModel> messagesToRemove;
            UnLoadedMessagesIntervalModel unLoadedMessagesIntervalModel;
            if (privateReduce)
            {
                messagesToRemove = _messages.TakePrivateMessages(_currentUser, SelectedUser).Take(5).ToList();
                unLoadedMessagesIntervalModel = new UnLoadedMessagesIntervalModel(messagesToRemove.First().MessageDate,
                    messagesToRemove.Last().MessageDate, SelectedUser, _currentUser);
                _unLoadedMessagesIntervals.Add(unLoadedMessagesIntervalModel);
            }
            else
            {
                messagesToRemove = _messages.TakeWhile(x => x.DestinationUser == null).Take(5).ToList();
                unLoadedMessagesIntervalModel = new UnLoadedMessagesIntervalModel(messagesToRemove.First().MessageDate,
                    messagesToRemove.Last().MessageDate);
                _unLoadedMessagesIntervals.Add(unLoadedMessagesIntervalModel);
            }
            foreach (MessageModel message in messagesToRemove) _messages.Remove(message);

            //no need to await since it is impossible for this to throw and we don't need anything in return
            _httpService.PostDataAsync("PostMessagesInterval", unLoadedMessagesIntervalModel);           
        }

        private bool CanGetPreviousPublicMessages() => _unLoadedMessagesIntervals.Any(x => x.From == null && x.To == null);
        private bool CanGetPreviousPrivateMessages() => _unLoadedMessagesIntervals.HasPrivateIntervals(_currentUser, _selectedUser);

        private void GetPreviousPublicMessages()
        {
            var interval = _unLoadedMessagesIntervals.LastOrDefault(x => x.From == null && x.To == null);
            _connection.SendAsync("SendPreviousPublicMessages", interval);
            _unLoadedMessagesIntervals.Remove(interval);
        }

        private void GetPreviousPrivateMessages()
        {
            var interval = _unLoadedMessagesIntervals.TakePrivateIntervals(_currentUser, _selectedUser).LastOrDefault();
            _connection.SendAsync("SendPreviousPrivateMessages", interval);
            _unLoadedMessagesIntervals.Remove(interval);
        }

        private void LoadPreviousMessages(List<MessageModel> messages)
        {
            messages.ConvertRTFToFlowDocument();
            foreach (var message in messages) _messages.Add(message);
        }

        private void ReceiveUsers(ObservableCollection<UserModel> users) => Users = users;
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
        private void CloseDialog() => DialogHost.CloseDialogCommand.Execute(null, null);

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
                var imageLink = await _httpService.PostProfileImage(profileImageDataModel);
                ChangeProfilePicture(imageLink);
                IsUploading = false;
            }
        }

        private void ChangeProfilePicture(string profilePictureSource) => _currentUser.ProfilePicture = profilePictureSource;

        private void CreateHandlers()
        {
            _connection.On<ObservableCollection<UserModel>>("ReceiveUsers", ReceiveUsers);
            _connection.On<ObservableCollection<MessageModel>>("ReceiveMessages", ReceiveMessages);
            _connection.On<List<MessageModel>>("LoadPreviousMessages", LoadPreviousMessages);
        }

        private void ConstructError(string errorSubject, string errorMessage) => Error = new ErrorModel(errorSubject, errorMessage);

        private async Task DisplayError()
        {
            await DialogHost.Show(Error, "ChatDialog");
            Error = null;
        }

        private void ChangeLanguage(ILanguage language)
        {
            language.ChangeLanguage();
            string[] defaultThemes = Enum.GetNames(typeof(Helper.Enums.Theme));
            string[] defaultMessageDisplayOptions = Enum.GetNames(typeof(MessageDisplay));
            var newThemes = language.ChangeBoundEnumLanguage(defaultThemes);
            var newMessageDisplayOptions = language.ChangeBoundEnumLanguage(defaultMessageDisplayOptions);
            Themes = newThemes;
            MessageDisplayOptions = newMessageDisplayOptions;
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
