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

        int idRutaAux = 0;
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
            idRutaAux = idRuta;
            Device.BeginInvokeOnMainThread(async () =>
            {
                RestClient cliente = new RestClient();

                var rutas = await cliente.GetRutas<Rutas>("http://189.211.201.181:1200/BusGPSWebService/api/rutascoordenadas/idruta/" + idRuta, "coordenadas");
                //customMap.RouteCoordinates.Clear();
                //customMap.RouteCoordinates.Add(new Position(20.1, 20.1));
                Debug.WriteLine(rutas.coordenadas[0].Latitud);
                customMap.RouteCoordinates.Clear();
                foreach (CoordenadasRuta posicion in rutas.coordenadas)
                {
                    customMap.RouteCoordinates.Add(new Position(posicion.Latitud, posicion.Longitud));
                }

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("idRuta", idRuta.ToString())
                });

                var pines = await cliente.PostRutas<List<Ubicacion>>("http://189.211.201.181:1200/BusGPSWebService/api/vehiculorutacoordenadas","listaUbicaciones",formContent);
                Debug.WriteLine(pines);
                if (pines.Count > 0)
                {
                    foreach (Ubicacion ubic in pines)
                    {
                        CustomPin pin = new CustomPin
                        {
                            Type = PinType.Place,
                            Position = new Position(ubic.latitud, ubic.longitud),
                            Label = "click para ver detalle",
                            Address = "Chofer"+ubic.id,
                            IdVehiculo = ubic.idVehiculo,
                            Color = Color.Blue,

                        };

                        arrPines.Add(pin);
                    }
                }
                //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                //new Position(20.5923831, -100.4113046), Distance.FromMiles(4.0)));
                customMap.Items = new ObservableCollection<CustomPin> { };
                foreach (CustomPin pins in arrPines)
                {
                    customMap.Items.Add(pins);
                    customMap.Pins.Add(pins);
                    pins.Clicked += MostrarDetalle;
                }


                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(20.5923831, -100.4113046), Distance.FromMiles(4.0)));
                Content = new StackLayout
                {
                    HeightRequest = 100f,
                    Children = {
                    //boton,
                        customMap
                }
                };
                //boton.Clicked += OnButtonClicked;
                consultarPosicion();
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

            Device.BeginInvokeOnMainThread(async () =>
            {

                while(true)
                {
                    //Task.Delay(TimeSpan.FromSeconds(1)).Wait(); // Retardo
                    int w = 0;
                    for (int i = 0; i <= 1000;i++){
                        //Debug.WriteLine(i);
                        w = w + i;
                    }
                    HttpClient client = new HttpClient();
                    RestClient rCli = new RestClient();
                    var formContent = new FormUrlEncodedContent(new[]
                           {
                        //new KeyValuePair<string, string>("idVehiculo", pin.Id),
                        new KeyValuePair<string, string>("idRuta", idRutaAux.ToString()),
                        new KeyValuePair<string, string>("activo","1"),
                    });
                    var respuesta = await client.PostAsync("http://189.211.201.181:1200/BusGPSWebService/api/vehiculorutacoordenadas", formContent);
                    var jsonRespuesta = respuesta.Content.ReadAsStringAsync();
                    var jsonArmado = "{\"listaUbicaciones\":" + jsonRespuesta.Result + "}";
                    var jsonFinal = jsonRespuesta.Result;
                    Debug.WriteLine(jsonArmado);
                    var jsonCompleto = Newtonsoft.Json.JsonConvert.DeserializeObject<ListUbicacion>(jsonArmado);
                    int count = 0;
                    foreach(Ubicacion ubicacion in jsonCompleto.listaUbicaciones){
                        
                        double latitud = ubicacion.latitud;
                        double longitud = ubicacion.longitud;
                        arrPines[count].Position = new Position(latitud, longitud);    
                        count += 1;
                    }

                    //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitud, longitud), Distance.FromMiles(3.0)));
                }
            });


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
