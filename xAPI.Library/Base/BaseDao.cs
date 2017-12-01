using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Library.Conexion;
using System.Data.SqlClient;
using System.Data;
using xAPI.Library.General;

using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Collections;
using System.Data.SqlTypes;
using xAPI.Library.Connection;

namespace xAPI.Library.Base
{
    
    /// <summary>
    /// This is the Parent class for all the data access layer's classes without exception.
    /// </summary>
    public class BaseDao
    {
        public static string DbConnection
        {
            get { return clsConexion.DbMax; }
        }

        //#region Singleton
        //private static BaseDao instance = null;
        //public static BaseDao Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //            instance = new BaseDao();
        //        return instance;
        //    }
        //}
        //#endregion


        public static String GetColumnsWithNoData(List<String> infoColumn)
        {
            String stringJson = "{";

            foreach (String column in infoColumn)
            {
                String PropertyJson = "\"" + column + "\":null,";
                stringJson += PropertyJson;
            }

            stringJson = stringJson.Substring(0, stringJson.Length - 1) + "}";

            return stringJson;
        }

        public static String GetEntity(SqlDataReader ObjDr, List<String> infoColumn)
        {
            Object obj = new Object();

            String stringJson = "{";

            foreach (String column in infoColumn)
            {
                String PropertyJson = "";

                if(ObjDr[column] is DateTime)
                    PropertyJson = "\"" + column + "\":\"" + Convert.ToDateTime(ObjDr[column].ToString()).ToString("MM/dd/yyyy") + "\",";
                else
                    PropertyJson = "\"" + column + "\":\"" + ObjDr[column].ToString().Trim() + "\",";
                stringJson += PropertyJson;
            }

            stringJson = stringJson.Substring(0, stringJson.Length - 1) + "}";

            return stringJson;
        }
        public static Dictionary<string, object> GetEntityObject(SqlDataReader ObjDr, List<String> infoColumn)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            String key, value;
           // String stringJson = "{";

            foreach (String column in infoColumn)
            {
                //String PropertyJson = "";

                if (ObjDr[column] is DateTime)
                {
                    key = column;
                    value = Convert.ToDateTime(ObjDr[column].ToString()).ToString("MM/dd/yyyy");
                    //PropertyJson = "\"" + column + "\":\"" + Convert.ToDateTime(ObjDr[column].ToString()).ToString("MM/dd/yyyy") + "\",";
                    dictionary.Add(key, value);
                }
                else
                {
                    key = column;
                    value = ObjDr[column].ToString().Trim();
                    //PropertyJson = "\"" + column + "\":\"" + ObjDr[column].ToString().Trim() + "\",";
                    dictionary.Add(key, value);
                }
                //stringJson += PropertyJson;
            }
           
           // stringJson = stringJson.Substring(0, stringJson.Length - 1) + "}";
            return dictionary;//ObjectsUtilities.ToObject<object>(dictionary);
        }
        
        public static String GetEntityEmpty(List<String> infoColumn)
        {
            Object obj = new Object();

            String stringJson = "{";

            foreach (String column in infoColumn)
            {
                String PropertyJson = "";
                PropertyJson = "\"" + column + "\":\"EMPTY\",";
                stringJson += PropertyJson;
            }

            stringJson = stringJson.Substring(0, stringJson.Length - 1) + "}";

            return stringJson;
        }


        public static String GetRows(SqlDataReader ObjDr)
        {
            String StringJson = "";
            List<String> infoColumn = new List<String>();
            Int32 nroColumns = ObjDr.FieldCount;

            for (Int32 i = 0; i < nroColumns; i++)
            {
                String columnName = ObjDr.GetName(i);
                infoColumn.Add(columnName);

                //Type typeColumn = ObjDr.GetFieldType(i);
                //infoColumn.Add(columnName, typeColumn);
                //String columnName2 = ObjDr.GetDataTypeName(i);
            }
                
            //StringJson += GetColumnsWithNoData(infoColumn) + ",";      
            if (ObjDr.HasRows)
            {
                while (ObjDr.Read())
                {
                    StringJson += GetEntity(ObjDr, infoColumn) + ",";
                }

                StringJson = "[" + ((StringJson.Length > 0) ? StringJson.Substring(0, StringJson.Length - 1) : "") + "]";
            }
            else
            {
                StringJson +="["+ GetEntityEmpty(infoColumn) +"]";
            
            }

            return StringJson;
        }
        public static List<object> GetRowsObjects(SqlDataReader ObjDr)
        {
            List<object> StringJson = new List<object>();
            List<String> infoColumn = new List<String>();
            Int32 nroColumns = ObjDr.FieldCount;

            for (Int32 i = 0; i < nroColumns; i++)
            {
                String columnName = ObjDr.GetName(i);
                infoColumn.Add(columnName);

                //Type typeColumn = ObjDr.GetFieldType(i);
                //infoColumn.Add(columnName, typeColumn);
                //String columnName2 = ObjDr.GetDataTypeName(i);
            }

            //StringJson += GetColumnsWithNoData(infoColumn) + ",";      
            if (ObjDr.HasRows)
            {
                while (ObjDr.Read())
                {
                    StringJson.Add(GetEntityObject(ObjDr, infoColumn));
                   // StringJson += GetEntity(ObjDr, infoColumn) + ",";
                }

                //StringJson = "[" + ((StringJson.Length > 0) ? StringJson.Substring(0, StringJson.Length - 1) : "") + "]";
            }
            else
            {
               // StringJson += "[" + GetEntityEmpty(infoColumn) + "]";

            }

            return StringJson;
        }
        public static String GetJsonString(ref BaseEntity Entity, String SqlCommandString, Dictionary<String, Object> parameters)
        {
            String StringjSon = "[]";
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(SqlCommandString, clsConnection.GetConnection());
                
                foreach (KeyValuePair<String, Object> Par in parameters)
                {
                    cmd.Parameters.AddWithValue(Par.Key, Par.Value);
                }
                if (!SqlCommandString.ToLower().Contains("select"))
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                StringjSon = GetRows(dr);

            }
            catch (Exception ex)
            {
                StringjSon = "[]";
                Entity.Errors.Add(new BaseEntity.ListError(ex, "An error ocurred while trying to access database"));
            }
            finally
            {
                dr.Close();
                cmd.Connection.Close();
            }

            return StringjSon;
        }
        public static String GetJsonStringV2(ref BaseEntity Entity, String SqlCommandString, Dictionary<String, Object> parameters)
        {
            String StringjSon = "[]";
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            Dictionary<string, object> objresult = new Dictionary<string, object>();
            try
            {
                int count = 1;
                cmd = new SqlCommand(SqlCommandString, clsConnection.GetConnection());

                foreach (KeyValuePair<String, Object> Par in parameters)
                {
                    cmd.Parameters.AddWithValue(Par.Key, Par.Value);
                }
                if (!SqlCommandString.ToLower().Contains("select"))
                    cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
               
                //StringjSon = "{" + "result" + count.ToString() + ":" + GetRows(dr);
                //StringjSon = "{result" + count.ToString() + ": " + GetRows(dr);
                if (dr.HasRows)
                {
                    objresult.Add("result" + count.ToString(), GetRowsObjects(dr));
                    do
                    {
                        count++;
                        while (dr.Read())
                        {
                            objresult.Add("result" + count.ToString(), GetRowsObjects(dr));
                            //StringjSon += "," + "result" + count.ToString() + ":" + GetRows(dr); 
                        }

                    } while (dr.NextResult());
                }
                //StringjSon += "}";
                StringjSon = (new JavaScriptSerializer()).Serialize(objresult);

            }
            catch (Exception ex)
            {
                StringjSon = "[]";
                Entity.Errors.Add(new BaseEntity.ListError(ex, "An error ocurred while trying to access database"));
            }
            finally
            {
                dr.Close();
                cmd.Connection.Close();
            }

            return StringjSon;
        }
        public static DataTable GetDataTable(ref BaseEntity Base, String SqlCommandString, Dictionary<String, Object> parameters)
        {
            SqlConnection ObjConexion = null;
            DataTable dt = new DataTable();
            try
            {
                ObjConexion = clsConexion.ObtenerConexion();
                SqlCommand ObjCmd = new SqlCommand(SqlCommandString, ObjConexion);
                foreach (KeyValuePair<String, Object> Par in parameters)
                {
                    ObjCmd.Parameters.AddWithValue(Par.Key, Par.Value);
                }
                ObjCmd.CommandType = CommandType.StoredProcedure;
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "An Error Occurred When Attempting to Access Database."));
            }
            finally
            {
                ObjConexion.Close();
            }

            return dt;
        }

        /*Se agrego para verificar la exportación de excel cualquier cosa quitarlo*/
        public static List<DataTable> GetListDataTable(ref BaseEntity Base, String SqlCommandString)
        {
            SqlConnection ObjConexion = null;
            DataTable dt = new DataTable();
            List<DataTable> ListDetails = new List<DataTable>();
            SqlDataReader objDr = null;
            try
            {
                ObjConexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand(SqlCommandString, ObjConexion);
                ObjCmd.CommandTimeout = 0;
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@PlanillaId", 1);
                objDr = ObjCmd.ExecuteReader();
                while (!objDr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(objDr);
                    ListDetails.Add(dt);
                }

            }
            catch (Exception ex)
            {
                ListDetails = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la base Dao de xLibrary"));
            }
            finally
            {
                ObjConexion.Close();
            }

            return ListDetails;
        }

        /*Aqui termina la funcion para la exportación de excel*/

     /*-------------------------------------------------------------------------*/
        /*Se agrego para verificar la exportación de excel con ID de la planilla*/
        public static DataTable GetListDataTableID(ref BaseEntity Base, String SqlCommandString, Int64 idplanilla)
        {                         
            SqlConnection ObjConexion = null;
            DataTable dt = null;
            //DataTable ListDetails = new DataTable();
            SqlDataReader objDr = null;
            try
            {
                ObjConexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand(SqlCommandString, ObjConexion);
                ObjCmd.CommandTimeout = 0;
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@PlanillaId", idplanilla);
                objDr = ObjCmd.ExecuteReader();
                while (!objDr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(objDr);
                    //ListDetails.Add(dt);
                }

            }
            catch (Exception ex)
            {
                dt = null;
                //ListDetails = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la base Dao de xLibrary"));
            }
            finally
            {
                ObjConexion.Close();
            }

            return dt;
        }

        /*Aqui termina la funcion para la exportación de excel*/

        #region Exportacion Excel AFP
        
        public static DataTable GetListDatatableIdAFP(ref BaseEntity entidad, String sqlCommandString, Int32 IdPlanilla,Int32 EmpresaId)
        {
            DataTable dt = null;
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try {
                cmd = new SqlCommand(sqlCommandString, clsConnection.GetConnection());
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", EmpresaId);
                cmd.Parameters.AddWithValue("@PlanillaId", IdPlanilla);
                dr = cmd.ExecuteReader();
                while (!dr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }catch(Exception ex) {
                dt = null;
                entidad.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la baseDao de xLibrary"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }
        #endregion

        #region Exportación_AFP_Detalle
        public static DataTable GetListDatatableIdDetalleAFP(ref BaseEntity entidad, String sqlCommandString, Int32 IdPlanilla, Int32 EmpresaId)
        {
            DataTable dt = null;
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sqlCommandString, clsConnection.GetConnection());
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", EmpresaId);
                cmd.Parameters.AddWithValue("@PlanillaId", IdPlanilla);
                dr = cmd.ExecuteReader();
                while (!dr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            catch (Exception ex)
            {
                dt = null;
                entidad.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la baseDao de xLibrary"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }
        #endregion



        #region verifica la exportacion de la planilla de gratificaion mensual
        public static DataTable GetListDataTableIDGrati(ref BaseEntity Base, String SqlCommandString, Int64 GratificacionID)
        {
            SqlConnection ObjConexion = null;
            DataTable dt = null;
            SqlDataReader objDr = null;
            try
            {
                ObjConexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand(SqlCommandString, ObjConexion);
                ObjCmd.CommandTimeout = 0;
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@GratificacionID", GratificacionID);
                objDr = ObjCmd.ExecuteReader();
                while (!objDr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(objDr);
                }

            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la base Dao de xLibrary"));
            }
            finally
            {
                ObjConexion.Close();
            }

            return dt;
        }

        #endregion 



        #region  Exportacion_Excel_CalculoQuintaCategoria_por_deivi_gomez
        public static DataTable GetListDataTableIDQuintaCategoria(ref BaseEntity Base, String SqlCommandString, Int32 QuintaID)
        {
            SqlConnection ObjConexion = null;
            DataTable dt = null;
            SqlDataReader objDr = null;
            try
            {
                ObjConexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand(SqlCommandString, ObjConexion);
                ObjCmd.CommandTimeout = 0;
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@Renta5taCategoID", QuintaID);
                objDr = ObjCmd.ExecuteReader();
                while (!objDr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(objDr);
                }

            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la base Dao de xLibrary"));
            }
            finally
            {
                ObjConexion.Close();
            }

            return dt;
        }

        #endregion 




        public static DateTime ValidateDateTime(DateTime dateTimeValue) {
            if (DateTime.Compare(dateTimeValue, Convert.ToDateTime( SqlDateTime.MinValue) ) > 0 &&
                DateTime.Compare(dateTimeValue,  Convert.ToDateTime(SqlDateTime.MaxValue)) < 0) {

                    return dateTimeValue;           
            }
            return DateTime.Now;        
        }


        #region  Exportacion_Excel_Gratificacion_Mes_por_deivi_gomez

        public static DataTable Exportar_Excel_Gratificacion_Mes(ref BaseEntity Base, String SqlCommandString, Int32 idGratificacion)
        {
            SqlConnection ObjConexion = null;
            DataTable dt = null;
            //DataTable ListDetails = new DataTable();
            SqlDataReader objDr = null;
            try
            {
                ObjConexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand(SqlCommandString, ObjConexion);
                ObjCmd.CommandTimeout = 0;
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@PlanillaId", idGratificacion);
                objDr = ObjCmd.ExecuteReader();
                while (!objDr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(objDr);
                    //ListDetails.Add(dt);
                }

            }
            catch (Exception ex)
            {
                dt = null;
                //ListDetails = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la base Dao de xLibrary"));
            }
            finally
            {
                ObjConexion.Close();
            }

            return dt;
        }



        #endregion

        #region  Exportacion_boleta_trabajador_por_deivi_gomez
        public static DataTable ObtenerDatosTrabajadorDAO(ref BaseEntity Base, String SqlCommandString, Int32 empresaid, Int32 planillaid, Int32 empleadoid)
        {
            SqlConnection ObjConexion = null;
            DataTable dt = null;
            SqlDataReader objDr = null;
            try
            {
                ObjConexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand(SqlCommandString, ObjConexion);
                ObjCmd.CommandTimeout = 0;
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@EmpresaId", empresaid);
                ObjCmd.Parameters.AddWithValue("@PlanillaId", planillaid);
                ObjCmd.Parameters.AddWithValue("@EmpleadoId", empleadoid);
                objDr = ObjCmd.ExecuteReader();
                while (!objDr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(objDr);
                }

            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la base Dao de xLibrary"));
            }
            finally
            {
                ObjConexion.Close();
            }

            return dt;
        }

        #endregion 

        #region  Exportacion_beneficios_boleta_trabajador_por_deivi_gomez
        public static DataTable ObtenerBeneficiosTrabajadorDAO(ref BaseEntity Base, String SqlCommandString, Int32 empresaid, Int32 planillaid, Int32 empleadoid)
        {
            SqlConnection ObjConexion = null;
            DataTable dt = null;
            SqlDataReader objDr = null;
            try
            {
                ObjConexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand(SqlCommandString, ObjConexion);
                ObjCmd.CommandTimeout = 0;
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@EmpresaId", empresaid);
                ObjCmd.Parameters.AddWithValue("@PlanillaId", planillaid);
                ObjCmd.Parameters.AddWithValue("@EmpleadoId", empleadoid);
                objDr = ObjCmd.ExecuteReader();
                while (!objDr.IsClosed)
                {
                    dt = new DataTable();
                    dt.Load(objDr);
                }

            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrió un error en la base Dao de xLibrary"));
            }
            finally
            {
                ObjConexion.Close();
            }

            return dt;
        }

        #endregion 




    }



    public static class ObjectsUtilities {
        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            T someObject = new T();
            Type someObjectType = someObject.GetType();

            foreach (KeyValuePair<string, object> item in source)
            {
                someObjectType.GetProperty(item.Key).SetValue(someObject, item.Value, null);
            }

            return someObject;
        }
    }


 
}
