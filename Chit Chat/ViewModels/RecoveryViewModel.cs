using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.ViewModels
{
    class RecoveryViewModel : ViewModelBase
    {
        private string _email;
        private int _code;
        private bool _codeVerified;

        public string Email
        {
            get => _email;
            set => SetPropertyValue(ref _email, value);
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
    }
}
