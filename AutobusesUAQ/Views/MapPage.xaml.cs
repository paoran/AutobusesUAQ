using System;
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
        Button boton = new Button
        {
            Text = "Mover!",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };


        double lat = 20.5923831;

        CustomMap customMap = new CustomMap
        {
            MapType = MapType.Street,
            WidthRequest = 50,//App.ScreenWidth,
            HeightRequest = 50//App.ScreenHeight
        };



        List<CustomPin> arrPines = new List<CustomPin>();

        public MapPage(int idRuta)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                RestClient cliente = new RestClient();

                var rutas = await cliente.GetRutas<Rutas>("http://189.211.201.181:1200/BusGPSWebService/api/rutascoordenadas/idruta/" + idRuta, "coordenadas");
                customMap.RouteCoordinates.Clear();
                customMap.RouteCoordinates.Add(new Position(20.1, 20.1));
                Debug.WriteLine(rutas.coordenadas[0].Latitud);
                customMap.RouteCoordinates.Clear();
                foreach (CoordenadasRuta posicion in rutas.coordenadas)
                {
                    customMap.RouteCoordinates.Add(new Position(posicion.Latitud, posicion.Longitud));
                }

                CustomPin pin = new CustomPin
                {
                    Type = PinType.Place,
                    Position = new Position(20.5923831, -100.4113046),
                    Label = "Autobus Nuevo",
                    Address = "Chofer",
                    Id = "2",
                    Color = Color.Blue,

                };

                CustomPin pin2 = new CustomPin
                {
                    Type = PinType.Place,
                    Position = new Position(20.6, -100.411305),
                    Label = "Autobus anterior",
                    Address = "Chofer",
                    Id = "1",
                    Color = Color.Black,

                };

                arrPines.Add(pin);
                arrPines.Add(pin2);
                //InitializeComponent();
                //ObservableCollection<CustomPin> listaPin = new ObservableCollection<CustomPin>{};
                //pin.Clicked += MostrarDetalle;
                //listaPin.Add(pin);
                pin.Clicked += MostrarDetalle;
                customMap.Items = new ObservableCollection<CustomPin> { pin };
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
                        //customMap.CustomPins.CollectionChanged += UpdatePinsOnMap;

                        Content = customMap;
                    }
                })).Start();
                foreach (CustomPin pins in arrPines)
                {
                    pins.Clicked += MostrarDetalle;
                    customMap.Items = new ObservableCollection<CustomPin> { pins };
                    customMap.Pins.Add(pins);
                }


                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(20.5923831, -100.4113046), Distance.FromMiles(1.0)));




                Content = new StackLayout
                {
                    HeightRequest = 100f,
                    Children = {
                    boton,
                        customMap
                }
                };
                boton.Clicked += OnButtonClicked;
            });
            //consultarPosicion();
        }

        void OnButtonClicked(object sender, EventArgs e)
        {

            consultarPosicion();
        }

        async Task consultarPosicion()
        {

            RestClient cliente = new RestClient();
            lat = 20.5923831;




            Device.BeginInvokeOnMainThread(async () =>
            {

                for (int i = 0; i < 10; i++)
                {
                    HttpClient client = new HttpClient();
                    var formContent = new FormUrlEncodedContent(new[]
                           {
                        //new KeyValuePair<string, string>("idVehiculo", pin.Id),
                        //new KeyValuePair<string, string>("idRuta", idRuta),
                    new KeyValuePair<string, string>("activo","1"),
                });
                    var respuesta = await client.PostAsync("http://189.211.201.181:1200/BusGPSWebService/api/vehiculorutacoordenadas", formContent);
                    var jsonRespuesta = respuesta.Content.ReadAsStringAsync();
                    var jsonArmado = "{\"ListUbicacion\":" + jsonRespuesta.Result + "}";
                    Debug.WriteLine(jsonArmado);
                    Debug.WriteLine(jsonArmado);
                    var jsonCompleto = Newtonsoft.Json.JsonConvert.DeserializeObject<ListUbicacion>(jsonArmado);
                    double latitud = lat + (i * 0.01);
                    Task.Delay(1000).Wait(); // Retardo
                    arrPines[1].Position = new Position(latitud, -100.4113046);
                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitud, -100.4113046), Distance.FromMiles(1.0)));
                }
            });


        }

        void MostrarDetalle(object sender, EventArgs e)
        {
            Pin pin = (Pin)sender;

            var newPage = new DetalleCamion();
            Navigation.PushAsync(newPage);
        }
    }
}
