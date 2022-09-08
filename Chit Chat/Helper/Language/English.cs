using ChitChat.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChitChat.Helper.Language
{
    class English : ILanguage
    {
        public void ChangeLanguage()
        {
            App.Culture = new CultureInfo("en-US");
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsEnglish"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountEnglish"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePictureEnglish"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNameEnglish"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearanceEnglish"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemeEnglish"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayEnglish"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguageEnglish"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationEnglish"];
            Application.Current.Resources["LogsDefault"] = Application.Current.Resources["LogsEnglish"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutEnglish"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryEnglish"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersEnglish"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessageEnglish"];
            Application.Current.Resources["PrivatelyChattingDefault"] = Application.Current.Resources["PrivatelyChattingEnglish"];
            Application.Current.Resources["LoadPreviousMessagesDefault"] = Application.Current.Resources["LoadPreviousMessagesEnglish"];
            Application.Current.Resources["ArabicDefault"] = Application.Current.Resources["ArabicEnglish"];
            Application.Current.Resources["EnglishDefault"] = Application.Current.Resources["EnglishEnglish"];
            Application.Current.Resources["FinnishDefault"] = Application.Current.Resources["FinnishEnglish"];
            Application.Current.Resources["HebrewDefault"] = Application.Current.Resources["HebrewEnglish"];
            Application.Current.Resources["JapaneseDefault"] = Application.Current.Resources["JapaneseEnglish"];
            Application.Current.Resources["PolishDefault"] = Application.Current.Resources["PolishEnglish"];
        }
    }
}
