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
    class Japanese : ILanguage
    {
        public void ChangeLanguage()
        {
            App.Culture = new CultureInfo("ja-JP");
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsJapanese"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountJapanese"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePictureJapanese"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNameJapanese"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearanceJapanese"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemeJapanese"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayJapanese"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguageJapanese"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationJapanese"];
            Application.Current.Resources["LogsDefault"] = Application.Current.Resources["LogsJapanese"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutJapanese"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryJapanese"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersJapanese"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessageJapanese"];
            Application.Current.Resources["PrivatelyChattingDefault"] = Application.Current.Resources["PrivatelyChattingJapanese"];
            Application.Current.Resources["LoadPreviousMessagesDefault"] = Application.Current.Resources["LoadPreviousMessagesJapanese"];
            Application.Current.Resources["ArabicDefault"] = Application.Current.Resources["ArabicJapanese"];
            Application.Current.Resources["EnglishDefault"] = Application.Current.Resources["EnglishJapanese"];
            Application.Current.Resources["FinnishDefault"] = Application.Current.Resources["FinnishJapanese"];
            Application.Current.Resources["HebrewDefault"] = Application.Current.Resources["HebrewJapanese"];
            Application.Current.Resources["JapaneseDefault"] = Application.Current.Resources["JapaneseJapanese"];
            Application.Current.Resources["PolishDefault"] = Application.Current.Resources["PolishJapanese"];
        }
    }
}
