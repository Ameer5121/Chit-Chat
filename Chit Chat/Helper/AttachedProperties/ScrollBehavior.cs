using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChitChat.Helper.AttachedProperties
{
    class ScrollBehavior
    {
        private static List<ListBox> _listBoxes = new List<ListBox>();
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
            _listBoxes.Add(d as ListBox);
            if (_listBoxes.Count > 1)
                return;
            _listBoxes[0].Loaded += OnLoaded;
            _listBoxes[0].Unloaded += OnUnLoaded;
        }

        private static void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            _publicMessagesCollection.CollectionChanged -= OnCollectionChanged;
            _privateMessagesCollection.CollectionChanged -= OnCollectionChanged;
             _listBoxes[0].Unloaded -= OnUnLoaded;
             _listBoxes[0].Loaded -= OnLoaded;
            _listBoxes.Clear();
            _privateMessagesCollection = null;
           _publicMessagesCollection = null;
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            _publicMessagesCollection = _listBoxes[0].ItemsSource as INotifyCollectionChanged;
            _privateMessagesCollection = _listBoxes[1].ItemsSource as INotifyCollectionChanged;
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
                    _listBoxes[0].ScrollIntoView(messageModel);
                    _listBoxes[0].SelectedItem = messageModel;
                }
                else
                {
                    _listBoxes[1].ScrollIntoView(messageModel);
                    _listBoxes[1].SelectedItem = messageModel;
                }              
            }
        }
    }
}
