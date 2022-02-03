using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class DataModel
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public List<UnLoadedMessagesIntervalModel> UnLoadedMessagesIntervalModels { get; set; }

    }
}
