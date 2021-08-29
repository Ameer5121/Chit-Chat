using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChitChat.Helper.Extensions
{
    static class FileDialogExtensions
    {
        public static string ConvertImageToBase64(this OpenFileDialog openFileDialog)
        {
            using (var stream = openFileDialog.OpenFile())
            {
                byte[] imageBytes = new byte[stream.Length];
                stream.Read(imageBytes, 0, imageBytes.Length);
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}
