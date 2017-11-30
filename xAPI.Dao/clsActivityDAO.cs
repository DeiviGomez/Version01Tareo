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
    public class clsActivityDAO
    {
        #region Singleton
        private static clsActivityDAO instance = null;
        public static clsActivityDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new clsActivityDAO();
                return instance;
            }
        }
        #endregion
        public DataTable Activity_ListAll(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Actividad", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                dt = null;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Recursos No Encontrados"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }
    }
}
