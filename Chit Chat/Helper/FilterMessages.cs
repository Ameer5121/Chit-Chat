using ChitChat.Models;
using ChitChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChitChat.Helper
{
    class FilterMessages
    {
        private static ListView _listView;
        private static ObservableCollection<MessageModel> _collection;
        private static UserModel _currentUser;
        public static FilterType GetFilterMessageValue(DependencyObject obj)
        {
            return (FilterType)obj.GetValue(FilterMessagesProperty);
        }

        public static void SetFilterMessages(DependencyObject obj, FilterType value)
        {
            obj.SetValue(FilterMessagesProperty, value);
        }


        public static readonly DependencyProperty FilterMessagesProperty =
            DependencyProperty.RegisterAttached("FilterMessages", typeof(FilterType), typeof(FilterMessages), new PropertyMetadata(FilterType.Undefined, OnTrue));

        private static void OnTrue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _listView = d as ListView;
            _listView.Loaded += OnLoaded;
            _listView.Unloaded += OnUnLoaded;
        }
        private static void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            _collection.CollectionChanged -= OnCollectionChanged;
            _listView.Unloaded -= OnUnLoaded;
            _listView.Loaded -= OnLoaded;
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            _collection = _listView.ItemsSource as ObservableCollection<MessageModel>;
            _currentUser = ChatViewModel.GetCurrentUser();
            if (_collection != null)
            {
                _collection.CollectionChanged += OnCollectionChanged;
            }
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (GetFilterMessageValue(_listView))
            {
                case FilterType.Everyone:
                    _listView.ItemsSource = _collection.Where(x => x.DestinationUser == null);
                    break;
                case FilterType.Private:
                    UserModel selectedUser = _listView.SelectedItem as UserModel;
                    _listView.ItemsSource = _collection.Where(x => x.DestinationUser == _currentUser && x.DestinationUser == selectedUser);
                    break;
            }
            _listView.ScrollIntoView(e.NewItems[0]);
            _listView.SelectedItem = e.NewItems[0];
        }

        public enum FilterType
        {
            Everyone,
            Private,
            Undefined
        }
    }
}
