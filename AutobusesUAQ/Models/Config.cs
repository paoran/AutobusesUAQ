using System;
namespace AutobusesUAQ.Models
{
    public class Config
    {
        public Config()
        {
            this.ipPrueba = "http://148.220.4.60/";
            this.ipProduccion = "http://148.220.4.60/";
        }
        public string ipPrueba { get; set; }
        public string ipProduccion { get; set; }
    }
}
