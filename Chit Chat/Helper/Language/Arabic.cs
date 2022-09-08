using ChitChat.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ChitChat.Helper.Language
{
    class Arabic : ILanguage
    {
        public void ChangeLanguage()
        {
            App.Culture = new CultureInfo("ar-SA");
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsArabic"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountArabic"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePictureArabic"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNameArabic"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearanceArabic"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemeArabic"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayArabic"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguageArabic"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationArabic"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutArabic"];
            Application.Current.Resources["LogsDefault"] = Application.Current.Resources["LogsArabic"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryArabic"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersArabic"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessageArabic"];
            Application.Current.Resources["PrivatelyChattingDefault"] = Application.Current.Resources["PrivatelyChattingArabic"];
            Application.Current.Resources["LoadPreviousMessagesDefault"] = Application.Current.Resources["LoadPreviousMessagesArabic"];
            Application.Current.Resources["ArabicDefault"] = Application.Current.Resources["ArabicArabic"];
            Application.Current.Resources["EnglishDefault"] = Application.Current.Resources["EnglishArabic"];
            Application.Current.Resources["FinnishDefault"] = Application.Current.Resources["FinnishArabic"];
            Application.Current.Resources["HebrewDefault"] = Application.Current.Resources["HebrewArabic"];
            Application.Current.Resources["JapaneseDefault"] = Application.Current.Resources["JapaneseArabic"];
            Application.Current.Resources["PolishDefault"] = Application.Current.Resources["PolishArabic"];
        }
    }
}
