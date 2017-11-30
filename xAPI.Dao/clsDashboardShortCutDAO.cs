using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Library;
using xAPI.Library.Conexion;
using System.Data.SqlClient;
using System.Data;
using xAPI.Entity;
using xAPI.Library.Base;
using xAPI.Library.General;
using xAPI.Library.Connection;



namespace xAPI.Dao
{
    public class clsDashboardShortCutDAO : BaseDao
    {
        #region singleton
        private static readonly clsDashboardShortCutDAO _instance = new clsDashboardShortCutDAO();
        public static clsDashboardShortCutDAO Instance
        {
            get { return clsDashboardShortCutDAO._instance; }
        }
        #endregion

        #region ListDashboard

        public List<clsDashboardShortCut> ListDashboardShortCut(ref BaseEntity Base, int UserId)
        {

            SqlCommand cmd = null;
            IDataReader dr = null;
            List<clsDashboardShortCut> lista = null;
                      
            try 
	{
        cmd = new SqlCommand("sp_xP_Listar_Accesos_Directos", clsConnection.GetConnection());
        cmd.Parameters.AddWithValue("@USERID", UserId);
        cmd.CommandType = CommandType.StoredProcedure;
        dr = cmd.ExecuteReader();
        lista = new List<clsDashboardShortCut>();

        while(dr.Read())
        {
            clsDashboardShortCut dashboard = new clsDashboardShortCut();
           
            dashboard.Permisos.ID = Convert.ToInt32(dr["PermisosId"]);
            dashboard.nombre = dr["Nombre"].ToString();
            dashboard.icono = dr["Icono"].ToString();
            dashboard.url = dr["Url"].ToString();
            //dashboard.Permisos.Descripcion = dr["Descripcion"].ToString();
            //dashboard.Permisos.Descripcion = dr["Descripcion"].ToString();//
            dashboard.Permisos.Descripcion = dr["Descripcion"].ToString();
            dashboard.id = Convert.ToInt32(dr["AccesoDirectoId"]);
            dashboard.MenuId = dr["MenuId"].ToString();
            dashboard.pagsecundaria = Convert.ToInt32(dr["PaginaSecundaria"]);
            dashboard.TipoUsuario.TipoUsuarioId = Convert.ToInt32(dr["TipoUsuarioId"]);
            lista.Add(dashboard);
        }

   	}
	catch (Exception e)
	{
        lista = null;
        Base.Errors.Add(new BaseEntity.ListError(e, "Error en cargar los accesos directos"));
		throw;
	}
        
    finally
            {
            clsConnection.DisposeCommand(cmd);
            
            }    
        return lista;
        }

        #endregion ListDashboard



        #region LlenarCombo


        public DataTable Listar_Empresa_Usuario(ref BaseEntity Base, int idUsuario)
        {
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {

                cmd = new SqlCommand("sp_xP_Listar_Empresa_Usuario", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuarioId", idUsuario);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del dashboard"));
            }
            finally { cmd.Connection.Close(); }
            return dt;
        }

        #endregion LlenarCombo
    }
}
