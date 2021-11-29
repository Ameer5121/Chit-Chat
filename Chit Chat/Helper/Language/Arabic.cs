using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChitChat.Helper.Language
{
    class Arabic : ILanguage
    {
        public void ChangeLanguage()
        {
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
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryArabic"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersArabic"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessageArabic"];
        }
    }
}
