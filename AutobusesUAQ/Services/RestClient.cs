using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutobusesUAQ.Services
{
    public class RestClient
    {
        public RestClient()
        {
        }
        public async Task<T> Get<T>(string url, string nombre)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(10000);
                var respuesta = await cliente.GetAsync(url);
                //Debug.Write(url);
                //if (                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               == System.Net.HttpStatusCode.OK)
                //{
                var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                var jsonArmado = "{\"" + nombre + "\":" + jsonRespuesta + "}";
                //var jsonNuevo = "{\"id\":1,\"derrotero\":\"camion1\",\"puntoInicio\":\"rectoria\",\"puntoFin\":\"CU\",\"frecuencia\":\"20 min\",\"" + nombre + "\":[{\"idCoordenadasRuta\":1, \"latitud\":20.5923831,\"longitud\":\"-100.4113046\",\"idRuta\":2},{\"idCoordenadasRuta\":2, \"latitud\":20.6208049,\"longitud\":\"-100.4213312\",\"idRuta\":2},{\"idCoordenadasRuta\":3, \"latitud\":20.6143688,\"longitud\":\"-100.3878606\",\"idRuta\":2},{\"idCoordenadasRuta\":4, \"latitud\":20.6303242,\"longitud\":\"-100.351588\",\"idRuta\":2}]}";
                Debug.WriteLine(jsonArmado);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                //}else{
                //    var jsonNuevo = "{\"" + nombre + "\":[{\"latitud\":20.5923831,\"longitud\":\"-100.4113046\"}]}";
                //    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonNuevo);
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
                return default(T);
            }
            //return default(T);
        }

        public async Task<T> GetRutas<T>(string url, string nombre)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(10000);
                var respuesta = await cliente.GetAsync(url);
                //Debug.Write(url);
                //if (                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               == System.Net.HttpStatusCode.OK)
                //{
                var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                var jsonArmado = "{'" + nombre + "':[" + jsonRespuesta + "]}";
                //var jsonNuevo = "{\"id\":1,\"derrotero\":\"camion1\",\"puntoInicio\":\"rectoria\",\"puntoFin\":\"CU\",\"frecuencia\":\"20 min\",\"" + nombre + "\":[{\"idCoordenadasRuta\":1, \"latitud\":20.5923831,\"longitud\":\"-100.4113046\",\"idRuta\":2},{\"idCoordenadasRuta\":2, \"latitud\":20.6208049,\"longitud\":\"-100.4213312\",\"idRuta\":2},{\"idCoordenadasRuta\":3, \"latitud\":20.6143688,\"longitud\":\"-100.3878606\",\"idRuta\":2},{\"idCoordenadasRuta\":4, \"latitud\":20.6303242,\"longitud\":\"-100.351588\",\"idRuta\":2}]}";
                Debug.WriteLine(jsonArmado);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonRespuesta);
                //}else{
                //    var jsonNuevo = "{\"" + nombre + "\":[{\"latitud\":20.5923831,\"longitud\":\"-100.4113046\"}]}";
                //    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonNuevo);
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
                return default(T);
            }
            //return default(T);
        }

        public async Task<T> PostRutas<T>(string url, string nombre, FormUrlEncodedContent datos)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(10000);
                var respuesta = await cliente.PostAsync(url, datos);
                var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                var jsonArmado = "[" + jsonRespuesta.ToString() + "]";
                Debug.WriteLine(jsonArmado);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonRespuesta);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
                return default(T);
            }
        }
        public async Task<T> convertirJson<T>(string json)
        {
            try
            {
                var jsonArmado = "{'listaVRC':" + json + "}";
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }
        public async Task<T> GetChoferes<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                var respuesta = await cliente.GetAsync(url);
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    var jsonArmado = "{'listaChoferes':" + jsonRespuesta + "}";
                    Debug.WriteLine(jsonArmado);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
                else
                {
                    var jsonArmado = "{'listaChoferes':[]}";
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }
    }
}
