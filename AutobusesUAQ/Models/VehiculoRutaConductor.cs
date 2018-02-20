using System;
using System.Collections.Generic;

namespace AutobusesUAQ.Models
{
    public class VehiculoRutaConductor
    {
        public VehiculoRutaConductor(){}
        public string nombre { get; set; }
        public string apaterno { get; set; }
        public string amaterno { get; set; }
        public string email { get; set; }
        public string rfc { get; set; }
        public string calle { get; set; }
        public string colonia { get; set; }
        public string cp { get; set; }
        public string exterior { get; set; }
        public string interior { get; set; }
        public string domicilioCompleto { get; set; }
        public string telCelular { get; set; }
        public string telCasa { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string anio { get; set; }
        public string placa { get; set; }
    }
    public class ListaVehiculoRutaConductor
    {
        public List<VehiculoRutaConductor> listaVRC { get; set; }
    }

}
