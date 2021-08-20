using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class ErrorModel
    {
        private string _errorSubject;
        private string _errorMessage;
        public string ErrorSubject => _errorSubject;
        public string ErrorMessage => _errorMessage;
        public ErrorModel(string ErrorSubject, string ErrorMessage)
        {
            _errorSubject = ErrorSubject;
            _errorMessage = ErrorMessage;
        }
    }
}
