using ChitChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChitChat.Views
{
    /// <summary>
    /// Interaction logic for PasswordRecoveryView2.xaml
    /// </summary>
    public partial class PasswordRecoveryView2 : UserControl
    {
        public PasswordRecoveryView2()
        {
            InitializeComponent();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            (DataContext as RecoveryViewModel).NewPassword = passwordBox.SecurePassword;
        }

        private void OnBackClick(object sender, RoutedEventArgs e) => (DataContext as RecoveryViewModel).Reset();
    }
}
