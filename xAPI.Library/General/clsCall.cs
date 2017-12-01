using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using xAPI.Library.Conexion;

namespace xAPI.Library.General
{
    /// <summary>
    /// Metodo de llamadas a WebServices Via POST
    /// </summary>
    public static class clsCall
    {
        /// <summary>
        /// Llamada Post con Envio de Parametros
        /// </summary>
        /// <typeparam name="T">Tipo de Dato que Recibe el Metodo Web Services. Ejm: clsUser,object[]</typeparam>
        /// <param name="obj">Parametro extensor de envio de Data al Webservices</param>
        /// <param name="method">Ejm: "xApi.svc/Login"</param>
        /// <returns>retorna un xml concatenado en String</returns>
        public static string Call<T>(this object obj, string method)
        {
            string xml = "";

            try
            {
                Uri url = new Uri(clsConexionWS.UriWebservices_Str + method);
                T data = (T)obj;
                //using (HttpResponseMessage response = new HttpClient().Post(url, HttpContentExtensions.CreateDataContract(data)))
                //{
                //    using (StreamReader rd = new StreamReader(response.Content.ReadAsStream()))
                //    {
                //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //        {

                //            xml = rd.ReadToEnd();
                //        }
                //    }
                //}
            }
            catch { xml = ""; }
            return xml;
        }
        /// <summary>
        /// Llama Post sin envio de Parametros
        /// </summary>
        /// <param name="method">Ejm: "xApi.svc/Login"</param>
        /// <returns>retorna un xml concatenado en String</returns>
        public static string Call(string method)
        {
            string xml = "";
            try
            {
                Uri url = new Uri(clsConexionWS.UriWebservices_Str + method);
                //using (HttpResponseMessage response = new HttpClient().Post(url, HttpContent.Create("")))
                //{
                //    using (StreamReader rd = new StreamReader(response.Content.ReadAsStream()))
                //    {
                //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //        {

                //            xml = rd.ReadToEnd();
                //        }
                //    }
                //}
            }
            catch { xml = ""; }
            return xml;
        }
    }
}
