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
    public class clsVacacionesDAO
    {
        #region singleton
        public static readonly clsVacacionesDAO _instancia = new clsVacacionesDAO();
        public static clsVacacionesDAO Instancia { get { return clsVacacionesDAO._instancia; } }
        #endregion

        #region Metodos
        /*Listar Vacaciones*/
        public List<clsVacaciones> ListarVacaciones(ref BaseEntity Base, Int32 EmpresaId) {
            SqlCommand cmd = null;
            IDataReader idr = null;
            List<clsVacaciones> lista = null;
            SqlConnection objConexion = null;
            try
            {
                objConexion = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Listar_Vacaciones", objConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", EmpresaId);
                idr = cmd.ExecuteReader();
                lista = new List<clsVacaciones>();
                while (idr.Read())
                {
                    clsVacaciones vaca = new clsVacaciones();
                    vaca.ID = Convert.ToInt32(idr["VacacionesId"]);
                    clsEmployee empleado = new clsEmployee();
                    empleado.ID = Convert.ToInt32(idr["EmpleadoId"]);
                    empleado.Nombre = idr["Nombre"].ToString();
                    empleado.Apellido = idr["Apellido"].ToString();
                    vaca.empleado = empleado;
                    vaca.FechaInicio = idr["FechaInicio"].ToString();
                    vaca.FechaTermino = idr["FechaTermino"].ToString();
                    vaca.Reingreso = idr["Reingreso"].ToString();
                    vaca.Estado = Convert.ToInt32(idr["estado"]);
                    lista.Add(vaca);
                }

            }
            catch (Exception e) { 
                lista = null;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa DAO de las Vacaciones"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return lista;
        }

        /*Listar Vacaciones Programadas*/
        public List<clsVacaciones> ListarVacacionesProgramadas(ref BaseEntity Base, Int32 EmpresaId)
        {
            SqlCommand cmd = null;
            IDataReader idr = null;
            List<clsVacaciones> lista = null;
            SqlConnection objConexion = null;
            try
            {
                objConexion = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Listar_Vacaciones_Programados", objConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", EmpresaId);
                idr = cmd.ExecuteReader();
                lista = new List<clsVacaciones>();
                while (idr.Read())
                {
                    clsVacaciones vaca = new clsVacaciones();
                    vaca.ID = Convert.ToInt32(idr["VacacionesId"]);
                    clsEmployee empleado = new clsEmployee();
                    empleado.ID = Convert.ToInt32(idr["EmpleadoId"]);
                    empleado.Nombre = idr["Nombre"].ToString();
                    empleado.Apellido = idr["Apellido"].ToString();
                    vaca.empleado = empleado;
                    vaca.FechaInicio = idr["FechaInicio"].ToString();
                    vaca.FechaTermino = idr["FechaTermino"].ToString();
                    vaca.Reingreso = idr["Reingreso"].ToString();
                    vaca.Estado = Convert.ToInt32(idr["estado"]);
                    lista.Add(vaca);
                }

            }
            catch (Exception e)
            {
                lista = null;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa DAO de las Vacaciones"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return lista;
        }

        /*Listar Vacaciones Truncas*/
        public List<clsVacaciones> ListarVacacionesTruncas(ref BaseEntity Base, Int32 EmpresaId)
        {
            SqlCommand cmd = null;
            IDataReader idr = null;
            List<clsVacaciones> lista = null;
            SqlConnection objConexion = null;
            try
            {
                objConexion = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Listar_Vacaciones_Truncas", objConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", EmpresaId);
                idr = cmd.ExecuteReader();
                lista = new List<clsVacaciones>();
                while (idr.Read())
                {
                    clsVacaciones vaca = new clsVacaciones();
                    vaca.ID = Convert.ToInt32(idr["VacacionesId"]);
                    clsEmployee empleado = new clsEmployee();
                    empleado.ID = Convert.ToInt32(idr["EmpleadoId"]);
                    empleado.Nombre = idr["Nombre"].ToString();
                    empleado.Apellido = idr["Apellido"].ToString();
                    vaca.empleado = empleado;
                    vaca.FechaInicio = idr["FechaInicio"].ToString();
                    vaca.FechaTermino = idr["FechaTermino"].ToString();
                    vaca.Reingreso = idr["Reingreso"].ToString();
                    vaca.Estado = Convert.ToInt32(idr["estado"]);
                    lista.Add(vaca);
                }

            }
            catch (Exception e)
            {
                lista = null;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa DAO de las Vacaciones"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return lista;
        }

        /*Buscar Vacaciones*/
        public DataTable Buscar_Vacaciones(ref BaseEntity Base, Int32 VacacionesId) {
            SqlConnection objc = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                objc = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Buscar_Vacaciones", objc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@vacacionesid", VacacionesId);
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception e)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa Dao de Vacaciones"));
            }
            return dt;
        }

        /*Editar Vacaciones*/
        public Boolean Editar_Vacaciones(ref BaseEntity Base, clsVacaciones obbjv) {
            Boolean editar = false;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Editar_Vacaciones", cn);
                cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.AddWithValue("@FechaInicio", DateTime.Parse(obbjv.FechaInicio));
               // cmd.Parameters.AddWithValue("@FechaTermino", DateTime.Parse(obbjv.FechaTermino));
               // cmd.Parameters.AddWithValue("@FechaReinicio", DateTime.Parse(obbjv.Reingreso));
                cmd.Parameters.AddWithValue("@VacacionesPlantillaId", obbjv.planvacaciones.ID);
                cmd.Parameters.AddWithValue("@VacacionesId", obbjv.ID);
                cmd.Parameters.AddWithValue("@updateBy", obbjv.Updatedby);
                cmd.Parameters.AddWithValue("@estado", obbjv.Estado);
                editar = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                editar = false;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa dao editar"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return editar;
        }
        /*Actualizar Vacaciones*/
        public Boolean Actualiza_Vacaciones(ref BaseEntity Base, clsVacaciones obbjv)
        {
            Boolean actualizar = false;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Actualizar_Vacaciones", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", DateTime.Parse(obbjv.FechaInicio));
                cmd.Parameters.AddWithValue("@FechaTermino", DateTime.Parse(obbjv.FechaTermino));
                cmd.Parameters.AddWithValue("@FechaReinicio", DateTime.Parse(obbjv.Reingreso));
                cmd.Parameters.AddWithValue("@Estado", obbjv.Estado);
                cmd.Parameters.AddWithValue("@VacacionesId", obbjv.VacacionesId);
                cmd.Parameters.AddWithValue("@updateBy", obbjv.Updatedby);
                actualizar = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                actualizar = false;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa dao editar"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return actualizar;
        }

        /*Actualiza Vacaciones Programadas*/
        public Boolean Actualizar_Vacaciones_Programadas(ref BaseEntity Base, clsVacaciones obbjv)
        {
            Boolean actualizar = false;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Actualizar_VacacionesDos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaReinicio", DateTime.Parse(obbjv.Reingreso));
                cmd.Parameters.AddWithValue("@Estado", obbjv.Estado);
                cmd.Parameters.AddWithValue("@VacacionesId", obbjv.VacacionesId);
                cmd.Parameters.AddWithValue("@updateBy", obbjv.Updatedby);
                actualizar = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                actualizar = false;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa dao editar"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return actualizar;
        }

        /*Busqueda de adelanto de vacaciones*/
        public DataTable ConsultaFechasAdelanto(ref BaseEntity Base, Int32 idEmpleado) {
            SqlConnection objc = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                objc = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_AdelantoVacaciones", objc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception e) { 
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa Dao de vacaciones"));
            }
            return dt;
        }

        /*Consultar vacaciones por empleado*/
        public List<clsVacaciones> listarVacacionesEmpleado(ref BaseEntity Base, Int32 EmpleadoId) {
            SqlCommand cmd = null;
            IDataReader idr = null;
            List<clsVacaciones> lista = null;
            SqlConnection objConexion = null;
            try{
                objConexion = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_VerSalidasVacaciones", objConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpleadoId", EmpleadoId);
                idr = cmd.ExecuteReader();
                lista = new List<clsVacaciones>();
                while (idr.Read()) {
                    clsVacaciones vaca = new clsVacaciones();
                    vaca.FechaInicio = idr["FechaIni"].ToString();
                    vaca.FechaTermino = idr["FechaT"].ToString();
                    vaca.Estado = Convert.ToInt32(idr["Estado"]);
                    lista.Add(vaca);
                }

            }
            catch (Exception e) {
                lista = null;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa DAO de las Vacaciones"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return lista;
        }

        /*Verificar dias Trabajados*/
        public DataTable ConsultarDiasPorEmpleado(ref BaseEntity Base, Int32 idEmpleado) {
            SqlConnection objc = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try{
                objc = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Verificar_Dias", objc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpleadoId", idEmpleado);
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception e) {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa dao de vacaciones"));
            }
            return dt;
        }

        /*Verificar regimen al que pertenece*/

        public DataTable ConsultarRegimen(ref BaseEntity Base, Int32 IdEmpleado) {
            SqlConnection objc = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try{
                objc = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Verificar_Regimen", objc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpleadoId", IdEmpleado);
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception e) {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa dao de vacaciones"));
            }
            return dt;
        }

        public clsVacaciones obtieneVistaPreviaVacaciones(int VacacionesId, string tag)
        {

            clsVacaciones us = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = null;

            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("xp_sP_ObtieneDatosVacaciones_Tag", cn);
                cmd.Parameters.AddWithValue("@VacacionesId", VacacionesId);
                cmd.Parameters.AddWithValue("@TagId", tag);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    us = new clsVacaciones();
                    us.JustTag = dr[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //Base.Errors.Add(new BaseEntity.ListError(ex, "Error on user in dao"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return us;
        }
        /*BuscarIdVacaciones*/
        public clsVacaciones Search_Vacaciones(ref BaseEntity Base, Int32 UserId)
        {
            clsVacaciones va = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = null;
            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Buscar_VacacionesId", cn);
                cmd.Parameters.AddWithValue("@VacacionesId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    va = new clsVacaciones();
                    va.VacacionesId = Convert.ToInt32(dr["VacacionesId"]);
                    va.EmpleadoId = Convert.ToInt32(dr["EmpleadoId"]);
                    clsEmployee emp = new clsEmployee();
                    emp.Nombre = dr[2].ToString();
                    va.empleado = emp;
                    va.FechaInicio = dr[3].ToString();
                    va.FechaTermino = dr[4].ToString();
                    va.Reingreso = dr[5].ToString();
                    va.Estado = Convert.ToInt32(dr["Estado"]);
                }

            }
            catch (Exception ex)
            {
                va = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en dao"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return va;
        }

        
        #endregion

    }
}
