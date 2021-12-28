﻿using ChitChat.Services;
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
    /// Interaction logic for PasswordRecoveryView.xaml
    /// </summary>
    public partial class PasswordRecoveryView: UserControl
    {
        public PasswordRecoveryView()
        {
            InitializeComponent();
            DataContext = new RecoveryViewModel(HttpService.HttpServiceInstance);
        }
    }
}
