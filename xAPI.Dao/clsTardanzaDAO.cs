using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using xAPI.Library.Base;
using xAPI.Library.Connection;
using xAPI.Entity;
using System.Globalization;
namespace xAPI.Dao
{
    public class clsTardanzaDAO : BaseDao
    {
        #region Singleton
        private static clsTardanzaDAO instance = null;
        public static clsTardanzaDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new clsTardanzaDAO();
                return instance;
            }
        }
        #endregion

        public Boolean EditarTardanza(ref BaseEntity Base, clsTardanza objTardanza,string nombredia)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;

            try
            {
                objConnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Editar_Tardanza", objConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                //string fecha = objTardanza.FechaTardanza.ToString("MM/dd/yyyy");

                cmd.Parameters.AddWithValue("@empleadoid", objTardanza.EmpleadoId);
                cmd.Parameters.AddWithValue("@fechaTareo", objTardanza.FechaTardanza);
                cmd.Parameters.AddWithValue("@nombredia", nombredia);
                cmd.Parameters.AddWithValue("@lastupdatedate", objTardanza.LastUpdateDate);
                cmd.Parameters.AddWithValue("@lastupdateby", objTardanza.LastUpdateBy);


                if (objTardanza.HoraEntradaManiana == null && objTardanza.HoraEntradaTarde == null && objTardanza.HoraSalidaManiana == null && objTardanza.HoraSalidaTarde == null)
                {
                    cmd.Parameters.AddWithValue("@horaentradam", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horasalidam", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horaentradat", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horsalidat", DBNull.Value);
                }

                if (objTardanza.HoraEntradaManiana != null && objTardanza.HoraEntradaTarde != null && objTardanza.HoraSalidaManiana != null && objTardanza.HoraSalidaTarde != null)
                {
                    cmd.Parameters.AddWithValue("@horaentradam", objTardanza.HoraEntradaManiana);
                    cmd.Parameters.AddWithValue("@horasalidam", objTardanza.HoraSalidaManiana);
                    cmd.Parameters.AddWithValue("@horaentradat", objTardanza.HoraEntradaTarde);
                    cmd.Parameters.AddWithValue("@horsalidat", objTardanza.HoraSalidaTarde);
                }

                if (objTardanza.HoraEntradaManiana != null && objTardanza.HoraSalidaManiana != null && objTardanza.HoraEntradaTarde == null && objTardanza.HoraSalidaTarde == null)
                {
                    cmd.Parameters.AddWithValue("@horaentradam", objTardanza.HoraEntradaManiana);
                    cmd.Parameters.AddWithValue("@horasalidam", objTardanza.HoraSalidaManiana);
                    cmd.Parameters.AddWithValue("@horaentradat", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horsalidat", DBNull.Value);
                }

                if (objTardanza.HoraEntradaManiana == null && objTardanza.HoraSalidaManiana == null && objTardanza.HoraEntradaTarde != null && objTardanza.HoraSalidaTarde != null)
                {
                    cmd.Parameters.AddWithValue("@horaentradam", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horasalidam", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horaentradat", objTardanza.HoraEntradaTarde);
                    cmd.Parameters.AddWithValue("@horsalidat", objTardanza.HoraSalidaTarde);
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
        public Boolean GuardarTardanza (ref BaseEntity Base , clsTardanza objTardiness, string nombredia)
        {
            Boolean success= false;
            SqlConnection objconnection = null;
            SqlCommand cmd = null;

            try
            {
                objconnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Guardar_Tardanza", objconnection);
                //cmd = new SqlCommand("sp_xP_Guardar_Tardanza", objconnection);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@empleadoid", objTardiness.EmpleadoId);
                cmd.Parameters.AddWithValue("@fechatareo", objTardiness.FechaTardanza);



                if (objTardiness.HoraEntradaManiana != null && objTardiness.HoraEntradaTarde != null && objTardiness.HoraSalidaManiana != null && objTardiness.HoraSalidaTarde != null)
                {
                    cmd.Parameters.AddWithValue("@horaentradam", objTardiness.HoraEntradaManiana);
                    cmd.Parameters.AddWithValue("@horasalidam", objTardiness.HoraSalidaManiana);
                    
                    cmd.Parameters.AddWithValue("@horaentradat", objTardiness.HoraEntradaTarde);
                    cmd.Parameters.AddWithValue("@horsalidat", objTardiness.HoraSalidaTarde);
                    

                }

                if (objTardiness.HoraEntradaManiana != null && objTardiness.HoraSalidaManiana != null  && objTardiness.HoraEntradaTarde == null && objTardiness.HoraSalidaTarde == null)
                {


                    cmd.Parameters.AddWithValue("@horaentradam", objTardiness.HoraEntradaManiana);
                    cmd.Parameters.AddWithValue("@horasalidam", objTardiness.HoraSalidaManiana);
                    
                    cmd.Parameters.AddWithValue("@horaentradat", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horsalidat", DBNull.Value);

                }


                if (objTardiness.HoraEntradaManiana == null && objTardiness.HoraSalidaManiana == null  && objTardiness.HoraEntradaTarde != null && objTardiness.HoraSalidaTarde != null)
                {

                    cmd.Parameters.AddWithValue("@horaentradam", DBNull.Value);
                    cmd.Parameters.AddWithValue("@horasalidam", DBNull.Value);

                    cmd.Parameters.AddWithValue("@horaentradat", objTardiness.HoraEntradaTarde);
                    cmd.Parameters.AddWithValue("@horsalidat", objTardiness.HoraSalidaTarde);
                 }            

                //cmd.Parameters.AddWithValue("@minutostarde", objTardiness.MinutosTardanza);
                //cmd.Parameters.AddWithValue("@estadojustificacion", objTardiness.EstadoJustificacion);
                //cmd.Parameters.AddWithValue("@plazojustificacion", objTardiness.PlazoJustificacion);
                //cmd.Parameters.AddWithValue("@createdate", objTardiness.CreatedDate);
               // cmd.Parameters.AddWithValue("@createby", objTardiness.CreatedBy);
                cmd.Parameters.AddWithValue("@lastuupdatedate", objTardiness.LastUpdateDate);
                cmd.Parameters.AddWithValue("@lastupdateby", objTardiness.LastUpdateBy);
                cmd.Parameters.AddWithValue("@nombredia", nombredia);
                //cmd.Parameters.AddWithValue("@estadoempleado", objTardiness.EstadoEmpleado);


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


        public DataTable Listar_Empleados_Tardanza_Diario(ref BaseEntity Entity, DateTime Day, string nombredia, Int32 idEmpTard)
        {

            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Empleados_Tardanza_Dia", clsConnection.GetConnection());
                //cmd = new SqlCommand("sp_xP_Listar_Empleados_Tardanza_Dia", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@fecha",Day);
                cmd.Parameters.AddWithValue("@empresaid", idEmpTard);
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
