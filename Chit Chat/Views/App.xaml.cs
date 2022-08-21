using ChitChat.Helper.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ChitChat.Services;

namespace ChitChat.Views
{
    /// <summary>
    /// Interaction logic for this.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            IoCContainerService.Register();
            
        }
        public static CultureInfo Culture { get; set; }
        public void ChangeTheme(Theme? theme)
        {
            if (theme == Theme.Light)
            {
                this.Resources["ChatWindow"] = this.Resources["WindowLightTheme"];
                this.Resources["EmojiWindow"] = this.Resources["EmojiWindowLightTheme"];
                this.Resources["LanguageChangeWindow"] = this.Resources["LanguageChangeWindowLightTheme"];
                this.Resources["SubWindow"] = this.Resources["SubWindowLightTheme"];
                this.Resources["WindowBorder"] = this.Resources["WindowBorderLightTheme"];
                this.Resources["Menu"] = this.Resources["MenuLightTheme"];
                this.Resources["MessageHistoryBorder"] = this.Resources["MessageHistoryBorderLightTheme"];
                this.Resources["Chat"] = this.Resources["ChatLightTheme"];
                this.Resources["PrivateChat"] = this.Resources["PrivateChatLightTheme"];
                this.Resources["UserSettings"] = this.Resources["UserSettingsLightTheme"];
                this.Resources["Users"] = this.Resources["UsersLightTheme"];
                this.Resources["Buttons"] = this.Resources["ButtonsLightTheme"];
                this.Resources["ButtonsBorderBrush"] = this.Resources["ButtonsBorderBrushLightTheme"];
                this.Resources["TextBoxBorderBrush"] = this.Resources["TextBoxBorderBrushLightTheme"];
                this.Resources["Text"] = this.Resources["TextLightTheme"];
                this.Resources["Text2"] = this.Resources["Text2LightTheme"];
                this.Resources["Icons"] = this.Resources["IconsLightTheme"];
                this.Resources["ExpanderButton"] = this.Resources["ExpanderButtonLightTheme"];
                this.Resources["TextBox"] = this.Resources["TextBoxLightTheme"];
                this.Resources["ChatDisplayName"] = this.Resources["ChatDisplayNameLightTheme"];
                this.Resources["UnderLineBrush"] = this.Resources["UnderLineBrushLightTheme"];
            }
            else
            {
                this.Resources["ChatWindow"] = this.Resources["WindowDarkTheme"];
                this.Resources["EmojiWindow"] = this.Resources["EmojiWindowDarkTheme"];
                this.Resources["LanguageChangeWindow"] = this.Resources["LanguageChangeWindowDarkTheme"];
                this.Resources["SubWindow"] = this.Resources["SubWindowDarkTheme"];
                this.Resources["WindowBorder"] = this.Resources["WindowBorderDarkTheme"];
                this.Resources["Menu"] = this.Resources["MenuDarkTheme"];
                this.Resources["MessageHistoryBorder"] = this.Resources["MessageHistoryBorderDarkTheme"];
                this.Resources["Chat"] = this.Resources["ChatDarkTheme"];
                this.Resources["PrivateChat"] = this.Resources["PrivateChatDarkTheme"];
                this.Resources["UserSettings"] = this.Resources["UserSettingsDarkTheme"];
                this.Resources["Users"] = this.Resources["UsersDarkTheme"];
                this.Resources["Buttons"] = this.Resources["ButtonsDarkTheme"];
                this.Resources["ButtonsBorderBrush"] = this.Resources["ButtonsBorderBrushDarkTheme"];
                this.Resources["TextBoxBorderBrush"] = this.Resources["TextBoxBorderBrushDarkTheme"];
                this.Resources["Text"] = this.Resources["TextDarkTheme"];
                this.Resources["Text2"] = this.Resources["Text2DarkTheme"];
                this.Resources["Icons"] = this.Resources["IconsDarkTheme"];
                this.Resources["ExpanderButton"] = this.Resources["ExpanderButtonDarkTheme"];
                this.Resources["TextBox"] = this.Resources["TextBoxDarkTheme"];
                this.Resources["ChatDisplayName"] = this.Resources["ChatDisplayNameDarkTheme"];
                this.Resources["UnderLineBrush"] = this.Resources["UnderLineBrushDarkTheme"];
            }
        }
    }
}
