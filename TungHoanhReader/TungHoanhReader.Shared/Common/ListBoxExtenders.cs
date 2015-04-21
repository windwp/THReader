﻿using System;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TungHoanhReader.Common
{



    public class ListViewExtenders : DependencyObject
    {
        #region Properties

        public static readonly DependencyProperty AutoScrollToCurrentItemProperty =
            DependencyProperty.RegisterAttached("AutoScrollToCurrentItem", typeof(bool), typeof(ListViewExtenders),
                new PropertyMetadata(default(bool), OnAutoScrollToCurrentItemChanged));

        /// <summary>
        /// Returns the value of the AutoScrollToCurrentItemProperty
        /// </summary>
        /// <param name="obj">The dependency-object whichs value should be returned</param>
        /// <returns>The value of the given property</returns>
        public static bool GetAutoScrollToCurrentItem(DependencyObject obj)
        {
            return (bool) obj.GetValue(AutoScrollToCurrentItemProperty);
        }

        /// <summary>
        /// Sets the value of the AutoScrollToCurrentItemProperty
        /// </summary>
        /// <param name="obj">The dependency-object whichs value should be set</param>
        /// <param name="value">The value which should be assigned to the AutoScrollToCurrentItemProperty</param>
        public static void SetAutoScrollToCurrentItem(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToCurrentItemProperty, value);
        }

        #endregion

        #region Events

        /// <summary>
        /// This method will be called when the AutoScrollToCurrentItem
        /// property was changed
        /// </summary>
        /// <param name="s">The sender (the ListBox)</param>
        /// <param name="e">Some additional information</param>
        public static void OnAutoScrollToCurrentItemChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var listView = s as ListView;
            if (listView != null)
            {
                var listBoxItems = listView.Items;
                if (listBoxItems != null)
                {
                    var newValue = (bool) e.NewValue;

                    var autoScrollToCurrentItemWorker = new SelectionChangedEventHandler((s1, e2) =>
                    {
                        if(e2.AddedItems!=null&&e2.AddedItems.Count>0)
                        OnAutoScrollToCurrentItem(listView, e2.AddedItems[0]);
                    });
                    if (newValue)
                        listView.SelectionChanged += autoScrollToCurrentItemWorker;
                    else
                        listView.SelectionChanged -= autoScrollToCurrentItemWorker;
                }
            }
        }


        /// <summary>
        /// This method will be called when the ListBox should
        /// be scrolled to the given index
        /// </summary>
        /// <param name="listView">The ListBox which should be scrolled</param>
        /// <param name="index">The index of the item to which it should be scrolled</param>
        public static void OnAutoScrollToCurrentItem(ListView listView, object item)
        {
            if (listView != null && listView.Items != null && listView.Items.Contains(item))
                listView.ScrollIntoView(item);
        }

        /// <summary>
        /// This method will be called when the ListBox should
        /// be scrolled to the given index
        /// </summary>
        /// <param name="listView">The ListBox which should be scrolled</param>
        /// <param name="index">The index of the item to which it should be scrolled</param>
        public static void OnAutoScrollToCurrentItem(ListView listView, int index)
        {
            if (listView != null && listView.Items != null && listView.Items.Count > index && index >= 0)
                listView.ScrollIntoView(listView.Items[index]);
        }

        #endregion
    }



}