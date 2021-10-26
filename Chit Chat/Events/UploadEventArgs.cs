using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ChitChat.Events
{
    public class UploadEventArgs : EventArgs
    {
        public BitmapImage Image { get; set; }
        public bool IsPrivate { get; set; }
        public UploadEventArgs(BitmapImage image, bool isPrivate)
        {
            this.Image = image;
            this.IsPrivate = isPrivate;
        }
    }
}
