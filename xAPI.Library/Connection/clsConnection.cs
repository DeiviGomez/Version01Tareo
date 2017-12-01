using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;


using System.Diagnostics;
using System.Data;

namespace xAPI.Library.Connection
{
    public class clsConnection
    {

        public static void DisposeCommand(SqlCommand cmd)
        {
            try
            {
                if (cmd != null)
                {
                    if (cmd.Connection != null)
                    {
                        cmd.Connection.Close();
                        cmd.Connection.Dispose();
                    }
                    cmd.Dispose();
                }
            }
            catch { } //don't blow up
        }
        public static void DisposeCommand(SqlConnection conect)
        {
            try
            {

                if (conect != null)
                {
                    conect.Close();
                    conect.Dispose();
                }

            }
            catch { } //don't blow up
        }
        public static SqlConnection GetCommisionConnection()
        {
            SqlConnection objConnection = null;
            String connString = null;

            if (ConfigurationManager.ConnectionStrings["asea_db_commission"] != null)
                connString = ConfigurationManager.ConnectionStrings["asea_db_commission"].ConnectionString;

            if (String.IsNullOrEmpty(connString)) return objConnection;

            objConnection = new SqlConnection(connString);
            objConnection.Open();
            return objConnection;
        }

        public static SqlConnection GetSoxialConnection()
        {
            SqlConnection objConnection = null;
            String connString = null;

            if (ConfigurationManager.ConnectionStrings["soxial_db"] != null)
                connString = ConfigurationManager.ConnectionStrings["soxial_db"].ConnectionString;

            if (String.IsNullOrEmpty(connString)) return objConnection;

            objConnection = new SqlConnection(connString);
            objConnection.Open();
            return objConnection;
        }

        public static SqlConnection GetSurveyConnection(){
            SqlConnection objConnection = null;
            String connString = null;
            if(ConfigurationManager.ConnectionStrings["asea_db_surveys"]!=null)
                connString = ConfigurationManager.ConnectionStrings["asea_db_surveys"].ConnectionString;
            if(String.IsNullOrEmpty(connString)) return objConnection;

            objConnection=new SqlConnection(connString);
            objConnection.Open();
            return objConnection;
        }
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

        public static SqlConnection GetConnection(String DbConnection)
        {
            SqlConnection objConexion = new SqlConnection(DbConnection);

            objConexion.Open();
            return objConexion;
        }
 

        public static SqlConnection GetConnection_Live()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["asea_db_live"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_live"].ConnectionString;
            }




            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }
        /// <summary>
        /// MODE 
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["planillas_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["planillas_db"].ConnectionString;
            }
            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }


        public static SqlConnection GetConnectionInfoTrax()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["asea_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db"].ConnectionString;
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


        public static SqlConnection GetTokenConnection() 
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["asea_db_token"] != null) 
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_token"].ConnectionString;
            }
            SqlConnection objConexion = new SqlConnection(connString);
            objConexion.Open();
            return objConexion;
        }

        public static SqlConnection GetConnectionCom()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["asea_db_com"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_com"].ConnectionString;
            }
            if (ConfigurationManager.ConnectionStrings["asea_db_commission"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_commission"].ConnectionString;
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

        public static SqlConnection GetConnectionLive()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["asea_db_live"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_live"].ConnectionString;
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
        /// <summary>
        /// MODE 
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnectionJob()
        {
            String connString = String.Empty;
            //#if DEBUG
            if (ConfigurationManager.ConnectionStrings["asea_db4"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db4"].ConnectionString;
            }
            //#else
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
            //#endif
            SqlConnection objConexion = new SqlConnection(connString);
            objConexion.Open();
            return objConexion;
        }
        public static SqlConnection GetConnectionInfotrax()
        {
            String connString = String.Empty;
            //#if DEBUG
            if (ConfigurationManager.ConnectionStrings["asea_db_infotrax"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_infotrax"].ConnectionString;
            }
            //#else
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
            //#endif
            SqlConnection objConexion = new SqlConnection(connString);
            objConexion.Open();
            return objConexion;
        }
        public static SqlConnection GetConnectionAsea()
        {
            String connString = String.Empty;
            //#if DEBUG
            if (ConfigurationManager.ConnectionStrings["asea_db5"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db5"].ConnectionString;
            }
            //#else
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
            //#endif
            SqlConnection objConexion = new SqlConnection(connString);
            objConexion.Open();
            return objConexion;
        }
        public static SqlConnection GetConnectionAseaxAsea()
        {
            String connString = String.Empty;
            //#if DEBUG
            if (ConfigurationManager.ConnectionStrings["asea_db2"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db2"].ConnectionString;
            }
            //#else
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
            //#endif
            SqlConnection objConexion = new SqlConnection(connString);
            objConexion.Open();
            return objConexion;
        }
        public static SqlConnection GetConnectionXirectCom()
        {
            String connString = String.Empty;
            //#if DEBUG
            if (ConfigurationManager.ConnectionStrings["asea_db_xsscom"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_xsscom"].ConnectionString;
            }
            //#else
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
            //#endif
            SqlConnection objConexion = new SqlConnection(connString);
            objConexion.Open();
            return objConexion;
        }
        
        public static SqlConnection GetConnectionBonus()
        {
            String connString = String.Empty;
            String connStringtmp = String.Empty;
            //#if DEBUG
            String conect = "asea_db2";
            String conect2 = "asea_db3";
            Boolean online = false;
          
            if (ConfigurationManager.ConnectionStrings[conect] != null)
            {
                connStringtmp = ConfigurationManager.ConnectionStrings[conect].ConnectionString;
                if (IsValidConnString(connStringtmp))
                {
                    connString = connStringtmp;
                    online = true;
                }
            }
            if (!online) {
                if (ConfigurationManager.ConnectionStrings[conect2] != null)
                {
                    connStringtmp = ConfigurationManager.ConnectionStrings[conect2].ConnectionString;
                    if (IsValidConnString(connStringtmp))
                    {
                        connString = connStringtmp;
                        online = true;
                    }
                }
            }
          
            SqlConnection objConexion = new SqlConnection(connString);
            objConexion.Open();
            return objConexion;
        }
        public static SqlConnection GetConnectionBonus_Prod()
        {
            String connString = String.Empty;
            String connStringtmp = String.Empty;
            //#if DEBUG
            String conect = "asea_db_prod";
         //   String conect2 = "asea_db_prod";
            Boolean online = false;

            if (ConfigurationManager.ConnectionStrings[conect] != null)
            {
                connStringtmp = ConfigurationManager.ConnectionStrings[conect].ConnectionString;
                if (IsValidConnString(connStringtmp))
                {
                    connString = connStringtmp;
                    online = true;
                }
            }
            //if (!online)
            //{
            //    if (ConfigurationManager.ConnectionStrings[conect2] != null)
            //    {
            //        connStringtmp = ConfigurationManager.ConnectionStrings[conect2].ConnectionString;
            //        if (IsValidConnString(connStringtmp))
            //        {
            //            connString = connStringtmp;
            //            online = true;
            //        }
            //    }
            //}

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


        public static SqlConnection GetConnection_Soxial()
        {
            String connString = "";

            if (ConfigurationManager.ConnectionStrings["soxial_db"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["soxial_db"].ConnectionString;
            }

            SqlConnection objConexion = new SqlConnection(connString);

            objConexion.Open();
            return objConexion;
        }
        public static SqlConnection GetConnection_Dating()
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
        #endregion

        public static SqlConnection GetConnectionAseaProd()
        {
            String connString = String.Empty;
            //#if DEBUG
            if (ConfigurationManager.ConnectionStrings["asea_db_prod"] != null)
            {
                connString = ConfigurationManager.ConnectionStrings["asea_db_prod"].ConnectionString;
            }
            //#else
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
            //#endif
            SqlConnection objConexion = new SqlConnection(connString);
            objConexion.Open();
            return objConexion;
        }

       
    }
}
