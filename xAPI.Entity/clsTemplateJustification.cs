using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using xAPI.Library.Base;
using xAPI.Library;

namespace xAPI.Entity
{
    public class clsTemplateJustification : BaseEntity
    {
        public int EmpresaId { get; set; }
        public String JustificacionPlantillaTipo { get; set; }
        public clsTypeJustification TypeJustification { get; set; }
        public String JustificacionContenido { get; set; }
        
    }
}
