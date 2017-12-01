using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;


namespace xAPI.Entity
{
    public class clsEmployee : BaseEntity
    {
        ///Datos personales del trabajador
        public int EmpleadoId { get; set; }
        public String Apellido { get; set; }
        public String Nombre { get; set; }
        public int TipoDocumentoId { get; set; }
        public String NumeroDocumento { get; set; }
        public String Sexo { get; set; }
        public int EstadoCivilId { get; set; }
        public int NacionalidadId { get; set; }
        public int Hijos { get; set; }
        public int CodigoLargaDistanciaId { get; set; }
        public String Telefono { get; set; }
        public String Celular { get; set; }
        public String Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int DepartamentoNac { get; set; }
        public int ProvinciaNac { get; set; }
        public int DistritoNac { get; set; }
        public int Estado { get; set; }
        public int PaisEmisor { get; set; }
        ///Direccion del trabajador
        public int DepartamentoId { get; set; }
        public int ProvinciaId { get; set; }
        public int DistritoId { get; set; }
        public int TipoZonaId { get; set; }
        public int TipoViaId { get; set; }
        public String NombreVia { get; set; }
        public String NumeroVia { get; set; }
        public String Interior { get; set; }
        public String NombreZona { get; set; }
        public String Referencia { get; set; }
        ///Direccion del trabajador2
        public int DepartamentoId2 { get; set; }
        public int ProvinciaId2 { get; set; }
        public int DistritoId2 { get; set; }
        public int TipoZonaId2 { get; set; }
        public int TipoViaId2 { get; set; }
        public String NombreVia2 { get; set; }
        public String NumeroVia2 { get; set; }
        public String Interior2 { get; set; }
        public String NombreZona2 { get; set; }
        public String Referencia2 { get; set; }
        ///Datos Laborales del trabajador
        public DateTime FechaInicioLaboral { get; set; }
        public DateTime FechaFinLaboral { get; set; }
        public int MotivoBajaId { get; set; }
        public int HorarioId { get; set; }
        public int TipoEmpleadoId { get; set; }
        public DateTime FechaInicioTipoEmpleado { get; set; }
        public DateTime FechaFinTipoEmpleado { get; set; }
        public int RegimenLaboralId { get; set; }
        public int CategoriaOcupacionalId { get; set; }
        public int NivelEducativoId { get; set; }
        public int OcupacionId { get; set; }
        public int CentroCostoId { get; set; }
        public int TipoPagoId { get; set; }
        public int PeriodicidadId { get; set; }
        public int TipoContratoId { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public DateTime FechaFinContrato { get; set; }
        public Decimal Remuneracion { get; set; }
        public Decimal ValorDia { get; set; }
        public Decimal ValorHora { get; set; }
        public Decimal ValorMinuto { get; set; }
        public DateTime FichaREMYPE { get; set; }
        public String RegREMYPE { get; set; }
        public Int32 EmpresaId { get; set; }
        public int Empleo { get; set; }
        public int JornadaLaboralId { get; set; }
        public int SituacionEspecialId { get; set; }
        public int Discapacitado { get; set; }
        public int Sindicalizado { get; set; }
        public int SituacionEmpleadoId { get; set; }
        ///Datos de Seguridad Social del trabajador
        public int RegimenSaludId { get; set; }
        public int RegimenPensionId { get; set; }
        public DateTime FechaInicioRegimenSalud { get; set; }
        public DateTime FechaFinRegimenSalud { get; set; }
        public String CUSPP { get; set; }
        public DateTime FechaInicioRegimenPension { get; set; }
        public DateTime FechafinRegimenPension { get; set; }
        public int EstadoAFP { get; set; }
        public int comision{ get; set; }

        //Datos Situación Educativa
        public tBaseEducationalSituation ListaSituacionEducativa { get; set; }

        //Datos Derecho Habiente
        public tBaseDerechoHabiente ListaDerechoHabiente { get; set; }

        ///Datos Tributarios
        public int Renta5taCategoria { get; set; }
        public int AplicaConvenio { get; set; }
        public int ConvenioId { get; set; }

        ///Datos de Personal en Formacion
        public int ModalidadFormativaId { get; set; }
        public DateTime FechaInicioFormacion { get; set; }
        public DateTime FechaFinFormacion { get; set; }
        public int NivelEducativoId1 { get; set; }
        public int OcupacionId1 { get; set; }
        public int CentroFormacion { get; set; }
        public String Codigo { get; set; }
        public String Local { get; set; }
        public int SeguroMedico { get; set; }
        public int Discapacidad { get; set; }
        public int HorarioNocturno { get; set; }
        public int SituacionEmpleadoId1 { get; set; }
        public String EjercicioRenta { get; set; }
        public DateTime FechaEmisionRenta { get; set; }
        public String NombreEmpresaRenta { get; set; }
        public String NumeroRucRenta { get; set; }
        public String DomicilioFiscalRenta { get; set; }
        public String RepresentanteRenta { get; set; }
        public String dniRepresentanteRenta { get; set; }
        public Decimal RemuneracionAfectRenta { get; set; }
        public Decimal RemuneracionNoAfectaRenta { get; set; }
        public Decimal UtilidadesRenta { get; set; }
        public Decimal MontoTrabajadorIndependienteRenta { get; set; }
        public Decimal ImpuestoRetenidoRenta { get; set; }


        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int LastUpdateBy { get; set; }
    }


    [Serializable]
    public class clsEducationalSituation
    {
        public Int32 SituacionEducativaEmpleadoId { get; set; }
        public Int32 EmpleadoId { get; set; }
        public Int32 NivelEducativoId { get; set; }
        public Int32 LocalEstudio { get; set; }
        public Int32 TipoInstitucionId { get; set; }
        public Int32 TipoRegimenId { get; set; }
        public Int32 InstitucionId { get; set; }
        public Int32 ProfesionId { get; set; }
        public String AnhoEgreso { get; set; }
        public Int32 Estado { get; set; }

        public DateTime CreatedDate { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public Int32 LastUpdateBy { get; set; }

        #region Methods
        public override int GetHashCode()
        {
            return this.EmpleadoId.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            clsEducationalSituation o = (clsEducationalSituation)obj;
            return (this.EmpleadoId == o.EmpleadoId);
        }
        #endregion
    }


    [Serializable]
    public class clsDerechohabiente
    {
        public Int32 DerechoHabienteId { get; set; }
        public Int32 EmpleadoId { get; set; }
        public Int32 TipoDocumento { get; set; }
        public String NDocumento { get; set; }
        public String ApellidoPaterno { get; set; }
        public String ApellidoMaterno { get; set; }
        public String Nombre { get; set; }
        public Int32 PaisEmisorDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Int32 Sexo { get; set; }
        public Int32 VinculoFamiliar { get; set; }
        public Int32 DocumentoSustentacion { get; set; }
        public DateTime MesConcepcion { get; set; }
        public Int32 Situacion { get; set; }
        public String NDocumentacion { get; set; }
        public Int32 EstadoCivil { get; set; }
        //DIRECCION 1:
        public Int32 Departamento1 { get; set; }
        public Int32 Provincia1 { get; set; }
        public Int32 Distrito1 { get; set; }
        public Int32 TipoVia1 { get; set; }
        public String NombreVia1 { get; set; }
        public String NumeroVia1 { get; set; }
        public String Interior1 { get; set; }
        public Int32 TipoZona1 { get; set; }
        public String NombreZona1 { get; set; }
        public String Referencia1 { get; set; }
        //DIRECCION 2:
        public Int32 Departamento2 { get; set; }
        public Int32 Provincia2 { get; set; }
        public Int32 Distrito2 { get; set; }
        public Int32 TipoVia2 { get; set; }
        public String NombreVia2 { get; set; }
        public String NumeroVia2 { get; set; }
        public String Interior2 { get; set; }
        public Int32 TipoZona2 { get; set; }
        public String NombreZona2 { get; set; }
        public String Referencia2 { get; set; }
        //------------------------------------------
        public Int32 CodigoCiudad { get; set; }
        public String Numero { get; set; }
        public String Correo { get; set; }


        public DateTime CreatedDate { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public Int32 LastUpdateBy { get; set; }

        #region Methods
        public override int GetHashCode()
        {
            return this.EmpleadoId.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            clsDerechohabiente o = (clsDerechohabiente)obj;
            return (this.EmpleadoId == o.EmpleadoId);
        }
        #endregion
    }

}
