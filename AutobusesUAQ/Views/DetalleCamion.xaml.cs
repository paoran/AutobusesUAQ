using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using AutobusesUAQ.Models;
using AutobusesUAQ.Services;
using Xamarin.Forms;

namespace AutobusesUAQ.Views
{
    public partial class DetalleCamion : ContentPage
    {
        public DetalleCamion(int idvehiculo,int idRuta)
        {
            InitializeComponent();
            cargaDetalleCamion(idRuta,idvehiculo);
        }

        async void cargaDetalleCamion(int idRuta, int idVehiculo){
            Config config = new Config();
            etiquetaCargando.Text = "Cargando detalle, por favor espere...";
            svDetalle.Content = etiquetaCargando;
            HttpClient cliente = new HttpClient();
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("idRuta",idRuta.ToString()),
                new KeyValuePair<string, string>("idVehiculo",idVehiculo.ToString())
            });
            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync(config.ipPrueba+"/BusGPSWebService/api/vehiculorutaconductor", formContent);
            var json = await response.Content.ReadAsStringAsync();
            RestClient c = new RestClient();
            var detalleCamion = await c.convertirJson<ListaVehiculoRutaConductor>(json);
            if (response.IsSuccessStatusCode)
            { 
                if(detalleCamion != null){
                    if (detalleCamion.listaVRC.Count > 0)
                    {
                        List<VehiculoRutaConductor> vrc = new List<VehiculoRutaConductor> {
                            new VehiculoRutaConductor{
                                apaterno = detalleCamion.listaVRC[0].apaterno,
                                amaterno = detalleCamion.listaVRC[0].amaterno,
                                email = detalleCamion.listaVRC[0].email,
                                rfc = detalleCamion.listaVRC[0].rfc,
                                domicilioCompleto = detalleCamion.listaVRC[0].calle +", Ext. "+detalleCamion.listaVRC[0].exterior+", Int. "+detalleCamion.listaVRC[0].interior+", Col. "+detalleCamion.listaVRC[0].colonia+", C.P.: "+detalleCamion.listaVRC[0].cp,
                                telCelular = detalleCamion.listaVRC[0].telCelular,
                                telCasa = detalleCamion.listaVRC[0].telCasa,
                                marca = detalleCamion.listaVRC[0].marca,
                                modelo = detalleCamion.listaVRC[0].modelo,
                                anio = detalleCamion.listaVRC[0].anio,
                                placa = detalleCamion.listaVRC[0].placa
                            }
                        };
                        Detalle.ItemsSource = vrc;
                        svDetalle.Content = Detalle;
                    }else{
                        //await DisplayAlert("Información", "No se encontró información.", "Aceptar");  
                        etiquetaCargando.Text = "No se encontró el detalle.";
                        svDetalle.Content = etiquetaCargando;
                    }
                } else{
                    //await DisplayAlert("Información", "No se encontró información.", "Aceptar");  
                    etiquetaCargando.Text = "No se encontró el detalle.";
                    svDetalle.Content = etiquetaCargando;
                }
            } else{
                //await DisplayAlert("Información", "No se encontró información.", "Aceptar");
                etiquetaCargando.Text = "Error de conexión.";
                svDetalle.Content = etiquetaCargando;
            }
        }
    }
}
