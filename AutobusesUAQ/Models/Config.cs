using System;
namespace AutobusesUAQ.Models
{
    public class Config
    {
        public Config()
        {
            this.ipPrueba = "http://148.240.202.160:1200";
            this.ipProduccion = "http://148.240.202.160:1200";
        }
        public string ipPrueba { get; set; }
        public string ipProduccion { get; set; }
    }
}
