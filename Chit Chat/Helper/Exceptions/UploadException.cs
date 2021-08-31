using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Exceptions
{
    class UploadException : Exception
    {
        private string _subject;
        public string Subject => _subject;
        public UploadException(string subject, string message) : base(message) 
        {
            _subject = subject;
        }
    }
}
