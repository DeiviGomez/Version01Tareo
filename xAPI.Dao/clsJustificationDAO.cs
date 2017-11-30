using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using xAPI.Library.Base;
using xAPI.Library;
using xAPI.Library.Connection;
using xAPI.Entity;

namespace xAPI.Dao
{
    public class clsJustificationDAO
    {
        #region Singleton
        private static readonly clsJustificationDAO _instance = new clsJustificationDAO();
        public static clsJustificationDAO Instance
        {
            get { return clsJustificationDAO._instance; }
        }
        #endregion

        
        #region GuardarJustificación
        public int SaveJustificacion(ref BaseEntity Entity, clsJustification objAsgJust)
        {
            int i = 0;
            SqlCommand cmd = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Guardar_Asignacion_Justificacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", objAsgJust.EmpresaId);
                cmd.Parameters.AddWithValue("@Archivo", objAsgJust.Archivo);
                cmd.Parameters.AddWithValue("@EmpleadoId", objAsgJust.EmpleadoId);
                cmd.Parameters.AddWithValue("@JustificacionPlantillaId", objAsgJust.justificacion.ID);
                cmd.Parameters.AddWithValue("@Motivo", objAsgJust.Motivo);
                cmd.Parameters.AddWithValue("@Fecha", objAsgJust.Fecha);
                if (objAsgJust.TardanzaId == 0)
                {
                    cmd.Parameters.AddWithValue("@TardanzaId", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TardanzaId", objAsgJust.TardanzaId);
                }
                if (objAsgJust.InasistenciaId == 0)
                {
                    cmd.Parameters.AddWithValue("@InasistenciaId", null);
                }
                else {
                    cmd.Parameters.AddWithValue("@InasistenciaId", objAsgJust.InasistenciaId);
                }
                
                cmd.Parameters.AddWithValue("@CreatedBy", objAsgJust.Createdby);

                i = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Error saving Memorandum"));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;

        }

        #endregion


        public int SaveJustificacion2(ref BaseEntity Entity, clsJustification objAsgJust) 
        {
            int i = 0;
            SqlCommand cmd = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Guardar_Asignacion_Justificacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", objAsgJust.EmpresaId);
                cmd.Parameters.AddWithValue("@Archivo", objAsgJust.Archivo);
                cmd.Parameters.AddWithValue("@EmpleadoId", objAsgJust.EmpleadoId);
                cmd.Parameters.AddWithValue("@JustificacionPlantillaId", objAsgJust.justificacion.ID);
                cmd.Parameters.AddWithValue("@Motivo", objAsgJust.Motivo);
                cmd.Parameters.AddWithValue("@Fecha", objAsgJust.Fecha);
                if (objAsgJust.TardanzaId == 0)
                {
                    cmd.Parameters.AddWithValue("@TardanzaId", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TardanzaId", objAsgJust.TardanzaId);
                }
                if (objAsgJust.InasistenciaId == 0)
                {
                    cmd.Parameters.AddWithValue("@InasistenciaId", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InasistenciaId", objAsgJust.InasistenciaId);
                }

                cmd.Parameters.AddWithValue("@CreatedBy", objAsgJust.Createdby);

                i = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Error saving Memorandum"));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;

        }

        #region ListarJustificación
        public DataTable ListJustificacion(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Asignacion_Justificacion", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Empresa", id);
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

        public DataTable ListarJustificaciones(ref BaseEntity Entity, Int32 empresaID)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_XP_Listar_Justificaciones", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empresaid", empresaID);
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
        #endregion

        #region Lista Historial Justificación
        public DataTable ListaHistorialJustificacion(ref BaseEntity Base, Int32 eid, Int32 id)
        {
            DataTable dt = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Historial_Justificacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", eid);
                cmd.Parameters.AddWithValue("@Empresa", id);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {

                cmd.Connection.Close();
            }
            return dt;


        }
        #endregion 

        #region Buscar
        public clsJustification SearchJustificacion(ref BaseEntity Base, int loanId)
        {
            clsJustification busq = null;
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Buscar_Asignacion_Justificacion", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@prmId", loanId);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    busq = new clsJustification();
                    busq.ID = Convert.ToInt32(dr["IDJUSTIFICACION"]);
                    busq.EmpresaId = Convert.ToInt32(dr["IDEMPRESA"]);
                    busq.Archivo = dr["DOCUMENTO"].ToString();

                    //obtener empleado de lista (id, nombre)
                    clsEmployee objEmp = new clsEmployee();
                    objEmp.Name = dr["NOMBEMPLEADO"].ToString(); //nombre
                    busq.EmpleadoId = Convert.ToInt32(dr["EmpleadoId"]);
                    busq.employee = objEmp; //guardo valores en mi objeto tipo clase

                    //obtener valor del combo (id)
                    clsTemplateJustification objTM = new clsTemplateJustification();
                    objTM.ID = Convert.ToInt32(dr["IDPLANTILLAJUSTIFICACION"]);
                    busq.justificacion = objTM;

                    //
                    busq.Motivo = dr["RAZON"].ToString();
                    busq.Fecha = Convert.ToDateTime(dr["FECHA"].ToString());


                }
            }
            catch (Exception ex)
            {
                busq = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error Justificación search"));
            }
            return busq;
        }
        #endregion

        #region Editar Justificación
        public int UpdateJustificacion(ref BaseEntity Base, clsJustification objJust)
        {
            SqlCommand cmd = null;
            int isCorrect = 0;
            try
            {
                cmd = new SqlCommand("sp_xP_Editar_Asignacion_Justificacion", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JustificacionId", objJust.ID);
                cmd.Parameters.AddWithValue("@Archivo", objJust.Archivo);
                cmd.Parameters.AddWithValue("@EmpleadoId", objJust.EmpleadoId);
                cmd.Parameters.AddWithValue("@JustificacionPlantillaId", objJust.justificacion.ID);
                cmd.Parameters.AddWithValue("@Motivo", objJust.Motivo);
                cmd.Parameters.AddWithValue("@Fecha", objJust.Fecha);

                if (objJust.TardanzaId == 0)
                {
                    cmd.Parameters.AddWithValue("@TardanzaId", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TardanzaId", objJust.TardanzaId);
                }
                if (objJust.InasistenciaId == 0)
                {
                    cmd.Parameters.AddWithValue("@InasistenciaId", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InasistenciaId", objJust.InasistenciaId);
                }
                cmd.Parameters.AddWithValue("@LastUpdateBy", objJust.Updatedby);

                isCorrect = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                isCorrect = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error occurred on Save Justificación"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }
        #endregion

        public int UpdateJustificacion2(ref BaseEntity Base, clsJustification objJust) 
        {
            SqlCommand cmd = null;
            int isCorrect = 0;
            try
            {
                cmd = new SqlCommand("sp_xP_Editar_Asignacion_Justificacion", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JustificacionId", objJust.ID);
                cmd.Parameters.AddWithValue("@Archivo", objJust.Archivo);
                cmd.Parameters.AddWithValue("@EmpleadoId", objJust.EmpleadoId);
                cmd.Parameters.AddWithValue("@JustificacionPlantillaId", objJust.justificacion.ID);
                cmd.Parameters.AddWithValue("@Motivo", objJust.Motivo);
                cmd.Parameters.AddWithValue("@Fecha", objJust.Fecha);

                if (objJust.TardanzaId == 0)
                {
                    cmd.Parameters.AddWithValue("@TardanzaId", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TardanzaId", objJust.TardanzaId);
                }
                if (objJust.InasistenciaId == 0)
                {
                    cmd.Parameters.AddWithValue("@InasistenciaId", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InasistenciaId", objJust.InasistenciaId);
                }
                cmd.Parameters.AddWithValue("@LastUpdateBy", objJust.Updatedby);

                isCorrect = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                isCorrect = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error occurred on Save Justificación"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }


        #region Eliminar
        public Boolean DeleteJustificacion(ref BaseEntity Base, List<clsJustification> lstIds)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            try
            {//ToDo:VERIFICAR SI DAO DE DLETE ESTA BIEN
                foreach (clsJustification item in lstIds)
                {
                    objConnection = clsConnection.GetConnection();
                    cmd = new SqlCommand("sp_xP_Eliminar_Asignacion_Justificacion", objConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", item.ID);
                    cmd.Parameters.AddWithValue("@UpdatedBy", item.Updatedby);

                    success = cmd.ExecuteNonQuery() > 0 ? true : false;
                }

            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error in database"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return success;
        }

        #endregion

        #region vistaPrevia Justificación
        public clsJustification obtieneVistaPreviaJustificacion(int JustificacionId, string tag) {

            clsJustification us = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = null;

            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("xp_sP_ObtieneDatos_Tag", cn);
                cmd.Parameters.AddWithValue("@JUSTIFICACIONID", JustificacionId);
                cmd.Parameters.AddWithValue("@TAGID", tag);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    us = new clsJustification();
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
        #endregion


    }
}
