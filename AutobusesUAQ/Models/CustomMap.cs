using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using AutobusesUAQ.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AutobusesUAQ
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty ItemsProperty = BindableProperty.Create("Items", typeof(ObservableCollection<CustomPin>), typeof(CustomMap),
           null, propertyChanged: OnItemsChanged);

        public ObservableCollection<CustomPin> Items
        {
            get { return (ObservableCollection<CustomPin>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = bindable as CustomMap;

            if (oldValue is INotifyCollectionChanged)
                (oldValue as INotifyCollectionChanged).CollectionChanged -= map.OnCollectionChanged;
            if (newValue is INotifyCollectionChanged)
                (newValue as INotifyCollectionChanged).CollectionChanged += map.OnCollectionChanged;

            map.OnCollectionChanged(map, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            if (newValue != null)
                map.OnCollectionChanged(map, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList)newValue));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
                Pins.Clear();

            if (e.OldItems != null)
            {
                foreach (CustomPin pin in e.OldItems)
                {
                    Pins.Remove(pin);
                    pin.PropertyChanged -= OnPropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (CustomPin pin in e.NewItems)
                {
                    Pins.Add(pin);
                    pin.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // We should be able to just replace the changed pin, but rebuild is required to force map refresh
            Pins.Clear();
            foreach (var pin in Items)
                Pins.Add(pin);
        }

        public ObservableCollection<CustomPin> CustomPins { get; set; }
        public List<Position> RouteCoordinates { get; set; }

        public CustomMap()
        {
            RouteCoordinates = new List<Position>();
        }
    }
}
