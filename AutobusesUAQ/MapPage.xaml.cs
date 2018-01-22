using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace AutobusesUAQ
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            var map = new Map(
                MapSpan.FromCenterAndRadius(
                    new Position(19.32291667, -99.224245), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var stack = new StackLayout { Spacing = 0 };
            var position = new Position(19.32291667, -99.224245); // Latitude, Longitude

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "mi pin",
                Address = "custom detail info"
            };
            map.Pins.Add(pin);
            stack.Children.Add(map);
            Content = stack;
        }
    }
}
