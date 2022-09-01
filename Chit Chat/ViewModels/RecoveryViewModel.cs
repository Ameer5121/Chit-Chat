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
        private bool _passwordChanged;
        private SecureString _newPassword;
        private string _passwordConstraintMessage;
        private bool _isSending;
        private IHttpService _httpService;
        public event EventHandler EmailSent;
        public RecoveryViewModel(IHttpService httpService)
        {
            _httpService = httpService;
            NewPassword = new SecureString();
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

        public bool IsSending
        {
            get => _isSending;
            set => SetPropertyValue(ref _isSending, value);
        }

        public string PasswordConstraintMessage
        {
            get { return _passwordConstraintMessage; }
            set => SetPropertyValue(ref _passwordConstraintMessage, value);
        }

        public SecureString NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                if (value.Length < 6) PasswordConstraintMessage = "Password must be 6 or longer in length!";
                else if (value.PasswordIsWeak()) PasswordConstraintMessage = "Password is too weak or common to use!";
                else PasswordConstraintMessage = default;

            }
        }

        public ICommand SendEmailCommand => new RelayCommand(SendEmail, CanSendEmail);
        public ICommand SendPasswordCommand => new RelayCommand(SendPassword, CanSendPassword);

        private bool CanSendEmail() => _email.Length > 0 && !_isSending;

        private async Task SendEmail()
        {
            IsSending = true;
            try
            {
                Email.Validate();
                await _httpService.PostRecoveryDataAsync("PostEmail", Email);

            }
            catch (FormatException e)
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
            }
            catch (RecoveryException e)
            {
                RecoveryStatus = e.Message;
                IsSending = false;
                return;
            }
            IsSending = false;
            RecoveryStatus = default;
            EmailSent?.Invoke(this, EventArgs.Empty);
        }

        private bool CanSendPassword() => NewPassword.Length > 6 && !NewPassword.PasswordIsWeak() && !_passwordChanged && _code >= 100000 && _code <= 999999;

        private async Task SendPassword()
        {
            IsSending = true;
            try
            {
                await _httpService.PostRecoveryDataAsync("PostPassword", new PasswordChangeModel(Code, NewPassword.DecryptPassword(), _email));
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
            _passwordChanged = true;
            RecoveryStatus = "Password has been changed!";
        }

        public void Reset()
        {
            _passwordChanged = false;
            RecoveryStatus = default;
        }
    }
}
