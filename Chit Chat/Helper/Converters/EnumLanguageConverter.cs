using ChitChat.Helper.Enums;
using ChitChat.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ChitChat.Helper.Converters
{
    class EnumLanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }              
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (App.Culture == null) return value;
            else
            {
                var values = parameter.ToString().Split(" ");
                var currentLanguage = App.Culture.EnglishName.Split(" ")[0];
                var currentTheme = value as string;
                foreach(var resource in values)
                {
                    if ((string)Application.Current.Resources[$"{resource}{currentLanguage}"] == currentTheme) return resource;
                }
            }
            return null;
        }
    }
}
