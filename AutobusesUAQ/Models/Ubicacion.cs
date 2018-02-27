using System;
using System.Collections.Generic;

namespace AutobusesUAQ.Models
{
    public class Ubicacion
    {
        public int id { get; set; }
        public int idVehiculo { get; set; }
        public double idRuta { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }


    }
    public class ListUbicacion
    {
        public List<Ubicacion> listaUbicaciones { get; set; }
    }
}
