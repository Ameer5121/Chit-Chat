using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Exceptions
{
    class SendException : Exception
    {
        private string _subject;
        public string Subject => _subject;
        public SendException(string subject, string message) : base(message)
        {
            _subject = subject;
        }
    }
}
