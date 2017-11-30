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
    public class clsTemplateJustificationDao : BaseDao
    {
        #region Singleton

        public static readonly clsTemplateJustificationDao _instance = new clsTemplateJustificationDao();
        public static clsTemplateJustificationDao Instance
        {
            get { return clsTemplateJustificationDao._instance; }
        }
        #endregion

        #region RegistrarPlantillaJustificacion
        public int SaveTemplTemplateJustification(ref BaseEntity Entity, clsTemplateJustification objJust)
        {
            int i = 0;
            SqlCommand cmd = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {

                cmd = new SqlCommand("sp_xP_Guardar_Plantilla_Justificacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", objJust.EmpresaId);
                cmd.Parameters.AddWithValue("@JustificacionPlantillaTipo", objJust.TypeJustification.ID);
                cmd.Parameters.AddWithValue("@JustificacionContenido", objJust.JustificacionContenido);
                cmd.Parameters.AddWithValue("@CreatedBy", objJust.Createdby);

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

        #region ListarPlantillaJustificaciones
        public DataTable ListTemplateJustidication(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Plantilla_Justificacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
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
                Entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
        #endregion

        #region ListarPlantillaJustificaciones2
        public DataTable ListTemplateJustidication2(ref BaseEntity Entity, Int32 id) 
        {
            DataTable dt = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Reportar_Plantilla_Justificacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
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
                Entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
        #endregion


        #region Eliminar
        public Boolean DeleteTemplateJustification(ref BaseEntity Base, List<clsTemplateJustification> lstIds)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            try
            {//ToDo:VERIFICAR SI DAO DE DLETE ESTA BIEN
                foreach (clsTemplateJustification item in lstIds)
                {
                    objConnection = clsConnection.GetConnection();
                    cmd = new SqlCommand("sp_xP_Eliminar_Plantilla_Justificacion", objConnection);
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
        public clsTemplateJustification SearchTemplateJustification(ref BaseEntity Base, int loanId)
        {
            clsTemplateJustification busq = null;
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Buscar_Plantilla_Justificacion", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@prmId", loanId);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    busq = new clsTemplateJustification();
                    busq.ID = Convert.ToInt32(dr["JustificacionPlantillaId"]);
                    busq.EmpresaId = Convert.ToInt32(dr["EmpresaId"]);
                        clsTypeJustification t = new clsTypeJustification();
                        t.ID = Convert.ToInt32(dr["JustificacionPlantillaTipo"]);
                    busq.TypeJustification = t;
                    //busq.JustificacionPlantillaTipo = dr["JustificacionPlantillaTipo"].ToString();
                    busq.JustificacionContenido = dr["JustificacionContenido"].ToString();
                }
            }
            catch (Exception ex)
            {
                busq = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error Template Justification search"));
            } return busq;
        }
        #endregion

        #region Editar
        public int JustificationTemplate_Update(ref BaseEntity Base, clsTemplateJustification objUpJust)
        {
            SqlCommand cmd = null;
            int isCorrect = 0;
            try
            {
                cmd = new SqlCommand("sp_xP_Editar_Plantilla_Justificacion", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JustificacionPlantillaId", objUpJust.ID);
                cmd.Parameters.AddWithValue("@JustificacionPlantillaTipo", objUpJust.TypeJustification.ID);
                cmd.Parameters.AddWithValue("@JustificacionContenido", objUpJust.JustificacionContenido);
                cmd.Parameters.AddWithValue("@LastUpdateBy", objUpJust.Updatedby);

                isCorrect = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                isCorrect = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error occurred on Save Template Justification"));
            }
            finally { cmd.Connection.Close(); }
            return isCorrect;
        }
        #endregion

        #region ContenidoPlantillaJustificación
        public string Listar_Contenido_Justificacion(ref BaseEntity Entity, Int32 id)
        {
            String i = String.Empty;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Contenido_Justificacion", cn);
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
        public int Listar_Plantilla_Justificacion_Id(ref BaseEntity Entity, Int32 id)
        {
            Int32 i = 0;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Plantilla_Justificacion_ID", cn);
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
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Error al traer contenido justificacion"));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;

        }

        //para la vista de la plantilla justificación
        public int Listar_Plantilla_Justificacion_Id_2(ref BaseEntity Entity, Int32 id)
        {
            Int32 i = 0;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            SqlConnection cn = clsConnection.GetConnection();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Plantilla_Justificacion_ID_2", cn);
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
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Error al traer contenido justificacion"));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return i;

        }
        #endregion

        #region Valida_Justificacion
        public Boolean Valida_Justificacion(ref BaseEntity Base, Int32 tipo, Int32 Empresa)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Valida_Justificacion", clsConnection.GetConnection());
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
