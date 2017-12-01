using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    #region BaseId
    [Serializable]
    public class tBaseId
    {
        public Int32 Id { get; set; }
        public Int16 Status { get; set; }
        public Int16 Action { get; set; }
    }

    [Serializable]
    public class tBaseIdList : List<tBaseId>, IEnumerable<SqlDataRecord>/* hereda la interfaz Ienumerable la cual tiene el metodo getEnumertor si o si implementado  */
    {
        /*al jectura el procedure madna a llamar al metodo  tipo SqlDataRecord*/
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()/*devuelve un tipo Ienumretaor, permite solo lectura a una colección de objetos*/
        {
            /*el cual pirmero configura la tabla sql data record envia una fila unica de datos */
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("STATUS", SqlDbType.SmallInt),
                new SqlMetaData("ACTION", SqlDbType.SmallInt)
                );
            /*recoore la lista de obejtos asignaleSqlDataRecord el valor de la lista */
            foreach (tBaseId data in this)/**recorre una lista enmurada y envia uno por uno son promresas*/
            {
                ret.SetInt32(0, data.Id);
                ret.SetInt16(1, data.Status);
                ret.SetInt16(2, data.Action);
                yield return ret;/*promesas yield*/
            }
        }
    }

    [Serializable]
    public class tBaseExportDetails
    {
        public Int32 Id { get; set; }
        public Int32 OrderExportId { get; set; }
        public Int32 FilterField { get; set; }
        public String FilterName { get; set; }
        public Int32 FilterType { get; set; }
        public Int16 Status { get; set; }
        public Int16 Action { get; set; }
    }

    [Serializable]
    public class tBaseExportDetailsList : List<tBaseExportDetails>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("ExportId", SqlDbType.Int),
                new SqlMetaData("FilterField", SqlDbType.Int),
                new SqlMetaData("FilterName", SqlDbType.VarChar, 500),
                new SqlMetaData("FilterType", SqlDbType.Int),
                new SqlMetaData("STATUS", SqlDbType.SmallInt),
                new SqlMetaData("ACTION", SqlDbType.SmallInt)
                );
            foreach (tBaseExportDetails data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetInt32(1, data.OrderExportId);
                ret.SetInt32(2, data.FilterField);
                ret.SetString(3, String.IsNullOrEmpty(data.FilterName) ? "" : data.FilterName);
                ret.SetInt32(4, data.FilterType);
                ret.SetInt16(5, data.Status);
                ret.SetInt16(6, data.Action);
                yield return ret;
            }
        }
    }
    [Serializable]
    public class tBaseExportColumns
    {
        public Int32 Id { get; set; }
        public Int32 ExportId { get; set; }
        public Int32 OrderColumn { get; set; }
        public String ColumnName { get; set; }
        public String ColumnType { get; set; }
        public String ColumnValue { get; set; }
        public String TableName { get; set; }
        public Int16 Status { get; set; }
        public Int16 Action { get; set; }
    }

    [Serializable]
    public class tBaseExportColumnsList : List<tBaseExportColumns>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("ExportId", SqlDbType.Int),
                new SqlMetaData("OrderColumn", SqlDbType.Int),
                new SqlMetaData("ColumnName", SqlDbType.VarChar, 500),
                new SqlMetaData("ColumnType", SqlDbType.VarChar, 500),
                new SqlMetaData("ColumnValue", SqlDbType.VarChar, 500),
                new SqlMetaData("TableName", SqlDbType.VarChar, 500),
                new SqlMetaData("STATUS", SqlDbType.SmallInt),
                new SqlMetaData("ACTION", SqlDbType.SmallInt)
                );
            foreach (tBaseExportColumns data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetInt32(1, data.ExportId);
                ret.SetInt32(2, data.OrderColumn);
                ret.SetString(3, String.IsNullOrEmpty(data.ColumnName) ? "" : data.ColumnName);
                ret.SetString(4, String.IsNullOrEmpty(data.ColumnType) ? "" : data.ColumnType);
                ret.SetString(5, String.IsNullOrEmpty(data.ColumnValue) ? "" : data.ColumnValue);
                ret.SetString(6, String.IsNullOrEmpty(data.TableName) ? "" : data.TableName);
                ret.SetInt16(7, data.Status);
                ret.SetInt16(8, data.Action);
                yield return ret;
            }
        }
    }
    

    [Serializable]
    public class tBaseEducationalSituation : List<clsEducationalSituation>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("SituacionEducativaEmpleadoId", SqlDbType.Int),
                new SqlMetaData("EmpleadoId", SqlDbType.Int),
                new SqlMetaData("NivelEducativoId", SqlDbType.Int),
                new SqlMetaData("LocalEstudio", SqlDbType.Int),
                new SqlMetaData("TipoInstitucionId", SqlDbType.Int),
                new SqlMetaData("TipoRegimenId", SqlDbType.Int),
                new SqlMetaData("InstitucionId", SqlDbType.Int),
                new SqlMetaData("ProfesionId", SqlDbType.Int),
                new SqlMetaData("AnhoEgreso", SqlDbType.NVarChar, 10),
                new SqlMetaData("Estado", SqlDbType.Int),
                new SqlMetaData("CreatedDate", SqlDbType.DateTime),
                new SqlMetaData("CreatedBy", SqlDbType.Int),
                new SqlMetaData("LastUpdateDate", SqlDbType.DateTime),
                new SqlMetaData("LastUpdateBy", SqlDbType.Int)
                );
            foreach (clsEducationalSituation data in this)
            {
                ret.SetInt32(0, data.SituacionEducativaEmpleadoId);
                ret.SetInt32(1, data.EmpleadoId);
                ret.SetInt32(2, data.NivelEducativoId);
                ret.SetInt32(3, data.LocalEstudio);
                ret.SetInt32(4, data.TipoInstitucionId);
                ret.SetInt32(5, data.TipoRegimenId);
                ret.SetInt32(6, data.InstitucionId);
                ret.SetInt32(7, data.ProfesionId);
                ret.SetString(8, data.AnhoEgreso);
                ret.SetInt32(9, data.Estado);
                ret.SetDateTime(10, data.CreatedDate);
                ret.SetInt32(11, data.CreatedBy);
                ret.SetDateTime(12, data.LastUpdateDate);
                ret.SetInt32(13, data.LastUpdateBy);
                yield return ret;
            }
        }
    }


    [Serializable]
    public class tBaseDerechoHabiente : List<clsDerechohabiente>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("DerechoHabienteId" ,   SqlDbType.Int),
                new SqlMetaData("EmpleadoId",           SqlDbType.Int),
                new SqlMetaData("TipoDocumento",        SqlDbType.Int),
                new SqlMetaData("NDocumento",           SqlDbType.VarChar, 50),
                new SqlMetaData("ApellidoPaterno",      SqlDbType.VarChar, 50),
                new SqlMetaData("ApellidoMaterno",      SqlDbType.VarChar, 50),
                new SqlMetaData("Nombre",               SqlDbType.VarChar, 100),
                new SqlMetaData("PaisEmisorDocumento",  SqlDbType.Int),
                new SqlMetaData("FechaNacimiento",      SqlDbType.Date),
                new SqlMetaData("Sexo",                 SqlDbType.Int),
                new SqlMetaData("VinculoFamiliar",      SqlDbType.Int),
                new SqlMetaData("DocumentoSustentacion", SqlDbType.Int),
                new SqlMetaData("MesConcepcion",        SqlDbType.Date),
                new SqlMetaData("Situacion",            SqlDbType.Int),
                new SqlMetaData("NDocumentacion",       SqlDbType.VarChar, 50),
                new SqlMetaData("EstadoCivil",          SqlDbType.Int),
                //DIRECCION 1: 
                new SqlMetaData("Departamento1",SqlDbType.Int),
                new SqlMetaData("Provincia1   ",SqlDbType.Int),
                new SqlMetaData("Distrito1    ",SqlDbType.Int),
                new SqlMetaData("TipoVia1     ",SqlDbType.Int),
                new SqlMetaData("NombreVia1   ",SqlDbType.VarChar, 50),
                new SqlMetaData("NumeroVia1   ", SqlDbType.VarChar, 15),
                new SqlMetaData("Interior1    ", SqlDbType.VarChar, 5),
                new SqlMetaData("TipoZona1    ",SqlDbType.Int),
                new SqlMetaData("NombreZona1  ",SqlDbType.VarChar, 50),
                new SqlMetaData("Referencia1  ", SqlDbType.VarChar, 50),
                //DIRECCION 2: 
                new SqlMetaData("Departamento2", SqlDbType.Int),
                new SqlMetaData("Provincia2   ", SqlDbType.Int),
                new SqlMetaData("Distrito2    ", SqlDbType.Int),
                new SqlMetaData("TipoVia2     ", SqlDbType.Int),
                new SqlMetaData("NombreVia2   ", SqlDbType.VarChar, 50),
                new SqlMetaData("NumeroVia2   ", SqlDbType.VarChar, 15),
                new SqlMetaData("Interior2    ", SqlDbType.VarChar, 5),
                new SqlMetaData("TipoZona2    ", SqlDbType.Int),
                new SqlMetaData("NombreZona2  ", SqlDbType.VarChar, 50),
                new SqlMetaData("Referencia2  ", SqlDbType.NVarChar, 50),
                //-----------------------------------------------
                new SqlMetaData("CodigoCiudad",SqlDbType.Int),
                new SqlMetaData("Numero      ", SqlDbType.VarChar, 50),
                new SqlMetaData("Correo      ", SqlDbType.VarChar, 50),
                
                new SqlMetaData("CreatedDate", SqlDbType.DateTime),
                new SqlMetaData("CreatedBy", SqlDbType.Int),
                new SqlMetaData("LastUpdateDate", SqlDbType.DateTime),
                new SqlMetaData("LastUpdateBy", SqlDbType.Int)



                );
            foreach (clsDerechohabiente data in this)
            {
                ret.SetInt32(0, data.DerechoHabienteId);
                ret.SetInt32(1, data.EmpleadoId);
                ret.SetInt32(2, data.TipoDocumento);
                ret.SetString(3, data.NDocumento);
                ret.SetString(4, data.ApellidoPaterno);
                ret.SetString(5, data.ApellidoMaterno);
                ret.SetString(6, data.Nombre);
                ret.SetInt32(7, data.PaisEmisorDocumento);
                ret.SetDateTime(8, data.FechaNacimiento);
                ret.SetInt32(9, data.Sexo);
                ret.SetInt32(10, data.VinculoFamiliar);
                ret.SetInt32(11, data.DocumentoSustentacion);
                ret.SetDateTime(12, data.MesConcepcion);
                ret.SetInt32(13, data.Situacion);
                ret.SetString(14, data.NDocumentacion);
                ret.SetInt32(15, data.EstadoCivil);               
                ret.SetInt32(16, data.Departamento1);
                ret.SetInt32(17, data.Provincia1);
                ret.SetInt32(18, data.Distrito1);
                ret.SetInt32(19, data.TipoVia1);
                ret.SetString(20, data.NombreVia1);
                ret.SetString(21, data.NumeroVia1);
                ret.SetString(22, data.Interior1);
                ret.SetInt32(23, data.TipoZona1);
                ret.SetString(24, data.NombreZona1);
                ret.SetString(25, data.Referencia1);
                ret.SetInt32(26, data.Departamento2);
                ret.SetInt32(27, data.Provincia2);
                ret.SetInt32(28, data.Distrito2);
                ret.SetInt32(29, data.TipoVia2);
                ret.SetString(30, data.NombreVia2);
                ret.SetString(31, data.NumeroVia2);
                ret.SetString(32, data.Interior2);
                ret.SetInt32(33, data.TipoZona2);
                ret.SetString(34, data.NombreZona2);
                ret.SetString(35, data.Referencia2);
                //----------------------------------
                ret.SetInt32(36, data.CodigoCiudad);
                ret.SetString(37, data.Numero);
                ret.SetString(38, data.Correo);
                ret.SetDateTime(39, data.CreatedDate);
                ret.SetInt32(40, data.CreatedBy);
                ret.SetDateTime(41, data.LastUpdateDate);
                ret.SetInt32(42, data.LastUpdateBy);
                yield return ret;
            }
        }
    }


    public class tBaseDireccionesAnexas : List<clsAddressesRisk>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("DireccionesAnexasId", SqlDbType.Int),
                new SqlMetaData("EmpresaId", SqlDbType.Int),
                new SqlMetaData("DepartamentoId", SqlDbType.Int),
                new SqlMetaData("ProvinciaId", SqlDbType.Int),
                new SqlMetaData("DistritoId", SqlDbType.Int),
                new SqlMetaData("Direccion", SqlDbType.NVarChar,100),
                new SqlMetaData("Riesgo", SqlDbType.Bit),
                new SqlMetaData("CreatedDate", SqlDbType.DateTime),
                new SqlMetaData("CreatedBy", SqlDbType.Int),
                new SqlMetaData("LastUpdateDate", SqlDbType.DateTime),
                new SqlMetaData("LastUpdateBy", SqlDbType.Int),
                new SqlMetaData("Estado", SqlDbType.Int)
                );
            foreach (clsAddressesRisk data in this)
            {
                ret.SetInt32(0, data.ID);
                ret.SetInt32(1, data.EmpresaId);
                ret.SetInt32(2, data.DepartamentoId);
                ret.SetInt32(3, data.ProvinciaId);
                ret.SetInt32(4, data.DistritoId);
                ret.SetString(5, data.Direccion);
                ret.SetBoolean(6, data.Riesgo);
                ret.SetDateTime(7, data.CreationDate);
                ret.SetInt32(8, data.Createdby);
                ret.SetDateTime(9, data.UpdateDate);
                ret.SetInt32(10, data.Updatedby);
                ret.SetInt32(11, data.Estado);
                yield return ret;
            }
        }
    }

    [Serializable]
    public class tBaseUserSettings
    {
        public Int32 Id { get; set; }
        public Int32 Code { get; set; }
        public Int32 Table { get; set; }
        public Int16 Status { get; set; }
        public Int16 Action { get; set; }
        public String Url { get; set; }
        #region Methods
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            tBaseUserSettings o = (tBaseUserSettings)obj;
            return (this.Id == o.Id);
        }
        #endregion
    }

    [Serializable]
    public class tBaseUserSettingsList : List<tBaseUserSettings>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("CODE   ", SqlDbType.Int),
                new SqlMetaData("TABLE", SqlDbType.Int),
                new SqlMetaData("STATUS", SqlDbType.SmallInt),
                new SqlMetaData("ACTION", SqlDbType.SmallInt)
                );
            foreach (tBaseUserSettings data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetInt32(1, data.Code);
                ret.SetInt32(2, data.Table);
                ret.SetInt16(3, data.Status);
                ret.SetInt16(4, data.Action);
                yield return ret;
            }
        }
    }

    [Serializable]
    public class tBaseAddress
    {
        public Int32 ID { get; set; }
        public Int16 Addresstype { get; set; }
        public String Address { get; set; }
        public String Address1 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Zip { get; set; }
        public String County { get; set; }
        public String Country { get; set; }
        public Int16 Setasdefault { get; set; }
        public Int32 Position { get; set; }
        public Int16 Action { get; set; }
        public Int32 Status { get; set; }
    }

    [Serializable]
    public class tBaseAddressList : List<tBaseAddress>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("ADDRESSTYPE", SqlDbType.SmallInt),
                new SqlMetaData("ADDRESS", SqlDbType.VarChar, 50),
                new SqlMetaData("ADDRESS1", SqlDbType.VarChar, 50),
                new SqlMetaData("CITY", SqlDbType.VarChar, 50),
                new SqlMetaData("STATE", SqlDbType.VarChar, 50),
                new SqlMetaData("ZIP", SqlDbType.VarChar, 50),
                new SqlMetaData("COUNTY", SqlDbType.VarChar, 50),
                new SqlMetaData("COUNTRY", SqlDbType.VarChar, 50),
                new SqlMetaData("SETASDEFAULT", SqlDbType.SmallInt),
                new SqlMetaData("ACTION", SqlDbType.SmallInt),
                new SqlMetaData("POSITION", SqlDbType.Int),
                new SqlMetaData("STATUS", SqlDbType.Int)
                );
            foreach (tBaseAddress data in this)
            {
                ret.SetInt32(0, data.ID);
                ret.SetInt16(1, data.Addresstype);
                ret.SetString(2, data.Address);
                ret.SetString(3, data.Address1);
                ret.SetString(4, data.City);
                ret.SetString(5, data.State);
                ret.SetString(6, data.Zip);
                ret.SetString(7, !String.IsNullOrEmpty(data.County) ? data.County : "");
                ret.SetString(8, data.Country);
                ret.SetInt16(9, data.Setasdefault);
                ret.SetInt16(10, data.Action);
                ret.SetInt32(11, data.Position);
                ret.SetInt32(12, data.Status);
                yield return ret;
            }
        }
    }
    [Serializable]
    public class tBaseBank
    {
        public int ID { get; set; }
        public int AccountType { get; set; }
        public string BankName { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public Int16 Setasdefault { get; set; }
        public Int16 Action { get; set; }
        public Int32 Position { get; set; }
    }
    [Serializable]
    public class tBaseBankList : List<tBaseBank>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                //new SqlMetaData("DISTRIBUTORID", SqlDbType.Int),
                new SqlMetaData("ACCOUNTTYPE", SqlDbType.Int),
                new SqlMetaData("BANKNAME", SqlDbType.VarChar, 50),
                new SqlMetaData("ROUTINGNUMBER", SqlDbType.VarChar, 50),
                new SqlMetaData("ACCOUNTNUMBER", SqlDbType.VarChar, 50),
                new SqlMetaData("SETASDEFAULT", SqlDbType.SmallInt),
                new SqlMetaData("ACTION", SqlDbType.SmallInt),
                new SqlMetaData("POSITION", SqlDbType.Int)
                );
            foreach (tBaseBank data in this)
            {
                ret.SetInt32(0, data.ID);
                ret.SetInt32(1, data.AccountType);
                ret.SetString(2, data.BankName);
                ret.SetString(3, data.RoutingNumber);
                ret.SetString(4, data.AccountNumber);
                ret.SetInt16(5, data.Setasdefault);
                ret.SetInt16(6, data.Action);
                ret.SetInt32(7, data.Position);
                yield return ret;
            }
        }
    }

  
   
    #endregion

    
    #region translation
    [Serializable]
    public class tBaseLanguageTranslator
    {
        public Int32 Id { get; set; }
        public String English { get; set; }
        public String Spanishusa { get; set; }

        public String Spanish { get; set; }
        public String FrenchCanadien { get; set; }
        public String German { get; set; }
        public String Hungary { get; set; }
        public String France { get; set; }
        public String Ireland { get; set; }
        public String Netherlands { get; set; }
        public String Austria { get; set; }
        public String Italy { get; set; }
        public String GreatBritain { get; set; }
        public String Slovenia { get; set; }
        public String Norway { get; set; }
        public String Dutch { get; set; }
        public String FrenchBelgium { get; set; }
        public String DutchBelgium { get; set; }
        public String Swedish { get; set; }
        public String Croatian { get; set; }
        public String Denmark { get; set; }
        public String Finland { get; set; }
        public String SpanishMexico { get; set; }
        public String Canada { get; set; }
        public String Australia { get; set; }
        public String Newzealand { get; set; }
    }

    [Serializable]
    public class tBaseLanguageTranslatorList : List<tBaseLanguageTranslator>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("ENGLISH", SqlDbType.NVarChar, -1),
                new SqlMetaData("SPANISH", SqlDbType.NVarChar, -1),
                new SqlMetaData("FRENCHCANADIEN", SqlDbType.NVarChar, -1),
                new SqlMetaData("GERMAN", SqlDbType.NVarChar, -1),
                new SqlMetaData("HUNGARY", SqlDbType.NVarChar, -1),
                new SqlMetaData("FRANCE", SqlDbType.NVarChar, -1),
                new SqlMetaData("IRELAND", SqlDbType.NVarChar, -1),
                new SqlMetaData("NETHERLANS", SqlDbType.NVarChar, -1),
                new SqlMetaData("AUSTRIA", SqlDbType.NVarChar, -1),
                new SqlMetaData("ITALY", SqlDbType.NVarChar, -1),
                new SqlMetaData("GREATBRITAIN", SqlDbType.NVarChar, -1),
                new SqlMetaData("SLOVENIA", SqlDbType.NVarChar, -1),
                new SqlMetaData("NORWAY", SqlDbType.NVarChar, -1),
                new SqlMetaData("DUTCH", SqlDbType.NVarChar, -1),
                new SqlMetaData("FRENCHBELGIUM", SqlDbType.NVarChar, -1),
                new SqlMetaData("DUTCHBELGIUM", SqlDbType.NVarChar, -1),
                new SqlMetaData("SWEDISH", SqlDbType.NVarChar, -1),
                new SqlMetaData("CROATIAN", SqlDbType.NVarChar, -1),
                new SqlMetaData("DENMARK", SqlDbType.NVarChar, -1),
                new SqlMetaData("FINLAND", SqlDbType.NVarChar, -1),
                new SqlMetaData("SPANISHMEXICO", SqlDbType.NVarChar, -1),
                new SqlMetaData("CANADA", SqlDbType.NVarChar, -1),
                new SqlMetaData("AUSTRALIA", SqlDbType.NVarChar, -1),
                new SqlMetaData("NEWZEALAND", SqlDbType.NVarChar, -1),
                new SqlMetaData("SPANISHUSA", SqlDbType.NVarChar, -1)
                );
            foreach (tBaseLanguageTranslator data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetString(1, data.English);
                ret.SetString(2, data.Spanish);
                ret.SetString(3, data.FrenchCanadien);
                ret.SetString(4, data.German);
                ret.SetString(5, data.Hungary);
                ret.SetString(6, data.France);
                ret.SetString(7, data.Ireland);
                ret.SetString(8, data.Netherlands);
                ret.SetString(9, data.Austria);
                ret.SetString(10, data.Italy);
                ret.SetString(11, data.GreatBritain);
                ret.SetString(12, data.Slovenia);
                ret.SetString(13, data.Norway);
                ret.SetString(14, data.Dutch);
                ret.SetString(15, data.FrenchBelgium);
                ret.SetString(16, data.DutchBelgium);
                ret.SetString(17, data.Swedish);
                ret.SetString(18, data.Croatian);
                ret.SetString(19, data.Denmark);
                ret.SetString(20, data.Finland);
                ret.SetString(21, data.SpanishMexico);
                ret.SetString(22, data.Canada ?? "");
                ret.SetString(23, data.Australia ?? "");
                ret.SetString(24, data.Newzealand ?? "");
                ret.SetString(25, data.Spanishusa ?? "");
                yield return ret;
            }
        }
    }
    #endregion


    [Serializable]
    public class tBaseIdv3
    {
        public Int32 Id { get; set; }
        public Int32 Id2 { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Quantity2 { get; set; }
        public Int16 Status { get; set; }
        public Int16 Action { get; set; }
    }

    [Serializable]
    public class tBaseIdListv3 : List<tBaseIdv3>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("ID2", SqlDbType.Int),
                new SqlMetaData("QUANTITY", SqlDbType.Decimal),
                new SqlMetaData("QUANTITY2", SqlDbType.Decimal),
                new SqlMetaData("STATUS", SqlDbType.SmallInt),
                new SqlMetaData("ACTION", SqlDbType.SmallInt)
                );
            foreach (tBaseIdv3 data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetInt32(1, data.Id2);
                ret.SetDecimal(2, data.Quantity);
                ret.SetDecimal(3, data.Quantity2);
                ret.SetInt16(4, data.Status);
                ret.SetInt16(5, data.Action);
                yield return ret;
            }
        }
    }
 

    [Serializable]
    public class tBaseInput
    {
        public Int32 Id { get; set; }
        public String Property { get; set; }
        public Int32 DataTypeId { get; set; }
        public String Notes { get; set; }
        public Int32 Status { get; set; }
        public Int32 MethodId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    public class tBaseInputList : List<tBaseInput>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("PROPERTY", SqlDbType.VarChar, 50),
                new SqlMetaData("DATATYPEID", SqlDbType.Int),
                new SqlMetaData("NOTES", SqlDbType.VarChar, 500),
                new SqlMetaData("STATUS", SqlDbType.Int),
                new SqlMetaData("METHODID", SqlDbType.Int),
                new SqlMetaData("CREATEDBY", SqlDbType.Int),
                new SqlMetaData("UPDATEDBY", SqlDbType.Int)
                );
            foreach (tBaseInput data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetString(1, data.Property);
                ret.SetInt32(2, data.DataTypeId);
                ret.SetString(3, data.Notes);
                ret.SetInt32(4, data.Status);
                ret.SetInt32(5, data.MethodId);
                ret.SetInt32(6, data.CreatedBy);
                ret.SetInt32(7, data.UpdatedBy);
                yield return ret;
            }
        }
    }

    [Serializable]
    public class tBaseOutput
    {
        public Int32 Id { get; set; }
        public String Property { get; set; }
        public Int32 DataTypeId { get; set; }
        public String Notes { get; set; }
        public Int32 Status { get; set; }
        public Int32 MethodId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    public class tBaseOutputList : List<tBaseOutput>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("PROPERTY", SqlDbType.VarChar, 50),
                new SqlMetaData("DATATYPEID", SqlDbType.Int),
                new SqlMetaData("NOTES", SqlDbType.VarChar, 500),
                new SqlMetaData("STATUS", SqlDbType.Int),
                new SqlMetaData("METHODID", SqlDbType.Int),
                new SqlMetaData("CREATEDBY", SqlDbType.Int),
                new SqlMetaData("UPDATEDBY", SqlDbType.Int)
                );
            foreach (tBaseOutput data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetString(1, data.Property);
                ret.SetInt32(2, data.DataTypeId);
                ret.SetString(3, data.Notes);
                ret.SetInt32(4, data.Status);
                ret.SetInt32(5, data.MethodId);
                ret.SetInt32(6, data.CreatedBy);
                ret.SetInt32(7, data.UpdatedBy);
                yield return ret;
            }
        }
    }

    #region AlertSystem_Market
    [Serializable]
    public class tBaseAlertSystem_Market
    {
        public Int32 AlertId { get; set; }
        public Int32 MarketId { get; set; }
        public Int16 Status { get; set; }
    }

    [Serializable]
    public class tBaseAlertSystem_MarketList : List<tBaseAlertSystem_Market>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ALERTID", SqlDbType.Int),
                new SqlMetaData("MARKETID", SqlDbType.Int),
                new SqlMetaData("STATUS", SqlDbType.SmallInt)
                );
            foreach (tBaseAlertSystem_Market data in this)
            {
                ret.SetInt32(0, data.AlertId);
                ret.SetInt32(1, data.MarketId);
                ret.SetInt16(2, data.Status);
                yield return ret;
            }
        }
    }
    #endregion

    [Serializable]
    public class srProductFilters
    {
        public Int32 Id { get; set; }
    }

    [Serializable]
    public class tBaseProducFiltersList : List<srProductFilters>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int)
                );
            foreach (srProductFilters data in this)
            {
                ret.SetInt32(0, data.Id);
                yield return ret;
            }
        }
    }

    [Serializable]
    public class tBaseBL
    {
        public Int64 Legacy { get; set; }
    }

    [Serializable]
    public class tBaseBlockList : List<tBaseBL>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("LEGACY", SqlDbType.BigInt)
                );
            foreach (tBaseBL data in this)
            {
                ret.SetInt64(0, data.Legacy);
                yield return ret;
            }
        }
    }

    #region Query Reports
    [Serializable]
    public class tQueryFields
    {
        public Int32 Id { get; set; }
        public Int32 QueryId { get; set; }
        public Int32 TableQueryId { get; set; }
        public Int32 TableQueryColumnId { get; set; }
        public String Alias { get; set; }
        public Int16 status { get; set; }
    }

    [Serializable]
    public class tQueryFieldsList : List<tQueryFields>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("QUERYID", SqlDbType.Int),
                new SqlMetaData("TABLEQUERYID", SqlDbType.Int),
                new SqlMetaData("TABLEQUERYCOLUMNID", SqlDbType.Int),
                new SqlMetaData("ALIAS", SqlDbType.NVarChar, 500),
                new SqlMetaData("STATUS", SqlDbType.SmallInt)
                );
            foreach (tQueryFields data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetInt32(1, data.QueryId);
                ret.SetInt32(2, data.TableQueryId);
                ret.SetInt32(3, data.TableQueryColumnId);
                ret.SetString(4, data.Alias);
                ret.SetInt16(5, data.status);
                yield return ret;
            }
        }
    }

    [Serializable]
    public class tQueryFilters
    {
        public Int32 Id { get; set; }
        public Int32 QueryId { get; set; }
        public Int32 TableQueryId { get; set; }
        public Int32 TableQueryColumnId { get; set; }
        public Int32 QueryComparisonId { get; set; }
        public String Value { get; set; }
        public Int16 Operator { get; set; }
        public Int16 status { get; set; }
    }

    [Serializable]
    public class tQueryFiltersList : List<tQueryFilters>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ID", SqlDbType.Int),
                new SqlMetaData("QUERYID", SqlDbType.Int),
                new SqlMetaData("TABLEQUERYID", SqlDbType.Int),
                new SqlMetaData("TABLEQUERYCOLUMNID", SqlDbType.Int),
                new SqlMetaData("QUERYCOMPARISONID", SqlDbType.Int),
                new SqlMetaData("VALUE", SqlDbType.NVarChar, 500),
                new SqlMetaData("OPERATOR", SqlDbType.SmallInt),
                new SqlMetaData("STATUS", SqlDbType.SmallInt)
                );
            foreach (tQueryFilters data in this)
            {
                ret.SetInt32(0, data.Id);
                ret.SetInt32(1, data.QueryId);
                ret.SetInt32(2, data.TableQueryId);
                ret.SetInt32(3, data.TableQueryColumnId);
                ret.SetInt32(4, data.QueryComparisonId);
                ret.SetString(5, data.Value);
                ret.SetInt16(6, data.Operator);
                ret.SetInt16(7, data.status);
                yield return ret;
            }
        }
    }
    #endregion
}
