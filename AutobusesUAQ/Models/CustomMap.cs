using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutobusesUAQ.Models;
using Xamarin.Forms.Maps;

namespace AutobusesUAQ
{
    public class CustomMap: Map
    {
        public ObservableCollection<CustomPin> CustomPins { get; set; }

        public List<Position> RouteCoordinates { get; set; }

        public CustomMap()
        {
            RouteCoordinates = new List<Position>();
        }
    }
}
