using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using xAPI.Entity;
using xAPI.Library.Conexion;
using xAPI.Library.Base;
using System.Data.SqlClient;
using xAPI.Library.General;
using xAPI.Library.Connection;
namespace xAPI.Dao
{
    public class clsCompanyDAO
    {
        #region Singleton
        private static readonly clsCompanyDAO _instance = new clsCompanyDAO();
        public static clsCompanyDAO Instance
        {
            get { return clsCompanyDAO._instance; }
        }
        #endregion
        public List<clsDepartment> ListDepartament_v1(ref BaseEntity Entity)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<clsDepartment> lstobj = null;

            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Departamentos", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    lstobj = new List<clsDepartment>();
                    while (dr.Read())
                    {
                        clsDepartment obj = new clsDepartment();
                        obj.ID = dr.GetColumnValue<Int32>("ID"); 
                        obj.Description = dr.GetColumnValue<String>("DESCRIPCION");
                        
                        lstobj.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                lstobj = null;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Recursos No Encontrados"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return lstobj;
        }

        public DataTable GetAll_ListCompany(ref BaseEntity Entity)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Empresa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Recursos No Encontrados"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }
        public DataTable GetAll_ListCompanyV1(ref BaseEntity Entity, Int32 user)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Empresa_v1", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User",user);
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Recursos No Encontrados"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return dt;
        }
        public DataTable List_Company_User_v_1(ref BaseEntity entidad, Int32 usuario)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                if (usuario != 1)
                {
                    cmd = new SqlCommand("sp_xP_Listar_Empresa_Usuario_V1", clsConnection.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    dt.Load(cmd.ExecuteReader());
                }
                else 
                {
                    cmd = new SqlCommand("sp_xP_Listar_Empresa", clsConnection.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@usuario", usuario);
                    dt.Load(cmd.ExecuteReader());
                }
            }
            catch (Exception ex)
            {
                dt = null;
                entidad.Errors.Add(new BaseEntity.ListError(ex, "Recursos No Encontrados"));
            }
            finally 
            {
                clsConnection.DisposeCommand(cmd);
                
            }
            return dt;
        }

        public Boolean Company_Delete(ref BaseEntity Entity, tBaseIdList BaseList)
        {
            bool success = false;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Eliminar_Masivo_Empresa", clsConnection.GetConnection());
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@TYPE_BASEID", Value = BaseList, SqlDbType = SqlDbType.Structured, TypeName = "dbo.TY_BASEID" });
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Se ha producido un error al eliminar un recurso."));

            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return success;
        }
        public Boolean ActualizarDireccionesAnexas(ref BaseEntity Entity, int NuevoId)
        {
            bool success = false;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Eliminar_DireccionesAnexas", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", NuevoId);
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Entity.Errors.Add(new BaseEntity.ListError(ex, "Se ha producido un error al eliminar un recurso."));

            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return success;
        }
        public DataTable Load_Company(ref BaseEntity objBase, Int32 EmpresaId)
        {
            DataTable dt = null;

            try
            {
                using (var objCommand = new SqlCommand("sp_xP_Datos_Empresas_Por_Id", clsConnection.GetConnection()))
                {
                    objCommand.CommandType = CommandType.StoredProcedure;
                    objCommand.Parameters.AddWithValue("@idempresa", EmpresaId);

                    using (var objDataReader = objCommand.ExecuteReader())
                    {
                        if (objDataReader.HasRows)
                        {
                            dt = new DataTable();
                            dt.Load(objDataReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
                objBase.Errors.Add(new BaseEntity.ListError() { Error = ex, MessageClient = ex.Message });
            }

            return dt;
        }

        public bool CompanySaveData(ref BaseEntity Entity, clsCompany Empresa) {
            bool bolSuccess = false;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Guardar_Masivo_Empresa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter outputParam = cmd.Parameters.Add("@NEWID", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output; //establece que es un valor de salida que llega del sp
                if (Empresa.ListaDireccionesAnexas.Count != 0)
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@TYPE_DireccionesAnexas", Value = Empresa.ListaDireccionesAnexas, SqlDbType = SqlDbType.Structured, TypeName = "dbo.TY_DIRECCIONES_ANEXAS" });
                cmd.Parameters.AddWithValue("@ID", Empresa.ID);
                cmd.Parameters.AddWithValue("@NUMBERRUC", Empresa.NumeroRuc);
                cmd.Parameters.AddWithValue("@BUSINESSNAME", Empresa.RazonSocial);
                cmd.Parameters.AddWithValue("@TRADENAME", Empresa.NombreComercial);
                cmd.Parameters.AddWithValue("@TYPETAXPAYER", Empresa.TipoContribuyente);
                cmd.Parameters.AddWithValue("@ADDRESS", Empresa.Direccion);
                cmd.Parameters.AddWithValue("@DEPARTMENT", Empresa.Departamento);
                cmd.Parameters.AddWithValue("@PROVINCE", Empresa.Provincia);
                cmd.Parameters.AddWithValue("@DISTRICT", Empresa.Distrito);
                cmd.Parameters.AddWithValue("@TELEPHONE", Empresa.Telefono);
                cmd.Parameters.AddWithValue("@STATUSCOMPANY", Empresa.EstadoEmpresa);
                cmd.Parameters.AddWithValue("@EMAIL", Empresa.CorreoElectronico);
                cmd.Parameters.AddWithValue("@EMPLOYERTYPEID", Empresa.TipoEmpleadorId);
                cmd.Parameters.AddWithValue("@ACTIVITYID", Empresa.ActividadId);
                cmd.Parameters.AddWithValue("@CONTRIBUTIONSSENATI", Empresa.AporteSenati);
                cmd.Parameters.AddWithValue("@CREATEDBY", Empresa.Createdby);
                cmd.Parameters.AddWithValue("@STATUS", Empresa.Status);
                ////Anex & Riesgo
                //cmd.Parameters.AddWithValue("@DEPARTMENTID", Empresa.DireccionesRiesgo.DepartamentoId);
                //cmd.Parameters.AddWithValue("@PROVINCEID", Empresa.DireccionesRiesgo.ProvinciaId);
                //cmd.Parameters.AddWithValue("@DISTRICTID", Empresa.DireccionesRiesgo.DistritoId);
                //cmd.Parameters.AddWithValue("@ADDRESSDA", Empresa.DireccionesRiesgo.Direccion);
                //cmd.Parameters.AddWithValue("@RISK", Empresa.DireccionesRiesgo.Riesgo);
                //Labor Social
                cmd.Parameters.AddWithValue("@REGISTEREDREMYPE", Empresa.LaborSocialEmpresa.RegistradaRemype);
                cmd.Parameters.AddWithValue("@PENSIONERREGIME", Empresa.LaborSocialEmpresa.RegimenPensionario);
                cmd.Parameters.AddWithValue("@COMPANYDEDICATEDID", Empresa.LaborSocialEmpresa.EmpresaDedicaId);
                cmd.Parameters.AddWithValue("@PEOPLEDESABILITIES", Empresa.LaborSocialEmpresa.PersonalDiscapacidad);
                cmd.Parameters.AddWithValue("@AGENCIEMPLOYEES", Empresa.LaborSocialEmpresa.AgenciaEmpleos);
                cmd.Parameters.AddWithValue("@MOVINGCOMPANYWORKERS", Empresa.LaborSocialEmpresa.DesplazaPersonal);
                cmd.Parameters.AddWithValue("@THIRDDISPLACE", Empresa.LaborSocialEmpresa.EmpleadorDestacaSupersonal);
                cmd.Parameters.AddWithValue("@FechaREMYPE", Empresa.LaborSocialEmpresa.FichaREMYPE);
                cmd.Parameters.AddWithValue("@RegREMYPE", Empresa.LaborSocialEmpresa.RegREMYPE);
                cmd.Parameters.AddWithValue("@TipoEmpresa", Empresa.LaborSocialEmpresa.TipoEmpresa);
                //Repre. Legal
                cmd.Parameters.AddWithValue("@DOCUMENTTYPEID", Empresa.RepresentateLegal.TipoDocumentoId);
                cmd.Parameters.AddWithValue("@DOCUMENTNUMBER", Empresa.RepresentateLegal.NumeroDocumento);
                cmd.Parameters.AddWithValue("@NAMESURNAMERL", Empresa.RepresentateLegal.NombreRepresentateLegal);
                cmd.Parameters.AddWithValue("@BIRTHDATE", Empresa.RepresentateLegal.FechaNacimiento);
                cmd.Parameters.AddWithValue("@TELEPHONERL", Empresa.RepresentateLegal.Telefono);
                cmd.Parameters.AddWithValue("@POSITION", Empresa.RepresentateLegal.Cargo);
                cmd.Parameters.AddWithValue("@ADDRESSRL", Empresa.RepresentateLegal.Direccion);
                cmd.Parameters.AddWithValue("@DEPARTURESUNARP", Empresa.RepresentateLegal.PartidaSunarp);
                cmd.Parameters.AddWithValue("@DATELEGALREPRESENTATION", Empresa.RepresentateLegal.FechaIRepresentacion);
                //Resp. Empresa
                cmd.Parameters.AddWithValue("@NAMESURNAMERE", Empresa.Responsable.NombreApellido);
                cmd.Parameters.AddWithValue("@AREA", Empresa.Responsable.Area);
                cmd.Parameters.AddWithValue("@TELEPHONERE", Empresa.Responsable.Telefono);
                cmd.Parameters.AddWithValue("@MOBILERE", Empresa.Responsable.Movil);
                cmd.Parameters.AddWithValue("@CodEstablecimiento", Empresa.CodigoEstablecimiento);
                cmd.ExecuteNonQuery();
                if (Empresa.ID == 0)
                {
                    Empresa.ID = Convert.ToInt32(cmd.Parameters["@NEWID"].Value);//setea el id  que se establecio como parametro de salida a la empresa id
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
        public clsCompany CompanyAll(ref BaseEntity Base, Int16 EmpresaId)
        {
            clsCompany ObjEmpresa = null;
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Datos_Empresa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SetCompanyID", EmpresaId);
                dr = cmd.ExecuteReader();
                //if (dr.Read())
                    //User = GetEntity_v3(dr);
                if (dr.Read())
                {
                    ObjEmpresa = DataCompany(dr);
                }
            }
            catch (Exception ex)
            {

                ObjEmpresa = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Recursos No Encontrados"));
            }
            finally 
            {
                if (dr != null) { dr.Close(); }
                clsConnection.DisposeCommand(cmd);
            }
            return ObjEmpresa;
        }
        public DataTable LoadDireccionesAnexas(ref BaseEntity Base, Int32 id)
        {
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("sp_xP_Listar_Datos_DireccionesAnexas", clsConnection.GetConnection());
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                dt = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, "No found."));
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
        private clsCompany DataCompany(SqlDataReader dr)
        {

            clsCompany ObjEmpresa = new clsCompany();
            ObjEmpresa.ID = Convert.ToInt32(dr["ID_EMPRESA"]);
            ObjEmpresa.CodigoEstablecimiento = Convert.ToString(dr["COD_ESTABLECIMIENTO"]);
            ObjEmpresa.NumeroRuc = Convert.ToString(dr["RUC"]);
            ObjEmpresa.RazonSocial = Convert.ToString(dr["RAZON_SOCIAL"]);
            ObjEmpresa.NombreComercial = Convert.ToString(dr["NOMBRE_COMERCIAL"]);
            ObjEmpresa.TipoContribuyente = Convert.ToString(dr["TIPO_CONTRIBUYENTE"]);
            ObjEmpresa.Direccion = Convert.ToString(dr["DIRECCION"]);
            ObjEmpresa.Departamento = Convert.ToString(dr["DEPARTAMENTO"]);
            ObjEmpresa.Provincia = Convert.ToString(dr["PROVINCIA"]);
            ObjEmpresa.Distrito = Convert.ToString(dr["DISTRITO"]);
            ObjEmpresa.Telefono = Convert.ToString(dr["E_TELEFONO"]);
            ObjEmpresa.EstadoEmpresa = Convert.ToString(dr["ESTADO_EMPRESA"]);
            ObjEmpresa.CorreoElectronico = Convert.ToString(dr["CORREO_ELECTRONICO"]);
            ObjEmpresa.TipoEmpleadorId = Convert.ToInt32(dr["TIPO_EMPLEADORID"]);
            ObjEmpresa.ActividadId = Convert.ToInt32(dr["ACTIVIDADID"]);
            ObjEmpresa.AporteSenati = Convert.ToBoolean(dr["APORTE_SENATI"]);
            ObjEmpresa.Status = Convert.ToInt32(dr["ESTADO"]);
            //ObjEmpresa.DireccionesRiesgo = DataAddressesRisk(dr);
            ObjEmpresa.LaborSocialEmpresa = DataLabourSocialCompany(dr);
            ObjEmpresa.RepresentateLegal = DataLegalRepresentative(dr);
            ObjEmpresa.Responsable = DataInCharge(dr);
            return ObjEmpresa;
        }
        //private clsAddressesRisk DataAddressesRisk(SqlDataReader dr)
        //{
        //    clsAddressesRisk DireccionesRiesgo = new clsAddressesRisk();
        //    DireccionesRiesgo.DepartamentoId = Convert.ToInt32(dr["A_DEPARTAMENTOID"]);
        //    DireccionesRiesgo.ProvinciaId = Convert.ToInt32(dr["A_PROVINCIAID"]);
        //    DireccionesRiesgo.DistritoId = Convert.ToInt32(dr["A_DISTRITOID"]);
        //    DireccionesRiesgo.Direccion = Convert.ToString(dr["A_DIRECCION"]);
        //    DireccionesRiesgo.Riesgo = Convert.ToBoolean(dr["A_RIESGO"]);
        //    return DireccionesRiesgo;
        //}
        private clsLabourSocialCompany DataLabourSocialCompany(SqlDataReader dr)
        {
            clsLabourSocialCompany LaborSocialEmpresa = new clsLabourSocialCompany();
            LaborSocialEmpresa.RegistradaRemype = Convert.ToBoolean(dr["REGISTRADA_REMYPE"]);
            LaborSocialEmpresa.FichaREMYPE = Convert.ToString(dr["FechaREMYPE"]);
            LaborSocialEmpresa.RegREMYPE = Convert.ToString(dr["RegREMYPE"]);
            LaborSocialEmpresa.RegimenPensionario = Convert.ToBoolean(dr["REGIMEN_PENSIONARIO"]);
            LaborSocialEmpresa.EmpresaDedicaId = Convert.ToInt32(dr["EMPRESA_DEDICAID"]);
            LaborSocialEmpresa.PersonalDiscapacidad = Convert.ToBoolean(dr["PERSONAL_DISCAPACIDAD"]);
            LaborSocialEmpresa.AgenciaEmpleos = Convert.ToBoolean(dr["AGENCIA_EMPLEOS"]);
            LaborSocialEmpresa.DesplazaPersonal = Convert.ToBoolean(dr["DESPLAZA_PERSONAL"]);
            LaborSocialEmpresa.EmpleadorDestacaSupersonal = Convert.ToBoolean(dr["EMPLEADORDESTACA_PERSONAL"]);
            LaborSocialEmpresa.TipoEmpresa = Convert.ToInt32(dr["TIPO_EMPRESA"]);

            return LaborSocialEmpresa;
        }
        private clsLegalRepresentative DataLegalRepresentative(SqlDataReader dr)
        {
            clsLegalRepresentative RepresentateLegal = new clsLegalRepresentative();
            RepresentateLegal.TipoDocumentoId = Convert.ToInt32(dr["TIPO_DOCUMENTOID"]);
            RepresentateLegal.NumeroDocumento = Convert.ToString(dr["NUMERO_DOCUMENTO"]);
            RepresentateLegal.NombreRepresentateLegal = Convert.ToString(dr["R_NOMBRE_APELLIDO"]);
            RepresentateLegal.FechaNacimiento = Convert.ToDateTime(dr["FECHA_NACIMIENTO"]);
            RepresentateLegal.Telefono = Convert.ToString(dr["R_TELEFONO"]);
            RepresentateLegal.Cargo = Convert.ToString(dr["CARGO"]);
            RepresentateLegal.Direccion = Convert.ToString(dr["R_DIRECCION"]);
            RepresentateLegal.PartidaSunarp = Convert.ToString(dr["PARTIDA_SUNARP"]);
            RepresentateLegal.FechaIRepresentacion = Convert.ToDateTime(dr["FECHAI_REPRESENTACION"]);
            return RepresentateLegal;
        }
        private clsInCharge DataInCharge(SqlDataReader dr)
        {
            clsInCharge Responsable = new clsInCharge();
            Responsable.NombreApellido = Convert.ToString(dr["I_NOMBRE_APELLIDO"]);
            Responsable.Area = Convert.ToString(dr["AREA"]);
            Responsable.Telefono = Convert.ToString(dr["I_TELEFONO"]);
            Responsable.Movil = Convert.ToString(dr["I_MOVIL"]);
            return Responsable;
        }

        //Valida empresa si existe

        public Boolean Valida_empresa(ref BaseEntity Base, String ruc, String razon)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Valida_Empresa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ruc", ruc);
                cmd.Parameters.AddWithValue("@razon", razon);
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
        public bool CompanyEmailValidate(ref BaseEntity Base, String email, int companyId)
        {
            Boolean success = false;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                cmd = new SqlCommand("sp_xP_Valida_Correo_Empresa", clsConnection.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@companyId", companyId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Base.Errors.Add(new BaseEntity.ListError(ex, "Error in Company Dao"));
            }
            finally
            {
                clsConnection.DisposeCommand(cmd);
            }
            return success;
        }
     }
}
