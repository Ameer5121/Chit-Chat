using ChitChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper
{
    public class Logger : ViewModelBase
    {
        private string _status;
        public string Status
        {
            get => _status;
            set => SetPropertyValue(ref _status, value);
        }
        public async Task LogMessage(string message)
        {
            Status = message;
            await Task.Delay(2000);
            Status = default;
        }
    }
}
