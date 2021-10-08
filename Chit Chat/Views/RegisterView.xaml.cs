using ChitChat.Services;
using ChitChat.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnLoaded;
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoaded;
            this.Unloaded -= OnUnLoaded;
            (DataContext as HomeViewModel).Register -= ClearPassword;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as HomeViewModel).Register += ClearPassword;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            (DataContext as HomeViewModel).Password = passwordBox.SecurePassword;
        }

        private void ClearPassword(object sender, EventArgs e)
        {
            passBox.Clear();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            ClearPassword(this, EventArgs.Empty);
            (DataContext as HomeViewModel).ClearCredentials();
        }
    }
}
