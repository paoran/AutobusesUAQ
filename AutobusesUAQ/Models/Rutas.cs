using System;
using System.Collections.Generic;

namespace AutobusesUAQ.Models
{
    public class Rutas
    {
        public int id { get; set; }
        public string derrotero { get; set; }
        public string puntoInicio { get; set; }
        public string pintoFin { get; set; }
        public int frecuencia { get; set; }
        public bool activo { get; set; }

        public List<Rutas> lstRutas { get; set; }

        public List<CoordenadasRuta> coordenadas { get; set; }
    }
}
