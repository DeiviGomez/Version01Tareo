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
    public class clsMemorandumDAO
    {
        #region Singleton

        public static readonly clsMemorandumDAO _instance = new clsMemorandumDAO();
        public static clsMemorandumDAO Instance
        {
            get { return clsMemorandumDAO._instance; }
        }
        #endregion 

        #region GuardarMemorandum
        public int SaveMemorandum(ref BaseEntity Entity, clsMemorandums objAsgMemo)
        {
            int i = 0;
            SqlCommand cmd = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Guardar_Asignacion_Memorandum", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", objAsgMemo.EmpresaId);
                cmd.Parameters.AddWithValue("@Archivo", objAsgMemo.Archivo);
                cmd.Parameters.AddWithValue("@EmpleadoId", objAsgMemo.EmpleadoId);
                cmd.Parameters.AddWithValue("@MemorandumPlantillaId", objAsgMemo.memorandum.ID);
                cmd.Parameters.AddWithValue("@Motivo", objAsgMemo.Motivo);
                cmd.Parameters.AddWithValue("@Descripcion", objAsgMemo.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha", objAsgMemo.Fecha);
                cmd.Parameters.AddWithValue("@TipoMemorandum", objAsgMemo.TipoMemorandum);
                cmd.Parameters.AddWithValue("@CreatedBy", objAsgMemo.Createdby);

                if (objAsgMemo.TareoTardanzaId == 0)
                {
                    cmd.Parameters.AddWithValue("@tareotardanza", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@tareotardanza", objAsgMemo.TareoTardanzaId);
                }

                if (objAsgMemo.TareoInasistenciaId ==0)
                {
                    cmd.Parameters.AddWithValue("@tareoinasistencia", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("tareoinasistencia", objAsgMemo.TareoInasistenciaId);
                }


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

        public int SaveMemorandum2(ref BaseEntity Entity, clsMemorandums objAsgMemo) 
        {
            int i = 0;
            SqlCommand cmd = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Guardar_Asignacion_Memorandum", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", objAsgMemo.EmpresaId);
                cmd.Parameters.AddWithValue("@Archivo", objAsgMemo.Archivo);
                cmd.Parameters.AddWithValue("@EmpleadoId", objAsgMemo.EmpleadoId);
                cmd.Parameters.AddWithValue("@MemorandumPlantillaId", objAsgMemo.memorandum.ID);
                cmd.Parameters.AddWithValue("@Motivo", objAsgMemo.Motivo);
                cmd.Parameters.AddWithValue("@Descripcion", objAsgMemo.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha", objAsgMemo.Fecha);
                cmd.Parameters.AddWithValue("@TipoMemorandum", objAsgMemo.TipoMemorandum);
                cmd.Parameters.AddWithValue("@CreatedBy", objAsgMemo.Createdby);

                if (objAsgMemo.TareoTardanzaId == 0)
                {
                    cmd.Parameters.AddWithValue("@tareotardanza", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@tareotardanza", objAsgMemo.TareoTardanzaId);
                }

                if (objAsgMemo.TareoInasistenciaId == 0)
                {
                    cmd.Parameters.AddWithValue("@tareoinasistencia", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("tareoinasistencia", objAsgMemo.TareoInasistenciaId);
                }


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

        #region ListarMemorandum
        public DataTable ListMemorandum(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Asignacion_Memorandum", clsConnection.GetConnection());
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
        #endregion

        #region Buscar
        public clsMemorandums SearchMemorandum(ref BaseEntity Base, int loanId)
        {
            clsMemorandums busq = null;
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Buscar_Asignacion_Memorandum", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@prmId", loanId);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    
                    busq = new clsMemorandums();
                    busq.ID = Convert.ToInt32(dr["IDMEMORANDUM"]); 
                    busq.EmpresaId = Convert.ToInt32(dr["IDEMPRESA"]);
                    busq.Archivo = dr["DOCUMENTO"].ToString();
                    
                    //obtener empleado de lista (id, nombre)
                    clsEmployee objEmp = new clsEmployee();
                    objEmp.Name = dr["NOMBEMPLEADO"].ToString(); //nombre
                    busq.EmpleadoId = Convert.ToInt32(dr["EmpleadoId"]);
                    busq.employee = objEmp; //guardo valores en mi objeto tipo clase
                    
                    //obtener valor del combo (id)
                    clsTemplateMemorandumcs objTM = new clsTemplateMemorandumcs();
                    objTM.ID = Convert.ToInt32(dr["IDPLANTILLAMEMORANDUM"]);
                    busq.memorandum = objTM;

                    //
                    busq.Motivo = dr["RAZON"].ToString();
                    busq.Descripcion = dr["DESCRIPCION"].ToString();
                    busq.Fecha = Convert.ToDateTime(dr["FECHA"].ToString());
                    busq.TipoMemorandum = Convert.ToInt32(dr["TIPOMEMORANDUM"].ToString());

                }
            }
            catch (Exception ex)
            {
                busq = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error Memorandum search"));
            }
            return busq;
        }
        #endregion

        #region Editar Memoradum
        public int UpdateMemorandum(ref BaseEntity Base, clsMemorandums objMemo)
        {
            SqlCommand cmd = null;
            int isCorrect = 0;
            try
            {
                cmd = new SqlCommand("sp_xP_Editar_Asignacion_Memorandum", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemorandumId", objMemo.ID);
                cmd.Parameters.AddWithValue("@Archivo", objMemo.Archivo);
                cmd.Parameters.AddWithValue("@EmpleadoId", objMemo.EmpleadoId);
                cmd.Parameters.AddWithValue("@MemorandumPlantillaId", objMemo.memorandum.ID);
                cmd.Parameters.AddWithValue("@Motivo", objMemo.Motivo);
                cmd.Parameters.AddWithValue("@Descripcion", objMemo.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha", objMemo.Fecha);
                cmd.Parameters.AddWithValue("@TipoMemorandum", objMemo.TipoMemorandum);
                cmd.Parameters.AddWithValue("@LastUpdateBy", objMemo.Updatedby);

                isCorrect = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                isCorrect = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error occurred on Save Memorandum"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }
        #endregion

        public int UpdateMemorandum2(ref BaseEntity Base, clsMemorandums objMemo) 
        {
            SqlCommand cmd = null;
            int isCorrect = 0;
            try
            {
                cmd = new SqlCommand("sp_xP_Editar_Asignacion_Memorandum", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemorandumId", objMemo.ID);
                cmd.Parameters.AddWithValue("@Archivo", objMemo.Archivo);
                cmd.Parameters.AddWithValue("@EmpleadoId", objMemo.EmpleadoId);
                cmd.Parameters.AddWithValue("@MemorandumPlantillaId", objMemo.memorandum.ID);
                cmd.Parameters.AddWithValue("@Motivo", objMemo.Motivo);
                cmd.Parameters.AddWithValue("@Descripcion", objMemo.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha", objMemo.Fecha);
                cmd.Parameters.AddWithValue("@TipoMemorandum", objMemo.TipoMemorandum);
                cmd.Parameters.AddWithValue("@LastUpdateBy", objMemo.Updatedby);

                isCorrect = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                isCorrect = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error occurred on Save Memorandum"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }

        #region Eliminar
        public Boolean DeleteMemorandum(ref BaseEntity Base, List<clsMemorandums> lstIds)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            try
            {//ToDo:VERIFICAR SI DAO DE DLETE ESTA BIEN
                foreach (clsMemorandums item in lstIds)
                {
                    objConnection = clsConnection.GetConnection();
                    cmd = new SqlCommand("sp_xP_Eliminar_Asignacion_Memorandum", objConnection);
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

        #region vistaPrevia Memorandum
        public clsMemorandums obtieneVistaPreviaMemo(int MemorandumId, string tag)
        {

            clsMemorandums us = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = null;

            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("xp_sP_ObtieneDatosMemorandum_Tag", cn);
                cmd.Parameters.AddWithValue("@MemorandumId", MemorandumId);
                cmd.Parameters.AddWithValue("@TagId", tag);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    us = new clsMemorandums();
                    us.MemoTag = dr[0].ToString();
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
