using ChitChat.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ChitChat
{
    /// <summary>
    /// Interaction logic for this.xaml
    /// </summary>
    public partial class App : Application
    {
        public void ChangeTheme(Themes theme)
        {
            if (theme == Themes.Light)
            {
                this.Resources["DefaultWindowTheme"] = this.Resources["WindowLightTheme"];
                this.Resources["DefaultSubWindowTheme"] = this.Resources["SubWindowLightTheme"];
                this.Resources["WindowBorder"] = this.Resources["WindowBorderLightTheme"];
                this.Resources["Menu"] = this.Resources["MenuLightTheme"];
                this.Resources["MessageHistoryBorder"] = this.Resources["MessageHistoryBorderLightTheme"];
                this.Resources["Chat"] = this.Resources["ChatLightTheme"];
                this.Resources["PrivateChat"] = this.Resources["PrivateChatLightTheme"];
                this.Resources["MenuOptions"] = this.Resources["MenuOptionsLightTheme"];
                this.Resources["Buttons"] = this.Resources["ButtonsLightTheme"];
                this.Resources["ButtonsBorderBrush"] = this.Resources["ButtonsBorderBrushLightTheme"];
                this.Resources["TextBoxBorderBrush"] = this.Resources["TextBoxBorderBrushLightTheme"];
                this.Resources["Text"] = this.Resources["TextLightTheme"];
                this.Resources["Icons"] = this.Resources["IconsLightTheme"];
                this.Resources["ExpanderButton"] = this.Resources["ExpanderButtonLightTheme"];
                this.Resources["DefaultTextBoxTheme"] = this.Resources["TextBoxLightTheme"];
                this.Resources["DefaultChatDisplayName"] = this.Resources["ChatDisplayNameLightTheme"];
                this.Resources["DefaultUnderLineBrush"] = this.Resources["UnderLineBrushLightTheme"];
                this.Resources["DefaultTextElementTheme"] = this.Resources["MaterialDesignDarkBackground"];
            }
            else
            {
                this.Resources["DefaultWindowTheme"] = this.Resources["WindowDarkTheme"];
                this.Resources["DefaultSubWindowTheme"] = this.Resources["SubWindowDarkTheme"];
                this.Resources["WindowBorder"] = this.Resources["WindowBorderDarkTheme"];
                this.Resources["Menu"] = this.Resources["MenuDarkTheme"];
                this.Resources["MessageHistoryBorder"] = this.Resources["MessageHistoryBorderDarkTheme"];
                this.Resources["Chat"] = this.Resources["ChatDarkTheme"];
                this.Resources["PrivateChat"] = this.Resources["PrivateChatDarkTheme"];
                this.Resources["MenuOptions"] = this.Resources["MenuOptionsDarkTheme"];
                this.Resources["Buttons"] = this.Resources["ButtonsDarkTheme"];
                this.Resources["ButtonsBorderBrush"] = this.Resources["ButtonsBorderBrushDarkTheme"];
                this.Resources["TextBoxBorderBrush"] = this.Resources["TextBoxBorderBrushDarkTheme"];
                this.Resources["Text"] = this.Resources["TextDarkTheme"];
                this.Resources["Icons"] = this.Resources["IconsDarkTheme"];
                this.Resources["ExpanderButton"] = this.Resources["ExpanderButtonDarkTheme"];
                this.Resources["DefaultTextBoxTheme"] = this.Resources["TextBoxDarkTheme"];
                this.Resources["DefaultChatDisplayName"] = this.Resources["ChatDisplayNameDarkTheme"];
                this.Resources["DefaultUnderLineBrush"] = this.Resources["UnderLineBrushDarkTheme"];
                this.Resources["DefaultTextElementTheme"] = this.Resources["MaterialDesignLightBackground"];
            }
        }
    }
}
