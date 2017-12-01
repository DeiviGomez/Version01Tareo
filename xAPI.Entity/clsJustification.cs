using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using xAPI.Library.Base;
using xAPI.Library;

namespace xAPI.Entity
{
    public class clsJustification : BaseEntity
    {
        
        public int EmpresaId { get; set; }
        public String archivo;
        public int EmpleadoId { get; set; }
        public clsTemplateJustification justificacion { get; set; }
        public String Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public String JustificacionContenido { get; set; }
        public int TardanzaId { get; set; }
        public int InasistenciaId { get; set; }
        public String Archivo
        {
            get
            {
                if (archivo == null)
                {
                    archivo = "";
                } return archivo;
            }
            set { archivo = value; }
        }

        public clsEmployee employee { get; set; }
        public string JustTag { get; set; } //Variable creada para el tag
    }
}
