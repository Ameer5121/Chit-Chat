
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class ProfileImageUploadDataModel
    {
        public string Base64ImageData { get; set; }
        public UserModel Uploader { get; set; }

        public ProfileImageUploadDataModel(string base64ImageData, UserModel uploader)
        {
            Base64ImageData = base64ImageData;
            Uploader = uploader;
        }
    }
}
