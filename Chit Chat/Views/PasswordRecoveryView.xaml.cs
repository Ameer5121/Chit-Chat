using ChitChat.Services;
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
using Autofac;

namespace ChitChat.Views
{
    /// <summary>
    /// Interaction logic for PasswordRecoveryView.xaml
    /// </summary>
    public partial class PasswordRecoveryView: UserControl
    {
        private RecoveryViewModel _dataContext;
        public PasswordRecoveryView()
        {
            InitializeComponent();
            DataContext = new RecoveryViewModel(IoCContainerService._container.Resolve<IHttpService>());
            _dataContext = DataContext as RecoveryViewModel;
            Loaded += OnLoad;
            Unloaded += OnUnLoaded;
        }


        private void OnLoad(object sender, RoutedEventArgs e) => _dataContext.EmailSent += ChangeView;

        private void OnUnLoaded(object sender, RoutedEventArgs e) => _dataContext.EmailSent -= ChangeView;

        private void ChangeView(object sender, EventArgs e) => Transitioner.SelectedIndex = 0;
    }
}
