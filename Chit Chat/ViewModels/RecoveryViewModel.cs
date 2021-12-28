using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChitChat.Commands;
using ChitChat.Services;
using ChitChat.Helper.Extensions;
using System.Net.Http;
using ChitChat.Helper.Exceptions;

namespace ChitChat.ViewModels
{
    class RecoveryViewModel : ViewModelBase
    {
        private string _email;
        private string _recoveryStatus;
        private int _code;
        private bool _codeVerified;
        private bool _isSendingEmail;
        private IHttpService _httpService;
        public RecoveryViewModel(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public string Email
        {
            get => _email;
            set => SetPropertyValue(ref _email, value);
        }
        public string RecoveryStatus
        {
            get => _recoveryStatus;
            set => SetPropertyValue(ref _recoveryStatus, value);
        }

        public int Code
        {
            get => _code;
            set => SetPropertyValue(ref _code, value);
        }
        public bool CodeVerified
        {
            get => _codeVerified;
            set => SetPropertyValue(ref _codeVerified, value);
        }
        public SecureString Password { get; set; }

        public ICommand SendEmailCommand => new RelayCommand(SendEmail);

        private async Task SendEmail()
        {
            _isSendingEmail = true;
            try
            {
                Email.Validate();
                await _httpService.PostDataAsync("PostEmail", _email);

            }catch(FormatException e)
            {
                RecoveryStatus = "Invalid Email Format.";
                _isSendingEmail = false;
                return;
            }
            catch (HttpRequestException)
            {
                RecoveryStatus = "Could not connect to the server.";
                _isSendingEmail = false;
                return;
            }catch(RecoveryException e)
            {
                RecoveryStatus = e.Message;
                _isSendingEmail = false;
                return;
            }
           
        }
    }
}
