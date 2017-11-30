using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Library;
using System.Data.SqlClient;
using System.Data;
using xAPI.Entity;
using xAPI.Library.Base;
using xAPI.Library.Connection;
using xAPI.Entity;
using xAPI.Library.Connection;
using System.Globalization;
using xAPI.Library.General;


namespace xAPI.Dao
{
    public class clsEmployeeDAO : BaseDao
    {
        #region Singleton
        private static readonly clsEmployeeDAO _instance = new clsEmployeeDAO();
        public static clsEmployeeDAO Instance
        {
            get { return clsEmployeeDAO._instance; }
        }
        #endregion

        #region Metodos
        //List Document Type
        public DataTable ListDocType(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Documento", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //List Departament
        public DataTable ListDepartament(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Departamentos", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        ////Listar Provincia por Departamento
        public DataTable ListProvince(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Provincia_Por_Departamento", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iddepa", id);
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

        //Listar Nacionalidades
        public DataTable ListNationality(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Nacionalidades", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Listar Codigos Larga Distancia
        public DataTable ListCodTelef(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Codigos_Telefonicos", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Listar Tipo Via
        public DataTable ListVia(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Via", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Listar Tipo Zona
        public DataTable ListZone(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Zona", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Listar Tipo Empleado
        public DataTable ListEmployeeType(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Empleado", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Listar Regimen Laboral
        public DataTable ListLabourRegime(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Regimen_Laboral", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

         //Listar Categoria Ocupacional
        public DataTable ListOccupationalCategory(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Categoria_Ocupacional", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

         //Listar Nivel Educativo
        public DataTable ListEducationalLevel(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Nivel_Educativo", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Listar Ocupacion
        public DataTable ListOccupation(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Ocupacion", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

         //Listar Tipo de Pago
        public DataTable ListPaymentType(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Pago", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Listar Periodicidad
        public DataTable ListPeriodicity(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Periodicidad", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Listar Tipo Contrato
        public DataTable ListContractType(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Contrato", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //contrato Empleado
        public DataTable ListContratoEmpleado(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Tipo_Contrato_Por_Empresa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEmpresa", id);
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

        //List Comision
        public DataTable ListComisionEmpleado(ref BaseEntity Entity, Int32 id) {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Comision_Por_Empresa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpresaId", id);
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception e)
            {
                dt = null;
                Entity.Errors.Add(new BaseEntity.ListError(e, "No Found."));
            }
            finally {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }


        //List Motivo Baja
        public DataTable ListReasonLow(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Motivo_Baja", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //List Situacion del trabajador
        public DataTable ListEmployeeSit(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Situacion_Empleado", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //List Convenios 5ta. Categoria
        public DataTable ListAgreement(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Convenios", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //List Regimen Salud
        public DataTable ListRegimeHealth(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Regimen_Salud", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //List Regimen Pensionario
        public DataTable ListRegimePension(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Regimen_Pension", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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
        
        //Listar Personal en Formacion
        public DataTable ListTrainingMode(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Modalidad_Formativa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
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

        //Validar Nro de Documento del trabajador
        public Boolean Validate_Employee_By_Document_Number(ref BaseEntity Base, String NroDoc,Int32 Empresa)
        {
            Boolean success = false;
            SqlCommand cmd = null; 
            SqlDataReader dr = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Validate_Employee_By_Document_Number", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nrodoc", NroDoc);
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
                Base.Errors.Add(new BaseEntity.ListError(ex, "An error occurred saving data."));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }

            return success;
        }

        #endregion

        #region ListarTrabajador
        //Listar Trabajador
        public DataTable ListarTrabajador(ref BaseEntity Entity)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            DataTable dt = null;
            try
            {
                SqlConnection cn = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_LIST_EMPLOYEES", cn);
                cmd.CommandType = CommandType.StoredProcedure;
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
                Entity.Errors.Add(new BaseEntity.ListError(ex, "No found."));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        public DataTable ListarTrabajadorXEmpresa(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_XP_Listar_Empleado", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcompany", id);
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

        public clsEmployee Buscar_Empleado_Id(ref BaseEntity Entity, Int32 id, Int32 idempresa)
        {
            SqlDataReader dr;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            clsEmployee objempleado = new clsEmployee();
            try
            {
                cmd = new SqlCommand("sp_xP_Buscar_Empleado_Id", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("", id);
                cmd.Parameters.AddWithValue("", idempresa);
                dr = cmd.ExecuteReader();
                    if (dr.Read())
	                {
                        objempleado.EmpleadoId = Convert.ToInt32(dr["EmpleadoId"]);
                        objempleado.Nombre = dr["Nombre"].ToString();
                        objempleado.Apellido = dr["Apellido"].ToString();
	                }

            }
            catch (Exception ex)
            {
                //dt = null;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "No found."));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return objempleado;
        }
        

        public DataTable GetAll_Employee(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Empleados_Por_Empresas", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcompany", id);
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

        public DataTable GetAll_Employeev2(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_XP_Listar_Empleado", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcompany", id);
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

        public DataTable Listar_Postulante(ref BaseEntity Entity, Int32 id1, Int32 id2)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Postulantes_Por_Empresas", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcompany", id1);
                cmd.Parameters.AddWithValue("@iduser", id2);
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

        public Boolean Employee_Delete(ref BaseEntity Entity, tBaseIdList BaseList)
        {
            bool success = false;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Eliminar_Masivo_Empleado", clsConnection.GetConnection());
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@TYPE_BASEID", Value = BaseList, SqlDbType = SqlDbType.Structured, TypeName = "dbo.TY_BASEID" });
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "An error occurred deleting a resource."));

            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return success;
        }
        
        #endregion

        #region Guardar Trabajador
        public bool SaveEmployee(ref BaseEntity Base, clsEmployee Employee)
        {
            bool bolSuccess = false;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Guardar_Masivo_Empleado", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter outputParam = cmd.Parameters.Add("@NEWID", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;

                if (Employee.ListaSituacionEducativa.Count != 0) 
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@TYPE_EDUCATIONAL", Value = Employee.ListaSituacionEducativa, SqlDbType = SqlDbType.Structured, TypeName = "dbo.TY_DATOS_SITUACION_EDUCATIVA" });
               
                if (Employee.ListaDerechoHabiente != null)
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@TYPE_DERECHOHABIENTE", Value = Employee.ListaDerechoHabiente, SqlDbType = SqlDbType.Structured, TypeName = "dbo.TY_DATOS_DERECHO_HABIENTE" });
                

                cmd.Parameters.AddWithValue("@ID", Employee.EmpleadoId);
                cmd.Parameters.AddWithValue("@LASTNAME", Employee.Apellido);
                cmd.Parameters.AddWithValue("@NAME", Employee.Nombre);
                cmd.Parameters.AddWithValue("@DOCUMENTTYPE", Employee.TipoDocumentoId);
                cmd.Parameters.AddWithValue("@DOCUMENTNUMBER", Employee.NumeroDocumento);
                cmd.Parameters.AddWithValue("@SEX", Employee.Sexo);
                cmd.Parameters.AddWithValue("@CIVILSTATUS", Employee.EstadoCivilId);
                cmd.Parameters.AddWithValue("@NATIONALITY", Employee.NacionalidadId);
                cmd.Parameters.AddWithValue("@HIJOS", Employee.Hijos);
                cmd.Parameters.AddWithValue("@CODEPHONE", Employee.CodigoLargaDistanciaId);
                cmd.Parameters.AddWithValue("@PHONE", Employee.Telefono);
                cmd.Parameters.AddWithValue("@CELLPHONE", Employee.Celular);
                cmd.Parameters.AddWithValue("@EMAIL", Employee.Email);
                /////////////////////////////////////////////////////////////////////////
                if (Employee.FechaNacimiento != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@BIRTHDAY", Employee.FechaNacimiento);
                else
                    cmd.Parameters.AddWithValue("@BIRTHDAY", null);
                //////////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@DEPARMENT", Employee.DepartamentoNac);
                cmd.Parameters.AddWithValue("@PROVINCE", Employee.ProvinciaNac);
                cmd.Parameters.AddWithValue("@DISTRICT", Employee.DistritoNac);
                cmd.Parameters.AddWithValue("@CREATEDBY", Employee.CreatedBy);
                ///////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@DepartmentId", Employee.DepartamentoId);
                cmd.Parameters.AddWithValue("@ProvinceId", Employee.ProvinciaId);
                cmd.Parameters.AddWithValue("@DistrictId", Employee.DistritoId);
                cmd.Parameters.AddWithValue("@ZonetypeId", Employee.TipoZonaId);
                cmd.Parameters.AddWithValue("@ViaTypeId", Employee.TipoViaId);
                cmd.Parameters.AddWithValue("@NameVia", Employee.NombreVia);
                cmd.Parameters.AddWithValue("@NumberVia", Employee.NumeroVia);
                cmd.Parameters.AddWithValue("@Interior", Employee.Interior);
                cmd.Parameters.AddWithValue("@NameZone", Employee.NombreZona);
                cmd.Parameters.AddWithValue("@Reference", Employee.Referencia);
                ///////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@DepartmentId2", Employee.DepartamentoId2);
                cmd.Parameters.AddWithValue("@ProvinceId2", Employee.ProvinciaId2);
                cmd.Parameters.AddWithValue("@DistrictId2", Employee.DistritoId2);
                cmd.Parameters.AddWithValue("@ZonetypeId2", Employee.TipoZonaId2);
                cmd.Parameters.AddWithValue("@ViaTypeId2", Employee.TipoViaId2);
                cmd.Parameters.AddWithValue("@NameVia2", Employee.NombreVia2);
                cmd.Parameters.AddWithValue("@NumberVia2", Employee.NumeroVia2);
                cmd.Parameters.AddWithValue("@Interior2", Employee.Interior2);
                cmd.Parameters.AddWithValue("@NameZone2", Employee.NombreZona2);
                cmd.Parameters.AddWithValue("@Reference2", Employee.Referencia2);
                /////////////////////////////////////////////////////////////////////////
                if (Employee.FechaInicioLaboral != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@DateOfEmployment", Employee.FechaInicioLaboral);
                else
                    cmd.Parameters.AddWithValue("@DateOfEmployment", null);
                //////////////////////////////////////////////////////////////////////////
                if (Employee.FechaFinLaboral != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@DateOfWorkOrder", Employee.FechaFinLaboral);
                else
                    cmd.Parameters.AddWithValue("@DateOfWorkOrder", null);
                //////////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@ReasonLowId", Employee.MotivoBajaId);
                cmd.Parameters.AddWithValue("@ScheduleId", Employee.HorarioId);
                cmd.Parameters.AddWithValue("@EmployeeTypeId", Employee.TipoEmpleadoId);
                //////////////////////////////////////////////////////////////////////////
                if (Employee.FechaInicioTipoEmpleado != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@StartDateTypeEmployee", Employee.FechaInicioTipoEmpleado);
                else
                    cmd.Parameters.AddWithValue("@StartDateTypeEmployee", null);
                //////////////////////////////////////////////////////////////////////////
                if(Employee.FechaFinTipoEmpleado != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@EndDateTypeEmployee", Employee.FechaFinTipoEmpleado);
                else
                    cmd.Parameters.AddWithValue("@EndDateTypeEmployee", null);
                //////////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@LabourRegimeId", Employee.RegimenLaboralId);
                cmd.Parameters.AddWithValue("@OcupationalcategoryId", Employee.CategoriaOcupacionalId);
                cmd.Parameters.AddWithValue("@LevelEducativeId", Employee.NivelEducativoId);
                cmd.Parameters.AddWithValue("@OcupationId", Employee.OcupacionId);
                cmd.Parameters.AddWithValue("@CentercostId", Employee.CentroCostoId);
                cmd.Parameters.AddWithValue("@PaymentTypeId", Employee.TipoPagoId);
                cmd.Parameters.AddWithValue("@PeriodicityId", Employee.PeriodicidadId);
                cmd.Parameters.AddWithValue("@ContractTypeId", Employee.TipoContratoId);
                //////////////////////////////////////////////////////////////////////////
                if (Employee.FechaInicioContrato != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@ContractStartDate", Employee.FechaInicioContrato);
                else
                    cmd.Parameters.AddWithValue("@ContractStartDate", null);
                //////////////////////////////////////////////////////////////////////////
                if (Employee.FechaFinContrato != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@ContractEndDate", Employee.FechaFinContrato);
                else
                    cmd.Parameters.AddWithValue("@ContractEndDate", null);
                //////////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@AmountRemuneration", Employee.Remuneracion);
                cmd.Parameters.AddWithValue("@DayValue", Employee.ValorDia);
                cmd.Parameters.AddWithValue("@TimeValue", Employee.ValorHora);
                cmd.Parameters.AddWithValue("@MinuteValue", Employee.ValorMinuto);

                if (Employee.FichaREMYPE != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@FechaREMYPE", Employee.FichaREMYPE);

                cmd.Parameters.AddWithValue("@RegREMYPE", Employee.RegREMYPE);
                cmd.Parameters.AddWithValue("@CompanyId", Employee.EmpresaId);
                cmd.Parameters.AddWithValue("@Employment", Employee.Empleo);
                cmd.Parameters.AddWithValue("@WorkDayId", Employee.JornadaLaboralId);
                cmd.Parameters.AddWithValue("@SpecialSituationId", Employee.SituacionEspecialId);
                cmd.Parameters.AddWithValue("@Handicapped", Employee.Discapacitado);
                cmd.Parameters.AddWithValue("@Unionized", Employee.Sindicalizado);
                cmd.Parameters.AddWithValue("@EmployeeSituationId", Employee.SituacionEmpleadoId);
                /////////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@RegimehealthId", Employee.RegimenSaludId);
                cmd.Parameters.AddWithValue("@RegimePensionId", Employee.RegimenPensionId);
                
                if(Employee.FechaInicioRegimenSalud != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@StartDateHealthRegimen", Employee.FechaInicioRegimenSalud);
                else
                    cmd.Parameters.AddWithValue("@StartDateHealthRegimen", null);

                if(Employee.FechaFinRegimenSalud != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@EndDateHealthRegimen", Employee.FechaFinRegimenSalud);
                else
                    cmd.Parameters.AddWithValue("@EndDateHealthRegimen", null);

                cmd.Parameters.AddWithValue("@CUSPP", Employee.CUSPP);

                if(Employee.FechaInicioRegimenPension != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@StartDatePensionaryRegimen", Employee.FechaInicioRegimenPension);
                else
                    cmd.Parameters.AddWithValue("@StartDatePensionaryRegimen", null);

                if (Employee.FechafinRegimenPension != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@EndDatePensionaryRegimen", Employee.FechafinRegimenPension);
                else
                    cmd.Parameters.AddWithValue("@EndDatePensionaryRegimen", null);

                cmd.Parameters.AddWithValue("@EstadoAFP", Employee.EstadoAFP);
                /////////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@CategoryRent5ta", Employee.Renta5taCategoria);
                cmd.Parameters.AddWithValue("@AgreementApplies", Employee.AplicaConvenio);
                cmd.Parameters.AddWithValue("@TaxationAgreementsId", Employee.ConvenioId);
                /////////////////////////////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@TypeOfTrainingModeId", Employee.ModalidadFormativaId);
                if (Employee.FechaInicioFormacion != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@StartDateEmployee", Employee.FechaInicioFormacion);
                else
                    cmd.Parameters.AddWithValue("@StartDateEmployee",null);

                if (Employee.FechaFinFormacion != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@EndDateEmployee", Employee.FechaFinFormacion);
                else
                    cmd.Parameters.AddWithValue("@EndDateEmployee", null);

                cmd.Parameters.AddWithValue("@EducativeSituationId", Employee.NivelEducativoId1);
                cmd.Parameters.AddWithValue("@OccupationId", Employee.OcupacionId1);
                cmd.Parameters.AddWithValue("@TrainingCenter", Employee.CentroFormacion);
                cmd.Parameters.AddWithValue("@Code", Employee.Codigo);
                cmd.Parameters.AddWithValue("@Local", Employee.Local);
                cmd.Parameters.AddWithValue("@HealthInsurance", Employee.SeguroMedico);
                cmd.Parameters.AddWithValue("@Disability", Employee.Discapacidad);
                cmd.Parameters.AddWithValue("@NightTime", Employee.HorarioNocturno);
                cmd.Parameters.AddWithValue("@EmployeeSituationId1", Employee.SituacionEmpleadoId1);
                /*inlcuyo comision*/
                cmd.Parameters.AddWithValue("@comision", Employee.comision);
                cmd.Parameters.AddWithValue("@DocumentoEmisionNacionalidad", Employee.PaisEmisor);
                /*Datos del trabajador de Rentas Recibidas*/
                cmd.Parameters.AddWithValue("@Ejercicio",Employee.EjercicioRenta);
                if (Employee.FechaEmisionRenta != DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@FechaEmision", Employee.FechaEmisionRenta);
                else
                    cmd.Parameters.AddWithValue("@FechaEmision", null);

                cmd.Parameters.AddWithValue("@NombreEmpresa",Employee.NombreEmpresaRenta);
                cmd.Parameters.AddWithValue("@NumeroRuc",Employee.NumeroRucRenta);
                cmd.Parameters.AddWithValue("@DomicilioFiscal",Employee.DomicilioFiscalRenta);
                cmd.Parameters.AddWithValue("@Representate",Employee.RepresentanteRenta);
                cmd.Parameters.AddWithValue("@dniRepresentante",Employee.dniRepresentanteRenta);
                cmd.Parameters.AddWithValue("@RemuneracionAfecta",Employee.RemuneracionAfectRenta);
                cmd.Parameters.AddWithValue("@RemuneracionNoAfecta",Employee.RemuneracionNoAfectaRenta);
                cmd.Parameters.AddWithValue("@Utilidades",Employee.UtilidadesRenta);
                cmd.Parameters.AddWithValue("@MontoTrabajosIndependientes",Employee.MontoTrabajadorIndependienteRenta);
                cmd.Parameters.AddWithValue("@ImpuestoRetenido",Employee.ImpuestoRetenidoRenta);

                cmd.ExecuteNonQuery();
                if (Employee.EmpleadoId == 0)
                {
                    Employee.EmpleadoId = Convert.ToInt32(cmd.Parameters["@NEWID"].Value);
                    bolSuccess = true;
                }
                else
                    bolSuccess = true;
            }
            catch (Exception ex)
            {
                bolSuccess = false;
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return bolSuccess;
        
        }
        #endregion

        #region Editar trabajador
        ////cargar data para editar
        public DataTable SetData_Employee(ref BaseEntity Entity, Int32 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Datos_Empleado", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idempleado", id);
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

        #region Exportar CSV

        public DataTable Generar_Lista_Empleados(ref BaseEntity Entity, Int32 id, DateTime fecha1, DateTime fecha2)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Empleados_Por_Fecha_Registrada", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcompany", id);
                cmd.Parameters.AddWithValue("@fechaDesde", fecha1);
                cmd.Parameters.AddWithValue("@fechaHasta", fecha2);
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

        public DataTable ExpotacionEmpleadoCSV_e4(ref BaseEntity entity, Int32 id, DateTime fechadesde, DateTime fechahasta )
        {
            SqlConnection objconexion = null;
            DataTable dt = null;
            try
            {
                objconexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand("sp_xP_Exportar_Empleados_Por_Fecha_Registrada_Estructura_4", objconexion);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@idcompany", id);
                ObjCmd.Parameters.AddWithValue("@fechaDesde", fechadesde);
                ObjCmd.Parameters.AddWithValue("@fechaHasta", fechahasta);
                dt = new DataTable();
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                clsConnection.DisposeCommand(objconexion);
            }
            return dt;
        }

        public DataTable ExpotacionEmpleadoCSV_e5(ref BaseEntity entity, Int32 id, DateTime fechadesde, DateTime fechahasta)
        {
            SqlConnection objconexion = null;
            DataTable dt = null;
            try
            {
                objconexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand("sp_xP_Exportar_Empleados_Por_Fecha_Registrada_Estructura_5", objconexion);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@idcompany", id);
                ObjCmd.Parameters.AddWithValue("@fechaDesde", fechadesde);
                ObjCmd.Parameters.AddWithValue("@fechaHasta", fechahasta);
                dt = new DataTable();
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                clsConnection.DisposeCommand(objconexion);
            }
            return dt;
        }


        public DataTable ExpotacionEmpleadoCSV_e11(ref BaseEntity entity, Int32 id, DateTime fechadesde, DateTime fechahasta)
        {
            SqlConnection objconexion = null;
            DataTable dt = null;
            try
            {
                objconexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand("sp_xP_Exportar_Empleados_Por_Fecha_Registrada_Estructura_11_Baja", objconexion);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@idEmpresa", id);
                ObjCmd.Parameters.AddWithValue("@fechaDesde", fechadesde);
                ObjCmd.Parameters.AddWithValue("@fechaHasta", fechahasta);
                dt = new DataTable();
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                clsConnection.DisposeCommand(objconexion);
            }
            return dt;
        }


        public DataTable ExpotacionEmpleadoCSV_e17(ref BaseEntity entity, Int32 id, DateTime fechadesde, DateTime fechahasta)
        {
            SqlConnection objconexion = null;
            DataTable dt = null;
            try
            {
                objconexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand("sp_xP_Exportar_Empleados_Por_Fecha_Registrada_Estructura_17", objconexion);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@idcompany", id);
                ObjCmd.Parameters.AddWithValue("@fechaDesde", fechadesde);
                ObjCmd.Parameters.AddWithValue("@fechaHasta", fechahasta);
                dt = new DataTable();
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                clsConnection.DisposeCommand(objconexion);
            }
            return dt;
        }

        public DataTable ExpotacionEmpleadoCSV_e29(ref BaseEntity entity, Int32 id, DateTime fechadesde, DateTime fechahasta)
        {
            SqlConnection objconexion = null;
            DataTable dt = null;
            try
            {
                objconexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand("sp_xP_Exportar_Empleados_Por_Fecha_Registrada_Estructura_29", objconexion);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@idcompany", id);
                ObjCmd.Parameters.AddWithValue("@fechaDesde", fechadesde);
                ObjCmd.Parameters.AddWithValue("@fechaHasta", fechahasta);
                dt = new DataTable();
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                clsConnection.DisposeCommand(objconexion);
            }
            return dt;
        }

        public DataTable ExpotacionEmpleadoCSV_e13(ref BaseEntity entity, Int32 id, DateTime fechadesde, DateTime fechahasta)
        {
            SqlConnection objconexion = null;
            DataTable dt = null;
            try
            {
                objconexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand("sp_xP_Exportar_Empleados_Por_Fecha_Registrada_Estructura_13", objconexion);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@idcompany", id);
                ObjCmd.Parameters.AddWithValue("@fechaDesde", fechadesde);
                ObjCmd.Parameters.AddWithValue("@fechaHasta", fechahasta);
                dt = new DataTable();
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                clsConnection.DisposeCommand(objconexion);
            }
            return dt;
        }


        public DataTable ExpotacionEmpleadoCSV_e24(ref BaseEntity entity, Int32 id, DateTime fechadesde, DateTime fechahasta)
        {
            SqlConnection objconexion = null;
            DataTable dt = null;
            try
            {
                objconexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand("sp_xP_Exportar_Empleados_Por_Fecha_Registrada_Estructura_24", objconexion);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.Parameters.AddWithValue("@idcompany", id);
                ObjCmd.Parameters.AddWithValue("@fechaDesde", fechadesde);
                ObjCmd.Parameters.AddWithValue("@fechaHasta", fechahasta);
                dt = new DataTable();
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                clsConnection.DisposeCommand(objconexion);
            }
            return dt;
        }

        public String Obtener_NumeroEmpleadosPorFecha(ref BaseEntity Base, Int32 id, DateTime fechadesde, DateTime fechahasta)
        {

            String numero_empleados = "";
            //clsHorasExtrasAsignadas objHorasExtrasAsignadas = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Obtener_Numero_Empleados_Por_Fecha", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@idcompany", id);
                cmd.Parameters.AddWithValue("@fechaDesde", fechadesde);
                cmd.Parameters.AddWithValue("@fechaHasta", fechahasta);
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //objHorasExtrasAsignadas = new clsHorasExtrasAsignadas();
                    // objHorasExtrasAsignadas.HorasEstrasId = Convert.ToInt32(reader["HorasExtrasID"]);

                    numero_empleados = reader["NumeroEmpleados"].ToString();



                }
            }
            catch (Exception ex)
            {
                //  objHorasExtrasAsignadas = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "No Encontrado"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return numero_empleados;
        }
        #endregion

        #region ValidaDocumento
        public Boolean ValidaDocumento(ref BaseEntity Base, String documento , Int32 EmpresaId)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            SqlConnection objc = null;
            SqlDataReader dr = null;
            try
            {
                objc = clsConnection.GetConnection();
                cmd = new SqlCommand("sp_xP_ValidaEmpleado", objc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Dni", documento);
                cmd.Parameters.AddWithValue("@EmpresaId", EmpresaId);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return success;
        }
        #endregion 

        #region Valida Correo Empleado
        public bool EmployeeEmailValidate(ref BaseEntity Base, String email, Int32 employeeId, Int32 company)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Valida_Correo_Empleado", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@employeeId", employeeId);
                cmd.Parameters.AddWithValue("@company", company);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error in Employee Dao"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return success;
        }
        #endregion Valida Correo Empleado


        public DataTable Listar_Pais_Emisor_Documento(ref BaseEntity entity)
        {
            SqlConnection objconexion = null;
            DataTable dt = null;
            try
            {
                objconexion = clsConnection.GetConnection();
                SqlCommand ObjCmd = new SqlCommand("sp_xP_Listar_Pais_Emisor_Documento", objconexion);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                dt = new DataTable();
                dt.Load(ObjCmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                entity.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                clsConnection.DisposeCommand(objconexion);
            }
            return dt;
        }

        public Boolean EliminarSituacionEducativa(ref BaseEntity Base, Int32 IdSituacion, Int32 update)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            try {
                cmd = new SqlCommand("sp_xP_Eliminar_Situacion_Educativa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", IdSituacion);
                cmd.Parameters.AddWithValue("@UpdateBy", update);
                success = cmd.ExecuteNonQuery() > 0 ? true : false;

            } catch(Exception ex) {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error al Cargar la Capa Dao del Empleado"));
            }
            finally { clsConnection.DisposeCommand(cmd); }
            return success;
        }


    }
}
