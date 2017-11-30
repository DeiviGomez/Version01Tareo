using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using xAPI.Library.Base;
using xAPI.Library.Connection;
using xAPI.Entity;

namespace xAPI.Dao
{
    public class clsInasistenciaDAO : BaseDao
    {
        #region Singleton
        private static clsInasistenciaDAO instance = null;
        public static clsInasistenciaDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new clsInasistenciaDAO();
                return instance;
            }
        }
        #endregion

     

        public Boolean ValidarInasistencia(ref BaseEntity Base, clsInasistencia objInasistencia)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            int resul = 0;
            string fecha = "";
            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Validar_Inasistencia", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objInasistencia.EmpleadoId);
                fecha = (objInasistencia.FechaInasistencia).ToString("MM/dd/yyyy");
                cmd.Parameters.AddWithValue("@fechainasistencia", fecha);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    resul = Convert.ToInt32(dr["dato"]);
                }

                if (resul == 1)
                {
                    success = true;
                }
                return success;
            }
            catch (Exception)
            {
                
                throw;
            }


        }

        public Boolean EditarInasistencia(ref BaseEntity Base, clsInasistencia objInasistencia)
        {
            Boolean success = false;
            SqlConnection objconnection = null;
            SqlCommand cmd = null;

            try
            {
                objconnection = clsConnection.GetConnection();
                //cmd = new SqlCommand("sp_xP_Guardar_Inasistencia", objconnection);
                cmd = new SqlCommand("sp_xP_Editar_Inasistencia", objconnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objInasistencia.EmpleadoId);
                cmd.Parameters.AddWithValue("@fechatareo", objInasistencia.FechaInasistencia);
                //cmd.Parameters.AddWithValue("@estado", objInasistencia.Estado);
                //cmd.Parameters.AddWithValue("@createdate", objInasistencia.CreatedDate);
                //cmd.Parameters.AddWithValue("@createby", objInasistencia.CreatedBy);
                cmd.Parameters.AddWithValue("@lastupdatedate", objInasistencia.LastUpdateDate);
                cmd.Parameters.AddWithValue("@lastupdateby", objInasistencia.LastUpdateBy);

                success = cmd.ExecuteNonQuery() > 0 ? true : false;

            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en base de datos"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }

            return success;
        }

        public Boolean GuardarInasistencia(ref BaseEntity Base, clsInasistencia objInasistencia)
        {
            Boolean success = false;
            SqlConnection objconnection = null;
            SqlCommand cmd = null;

            try
            {
                objconnection = clsConnection.GetConnection();
                //cmd = new SqlCommand("sp_xP_Guardar_Inasistencia", objconnection);
                cmd = new SqlCommand("sp_xP_Guardar_Inasistencia", objconnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objInasistencia.EmpleadoId);
                cmd.Parameters.AddWithValue("@fechatareo", objInasistencia.FechaInasistencia);
                cmd.Parameters.AddWithValue("@estadojustificacion", objInasistencia.EstadoJustificacion);
                cmd.Parameters.AddWithValue("@plazojustificacion", objInasistencia.PlazoJustificacion);
                //cmd.Parameters.AddWithValue("@estado", objInasistencia.Estado);
                //cmd.Parameters.AddWithValue("@createdate", objInasistencia.CreatedDate);
                //cmd.Parameters.AddWithValue("@createby", objInasistencia.CreatedBy);
                cmd.Parameters.AddWithValue("@lastupdatedate", objInasistencia.LastUpdateDate);
                cmd.Parameters.AddWithValue("@lastupdateby", objInasistencia.LastUpdateBy);
                //cmd.Parameters.AddWithValue("@estadoempleado", objInasistencia.EstadoEmpleado);
                success = cmd.ExecuteNonQuery() > 0 ? true : false;

            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en base de datos"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }

            return success;
        }

        public DataTable Listar_Empleados_Inasistencias(ref BaseEntity Entity,DateTime Day ,string nombredia, Int32 idEmpInas)
        {
            DataTable dt = new DataTable();

            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Empleados_Inasistencia", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@fecha", Day);
                cmd.Parameters.AddWithValue("@empresaid", idEmpInas);
                cmd.Parameters.AddWithValue("@nombredia", nombredia);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "No Encontrado."));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }

            return dt;
        }
    }
}
