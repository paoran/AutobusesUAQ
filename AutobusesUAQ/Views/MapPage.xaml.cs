using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Threading;
using AutobusesUAQ.Models;
using System.Collections.ObjectModel;

namespace AutobusesUAQ
{
    public partial class MapPage : ContentPage
    {
       
        public MapPage()
        {

            var customMap = new CustomMap
            {
                MapType = MapType.Street,
                WidthRequest = 50,//App.ScreenWidth,
                HeightRequest = 50//App.ScreenHeight
            };

            var pin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(20.5923831, -100.4113046),
                Label = "Autobus Nuevo",
                Address = "Chofer",
                Id = "Xamarin",

            };
            customMap.CustomPins = new ObservableCollection<CustomPin> { pin };
            customMap.Pins.Add(pin);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(20.5923831, -100.4113046), Distance.FromMiles(4.0)));

            //pin.Position = new Position(20.582389, -100.4113046);
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                for (int i = 0; 10 > i; i++)
                {
                    Thread.Sleep(1000);
                    double lat = 20.5923831 + (i * 0.001);
                    pin.Position = new Position(lat, -100.4113046);
                    customMap.CustomPins.CollectionChanged += UpdatePinsOnMap;

                    Content = customMap;    
                }
            })).Start();

            Content = customMap;

            customMap.RouteCoordinates.Add(new Position(20.5923831, -100.4113046));
            customMap.RouteCoordinates.Add(new Position(20.6208049, -100.4213312));
            customMap.RouteCoordinates.Add(new Position(20.6143688, -100.3878606));
            customMap.RouteCoordinates.Add(new Position(20.6303242, -100.351588));

            //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(20.624274921182913, -100.36291678283692), Distance.FromMiles(.0)));

        }
        public void UpdatePinsOnMap(){
            
        }

       
    }
}
