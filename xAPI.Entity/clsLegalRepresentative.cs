using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    public class clsLegalRepresentative : BaseEntity
    {
        public Int32 EmpresaId { get; set; }
        public Int32 TipoDocumentoId { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreRepresentateLegal { get; set; }
        public DateTime FechaNacimiento { get; set; }/*date*/
        public string Telefono { get; set; }
        public string Cargo { get; set; }
        public string Direccion { get; set; }
        public string PartidaSunarp { get; set; }
        public DateTime FechaIRepresentacion { get; set; } /*date*/
    }
}
