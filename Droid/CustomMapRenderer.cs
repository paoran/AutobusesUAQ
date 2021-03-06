﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps.Model;
using AutobusesUAQ;
using AutobusesUAQ.Droid;
using AutobusesUAQ.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace AutobusesUAQ.Droid
{
    public class CustomMapRenderer : MapRenderer
    {
        List<Position> routeCoordinates;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                routeCoordinates = formsMap.RouteCoordinates;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);

            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(0x66FF0000);

            foreach (var position in routeCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }

            NativeMap.AddPolyline(polylineOptions);
        }
        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var cpin = pin as CustomPin;
            var hue = (float)cpin.Color.Hue % 1F * 360F;
            var alpha = (float)cpin.Opacity;

            var opts = new MarkerOptions();
            opts.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            opts.SetTitle(pin.Label);
            opts.SetSnippet(pin.Address);
            opts.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pinBus));

            return opts;
        }
    }
}
