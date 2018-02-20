using System;
using System.Collections.Generic;
using System.Net.Http;
using AutobusesUAQ.Models;
using AutobusesUAQ.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AutobusesUAQ.Views
{
    public partial class Inicio : ContentPage
    {

        CustomMap customMap = new CustomMap
        {
            MapType = MapType.Street,
            WidthRequest = 100,//App.ScreenWidth,
            HeightRequest = 100//App.ScreenHeight
        };


        Button boton = new Button
        {
            Text = "Aceptar",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        Picker picker = new Picker
        {
            Title = "Rutas",
            VerticalOptions = LayoutOptions.CenterAndExpand
        };

        Rutas listRutas = new Rutas();

        public Inicio()
        {
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(20.5923831, -100.4113046), Distance.FromMiles(4.0)));
            RestClient clienteRuta = new RestClient();
            Device.BeginInvokeOnMainThread(async () =>
            {

                listRutas = await clienteRuta.Get<Rutas>("http://189.211.201.181:1200/BusGPSWebService/api/rutas", "lstRutas");

                foreach (Rutas ruta in listRutas.lstRutas)
                {
                    picker.Items.Add(ruta.derrotero);
                }

            });


            picker.SelectedIndexChanged += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var idRuta = 0;
                    var posicionRuta = ((Picker)sender).SelectedIndex;
                    if (posicionRuta > -1)
                    {

                        idRuta = listRutas.lstRutas[posicionRuta].id;
                    }
                    if (idRuta == 0)
                    {
                        await DisplayAlert("Error", "Selecciona una subcategoria", "Aceptar");
                    }
                    RestClient cliente = new RestClient();

                    var rutas = await cliente.GetRutas<Rutas>("http://189.211.201.181:1200/BusGPSWebService/api/rutascoordenadas/idruta/" + idRuta, "coordenadas");
                    customMap.RouteCoordinates.Clear();
                    foreach (CoordenadasRuta posicion in rutas.coordenadas)
                    {
                        customMap.RouteCoordinates.Add(new Position(posicion.Latitud, posicion.Longitud));
                    }
                    var pila2 = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                                picker,
                                boton
                            }

                    };
                    Content = new StackLayout
                    {
                        Children = {
                                    customMap,
                                    pila2
                                }
                    };
                });
            };

            var pila = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    picker,
                    boton

                }

            };

            boton.Clicked += OnButtonClicked;

            Content = new StackLayout
            {
                Children = {
                    customMap,
                    pila
                }
            };

        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            var newPage = new MapPage(listRutas.lstRutas[picker.SelectedIndex].id);
            Navigation.PushAsync(newPage);
        }
    }
}
