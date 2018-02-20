using System;
using System.Collections.Generic;

namespace AutobusesUAQ.Models
{
    public class Ubicacion
    {
        public Ubicacion()
        {
        }

        public double latitud { get; set; }
        public double longitud { get; set; }
        //public double idGPS { get; set; }
        //public double idSIM { get; set; }
        //public double idCamion { get; set; }

    }
    public class ListUbicacion
    {
        public List<Ubicacion> listaUbicaciones { get; set; }
    }
}
