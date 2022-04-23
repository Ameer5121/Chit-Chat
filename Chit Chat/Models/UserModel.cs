
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ChitChat.Models
{
    public class UserModel
    {
      
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }
        public string ConnectionID { get; set; }

        public UserModel(string displayName, string profilePicture, string connectionID)
        {
            DisplayName = displayName;
            ProfilePicture = profilePicture;
            ConnectionID = connectionID;
        }
        public UserModel()
        {

        }
    }
}
