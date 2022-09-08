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
    class Hebrew : ILanguage
    {
        public void ChangeLanguage()
        {
            App.Culture = new CultureInfo("he-IL");
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsHebrew"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountHebrew"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePictureHebrew"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNameHebrew"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearanceHebrew"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemeHebrew"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayHebrew"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguageHebrew"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationHebrew"];
            Application.Current.Resources["LogsDefault"] = Application.Current.Resources["LogsHebrew"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutHebrew"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryHebrew"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersHebrew"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessageHebrew"];
            Application.Current.Resources["PrivatelyChattingDefault"] = Application.Current.Resources["PrivatelyChattingHebrew"];
            Application.Current.Resources["LoadPreviousMessagesDefault"] = Application.Current.Resources["LoadPreviousMessagesHebrew"];
            Application.Current.Resources["ArabicDefault"] = Application.Current.Resources["ArabicHebrew"];
            Application.Current.Resources["EnglishDefault"] = Application.Current.Resources["EnglishHebrew"];
            Application.Current.Resources["FinnishDefault"] = Application.Current.Resources["FinnishHebrew"];
            Application.Current.Resources["HebrewDefault"] = Application.Current.Resources["HebrewHebrew"];
            Application.Current.Resources["JapaneseDefault"] = Application.Current.Resources["JapaneseHebrew"];
            Application.Current.Resources["PolishDefault"] = Application.Current.Resources["PolishHebrew"];
        }
    }
}
