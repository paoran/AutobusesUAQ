using System;
using System.Collections.Generic;

namespace AutobusesUAQ.Models
{
    public class Choferes
    {
        public Choferes() { }
        public int id { get; set; }
        public string nombre { get; set; }
        public string apaterno { get; set; }
        public string amaterno { get; set; }
        public string nombreCompleto { get; set; }
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
    }
    public class ListaChoferes
    {
        public List<Choferes> listaChoferes { get; set; }
    }
}
