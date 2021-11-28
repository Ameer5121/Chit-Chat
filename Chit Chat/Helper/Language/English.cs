﻿using System;
using System.Collections.Generic;
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
            Application.Current.Resources["UserSettingsDefault"] = Application.Current.Resources["UserSettingsEnglish"];
            Application.Current.Resources["AccountDefault"] = Application.Current.Resources["AccountEnglish"];
            Application.Current.Resources["ChangeProfilePictureDefault"] = Application.Current.Resources["ChangeProfilePictureEnglish"];
            Application.Current.Resources["ChangeDisplayNameDefault"] = Application.Current.Resources["ChangeDisplayNameEnglish"];
            Application.Current.Resources["AppearanceDefault"] = Application.Current.Resources["AppearanceEnglish"];
            Application.Current.Resources["ThemeDefault"] = Application.Current.Resources["ThemeEnglish"];
            Application.Current.Resources["MessageDisplayDefault"] = Application.Current.Resources["MessageDisplayEnglish"];
            Application.Current.Resources["ChangeLanguageDefault"] = Application.Current.Resources["ChangeLanguageEnglish"];
            Application.Current.Resources["ApplicationDefault"] = Application.Current.Resources["ApplicationEnglish"];
            Application.Current.Resources["LogOutDefault"] = Application.Current.Resources["LogOutEnglish"];
            Application.Current.Resources["MessageHistoryDefault"] = Application.Current.Resources["MessageHistoryEnglish"];
            Application.Current.Resources["UsersDefault"] = Application.Current.Resources["UsersEnglish"];
        }
    }
}
