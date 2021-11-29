using System;
using System.Collections.Generic;
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
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsHebrew"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountHebrew"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePictureHebrew"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNameHebrew"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearanceHebrew"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemeHebrew"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayHebrew"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguageHebrew"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationHebrew"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutHebrew"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryHebrew"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersHebrew"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessageHebrew"];
        }
    }
}
