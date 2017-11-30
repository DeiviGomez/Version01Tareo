using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using xAPI.Entity;
using xAPI.Library.Conexion;
using xAPI.Library.Base;
using System.Data.SqlClient;
using xAPI.Library.General;
using xAPI.Library.Connection;


namespace xAPI.Dao
{
    public class clsDocumentTypeDAO
    {
        #region Singleton
        private static clsDocumentTypeDAO instance = null;
        public static clsDocumentTypeDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new clsDocumentTypeDAO();
                return instance;
            }
        }
        #endregion
        public DataTable Document_Type_ListAll(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Documento", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Recursos no Encontrados"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }
    }
}
