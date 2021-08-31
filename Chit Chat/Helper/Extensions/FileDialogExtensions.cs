using System;
using System.Collections.Generic;
using System.IO;
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

        public static bool IsBiggerThan5MB(this OpenFileDialog openFileDialog)
        {
            var size = new FileInfo(openFileDialog.FileName).Length;
            return size > 5 * Math.Pow(10, 6) ? true : false;
        }
    }
}
