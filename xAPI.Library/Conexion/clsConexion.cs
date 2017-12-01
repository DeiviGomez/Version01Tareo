using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using xAPI.Library.General;
using System.Xml;
using xAPI.Library.Security;
using System.Configuration;
using System.Data;

namespace xAPI.Library.Conexion
{
    /// <summary>
    /// Clase Conexion para la BD y Atributos para la Cadena de Conexion.
    /// </summary>
    public class clsConexion
    {

        #region Atributos-DBConnection
        public static string DbMax
        {
            get { return "Data Source=localhost;Initial Catalog=xirectbd;Integrated Security=True"; }
        }
        public static string DbAndrez
        {
            get { return "Data Source=.;Initial Catalog=XirectBD;Integrated Security=True"; }
            //get { return "Data Source=66.85.130.178;Initial Catalog=xMLM;User ID=sa;Password=13011970"; }

        }
        public static string DbSever
        {
            get { return "Data Source=Server-PC\\XIRECT;Initial Catalog=XirectDB;Integrated Security=True"; }
        }
        #endregion
        #region Conexion
        public static String GetDbName()
        {
            String dbName = "";

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(HttpContext.Current.Server.MapPath(@"~\Resources\constrings\ConStrings.xml"));//la ubicación del archivo XML con el que vamos a trabajar
                XmlNodeList conexion = xDoc.GetElementsByTagName("connections");
                XmlNodeList listaDatos = ((XmlElement)conexion[0]).GetElementsByTagName("connection");
                foreach (XmlElement nodo in listaDatos) //obtenemos el valor de cada uno de los nodos en la lista
                {
                    dbName = nodo.GetElementsByTagName("catalog")[0].InnerText;
                }
            }
            catch (Exception)
            {
                dbName = "";
            }

            return dbName;
        }

        public static SqlConnection ObtenerConexion(String DbConnection)
        {
            SqlConnection objConexion = new SqlConnection(DbConnection);

            objConexion.Open();
            return objConexion;
        }

        /// <summary>
        /// MODE 
        /// </summary>
        /// <returns></returns>
        public static SqlConnection ObtenerConexion()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["planillas_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["planillas_db"].ConnectionString;
            }


#if Debug
            if (ConfigurationManager.ConnectionStrings["asea_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db"].ConnectionString;
            }
#else
            //foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            //{
            //    string name = css.Name;
            //    string provider = css.ProviderName;

            //    if (IsValidConnString(css.ConnectionString))
            //    {
            //        connString = css.ConnectionString;
            //        break;
            //    }
            //}
#endif

            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }


        public static SqlConnection GetEngageCommissionsConnection()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["asea_db_engcomm"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_engcomm"].ConnectionString;
            }


#if Debug
            if (ConfigurationManager.ConnectionStrings["asea_db_engcomm"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_engcomm"].ConnectionString;
            }
#else
            //foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            //{
            //    string name = css.Name;
            //    string provider = css.ProviderName;

            //    if (IsValidConnString(css.ConnectionString))
            //    {
            //        connString = css.ConnectionString;
            //        break;
            //    }
            //}
#endif

            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }


        public static SqlConnection ObtenerConexion_Token()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["asea_db_token"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_token"].ConnectionString;
            }


#if Debug
            if (ConfigurationManager.ConnectionStrings["asea_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db"].ConnectionString;
            }
#else
            //foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            //{
            //    string name = css.Name;
            //    string provider = css.ProviderName;

            //    if (IsValidConnString(css.ConnectionString))
            //    {
            //        connString = css.ConnectionString;
            //        break;
            //    }
            //}
#endif

            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }
        public static SqlConnection GetConexioncom()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["asea_db_com"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_com"].ConnectionString;
            }


#if Debug
            if (ConfigurationManager.ConnectionStrings["asea_db_com"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_com"].ConnectionString;
            }
#else
            //foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            //{
            //    string name = css.Name;
            //    string provider = css.ProviderName;

            //    if (IsValidConnString(css.ConnectionString))
            //    {
            //        connString = css.ConnectionString;
            //        break;
            //    }
            //}
#endif

            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }
        /// <summary>
        /// Val Connection String
        /// </summary>
        /// <returns></returns>
        public static bool IsValidConnString(String ConnectionString)
        {
            Boolean success = true;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    if (conn.State != ConnectionState.Open)
                        success = false;
                    else
                        success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                }
            }
            return success;
        }




        public static SqlConnection ObtenerConexion_Dating()
        {

            SqlConnection objConexion = new SqlConnection(ConnectionString_FileXml());

            objConexion.Open();
            return objConexion;
        }
        public static string ConnectionString_FileXml()
        {
            string cadenaConexion = "";
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(HttpContext.Current.Server.MapPath(@"~\Resources\constrings\ConStrings.xml"));//la ubicación del archivo XML con el que vamos a trabajar
                XmlNodeList conexion = xDoc.GetElementsByTagName("connections");
                XmlNodeList listaDatos = ((XmlElement)conexion[0]).GetElementsByTagName("connection");
                foreach (XmlElement nodo in listaDatos) //obtenemos el valor de cada uno de los nodos en la lista
                {
                    XmlNodeList ndatasource = nodo.GetElementsByTagName("datasource");
                    XmlNodeList ncatalogr = nodo.GetElementsByTagName("catalog");
                    XmlNodeList nid = nodo.GetElementsByTagName("id");
                    XmlNodeList npassword = nodo.GetElementsByTagName("password");
                    if (!String.IsNullOrEmpty(nid[0].InnerText) && !String.IsNullOrEmpty(npassword[0].InnerText))
                        cadenaConexion = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ndatasource[0].InnerText, ncatalogr[0].InnerText, nid[0].InnerText, npassword[0].InnerText);
                    // cadenaConexion = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}",  clsEncryption.Decrypt(ndatasource[0].InnerText), clsEncryption.Decrypt(ncatalogr[0].InnerText),clsEncryption.Decrypt(nid[0].InnerText),clsEncryption.Decrypt(npassword[0].InnerText));
                    else
                        cadenaConexion = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", ndatasource[0].InnerText, ncatalogr[0].InnerText);
                    //cadenaConexion = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", clsEncryption.Decrypt(ndatasource[0].InnerText), clsEncryption.Decrypt(ncatalogr[0].InnerText));
                }
            }
            catch (Exception)
            {
                cadenaConexion = "";
            }
            return cadenaConexion;
        }

        public static SqlConnection ObtenerConexionDNN()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["dnn_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["dnn_db"].ConnectionString;
            }


#if Debug
            if (ConfigurationManager.ConnectionStrings["asea_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db"].ConnectionString;
            }
#else
            //foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            //{
            //    string name = css.Name;
            //    string provider = css.ProviderName;

            //    if (IsValidConnString(css.ConnectionString))
            //    {
            //        connString = css.ConnectionString;
            //        break;
            //    }
            //}
#endif

            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }

        public static SqlConnection ObtenerConexionDNNGlobal()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["dnn_db_aseaglobal"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["dnn_db_aseaglobal"].ConnectionString;
            }


#if Debug
            if (ConfigurationManager.ConnectionStrings["asea_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db"].ConnectionString;
            }
#else
            //foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            //{
            //    string name = css.Name;
            //    string provider = css.ProviderName;

            //    if (IsValidConnString(css.ConnectionString))
            //    {
            //        connString = css.ConnectionString;
            //        break;
            //    }
            //}
#endif

            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }

        #endregion
    }
}
