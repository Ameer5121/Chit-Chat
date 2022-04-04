using ChitChat.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ChitChat.Helper.AttachedProperties
{
    class ScrollBehavior
    {
        private static List<ListBox> _listBoxes = new List<ListBox>();
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
            var listBox = d as ListBox;
            _listBoxes.Add(listBox);
            listBox.Loaded += OnLoaded;
        }

        private static void OnUnLoaded(object sender, RoutedEventArgs e) => _listBoxes.Remove(sender as ListBox);
        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.ItemsSource != null)
            {
                listBox.Unloaded += OnUnLoaded;
                var messageCollection = listBox.ItemsSource as INotifyCollectionChanged;
                messageCollection.CollectionChanged += OnCollectionChanged;
            }
        }


        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var model = e.NewItems[0];
                foreach(var listbox in _listBoxes)
                {
                    if (listbox.ItemsSource == sender)
                    {
                        listbox.ScrollIntoView(model);
                        listbox.SelectedItem = model;
                    }
                }
            }
        }

    }
}
