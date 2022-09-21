using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using ChitChat.Events;
using ChitChat.Helper;
using ChitChat.Services;
using ChitChat.ViewModels;

namespace ChitChat.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Window
    {
        private HomeViewModel _homeViewModel;
        public HomeView()
        {
            InitializeComponent();
            DataContext = new HomeViewModel(IoCContainerService._container.Resolve<IHttpService>());
            _homeViewModel = (DataContext as HomeViewModel);
            _homeViewModel.SuccessfulConnect += ChangeWindow;
            _homeViewModel.CredentialLoad += OnCredentialsLoad;
            _homeViewModel.LoadCredentials();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
                this.DragMove();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void ChangeWindow(object sender, ConnectionEventArgs e)
        {
            ChatView chatView = new ChatView(e.ChatViewModelContext);
            chatView.Show();
            _homeViewModel.SuccessfulConnect -= ChangeWindow;
            this.Close();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            _homeViewModel.Password = passwordBox.SecurePassword;
        }

        private void OnCredentialsLoad(object sender, string e)
        {
            PasswordB.Password = e;
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            _homeViewModel.InRegisterScreen = true;
            Transitioner.SelectedIndex = 0;
        }
        private void RecoveryClick(object sender, RoutedEventArgs e) => Transitioner.SelectedIndex = 1;
    }
}
