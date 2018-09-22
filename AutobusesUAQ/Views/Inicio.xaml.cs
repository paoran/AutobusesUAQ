using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using AutobusesUAQ.Models;
using AutobusesUAQ.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AutobusesUAQ.Views
{
    public partial class Inicio : ContentPage
    {
        CancellationTokenSource _cts = new CancellationTokenSource();
        CustomMap customMap = new CustomMap
        {
            MapType = MapType.Street,
            WidthRequest = 100,//App.ScreenWidth,
            HeightRequest = 100//App.ScreenHeight
        };


        Button boton = new Button
        {
            Text = "Aceptar",
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.Center
        };
        Picker picker = new Picker
        {
            Title = "Rutas",
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        Rutas listRutas = new Rutas();



        public Inicio(CancellationTokenSource _ct)
        {
            _cts = _ct;
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(20.5923831, -100.4113046), Distance.FromMiles(4.0)));
            RestClient clienteRuta = new RestClient();
            Device.BeginInvokeOnMainThread(async () =>
            {

                listRutas = await clienteRuta.Get<Rutas>("/BusGPSWebService/api/rutas", "lstRutas");
                if (listRutas != null)
                {
                    if (listRutas.lstRutas.Count > 0)
                    {
                        foreach (Rutas ruta in listRutas.lstRutas)
                        {
                            picker.Items.Add(ruta.derrotero);
                        }
                    }else
                    {
                        await DisplayAlert("Aviso", "No hay rutas disponibles", "Aceptar");
                    }
                }else
                {
                    await DisplayAlert("Aviso", "No hay rutas disponibles", "Aceptar");
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
                        await DisplayAlert("Error", "Selecciona una ruta", "Aceptar");
                    }
                    RestClient cliente = new RestClient();

                    var rutas = await cliente.GetRutas<Rutas>("/BusGPSWebService/api/rutascoordenadas/idruta/" + idRuta, "coordenadas");
                    customMap.RouteCoordinates.Clear();
                    if(rutas != null){
                        if (rutas.coordenadas.Count > 0)
                        {
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
                        }else
                        {
                            await DisplayAlert("Aviso", "No se pudo cargar la ruta", "Aceptar");
                        }
                    }else{
                        await DisplayAlert("Aviso", "No se pudo cargar la ruta", "Aceptar");
                    }

                   
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
            if (picker.SelectedIndex != -1)
            {
                var newPage = new MapPage(listRutas.lstRutas[picker.SelectedIndex].id,_cts);
                Navigation.PushAsync(newPage);

            }else{
                DisplayAlert("Aviso", "Debes seleccionar una ruta para acceder a ella!", "Aceptar");   
            }
        }
    }
}
