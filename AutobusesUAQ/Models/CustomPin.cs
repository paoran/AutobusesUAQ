using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AutobusesUAQ.Models
{
    public class CustomPin : Pin
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(CustomPin), default(Color));
        public static readonly BindableProperty OpacityProperty = BindableProperty.Create("Opacity", typeof(double), typeof(CustomPin), default(double));


        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        public int IdVehiculo { get; set; }
        public string Url { get; set; }
    }
}
