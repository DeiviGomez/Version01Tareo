using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using xAPI.Library.Base;
using xAPI.Library;
using xAPI.Library.Connection;
using xAPI.Entity;

namespace xAPI.Dao
{
    public class clsTemplateMemorandumcsDAO
    {
        #region Singleton

        public static readonly clsTemplateMemorandumcsDAO _instance = new clsTemplateMemorandumcsDAO();
        public static clsTemplateMemorandumcsDAO Instance
        {
            get { return clsTemplateMemorandumcsDAO._instance; }
        }
        #endregion 

        #region GuardarPlantillaMemorandum
        public int SaveTemplTemplateMemorandum(ref BaseEntity Entity, clsTemplateMemorandumcs objMemo)
        {
            int i = 0;
            SqlCommand cmd = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Guardar_Plantilla_memorandum", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", objMemo.EmpresaId);
                cmd.Parameters.AddWithValue("@MemorandumPlantillaTipo", objMemo.MemorandumPlantillaTipo);
                cmd.Parameters.AddWithValue("@MemorandumContenido", objMemo.MemorandumContenido);
                cmd.Parameters.AddWithValue("@Principal", objMemo.Principal);
                cmd.Parameters.AddWithValue("@CreatedBy", objMemo.Createdby);

                i = cmd.ExecuteNonQuery(); 

            }
            catch (Exception ex)
            {
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Error saving Template Memorandum"));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;

        }

    #endregion

        #region ListarPlantillaMemorandum
        public DataTable ListTempMemorandum(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Plantilla_Memorandum", clsConnection.GetConnection());
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


         public DataTable ListTempMemorandum2(ref BaseEntity Entity, Int32 id) 
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Reportar_Plantilla_Memorandum", clsConnection.GetConnection());
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
        

        //harold bautista

        #region ListarTipoMemorandumPorEmpresa
        public DataTable ListTempMemorandumPorEmp(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Plantilla_Memorandum", clsConnection.GetConnection());
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

        //harold bautista




        #region Eliminar
        public Boolean DeleteTemplateMemorandum(ref BaseEntity Base, List<clsTemplateMemorandumcs> lstIds)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            try
            {//ToDo:VERIFICAR SI DAO DE DLETE ESTA BIEN
                foreach (clsTemplateMemorandumcs item in lstIds)
                {
                    objConnection = clsConnection.GetConnection();
                    cmd = new SqlCommand("sp_xP_Eliminar_Plantilla_Memorandum", objConnection);
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

        #region Buscar
        public clsTemplateMemorandumcs SearchTemplateMemorandum(ref BaseEntity Base, int loanId)
        {
            clsTemplateMemorandumcs busq = null;
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Buscar_Plantilla_Memorandum", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@prmId", loanId);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    busq = new clsTemplateMemorandumcs();
                    busq.ID = Convert.ToInt32(dr["MemorandumPlantillaId"]);
                    busq.EmpresaId = Convert.ToInt32(dr["EmpresaId"]);
                    busq.MemorandumPlantillaTipo = dr["MemorandumPlantillaTipo"].ToString();
                    busq.MemorandumContenido = dr["MemorandumContenido"].ToString();
                    busq.Principal = Convert.ToInt32(dr["Principal"]);
                }
            }
            catch (Exception ex)
            {
                busq = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error Template Memorandum search"));
            } return busq;
        }
        #endregion

        #region Editar
        public int MemorandumTemplate_Update(ref BaseEntity Base, clsTemplateMemorandumcs objMemo)
        {
            SqlCommand cmd = null;
            int isCorrect = 0;
            try
            {
                cmd = new SqlCommand("sp_xP_Editar_Plantilla_Memorandum", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemorandumPlantillaId", objMemo.ID);
                cmd.Parameters.AddWithValue("@MemorandumPlantillaTipo", objMemo.MemorandumPlantillaTipo);
                cmd.Parameters.AddWithValue("@MemorandumContenido", objMemo.MemorandumContenido);
                cmd.Parameters.AddWithValue("@Principal", objMemo.Principal);
                cmd.Parameters.AddWithValue("@LastUpdateBy", objMemo.Updatedby);

                isCorrect = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                isCorrect = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error occurred on Save Memorandum Template"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }
        #endregion

        #region ContenidoPlantillaMemorandum
        public string Listar_Contenido_Memorandum(ref BaseEntity Entity, Int32 id)
        {
            String i = String.Empty;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Contenido_Memorandum", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    i = dr[1].ToString();
                }
            }
            catch (Exception ex)
            {
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Error al traer contenido memorandum"));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;
        }
        public int Listar_Plantilla_Memorandum_Id(ref BaseEntity Entity, Int32 id)
        {
            Int32 i = 0;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Plantilla_Memorandum_ID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    i = Convert.ToInt32(dr[0].ToString());
                }
            }
            catch (Exception ex)
            {
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Error al traer contenido memorandum"));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;

        }

        //para la vista de la plantilla memorandum
        public int Listar_Plantilla_Memorandum_Id_2(ref BaseEntity Entity, Int32 id)
        {
            Int32 i = 0;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Plantilla_Memorandum_ID_2", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    i = Convert.ToInt32(dr[0].ToString());
                }
            }
            catch (Exception ex)
            {
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Error al traer contenido memorandum"));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;

        }
        #endregion

        #region Validad_Plantilla
        public Boolean Valida_PlantillaMemo(ref BaseEntity Base, String tipo, Int32 Empresa)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Valida_ValidaMemorandum", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@EmpresaId", Empresa);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en Dao"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }

            return success;
        }
        #endregion

    }
}
