using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChitChat.Helper
{
    class ScrollBehavior
    {
        private static ListView _listView;
        private static INotifyCollectionChanged _collection;
        public static bool GetScrollOnNewItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollOnNewItemProperty);
        }

        public static void SetScrollOnNewItem(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollOnNewItemProperty, value);
        }

        public static readonly DependencyProperty ScrollOnNewItemProperty =
            DependencyProperty.RegisterAttached(
                "ScrollOnNewItem",
                typeof(bool),
                typeof(ScrollBehavior),
                new UIPropertyMetadata(false, OnScrollOnNewItemChanged));

        private static void OnScrollOnNewItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false) return;
            _listView = d as ListView;
            _listView.Loaded += OnLoaded;
            _listView.Unloaded += OnUnLoaded;
        }

        private static void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            _collection.CollectionChanged -= OnCollectionChanged;
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            _collection = _listView.ItemsSource as INotifyCollectionChanged;
            if (_collection != null)
            {
                _collection.CollectionChanged += OnCollectionChanged;
                _listView.Loaded -= OnLoaded;
            }
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                _listView.ScrollIntoView(e.NewItems[0]);
                _listView.SelectedItem = e.NewItems[0];
            }
        }
    }
}
