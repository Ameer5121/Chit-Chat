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
using ChitChat.Models;

namespace ChitChat.ViewModels
{
    class RecoveryViewModel : ViewModelBase
    {
        private string _email;
        private string _recoveryStatus;
        private int _code;
        private bool _codeVerified;
        private bool _isSending;
        private IHttpService _httpService;
        public event EventHandler EmailSent;
        public RecoveryViewModel(IHttpService httpService)
        {
            _httpService = httpService;
            _email = "";
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
        public bool IsSending
        {
            get => _isSending;
            set => SetPropertyValue(ref _isSending, value);
        }
        public SecureString Password { get; set; }

        public ICommand SendEmailCommand => new RelayCommand(SendEmail, CanSendEmail);
        public ICommand VerifyCodeCommand => new RelayCommand(VerifyCode, CanVerifyCode);


        private bool CanSendEmail() => _email.Length > 0 || _isSending;

        private async Task SendEmail()
        {
            IsSending = true;
            try
            {
                Email.Validate();
               await _httpService.PostRecoveryDataAsync("PostEmail", Email);

            }catch(FormatException e)
            {
                RecoveryStatus = "Invalid Email Format.";
                IsSending = false;
                return;
            }
            catch (HttpRequestException)
            {
                RecoveryStatus = "Could not connect to the server.";
                IsSending = false;
                return;
            }catch(RecoveryException e)
            {
                RecoveryStatus = e.Message;
                IsSending = false;
                return;
            }
            IsSending = false;
            RecoveryStatus = default;
            EmailSent?.Invoke(this, EventArgs.Empty);
        }

        private bool CanVerifyCode() => _code >= 100000 && _code <= 999999;

        private async Task VerifyCode()
        {
            IsSending = true;
            try
            {
                await _httpService.PostRecoveryDataAsync("PostCode" ,new VerificationModel(_code, _email));
            }
            catch (HttpRequestException)
            {
                RecoveryStatus = "Could not connect to the server.";
                IsSending = false;
                return;
            }
            catch (RecoveryException e)
            {
                RecoveryStatus = e.Message;
                IsSending = false;
                return;
            }
            IsSending = false;
            RecoveryStatus = "Code has been verified! You can now change your password!";
            _codeVerified = true;
        }
    }
}
