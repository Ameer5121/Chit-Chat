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
    class Polish : ILanguage
    {
        public void ChangeLanguage()
        {
            App.Culture = new CultureInfo("pl-PL");
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsPolish"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountPolish"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePicturePolish"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNamePolish"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearancePolish"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemePolish"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayPolish"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguagePolish"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationPolish"];
            Application.Current.Resources["LogsDefault"] = Application.Current.Resources["LogsPolish"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutPolish"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryPolish"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersPolish"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessagePolish"];
            Application.Current.Resources["PrivatelyChattingDefault"] = Application.Current.Resources["PrivatelyChattingPolish"];
            Application.Current.Resources["LoadPreviousMessagesDefault"] = Application.Current.Resources["LoadPreviousMessagesPolish"];
            Application.Current.Resources["ArabicDefault"] = Application.Current.Resources["ArabicPolish"];
            Application.Current.Resources["EnglishDefault"] = Application.Current.Resources["EnglishPolish"];
            Application.Current.Resources["FinnishDefault"] = Application.Current.Resources["FinnishPolish"];
            Application.Current.Resources["HebrewDefault"] = Application.Current.Resources["HebrewPolish"];
            Application.Current.Resources["JapaneseDefault"] = Application.Current.Resources["JapanesePolish"];
            Application.Current.Resources["PolishDefault"] = Application.Current.Resources["PolishPolish"];
        }
    }
}
