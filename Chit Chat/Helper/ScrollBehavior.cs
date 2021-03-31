using ChitChat.Models;
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
        private static List<ListView> _listViews = new List<ListView>();
        private static INotifyCollectionChanged _publicMessagesCollection;
        private static INotifyCollectionChanged _privateMessagesCollection;
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
                new UIPropertyMetadata(false, OnDifferentValue));

        private static void OnDifferentValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _listViews.Add(d as ListView);
            if (_listViews.Count > 1)
                return;
            _listViews[0].Loaded += OnLoaded;
            _listViews[0].Unloaded += OnUnLoaded;
        }

        private static void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            _publicMessagesCollection.CollectionChanged -= OnCollectionChanged;
            _privateMessagesCollection.CollectionChanged -= OnCollectionChanged;
            _listViews[0].Unloaded -= OnUnLoaded;
            _listViews[0].Loaded -= OnLoaded;
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            _publicMessagesCollection = _listViews[0].ItemsSource as INotifyCollectionChanged;
            _privateMessagesCollection = _listViews[1].ItemsSource as INotifyCollectionChanged;
            _publicMessagesCollection.CollectionChanged += OnCollectionChanged;
            _privateMessagesCollection.CollectionChanged += OnCollectionChanged;

        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var messageModel = e.NewItems[0] as MessageModel;
                if (messageModel.DestinationUser == null)
                {
                    _listViews[0].ScrollIntoView(messageModel);
                    _listViews[0].SelectedItem = messageModel;
                }
                else
                {
                    _listViews[1].ScrollIntoView(messageModel);
                    _listViews[1].SelectedItem = messageModel;
                }              
            }
        }
    }
}
