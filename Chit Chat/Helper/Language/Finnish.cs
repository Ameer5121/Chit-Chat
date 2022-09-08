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
    class Finnish : ILanguage
    {
        public void ChangeLanguage()
        {
            App.Culture = new CultureInfo("fi-FI");
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsFinnish"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountFinnish"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePictureFinnish"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNameFinnish"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearanceFinnish"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemeFinnish"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayFinnish"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguageFinnish"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationFinnish"];
            Application.Current.Resources["LogsDefault"] = Application.Current.Resources["LogsFinnish"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutFinnish"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryFinnish"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersFinnish"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessageFinnish"];
            Application.Current.Resources["PrivatelyChattingDefault"] = Application.Current.Resources["PrivatelyChattingEnglish"];
            Application.Current.Resources["LoadPreviousMessagesDefault"] = Application.Current.Resources["LoadPreviousMessagesFinnish"];
            Application.Current.Resources["ArabicDefault"] = Application.Current.Resources["ArabicFinnish"];
            Application.Current.Resources["EnglishDefault"] = Application.Current.Resources["EnglishFinnish"];
            Application.Current.Resources["FinnishDefault"] = Application.Current.Resources["FinnishFinnish"];
            Application.Current.Resources["HebrewDefault"] = Application.Current.Resources["HebrewFinnish"];
            Application.Current.Resources["JapaneseDefault"] = Application.Current.Resources["JapaneseFinnish"];
            Application.Current.Resources["PolishDefault"] = Application.Current.Resources["PolishFinnish"];
        }
    }
}
