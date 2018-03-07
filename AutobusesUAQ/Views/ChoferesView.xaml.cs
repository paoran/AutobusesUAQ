using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutobusesUAQ.Models;
using AutobusesUAQ.Services;
using Xamarin.Forms;

namespace AutobusesUAQ
{
    public partial class ChoferesView : ContentPage
    {
        public ObservableCollection<Choferes> lchoferes { get; set; }
        public ChoferesView()
        {
            InitializeComponent();
            cargaChoferes();
        }

        async void cargaChoferes()
        {
            etiquetaCargando.Text = "Cargando choferes, por favor espere...";
            svChoferes.Content = etiquetaCargando;

            Device.BeginInvokeOnMainThread(async () =>
            {
                RestClient cliente = new RestClient();

                var choferesResp = await cliente.GetChoferes<ListaChoferes>("/BusGPSWebService/api/choferes");
                if (choferesResp != null)
                {
                    if (choferesResp.listaChoferes.Count > 0)
                    {
                        lchoferes = new ObservableCollection<Choferes>();
                        foreach (var chofer in choferesResp.listaChoferes)
                        {
                            lchoferes.Add(new Choferes
                            {
                                nombre = chofer.nombre,
                                apaterno = chofer.apaterno,
                                amaterno = chofer.amaterno,
                                rfc = chofer.rfc,
                                nombreCompleto = chofer.nombre+" "+chofer.apaterno+" "+chofer.amaterno,
                                email = chofer.email,
                                domicilioCompleto = chofer.calle + ", Ext. " + chofer.exterior + ", Int. " + chofer.interior + ", Col. " + chofer.colonia + ", C.P.: " + chofer.cp,
                                telCelular = chofer.telCelular,
                                telCasa = chofer.telCasa

                            });
                        }
                        listaChoferes.ItemsSource = lchoferes;
                        svChoferes.Content = listaChoferes;
                    }
                    else
                    {
                        etiquetaCargando.Text = "No se encontraron choferes.";
                        svChoferes.Content = etiquetaCargando;
                    }
                }
                else
                {
                    etiquetaCargando.Text = "Error de conexión.";
                    svChoferes.Content = etiquetaCargando;
                }
            });
        }
    }
}
