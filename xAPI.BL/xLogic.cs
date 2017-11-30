using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Entity;
using xAPI.Library.Base;
using xAPI.Dao;
using xAPI.Library;
using System.Data;
using System.Xml;
using xAPI.Library.Security;
using xAPI.Library.General;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.SqlClient;
using xAPI.Library.Connection;
using System.Web;
using System.Threading;


namespace xAPI.BL
{
    public class xLogic
    {
        #region singleton
        private static readonly xLogic _instance = new xLogic();
        public static xLogic Instance
        {
            get { return xLogic._instance; }
        }
        #endregion

      

        #region Inasistencias Empleados



        public Boolean GuardarInasistencias(ref BaseEntity Base, clsInasistencia objInasistencia)
        {
            return clsInasistenciaDAO.Instance.GuardarInasistencia(ref Base, objInasistencia);
        }

        public DataTable Listar_Empleados_Inasistencias(ref BaseEntity Base, DateTime day, string nombredia, Int32 idEmpInas)
        {
            Base = new BaseEntity();
            DataTable dt = null;
            try
            {
                dt = clsInasistenciaDAO.Instance.Listar_Empleados_Inasistencias(ref Base, day, nombredia, idEmpInas);
            }
            catch (Exception)
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
            }

            return dt;
        }

        public Boolean ValidarInasistencia(ref BaseEntity Base, clsInasistencia objInasistencia)
        {
            return clsInasistenciaDAO.Instance.ValidarInasistencia(ref Base, objInasistencia);
        }

        public Boolean EditarInasistencia(ref BaseEntity Base, clsInasistencia objInasistenia)
        {
            return clsInasistenciaDAO.Instance.EditarInasistencia(ref Base, objInasistenia);
        }

        #endregion

        #region Tardanzas Empleados

        public Boolean EditarTardaza(ref BaseEntity Base, clsTardanza objclsTardiness, string nombredia)
        {
            return clsTardanzaDAO.Instance.EditarTardanza(ref Base, objclsTardiness, nombredia);
        }

        public Boolean GuardarTardanza(ref BaseEntity Base, clsTardanza objclsTardiness, string nombredia)
        {
            return clsTardanzaDAO.Instance.GuardarTardanza(ref Base, objclsTardiness, nombredia);
        }

        public DataTable Listar_Tardanza_Dia(ref BaseEntity Base, DateTime day, string nombredia, Int32 idEmpTard)
        {
            Base = new BaseEntity();
            DataTable dt = null;
            try
            {
                dt = clsTardanzaDAO.Instance.Listar_Empleados_Tardanza_Diario(ref Base, day, nombredia, idEmpTard);
            }
            catch (Exception)
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
            }
            return dt;
        }

        #endregion

        #region Tareo Empleados  

        public Boolean Validar_Feriado(ref BaseEntity Base, DateTime fecha, int empresaid)
        {
            return clsTareoDAO.Instance.Validar_Feriado(ref Base, fecha, empresaid);
        }

        public Boolean Realizar_Registro_Vacaciones(ref BaseEntity Base)
        {
            return clsTareoDAO.Instance.Realizar_Registro_Vacaciones(ref Base);
        }
        /*Actualizar Vacaciones a Truncas*/
        public Boolean Realizar_UpdateVacacionesTruncas(ref BaseEntity Base)
        {
            return clsTareoDAO.Instance.Realizar_UpdateVacacionesTruncas(ref Base);
        }

        public Boolean Generar_Mes_Tareo(ref BaseEntity Base, DateTime DatoMes)
        {
            return clsTareoDAO.Instance.Generar_Mes_Tareo(ref Base, DatoMes);
        }

        public Boolean Realizar_Tareo_Mensaul(ref BaseEntity Base, int CompanyId, int UsuarioId)
        {
            return clsTareoDAO.Instance.Realizar_Tareo_Mensaul(ref Base, CompanyId, UsuarioId);
        }

        public Boolean Reescribir_Tareo_Mensaul(ref BaseEntity Base, int CompanyId, int UsuarioId)
        {
            return clsTareoDAO.Instance.Reescribir_Tareo_Mensaul(ref Base, CompanyId, UsuarioId);
        }


        public Boolean EditarTareo(ref BaseEntity Base, clsTareo objclsTareo)
        {
            return clsTareoDAO.Instance.EditarTareo(ref Base, objclsTareo);
        }

        public int ValidaEmpleadoDiaLaboral(ref BaseEntity Base, clsTareo objTareo, string nombredia)
        {
            Base = new BaseEntity();
            int respuesta = 0;
            try
            {
                respuesta = clsTareoDAO.Instance.ValidaEmpleadoDiaLaboral(ref Base, objTareo, nombredia);
            }
            catch (Exception)
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
            }
            return respuesta;
        }

        public int ValidaPartTime(ref BaseEntity Base, clsTareo objTareo, string nombredia)
        {
            Base = new BaseEntity();
            int respuesta = 0;
            try
            {
                respuesta = clsTareoDAO.Instance.ValidaPartime(ref Base, objTareo, nombredia);
            }
            catch (Exception)
            {

                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
            }

            return respuesta;
        }

        public int ValidaFullTime(ref BaseEntity Base, clsTareo objTareo, string nombredia)
        {
            Base = new BaseEntity();
            int respuesta = 0;
            try
            {
                respuesta = clsTareoDAO.Instance.ValidaFullTime(ref Base, objTareo, nombredia);
            }
            catch (Exception)
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
            }
            return respuesta;
        }
        public int ValidaInasistenciaPartime(ref BaseEntity Base, clsTareo objTareo, string nombredia)
        {
            Base = new BaseEntity();
            int respuesta = 0;
            try
            {
                respuesta = clsTareoDAO.Instance.ValidaInasistenciaPartime(ref Base, objTareo, nombredia);
            }
            catch (Exception)
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
            }
            return respuesta;
        }

        public DataTable Listar_Empleados_Vacaciones(ref BaseEntity Base, DateTime fecha)
        {
            Base = new BaseEntity();
            DataTable dt = null;

            try
            {
                dt = clsTareoDAO.Instance.Listar_Empleados_Vacaciones(ref Base, fecha);
            }
            catch (Exception)
            {

                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
            }
            return dt;
        }

        public DataTable Listar_Empleado_Fecha_horario(ref BaseEntity Base, String day, DateTime fecha, Int32 idEmp)
        {
            Base = new BaseEntity();
            DataTable dt = null;

            try
            {
                dt = clsTareoDAO.Instance.Listar_Empleado_Fecha_Horario(ref Base, day, fecha, idEmp);
            }
            catch (Exception)
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
            }

            return dt;

        }

        //public DataTable Listar_Empleados_Fecha_Horario(ref BaseEntity Base, String day)
        //{
        //    Base = new BaseEntity();
        //    DataTable dt = null;

        //    try
        //    {
        //        dt = clsTareoDAO.Instance.Listar_Empleados_Fecha_Horario(ref Base, day);
        //    }
        //    catch (Exception)
        //    {
        //        Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "Error en el envio de datos"));
        //    }

        //    return dt;


        //}

        //public clsEmployee GetEmployeeByID(ref BaseEntity Base, int Employeeid)
        //{
        //    return clsTareoDAO.Instance.GetEmployeeByID(ref Base, Employeeid);
        //}

        public Boolean GuardarTareo(ref BaseEntity Base, clsTareo objclsTareo, int empresaid)
        {
            return clsTareoDAO.Instance.GuardarTareo(ref Base, objclsTareo, empresaid);
        }

        public Boolean GuardarEditarTareoMasivo(ref BaseEntity Base, clsTareo objclsTareo, int empresaid)
        {
            return clsTareoDAO.Instance.GuardarEditarTareoMasivo(ref Base, objclsTareo, empresaid);
        }


        #endregion



        #region Login y Sesion
        public clsUser UserValidate(ref BaseEntity Base, string email, string password)
        {
            Base = new BaseEntity();
            clsUser objUser = null;
            try
            {
                if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
                {
                    objUser = clsUserDao.Instance.UserValidate(ref Base, email, clsEncryption.Encrypt(password));


                    String message = String.Empty;
                    if (objUser != null)
                    {
                    }
                    else
                        Base.Errors.Add(new BaseEntity.ListError(new Exception(), "Usuario o Password no validos"));
                }

            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "A ocurrido un error en la capa Logica"));
            }

            return objUser;
        }

        public Boolean User_UpdatePassword(ref BaseEntity Base, ref clsUser objUser)
        {
            Base = new BaseEntity();
            Boolean isCorrect = true;
            if (objUser != null)
            {
                isCorrect = clsUserDao.Instance.UpdatePassword(ref Base, ref objUser);
            }
            else
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "A ocurrido un error en la capa Logica."));
            }
            return isCorrect;
        }
        #endregion


        #region Vacaciones

        /*Obtener Id Vacaciones*/
        public clsVacaciones ObtenerID_Vacaciones(ref BaseEntity Base, int VacacionesId)
        {
            return clsVacacionesDAO.Instancia.Search_Vacaciones(ref Base, VacacionesId);
        }

        /* obtieneVistaP de Vacaciones*/
        public clsVacaciones ObtieneTagVistaPVacas(int a, string b)
        {

            return clsVacacionesDAO.Instancia.obtieneVistaPreviaVacaciones(a, b);
        }

        /*Listar Vacaciones*/
        public List<clsVacaciones> ListarVacaciones(ref BaseEntity Base, Int32 EmpresaId)
        {
            List<clsVacaciones> lista = new List<clsVacaciones>();
            try
            {
                lista = clsVacacionesDAO.Instancia.ListarVacaciones(ref Base, EmpresaId);
            }
            catch (Exception e) { Base.Errors.Add(new BaseEntity.ListError(e, "Error al cargar la capa Logica")); }
            return lista;
        }

        /*Listar Vacaciones Programadas*/
        public List<clsVacaciones> ListarVacacionesProgramadas(ref BaseEntity Base, Int32 EmpresaId)
        {
            List<clsVacaciones> lista = new List<clsVacaciones>();
            try
            {
                lista = clsVacacionesDAO.Instancia.ListarVacacionesProgramadas(ref Base, EmpresaId);
            }
            catch (Exception e) { Base.Errors.Add(new BaseEntity.ListError(e, "Error al cargar la capa Logica")); }
            return lista;
        }

        /*Listar Vacaciones Truncas*/
        public List<clsVacaciones> ListarVacacionesTruncas(ref BaseEntity Base, Int32 EmpresaId)
        {
            List<clsVacaciones> lista = new List<clsVacaciones>();
            try
            {
                lista = clsVacacionesDAO.Instancia.ListarVacacionesTruncas(ref Base, EmpresaId);
            }
            catch (Exception e) { Base.Errors.Add(new BaseEntity.ListError(e, "Error al cargar la capa Logica")); }
            return lista;
        }

        /*Buscar Vacaciones*/
        public DataTable Buscar_Vacaciones(ref BaseEntity Base, Int32 VacacionesId)
        {
            DataTable datavacaciones = new DataTable();
            try
            {
                datavacaciones = clsVacacionesDAO.Instancia.Buscar_Vacaciones(ref Base, VacacionesId);
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error al cargar las vacaciones en la capa logica"));
            }
            return datavacaciones;
        }

        /*Modificar Vacaciones*/
        public Boolean Editar_Vacaciones(ref BaseEntity Base, clsVacaciones objvacaciones)
        {
            Base = new BaseEntity();
            Boolean editar = false;
            try
            {
                editar = clsVacacionesDAO.Instancia.Editar_Vacaciones(ref Base, objvacaciones);
            }
            catch (Exception e)
            {
                Base.Errors.Add(new BaseEntity.ListError(e, "Ha ocurrido un error en la capa logica editar vacaciones"));
            }
            return editar;
        }

        /*Actualizar Vacaciones dos*/
        public Boolean Actualiza_Vacaciones(ref BaseEntity Base, clsVacaciones objvacaciones)
        {
            Base = new BaseEntity();
            Boolean Actualizar = false;
            try
            {
                Actualizar = clsVacacionesDAO.Instancia.Actualiza_Vacaciones(ref Base, objvacaciones);
            }
            catch (Exception e)
            {
                Base.Errors.Add(new BaseEntity.ListError(e, "Ha ocurrido un error en la capa logica Actualizar vacaciones"));
            }
            return Actualizar;
        }
        /**/
        public Boolean Actualiza_Vacaciones_Programadas(ref BaseEntity Base, clsVacaciones objvacaciones)
        {
            Base = new BaseEntity();
            Boolean Actualizar = false;
            try
            {
                Actualizar = clsVacacionesDAO.Instancia.Actualizar_Vacaciones_Programadas(ref Base, objvacaciones);
            }
            catch (Exception e)
            {
                Base.Errors.Add(new BaseEntity.ListError(e, "Ha ocurrido un error en la capa logica Actualizar vacaciones"));
            }
            return Actualizar;
        }

        /*Buscar Vacaciones*/

        public DataTable ConsultarVacaciones(ref BaseEntity Base, Int32 idEmpleado)
        {
            DataTable datadvacaciones = new DataTable();
            try
            {
                // vaca = clsVacacionesDAO.Instancia.ConsultarVacaciones(ref Base, idEmpleado);
                datadvacaciones = clsVacacionesDAO.Instancia.ConsultaFechasAdelanto(ref Base, idEmpleado);
            }
            catch (Exception e)
            {
                Base.Errors.Add(new BaseEntity.ListError(e, "Ha ocurrido un error en la capa logica consultar vacaciones"));
            }
            return datadvacaciones;
        }

        /*Consultar vacaciones por empleado*/

        public List<clsVacaciones> ListarVacacionesEmpleado(ref BaseEntity Base, Int32 EmpleadoId)
        {
            List<clsVacaciones> lista = new List<clsVacaciones>();
            try
            {
                lista = clsVacacionesDAO.Instancia.listarVacacionesEmpleado(ref Base, EmpleadoId);
            }
            catch (Exception e) { Base.Errors.Add(new BaseEntity.ListError(e, "Error al cargar la capa Logica")); }
            return lista;
        }

        /*consultar dias Trabajador*/
        public DataTable ConsultarDiasEmpleado(ref BaseEntity Base, Int32 EmpleadoId)
        {
            DataTable dtvacaciones = new DataTable();
            try
            {
                dtvacaciones = clsVacacionesDAO.Instancia.ConsultarDiasPorEmpleado(ref Base, EmpleadoId);
            }
            catch (Exception e)
            {
                Base.Errors.Add(new BaseEntity.ListError(e, "Ha ocurrido un error en la capa logica consultar dias trabajador"));
            }
            return dtvacaciones;
        }

        /*Consultar Regimen Trabajador*/
        public DataTable ConsultarRegimen(ref BaseEntity Base, Int32 EmpleadoId)
        {
            DataTable dtvacacionerregimen = new DataTable();
            try
            {
                dtvacacionerregimen = clsVacacionesDAO.Instancia.ConsultarRegimen(ref Base, EmpleadoId);
            }
            catch (Exception e)
            {
                Base.Errors.Add(new BaseEntity.ListError(e, "Ha ocurrido un error en la capa logica consultar regimen"));
            }
            return dtvacacionerregimen;
        }

        #endregion

        #region mantenedorUsuario
        /*Buscar usuario para editar*/
        public clsUser Search_User(ref BaseEntity Base, Int32 UsuarioId)
        {
            clsUser us = null;
            try
            {
                us = clsUserDao.Instance.Search_User(ref Base, UsuarioId);
            }
            catch (Exception ex)
            {
                us = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en la capa Logica"));
            }
            return us;
        }

        /*Listar Tipo de Usuario para combo Funcional*/
        public DataTable List_Type_User_Log(ref BaseEntity Base)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = clsUserDao.Instance.List_Type_User(ref Base);
            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error al cargar en la capa Logica"));
            } 
            return dt;
        }

        /*Save usuario Funcional*/
        public Boolean SaveUser(ref BaseEntity Base, clsUser objUser, int accion)
        {
            Boolean result = false;
            try
            {
                if (accion == 1)
                {
                    result = clsUserDao.Instance.SaveUser(ref Base, objUser);
                }
                else
                {
                    result = clsUserDao.Instance.User_Update(ref Base, objUser);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ha ocurrido un error en la capa logica"));
            } return result;

        }

        /*Lista de Usuario 2 Funcional*/
        public List<clsUser> listUser2(ref BaseEntity Base, Int32 idCompany)
        {
            List<clsUser> lista = new List<clsUser>();
            try
            {
                lista = clsUserDao.Instance.ListUser2(ref Base, idCompany);
            }
            catch (Exception ex)
            {
                lista = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error al cargar en la capa Logica"));
            }
            return lista;
        }

        /*delete usuario*/
        public Boolean DeleteUser(ref BaseEntity Base, List<clsUser> lstIds)
        {
            return clsUserDao.Instance.DeleteUser(ref Base, lstIds);
        }

        /*Eliminar Usuario postulante*/
        public Boolean EliminarUsuarioPostulante(ref BaseEntity Base, Int32 idUsuario) {
            Base = new BaseEntity();
            Boolean elimino = false;
            try {
                elimino = clsUserDao.Instance.Eliminar_Empleado_Postulante(ref Base, idUsuario);
            }
            catch (Exception ex) {
                Base.Errors.Add(new BaseEntity.ListError(ex, "A Ocurrido un error en la capa Logica"));
            }
            return elimino;
        }
        /*validar email*/

        public bool User_ValidateByEmail(ref BaseEntity Base, String Email)
        {
            Base = new BaseEntity();
            bool resp = false;
            try
            {
                resp = clsUserDao.Instance.ValidateByEmail(ref Base, Email);
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "A Ocurrido un error en la capa Logica"));
            }
            return resp;
        }

        /**/

        public bool user_Validate_Hora(ref BaseEntity Base, String Hora)
        {
            Base = new BaseEntity();
            bool resp = false;
            try
            {
                resp = clsUserDao.Instance.ValidateHora(ref Base, Hora);
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "A Ocurrido un error en la capa Logica"));
            }
            return resp;
        }



        #endregion mantenedorUsuario

        #region MantenedorAssign
        public DataTable Employee_GetByName(ref BaseEntity Base, String Name, Int32 idCompany)
        {
            DataTable dt = new DataTable();
            if (!String.IsNullOrEmpty(Name))
            {
                dt = clsUserDao.Instance.Employee_GetByName(ref Base, Name, idCompany);
            }
            else
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception { }, "A Ocurrido un error en la capa Logica"));
            }
            return dt;
        }
        #endregion

   
      
 
    

        #region GuardarPlantillaMemorandum
        public int SaveTemplTemplateMemorandum(ref BaseEntity Base, clsTemplateMemorandumcs objMemo, int accion, bool validate, bool validateEdit, String captura)
        {
            int result = 0;
            try
            {
                Base = new BaseEntity();
                if (accion == 1)
                {
                    if (!validate)
                    {
                        if (captura != "")
                        {
                            result = clsTemplateMemorandumcsDAO.Instance.SaveTemplTemplateMemorandum(ref Base, objMemo);
                        }
                        else { result = 3; }
                    }
                    else
                    {
                        result = 2;
                    }
                }
                else
                {
                    // Valida si es ya existe, sino revuelve valor 2 (ya existe)
                    if (validateEdit)
                    {
                        result = clsTemplateMemorandumcsDAO.Instance.MemorandumTemplate_Update(ref Base, objMemo);
                    }
                    else
                    {
                        if (!validate)
                        {
                            result = clsTemplateMemorandumcsDAO.Instance.MemorandumTemplate_Update(ref Base, objMemo);
                        }
                        else
                        {
                            result = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error ocurred on send data"));
            }
            return result;

        }
        #endregion

        #region Listar Plantilla Memorandum

        public DataTable ListTempMemorandum(ref BaseEntity Entity, Int32 id)
        {
            return clsTemplateMemorandumcsDAO.Instance.ListTempMemorandum(ref Entity, id);
        }
        #endregion

        public DataTable ListTempMemorandum2(ref BaseEntity Entity, Int32 id)
        {
            return clsTemplateMemorandumcsDAO.Instance.ListTempMemorandum2(ref Entity, id);
        }

        // Harold Bautista

        #region Listar Tipo Memorandum por Empresa

        public DataTable ListTempMemorandumPorEmp(ref BaseEntity Entity, Int32 id)
        {
            return clsTemplateMemorandumcsDAO.Instance.ListTempMemorandumPorEmp(ref Entity, id);
        }
        #endregion

        // Harold Bautista

        #region EliminarPlantillaMemorandum 

        public Boolean DeleteTemplateMemorandum(ref BaseEntity Base, List<clsTemplateMemorandumcs> lstIds)
        {
            return clsTemplateMemorandumcsDAO.Instance.DeleteTemplateMemorandum(ref Base, lstIds);
        }
        #endregion

        #region BuscarPlantillaMemorandum

        public clsTemplateMemorandumcs SearchTemplateMemorandum(ref BaseEntity Base, int loanId)
        {
            clsTemplateMemorandumcs busq = new clsTemplateMemorandumcs();
            try
            {
                return clsTemplateMemorandumcsDAO.Instance.SearchTemplateMemorandum(ref Base, loanId);
            }
            catch (Exception ex)
            {
                busq = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error send Identify"));
            } return busq;
        }

        #endregion

        #region Contenido_ID_Plantilla_Memorandum
        public String Listar_Contenido_Memorandum(ref BaseEntity Base, Int32 id)
        {
            String contenido = null;
            try
            {
                contenido = clsTemplateMemorandumcsDAO.Instance.Listar_Contenido_Memorandum(ref Base, id);
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrio un error al momento de listar los memorandums"));
            }
            return contenido;
        }




        public int Listar_Plantilla_Memorandum_Id(ref BaseEntity Base, Int32 id)
        {
            int idplantilla = 0;
            try
            {
                idplantilla = clsTemplateMemorandumcsDAO.Instance.Listar_Plantilla_Memorandum_Id(ref Base, id);
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrio un error al momento de retornar el id de la plantilla"));
            }
            return idplantilla;
        }


        public int Listar_Plantilla_Memorandum_Id_2(ref BaseEntity Base, Int32 id)
        {
            int idplantilla = 0;
            try
            {
                idplantilla = clsTemplateMemorandumcsDAO.Instance.Listar_Plantilla_Memorandum_Id_2(ref Base, id);
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "Ocurrio un error al momento de retornar el id de la plantilla"));
            }
            return idplantilla;
        }

        #endregion

        #region Listar Justificación
        public DataTable ListJustificacion(ref BaseEntity Entity, Int32 id)
        {
            return clsJustificationDAO.Instance.ListJustificacion(ref Entity, id);
        }

        public DataTable ListarJustificaciones(ref BaseEntity Entity, Int32 empresaID)
        {
            return clsJustificationDAO.Instance.ListarJustificaciones(ref Entity, empresaID);
        }
        #endregion

    


        #region Buscar Justificación

        public clsJustification SearchJustificacion(ref BaseEntity Base, int loanId)
        {
            clsJustification busq = new clsJustification();
            try
            {
                return clsJustificationDAO.Instance.SearchJustificacion(ref Base, loanId);
            }
            catch (Exception ex)
            {
                busq = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error send Identify"));
            } return busq;
        }

        #endregion




        #region Guardar Justificación
        public int SaveJustificacion(ref BaseEntity Base, clsJustification objJust, int accion)
        {
            int result = 0;
            try
            {
                Base = new BaseEntity();
                if (accion == 1)
                {
                    result = clsJustificationDAO.Instance.SaveJustificacion(ref Base, objJust);
                }
                else
                {
                    result = clsJustificationDAO.Instance.UpdateJustificacion(ref Base, objJust);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error ocurred on send data"));
            }
            return result;

        }
  
        public clsJustification ObtieneTagVistaPJust(int a, string b)
        {

            return clsJustificationDAO.Instance.obtieneVistaPreviaJustificacion(a, b);
        }
        #endregion

        #region obtieneVistaP de Memorandum
        public clsMemorandums ObtieneTagVistaPMemo(int a, string b)
        {

            return clsMemorandumDAO.Instance.obtieneVistaPreviaMemo(a, b);
        }
        #endregion


    
     
      

        #region ListaHistorialJustificación
        public DataTable ListaHistorialJustificacion(ref BaseEntity Base, Int32 id, Int32 id2)
        {
            return clsJustificationDAO.Instance.ListaHistorialJustificacion(ref Base, id, id2);
        }
        #endregion

      
        /*-----------COMPANY----------------*/
        #region Listar Actvidades
        public DataTable Activity_ListAll(ref BaseEntity Entity)
        {
            return clsActivityDAO.Instance.Activity_ListAll(ref Entity);
        }
        #endregion

      

        #region Guardar Memorandum 
        public int SaveMemorandum(ref BaseEntity Base, clsMemorandums objMemo, int accion)
        {
            int result = 0;
            try
            {
                Base = new BaseEntity();
                if (accion == 1)
                {
                    result = clsMemorandumDAO.Instance.SaveMemorandum(ref Base, objMemo);
                }
                else
                {
                    result = clsMemorandumDAO.Instance.UpdateMemorandum(ref Base, objMemo);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error ocurred on send data"));
            }
            return result;

        }
        #endregion

        public int SaveMemorandum2(ref BaseEntity Base, clsMemorandums objMemo, int accion)
        {
            int result = 0;
            try
            {
                Base = new BaseEntity();
                if (accion == 1)
                {
                    result = clsMemorandumDAO.Instance.SaveMemorandum2(ref Base, objMemo);
                }
                else
                {
                    result = clsMemorandumDAO.Instance.UpdateMemorandum2(ref Base, objMemo);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error ocurred on send data"));
            }
            return result;

        }

       

        #region Listar Memorandum
        public DataTable ListMemorandum(ref BaseEntity Entity, Int32 id)
        {
            return clsMemorandumDAO.Instance.ListMemorandum(ref Entity, id);
        }
        #endregion

        #region Buscar Memorandum

        public clsMemorandums SearchMemorandum(ref BaseEntity Base, int loanId)
        {
            clsMemorandums busq = new clsMemorandums();
            try
            {
                return clsMemorandumDAO.Instance.SearchMemorandum(ref Base, loanId);
            }
            catch (Exception ex)
            {
                busq = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error send Identify"));
            } return busq;
        }

        #endregion


        #region Eliminar Memorandum

        public Boolean DeleteMemorandum(ref BaseEntity Base, List<clsMemorandums> lstIds)
        {
            return clsMemorandumDAO.Instance.DeleteMemorandum(ref Base, lstIds);
        }
        #endregion

       

        


        #region Dashboard

        public List<clsDashboardShortCut> ListDashboardShortCut(ref BaseEntity Base, int userId)
        {
            List<clsDashboardShortCut> Lista = new List<clsDashboardShortCut>();

            try
            {
                Lista = clsDashboardShortCutDAO.Instance.ListDashboardShortCut(ref Base, userId);
            }
            catch (Exception ex)
            {

                Base.Errors.Add(new BaseEntity.ListError(ex, "Error en cargar la data, capa logica"));
            }
            return Lista;

        }

        #endregion Dashboard

        #region
        public DataTable Listar_Empresa_Usuario_Logica(ref BaseEntity Base, int idUsuario)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = clsDashboardShortCutDAO.Instance.Listar_Empresa_Usuario(ref Base, idUsuario);
            }
            catch (Exception ex)
            {

                Base.Errors.Add(new BaseEntity.ListError(ex, "Error al cargar en la capa Logica de Dashboard"));
            } return dt;
        }



        #endregion

     

 
     

        #region Exportacion Planilla Excel
        public static List<DataTable> GetListDataTable(ref BaseEntity Base, String SqlCommandString)
        {

            List<DataTable> ListDetails = new List<DataTable>();
            if (!String.IsNullOrEmpty(SqlCommandString))
            {
                ListDetails = BaseDao.GetListDataTable(ref Base, SqlCommandString);
            }
            else
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception(), "SQL Command String esta vacio o es nullo"));
            }
            return ListDetails;
        }

        #endregion

        #region  Exportacion Planilla Excel con ID
        public static DataTable GetListDataTableID(ref BaseEntity Base, String SqlCommandString, Int64 idplanilla)
        {

            DataTable ListDetails = new DataTable();
            if (!String.IsNullOrEmpty(SqlCommandString))
            {
                ListDetails = BaseDao.GetListDataTableID(ref Base, SqlCommandString, idplanilla);
            }
            else
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception(), "SQL Command String esta vacio o es nullo"));
            }
            return ListDetails;
        }
        #endregion



        #region  Exportacion_Excel_Gratificacion_Mes_por_deivi_gomez
        public static DataTable Exportar_Excel_Gratificacion_Mes(ref BaseEntity Base, String SqlCommandString, Int32 idGratificacion)
        {

            DataTable ListDetails = new DataTable();
            if (!String.IsNullOrEmpty(SqlCommandString))
            {
                ListDetails = BaseDao.GetListDataTableIDGrati(ref Base, SqlCommandString, idGratificacion);
            }
            else
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception(), "SQL Command String esta vacio o es nullo"));
            }
            return ListDetails;
        }

        #endregion



        #region  Exportacion_Excel_CalculoQuintaCategoria_por_deivi_gomez
        public static DataTable Exportar_Excel_Quinta_Categoria(ref BaseEntity Base, String SqlCommandString, Int32 QuintaID)
        {

            DataTable ListDetails = new DataTable();
            if (!String.IsNullOrEmpty(SqlCommandString))
            {
                ListDetails = BaseDao.GetListDataTableIDQuintaCategoria(ref Base, SqlCommandString, QuintaID);
            }
            else
            {
                Base.Errors.Add(new BaseEntity.ListError(new Exception(), "SQL Command String esta vacio o es nullo"));
            }
            return ListDetails;
        }

        #endregion




   

        #region Valida_PlantillaMemo
        public bool Valida_PlantillaMemorandum(ref BaseEntity Base, String tipo, Int32 Empresa)
        {
            Base = new BaseEntity();
            bool succes = false;
            try
            {
                succes = clsTemplateMemorandumcsDAO.Instance.Valida_PlantillaMemo(ref Base, tipo, Empresa);
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "error en la capa Lógica"));
            }
            return succes;
        }
        #endregion


  
        public clsEmployee Buscar_Empleado_Id(ref BaseEntity Entity, Int32 id, Int32 idempresa)
        {
            return clsEmployeeDAO.Instance.Buscar_Empleado_Id(ref Entity, id, idempresa);
        }

        public DataTable ListTemplateJustidication(ref BaseEntity Entity, Int32 id)
        {
            return clsTemplateJustificationDao.Instance.ListTemplateJustidication(ref Entity, id);
        }



        public DataTable Document_Type_ListAll(ref BaseEntity Entity)
        {
            return clsDocumentTypeDAO.Instance.Document_Type_ListAll(ref Entity);
        }
        public DataTable Company_Dedicated_ListAll(ref BaseEntity Entity)
        {
            return clsCompanyDedicatedDAO.Instance.Company_Dedicated_ListAll(ref Entity);
        }
 
        public DataTable Type_Employer_ListAll(ref BaseEntity Entity)
        {
            return clsTypeEmployerDAO.Instance.Type_Employer_ListAll(ref Entity);
        }
        public DataTable List_Company_GetAllV1(ref BaseEntity Entity, Int32 user)
        {
            return clsCompanyDAO.Instance.GetAll_ListCompanyV1(ref Entity, user);
        }
        public List<clsDepartment> Departament_List_v1(ref BaseEntity Entity)
        {
            List<clsDepartment> lstobj = null;
            lstobj = clsCompanyDAO.Instance.ListDepartament_v1(ref Entity);
            return lstobj;
        }
        public clsCompany CompanyAll(ref BaseEntity Base, Int16 EmpresaId)
        {
            Base = new BaseEntity();
            clsCompany objEmpresa = null;
            try
            {
                objEmpresa = clsCompanyDAO.Instance.CompanyAll(ref Base, EmpresaId);
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error ocurred on application level 2"));
            }
            return objEmpresa;
        }
        public DataTable Province_List(ref BaseEntity Entity, Int32 id)
        {
            return clsProvinceDAO.Instance.ListProvinceByDepartament(ref Entity, id);
        }
        public DataTable District_List(ref BaseEntity objBase, int ProvId)
        {
            return clsDistrictDAO.Instance.ListDistrictByProvince(ref objBase, ProvId);
        }

        public bool CompanySaveData(ref BaseEntity Entity, clsCompany Empresa)
        {
            Entity = new BaseEntity();
            bool bolSuccess = false;
            try
            {
                if (Entity != null)
                {
                    bolSuccess = clsCompanyDAO.Instance.CompanySaveData(ref Entity, Empresa);
                }
            }
            catch (Exception ex)
            {
                Entity.Errors.Add(new BaseEntity.ListError(ex, "An error occured sending data."));
            }
            return bolSuccess;
        }



        public Boolean Reescribir_Tareo_Diario(ref BaseEntity Base, DateTime fechaInicio, DateTime fechafin, int EmpresaId, int UsuarioId)
        {

            return clsTareoDAO.Instance.Reescribir_Tareo_Diario(ref Base, fechaInicio, fechafin, EmpresaId, UsuarioId);
        }




    }
}


