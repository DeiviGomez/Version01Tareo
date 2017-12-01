using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Library.Conexion
{
    /// <summary>
    /// Dominio de WebServices
    /// </summary>
    public class clsConexionWS
    {
        #region Atributos- Connection
        public static Uri UriWebservices
        {
            get { return new Uri("http://localhost:5084/Services/"); }
        }

        public static string UriWebservices_Str
        {
            get { return "http://localhost:5084/Services/"; }
        }
        #endregion
    }
}
