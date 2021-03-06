﻿using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Threading;
using AutobusesUAQ.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using AutobusesUAQ.Services;
using System.Threading.Tasks;
using System.Net.Http;
using AutobusesUAQ.Views;

namespace AutobusesUAQ
{
    public partial class MapPage : ContentPage
    {
        Config config = new Config();
        Button boton = new Button
        {
            Text = "Regresar",
            HorizontalOptions = LayoutOptions.FillAndExpand,
            TextColor = Color.White,
            BorderWidth = 2,
            //VerticalOptions = LayoutOptions.FillAndExpand
        };

        int idRutaAux = 0;
        double lat = 20.5923831;

        CustomMap customMap = new CustomMap
        {
            MapType = MapType.Street,
            WidthRequest = 50,//App.ScreenWidth,
            HeightRequest = 50//App.ScreenHeight
        };
        CancellationTokenSource _cts = new CancellationTokenSource();


        List<CustomPin> arrPines = new List<CustomPin>();

        public MapPage(int idRuta,CancellationTokenSource _ct)
        {
            _cts = _ct;
            NavigationPage.SetHasNavigationBar(this, false);
            idRutaAux = idRuta;
            Device.BeginInvokeOnMainThread(async () =>
            {
                NavigationPage.SetBackButtonTitle(this, "Test");
                RestClient cliente = new RestClient();
                customMap.RouteCoordinates.Clear();

                var rutas = await cliente.GetRutas<Rutas>("/BusGPSWebService/api/rutascoordenadas/idruta/" + idRuta, "coordenadas");
                if(rutas != null){
                    if(rutas.coordenadas.Count > 0){
                        foreach (CoordenadasRuta posicion in rutas.coordenadas)
                        {
                            customMap.RouteCoordinates.Add(new Position(posicion.Latitud, posicion.Longitud));
                        }

                        var formContent = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("idRuta", idRuta.ToString())
                        });
                        var pines = await cliente.PostRutas<List<Ubicacion>>(config.ipPrueba+"/BusGPSWebService/api/vehiculorutacoordenadas", "listaUbicaciones", formContent);
                        if (pines.Count > 0)
                        {
                            foreach (Ubicacion ubic in pines)
                            {
                                CustomPin pin = new CustomPin
                                {
                                    Type = PinType.Place,
                                    Position = new Position(ubic.latitud, ubic.longitud),
                                    Label = "click para ver detalle",
                                    Address = "Chofer" + ubic.id,
                                    IdVehiculo = ubic.idVehiculo,
                                    Color = Color.Blue,

                                };

                                arrPines.Add(pin);
                            }

                            customMap.Items = new ObservableCollection<CustomPin> { };
                            foreach (CustomPin pins in arrPines)
                            {
                                customMap.Items.Add(pins);
                                customMap.Pins.Add(pins);
                                pins.Clicked += MostrarDetalle;
                            }

                        }
                        customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(20.5923831, -100.4113046), Distance.FromMiles(4.0)));
                        boton.Clicked += OnButtonClicked;
                        Content = new StackLayout
                        {
                            HeightRequest = 100f,
                            Children = {
                                    boton,
                                    customMap
                            }
                        };
                        consultarPosicion(_cts);
                    }else{
                        Label error = new Label(){
                            Text = "Error de conexión.",
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            FontAttributes = FontAttributes.Bold,
                            FontFamily = "Futura-Medium",
                            FontSize = 20,
                            TextColor = Color.FromHex("#498224")
                        };
                        Content = new StackLayout
                        {
                            HeightRequest = 100f,
                            Children = {
                                error 
                            }
                        };
                        await DisplayAlert("Aviso", "No se pudo cargar la ruta", "Aceptar");
                    }
                }else{
                    Label error = new Label()
                    {
                        Text = "Error de conexión.",
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold,
                        FontFamily = "Futura-Medium",
                        FontSize = 20,
                        TextColor = Color.FromHex("#498224")
                    };
                    Content = new StackLayout
                    {
                        HeightRequest = 100f,
                        Children = {
                                error
                            }
                    };
                    await DisplayAlert("Aviso", "No se pudo cargar la ruta", "Aceptar");
                }
            });
            //consultarPosicion();
        }

        async Task consultarPosicion(CancellationTokenSource _cts)
        {

            RestClient cliente = new RestClient();

            Device.BeginInvokeOnMainThread(() => Show(_cts.Token).ContinueWith((arg) => { }));

        }

        public async Task Show(CancellationToken ct)
        {
            while (true)
            {
                //Task.Delay(TimeSpan.FromSeconds(1)).Wait(); // Retardo
                int w = 0;
                for (int i = 0; i <= 10; i++)
                {
                    //Debug.WriteLine(i);
                    w = w + i;
                    int x = 0;
                    for (int j = 0; j <= 1000000; j++)
                    {
                        //Debug.WriteLine(i);
                        x = x + j;
                    }
                }
                try
                {
                    HttpClient client = new HttpClient();
                    RestClient rCli = new RestClient();
                    var formContent = new FormUrlEncodedContent(new[]
                           {
                        //new KeyValuePair<string, string>("idVehiculo", pin.Id),
                        new KeyValuePair<string, string>("idRuta", idRutaAux.ToString()),
                        new KeyValuePair<string, string>("activo","1"),
                    });
                    var respuesta = await client.PostAsync(config.ipPrueba + "/BusGPSWebService/api/vehiculorutacoordenadas", formContent);
                    var jsonRespuesta = respuesta.Content.ReadAsStringAsync();
                    var jsonArmado = "{\"listaUbicaciones\":" + jsonRespuesta.Result + "}";
                    var jsonFinal = jsonRespuesta.Result;
                    Debug.WriteLine(jsonArmado);
                    var jsonCompleto = Newtonsoft.Json.JsonConvert.DeserializeObject<ListUbicacion>(jsonArmado);
                    int count = 0;
                    foreach (Ubicacion ubicacion in jsonCompleto.listaUbicaciones)
                    {

                        double latitud = ubicacion.latitud;
                        double longitud = ubicacion.longitud;
                        arrPines[count].Position = new Position(latitud, longitud);
                        count += 1;
                    }

                    if (ct.IsCancellationRequested)
                    {
                        // another thread decided to cancel
                        Console.WriteLine("Show canceled");
                        break;
                    }
                }
                catch (Exception ex)
                {

                }

                //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitud, longitud), Distance.FromMiles(3.0)));
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (_cts != null)
            {
                _cts.Cancel(); // <---- Cancel here
            }
            // If you want to continue going back
            base.OnBackButtonPressed();
            return false;
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel(); // <---- Cancel here
            }
            Navigation.PopAsync();

        }

        void MostrarDetalle(object sender, EventArgs e)
        {
            Debug.WriteLine("entro");
            CustomPin pin = (CustomPin)sender;
            var newPage = new DetalleCamion(pin.IdVehiculo,idRutaAux);
            //var newPage = new DetalleCamion((double)arrPines[0].Id, (int)idRutaAux);
            Navigation.PushAsync(newPage);
        }
    }
}
