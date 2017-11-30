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
    public class clsTareoDAO : BaseDao
    {

        #region Singleton
        private static clsTareoDAO instance = null;
        public static clsTareoDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new clsTareoDAO();
                return instance;
            }
        }
        #endregion


        public Boolean Generar_Mes_Tareo(ref BaseEntity Base, DateTime DatoMes)
        {

            SqlCommand cmd = null;
            Boolean isCorrect = false;
            try
            {
                cmd = new SqlCommand("sp_xP_Crear_Mes_Tareo", clsConnection.GetConnection());

                cmd.Parameters.AddWithValue("@date", DatoMes);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                isCorrect = true;
            }
            catch (Exception ex)
            {
                isCorrect = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "A ocurrido un error al Generar el Mes"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }

        public Boolean Realizar_Registro_Vacaciones(ref BaseEntity Base)
        {
            SqlCommand cmd = null;
            Boolean isCorrect = false;

            try
            {
                cmd = new SqlCommand("sp_xP_Guardar_Vacaciones", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                isCorrect = true;
            }
            catch (Exception ex)
            {
                isCorrect = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "A ocurrido un error en el Registro de Vacaciones"));
                
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }

        /*Realizar Update Vacaciones Truncas*/
        public Boolean Realizar_UpdateVacacionesTruncas(ref BaseEntity Base)
        {
            SqlCommand cmd = null;
            Boolean isCorrect = false;

            try
            {
                cmd = new SqlCommand("sp_xP_VacacionesTruncas", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                isCorrect = true;
            }
            catch (Exception ex)
            {
                isCorrect = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "A ocurrido un error en el Registro de Vacaciones"));

            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }

        public Boolean Realizar_Tareo_Mensaul(ref BaseEntity Base, int Companyid, int UsuarioId)
        {

            SqlCommand cmd = null;
            Boolean isCorrect = false;
            try
            {
                cmd = new SqlCommand("sp_xP_Tareo_Mensual", clsConnection.GetConnection());

                cmd.Parameters.AddWithValue("@Empresa", Companyid);
                cmd.Parameters.AddWithValue("@Usuario", UsuarioId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                isCorrect = true;
            }
            catch (Exception ex)
            {
                isCorrect = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "A ocurrido un error al Realizar el Tareo Mensual"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }



        #region Resscribir_Tareo_Mensual_por_Deivi_Gomez

        public Boolean Reescribir_Tareo_Mensaul(ref BaseEntity Base, int Companyid, int UsuarioId)
        {

            SqlCommand cmd = null;
            Boolean isCorrect = false;
            try
            {
                cmd = new SqlCommand("sp_xP_Chancar_Tareo_Mensual", clsConnection.GetConnection());

                cmd.Parameters.AddWithValue("@Empresa", Companyid);
                cmd.Parameters.AddWithValue("@Usuario", UsuarioId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                isCorrect = true;
            }
            catch (Exception ex)
            {
                isCorrect = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "A ocurrido un error al Realizar el Tareo Mensual"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }

        #endregion


        #region Resscribir_Tareo_Diario_por_Deivi_Gomez

        public Boolean Reescribir_Tareo_Diario(ref BaseEntity Base, DateTime fechaInicio, DateTime fechafin, int EmpresaId, int UsuarioId)
        {

            SqlCommand cmd = null;
            Boolean isCorrect = false;
            try
            {
                cmd = new SqlCommand("sp_xP_Reescribir_Tareo_Diario", clsConnection.GetConnection());

                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechafin", fechafin);
                cmd.Parameters.AddWithValue("@Empresa", EmpresaId);
                cmd.Parameters.AddWithValue("@Usuario", UsuarioId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                isCorrect = true;
            }
            catch (Exception ex)
            {
                isCorrect = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "A ocurrido un error al Reescribir el Tareo Diario"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }

        #endregion





        public DataTable Listar_Empleados_Vacaciones(ref BaseEntity Entity, DateTime fecha)
        {
            DataTable dt = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Empleados_Vacaciones", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.CommandType = CommandType.StoredProcedure;
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception)
            {
                dt = null;
                throw;
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }

        public DataTable Listar_Empleado_Fecha_Horario(ref BaseEntity Entity, String Day, DateTime fecha, Int32 idEmp)
        {
            DataTable dt = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Empleado_Fecha_Horario", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@nombredia", Day);                
                cmd.Parameters.AddWithValue("@Empresa", idEmp);

                //ToDo: Modificar asignacion Campo de id de empresa con valor de session 
                cmd.CommandType = CommandType.StoredProcedure;
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "No found."));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }

        public Boolean Validar_Feriado(ref BaseEntity Base, DateTime fecha, int empresaid)
        {
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            Boolean resultado = false;

            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Validar_Dia_Feriado", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@empresaid", empresaid);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (Convert.ToInt32(dr["resultado"]) == 1)
                    {
                        resultado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en base de datos"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return resultado;
        }

        public int ValidaEmpleadoDiaLaboral(ref BaseEntity Base, clsTareo objTareo, string nombredia)
        {
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            int valor = 0;
            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Validar_Dia_Laboral", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objTareo.TareoId);
                cmd.Parameters.AddWithValue("@Dia", nombredia);
                cmd.Parameters.AddWithValue("@EmpresaId", 1);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    valor = Convert.ToInt32(dr["valida"]);
                }     


              
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en base de datos"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return valor;
        }

        public int ValidaPartime(ref BaseEntity Base, clsTareo objTareo, string nombredia)
        { 
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            int valor = 0;
            int partime = 0 ;
            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Validar_PartTime", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objTareo.TareoId);
                cmd.Parameters.AddWithValue("@fechatareo", objTareo.FechaTareo);
                cmd.Parameters.AddWithValue("@nombredia", nombredia);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    valor = Convert.ToInt32(dr["valida"]);
                }

                if (valor == 1)
                {
                    partime = 1;
                }
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en base de datos"));
            }

            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return partime;
        }

        public int ValidaFullTime(ref BaseEntity Base, clsTareo objTareo, string nombredia)
        { 
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            int valor = 0;
            int fulltime = 0 ;
            try
            {
                 objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Valida_FullTime", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objTareo.TareoId);
                cmd.Parameters.AddWithValue("@fechatareo", objTareo.FechaTareo);
                cmd.Parameters.AddWithValue("@nombredia", nombredia);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    valor = Convert.ToInt32(dr["valida"]);
                }

                if (valor == 1)
                {
                    fulltime = 1;
                }
                      
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en base de datos"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return fulltime;

        }

        public int ValidaInasistenciaPartime(ref BaseEntity Base, clsTareo objTareo,string nombredia)
        {
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            int valor = 0;
            int parttime = 0 ;
            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Valida_Inasistencia_PartTime", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objTareo.TareoId);
                cmd.Parameters.AddWithValue("@fechatareo", objTareo.FechaTareo);
                cmd.Parameters.AddWithValue("@nombredia", nombredia);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    valor = Convert.ToInt32(dr["valida"]);
                }

                if (valor == 1)
                {
                    parttime = 1;
                }                    
           


            }
            catch (Exception ex )
            {                
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en base de datos"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return parttime;
        }

        public Boolean EditarTareo(ref BaseEntity Base, clsTareo objTareo)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;

            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Editar_Tareo", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objTareo.TareoId);
                cmd.Parameters.AddWithValue("@fechatareo", objTareo.FechaTareo);

                if (objTareo.HoraEntrada == null && objTareo.HoraSalida == null && objTareo.DescansoEntrada == null && objTareo.DescansoSalida == null)
                {
                    cmd.Parameters.AddWithValue("@horaentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@hoursalida", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansoentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansosalida", DBNull.Value);
                }


                if (objTareo.HoraEntrada != null && objTareo.HoraSalida != null && objTareo.DescansoEntrada != null && objTareo.DescansoSalida != null)
                {
                    cmd.Parameters.AddWithValue("@horaentrada", objTareo.HoraEntrada);
                    cmd.Parameters.AddWithValue("@hoursalida", objTareo.HoraSalida);
                    cmd.Parameters.AddWithValue("@descansoentrada", objTareo.DescansoEntrada);
                    cmd.Parameters.AddWithValue("@descansosalida", objTareo.DescansoSalida);
                }

                if (objTareo.DescansoEntrada == null && objTareo.DescansoSalida == null && objTareo.HoraEntrada != null && objTareo.HoraSalida != null)
                {
                    cmd.Parameters.AddWithValue("@horaentrada", objTareo.HoraEntrada);
                    cmd.Parameters.AddWithValue("@hoursalida", objTareo.HoraSalida);
                    cmd.Parameters.AddWithValue("@descansoentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansosalida", DBNull.Value);
                }

                if (objTareo.DescansoEntrada != null && objTareo.DescansoSalida != null && objTareo.HoraEntrada == null && objTareo.HoraSalida == null)
                {
                    cmd.Parameters.AddWithValue("@horaentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@hoursalida", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansoentrada", objTareo.DescansoEntrada);
                    cmd.Parameters.AddWithValue("@descansosalida", objTareo.DescansoSalida);
                }

 
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

        public Boolean GuardarTareo(ref BaseEntity Base, clsTareo objTareo,int empresaid)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;

            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Guardar_Tareo", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objTareo.TareoId);
                cmd.Parameters.AddWithValue("@fechatareo", objTareo.FechaTareo);

                if (objTareo.HoraEntrada == null && objTareo.HoraSalida == null && objTareo.DescansoEntrada == null && objTareo.DescansoSalida == null)
                {
                    cmd.Parameters.AddWithValue("@descansoentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansosalida", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horaentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@hoursalida", DBNull.Value);
                }

                if (objTareo.HoraEntrada != null && objTareo.HoraSalida != null && objTareo.DescansoEntrada != null && objTareo.DescansoSalida != null)
                {
                    cmd.Parameters.AddWithValue("@descansoentrada", objTareo.DescansoEntrada);
                    cmd.Parameters.AddWithValue("@descansosalida", objTareo.DescansoSalida);
                    cmd.Parameters.AddWithValue("@horaentrada", objTareo.HoraEntrada);
                    cmd.Parameters.AddWithValue("@hoursalida", objTareo.HoraSalida);
                }

                if (objTareo.DescansoEntrada == null && objTareo.DescansoSalida == null && objTareo.HoraEntrada != null && objTareo.HoraSalida != null)
                {
                    cmd.Parameters.AddWithValue("@descansoentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansosalida", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horaentrada", objTareo.HoraEntrada);
                    cmd.Parameters.AddWithValue("@hoursalida", objTareo.HoraSalida);
                }

                if (objTareo.HoraEntrada == null && objTareo.HoraSalida == null && objTareo.DescansoEntrada != null && objTareo.DescansoSalida != null)
                {
                    cmd.Parameters.AddWithValue("@horaentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@hoursalida", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansoentrada", objTareo.DescansoEntrada);
                    cmd.Parameters.AddWithValue("@descansosalida", objTareo.DescansoSalida);
                }            

                cmd.Parameters.AddWithValue("@createddate", objTareo.Createdate);
                cmd.Parameters.AddWithValue("@createdby", Convert.ToInt32(objTareo.Createdby));
                cmd.Parameters.AddWithValue("@lastupdatedate", objTareo.UpdateDate);
                cmd.Parameters.AddWithValue("@lastupdateby", objTareo.Updatedby);
                cmd.Parameters.AddWithValue("@estado", objTareo.Estado);
                cmd.Parameters.AddWithValue("@empresaid", empresaid);
                
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


        public Boolean GuardarEditarTareoMasivo(ref BaseEntity Base, clsTareo objTareo, int empresaid)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;

            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Editar_Tareo_Masivo", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empleadoid", objTareo.TareoId);
                cmd.Parameters.AddWithValue("@fechatareo", objTareo.FechaTareo);

                if (objTareo.HoraEntrada == null && objTareo.HoraSalida == null && objTareo.DescansoEntrada == null && objTareo.DescansoSalida == null)
                {
                    cmd.Parameters.AddWithValue("@descansoentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansosalida", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horaentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@hoursalida", DBNull.Value);
                }

                if (objTareo.HoraEntrada != null && objTareo.HoraSalida != null && objTareo.DescansoEntrada != null && objTareo.DescansoSalida != null)
                {
                    cmd.Parameters.AddWithValue("@descansoentrada", objTareo.DescansoEntrada);
                    cmd.Parameters.AddWithValue("@descansosalida", objTareo.DescansoSalida);
                    cmd.Parameters.AddWithValue("@horaentrada", objTareo.HoraEntrada);
                    cmd.Parameters.AddWithValue("@hoursalida", objTareo.HoraSalida);
                }

                if (objTareo.DescansoEntrada == null && objTareo.DescansoSalida == null && objTareo.HoraEntrada != null && objTareo.HoraSalida != null)
                {
                    cmd.Parameters.AddWithValue("@descansoentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansosalida", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horaentrada", objTareo.HoraEntrada);
                    cmd.Parameters.AddWithValue("@hoursalida", objTareo.HoraSalida);
                }

                if (objTareo.HoraEntrada == null && objTareo.HoraSalida == null && objTareo.DescansoEntrada != null && objTareo.DescansoSalida != null)
                {
                    cmd.Parameters.AddWithValue("@horaentrada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@hoursalida", DBNull.Value);
                    cmd.Parameters.AddWithValue("@descansoentrada", objTareo.DescansoEntrada);
                    cmd.Parameters.AddWithValue("@descansosalida", objTareo.DescansoSalida);
                }

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


    }
}
