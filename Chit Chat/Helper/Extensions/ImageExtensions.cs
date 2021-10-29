using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ChitChat.Helper.Extensions
{
    static class ImageExtensions
    {
        public static string ConvertImageToBase64(this BitmapImage image)
        {
            using (var stream = new FileStream(image.UriSource.LocalPath, FileMode.Open))
            {              
                byte[] imageBytes = new byte[stream.Length];
                stream.Read(imageBytes, 0, imageBytes.Length);
                return Convert.ToBase64String(imageBytes);
            }
        }

        public static bool IsBiggerThan5MB(this BitmapImage image)
        {
            var size = new FileInfo(image.UriSource.LocalPath).Length;
            return size > 5 * Math.Pow(10, 6) ? true : false;
        }
    }
}
