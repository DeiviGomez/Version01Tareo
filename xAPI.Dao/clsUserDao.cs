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
    public class clsUserDao : BaseDao//,IDao
    {
        #region Singleton
        private static clsUserDao instance = null;
        public static clsUserDao Instance
        {
            get
            {
                if (instance == null)
                    instance = new clsUserDao();
                return instance;
            }
        }
        #endregion

        #region Login
        /*Validar Usuario*/
        public clsUser UserValidate(ref BaseEntity Base, string email, string password)
        {
            clsUser User = null;
            SqlDataReader dr = null;/* DATAREADER:Trabaja apuntando a la conexion el data set hace una copia de datos y trabaja sin conexion*/
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Validar_Usuario", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                //SqlParameter outputParam = cmd.Parameters.Add("@res", SqlDbType.Int);
                //outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@UserName", email);
                cmd.Parameters.AddWithValue("@UserPassword", password);
                dr = cmd.ExecuteReader();
                //if (dr.Read())
                //    User = GetEntity_v3(dr);
                if (dr.Read())/* el metodo read del objeto datareader permite leer filas */
                {
                    User = new clsUser();
                    User.ID = Convert.ToInt32(dr["UsuarioId"]);
                    User.Nombres = dr["Nombres"].ToString();
                    User.Apellidos = dr["Apellidos"].ToString();
                    User.Email = dr["Email"].ToString();
                    User.Password = dr["Password"].ToString();
                    User.Imagen = dr["Imagen"].ToString();
                    User.TipoUsuario.ID = Convert.ToInt32(dr["TipoUsuarioId"]);
                    User.Estado = Convert.ToBoolean(dr["Estado"]);
                    User.Empresa.ID = Convert.ToInt32(dr["EmpresaId"]);
                    User.Empresa.RazonSocial = dr["RazonSocial"].ToString();
                    User.empresa.NumeroRuc = dr["NumeroRuc"].ToString();
                    //User.Postulante = Convert.ToInt32(cmd.Parameters["@res"].Value);
                    /*GUARDA EN EL OBJETO TODOS LOS DATOS DEL USUARIO PARA SETEARLO EN LA SESION*/
                }
            }
            catch (Exception ex)
            {
                User = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Usuario Invalido"));
            }
            finally
            {
                if (dr != null) { dr.Close(); }
                clsConnection.DisposeCommand(cmd);
            }
            return User;

            //    goto Outer;
            //Outer:
            //    User;
        }
        /*Modificar Password luego de logearse*/
        public Boolean UpdatePassword(ref BaseEntity entitym, ref clsUser objuser){
            Boolean iscorrect = false;
            SqlConnection objConnection = null;
            try
            {
                objConnection = clsConexion.ObtenerConexion();
                SqlCommand objcmd = new SqlCommand("sp_xP_Actualizar_Password_Usuario", objConnection);
                objcmd.Parameters.AddWithValue("@UserId", objuser.ID);
                objcmd.Parameters.AddWithValue("@Password", objuser.Password);
                objcmd.Parameters.AddWithValue("@LastUpdateBy", objuser.Updatedby);

                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.ExecuteNonQuery();
                iscorrect = true;
            }
            catch (Exception ex)
            {
                entitym.Errors.Add(new BaseEntity.ListError(ex, "Error en la Base de datos"));
            }
            finally { clsConnection.DisposeCommand(objConnection); }
            return iscorrect;
        }
        #endregion Login

        #region MantenedorUsuario

        /*Listar Tipo de Usuario*/
        public DataTable List_Type_User(ref BaseEntity Base)
        {
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Usuario", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }
            finally { cmd.Connection.Close(); }
            return dt;
        }

        /*Insertar usuario*/
        public Boolean SaveUser(ref BaseEntity Base, clsUser objUser)
        {
            Boolean success = false;
            SqlConnection objconnection = null;
            SqlCommand cmd = null;

            try{
                objconnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Guardar_Usuario", objconnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", objUser.Nombres);
                cmd.Parameters.AddWithValue("@LastName", objUser.Apellidos);
                cmd.Parameters.AddWithValue("@Email", objUser.Email);
                cmd.Parameters.AddWithValue("@Password", objUser.Password);
                cmd.Parameters.AddWithValue("@MyPicture", objUser.Imagen);
                cmd.Parameters.AddWithValue("@EmployeId", objUser.empleado.ID);
                cmd.Parameters.AddWithValue("@TyperUserId", objUser.TipoUsuario.TipoUsuarioId);
                cmd.Parameters.AddWithValue("@CompanyId", objUser.EmpresaUsuario.EmpresaId);
                cmd.Parameters.AddWithValue("@CreatedBy", objUser.Createdby);
                success = cmd.ExecuteNonQuery() > 0 ? true : false;
            }catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }finally {
                clsConnection.DisposeCommand(cmd);
            }
            return success;
        }

        /* Listar Usuario 2 */
        public List<clsUser> ListUser2(ref BaseEntity Base, Int32 idCompany) {
            SqlCommand cmd = null;
            IDataReader idr = null;
            List<clsUser> lista = null;
            SqlConnection objconnection = null;
            try{
                objconnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Listar_Usuario", objconnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCompany", idCompany);
                idr = cmd.ExecuteReader();
                lista = new List<clsUser>();
                while(idr.Read()){
                    clsUser user = new clsUser();
                    user.ID = Convert.ToInt32(idr["UsuarioId"]);
                    user.Nombres = idr["Nombres"].ToString();
                    user.Apellidos = idr["Apellidos"].ToString();
                    user.Email = idr["Email"].ToString();
                    user.Password = idr["Password"].ToString();
                    user.Estado = Convert.ToBoolean(idr["Estado"]);
                    user.TipoUsuario.TipoUsuarioId = Convert.ToInt32(idr["TipoUsuarioId"]);
                    user.TipoUsuario.Nombre = idr["NombreTipo"].ToString();
                    lista.Add(user);
                }
            }
            catch (Exception ex){
                lista = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return lista;
        }

        /*Eliminar usuario*/
        public Boolean DeleteUser(ref BaseEntity Base, List<clsUser> lstIds)
        {
            Boolean success = false;
            SqlConnection objConnection = null;
            SqlCommand cmd = null;
            try
            {
                foreach (clsUser item in lstIds)
                {
                    objConnection = clsConnection.GetConnection();
                    cmd = new SqlCommand("sp_xP_Eliminar_Usuario", objConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", item.ID);
                    cmd.Parameters.AddWithValue("@updateBy", item.Updatedby);

                    success = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }
            finally {
                clsConnection.DisposeCommand(cmd);
            }
            return success;
        }

        /*Buscar Usuario para editar*/

        public clsUser Search_User(ref BaseEntity Base, Int32 UserId) {
            clsUser us = null;
            SqlCommand cmd= null;
            SqlDataReader dr = null;
            SqlConnection cn = null;

            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Buscar_Usuario", cn);
                cmd.Parameters.AddWithValue("@userId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    us = new clsUser();
                    us.ID = Convert.ToInt32(dr["UsuarioId"]);
                    us.Nombres = dr["Nombres"].ToString();
                    us.Apellidos = dr["Apellidos"].ToString();
                    us.Email = dr["Email"].ToString();
                    us.Password = dr["Password"].ToString();
                    us.Imagen = dr["Imagen"].ToString();
                    clsEmployee em = new clsEmployee();
                    em.ID = Convert.ToInt32(dr["EmpleadoId"]);
                    us.empleado = em;
                    clsTypeUser ty = new clsTypeUser();
                    ty.ID = Convert.ToInt32(dr["TipoUsuarioId"]);
                    us.TipoUsuario = ty;
                    us.Estado = Convert.ToBoolean(dr["Estado"]);
                }

            }
            catch (Exception ex)
            {
                us = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return us;
        }

        /*Editar Usuario*/
        public Boolean User_Update(ref BaseEntity Base, clsUser objuser) {
            Boolean update = false;
            SqlConnection objconnection = null;
            SqlCommand cmd = null;
            try
            {
                objconnection = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Editar_Usuario", objconnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", objuser.ID);
                cmd.Parameters.AddWithValue("@Name", objuser.Nombres);
                cmd.Parameters.AddWithValue("@LastName", objuser.Apellidos);
                cmd.Parameters.AddWithValue("@Email", objuser.Email);
                cmd.Parameters.AddWithValue("@Password", objuser.Password);
                cmd.Parameters.AddWithValue("@Mypicture", objuser.Imagen);
                cmd.Parameters.AddWithValue("@EmployeeId", objuser.Empleado.ID);
                cmd.Parameters.AddWithValue("@TyperUserId", objuser.TipoUsuario.TipoUsuarioId);
                cmd.Parameters.AddWithValue("@updateBy", objuser.Updatedby);

                update = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                update = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return update;
        }

        /*validacion de email*/

        public Boolean ValidateByEmail(ref BaseEntity Base, String Email)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            SqlConnection objc = null;
            SqlDataReader dr = null;
            try
            {
                objc = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Validar_Email", objc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", Email);
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return success;
        }

        /**/
        public Boolean ValidateHora(ref BaseEntity Base,  String Hora)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            SqlConnection objc = null;
            SqlDataReader dr = null;
            try
            {
                objc = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_ValidarHora", objc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fecha", Hora);
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return success;
        }

        #endregion MantenedorUsuario

        #region MantenedorAssign
        public DataTable Employee_GetByName(ref BaseEntity Base, String name, Int32 idCompany)
        {
            SqlConnection objConexion = null;
            DataTable dt = new DataTable();
            try
            {
                objConexion = clsConnection.GetConnection();
                SqlCommand cmd = new SqlCommand("sp_xP_Buscar_Empleado", objConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@companyId", idCompany);
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Dao del usuario"));
            }
            return dt;
        }
        #endregion


        #region EliminarUsuarioPostulante
        public Boolean Eliminar_Empleado_Postulante(ref BaseEntity Base, Int32 idUsuario) {
            SqlConnection cn = null;
            Boolean elimino = false;
            SqlCommand cmd = null;
            try
            {
                cn = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_Eliminar_Usuario_Postulante", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                elimino = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception e)
            {
                elimino = false;
                Base.Errors.Add(new BaseEntity.ListError(e, "Error en la capa Dao del usuario"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return elimino;
        }
        #endregion
    }
}
