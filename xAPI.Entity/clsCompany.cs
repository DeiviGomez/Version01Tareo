using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;
using xAPI.Entity;

namespace xAPI.Entity
{
    public class clsCompany : BaseEntity
    {
        public string NumeroRuc { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string TipoContribuyente { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Telefono { get; set; }
        public string EstadoEmpresa { get; set; }
        public string CorreoElectronico { get; set; }
        public DateTime MesProceso { get; set; }
        public Int32 Sheettype { get; set; }
        public Int32 TipoEmpleadorId { get; set; }
        public Int32 ActividadId { get; set; }
        public bool AporteSenati { get; set; }
        public clsLabourSocialCompany LaborSocialEmpresa { get; set; }
        public clsLegalRepresentative RepresentateLegal { get; set; }
        public clsInCharge Responsable { get; set; }
        public clsAddressesRisk DireccionesRiesgo { get; set; }
        public tBaseDireccionesAnexas ListaDireccionesAnexas { get; set; }
        public String CodigoEstablecimiento { get; set; }

    }
}
