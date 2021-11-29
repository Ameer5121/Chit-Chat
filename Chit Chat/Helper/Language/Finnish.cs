using System;
using System.Collections.Generic;
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
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsFinnish"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountFinnish"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePictureFinnish"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNameFinnish"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearanceFinnish"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemeFinnish"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayFinnish"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguageFinnish"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationFinnish"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutFinnish"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryFinnish"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersFinnish"];
            Application.Current.Resources["SendAMessageDefault"] = Application.Current.Resources["SendAMessageFinnish"];
        }
    }
}
