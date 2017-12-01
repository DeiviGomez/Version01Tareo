using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using xAPI.Library.Base;
using xAPI.Library;

namespace xAPI.Entity
{
    public class clsMemorandums : BaseEntity
    {
        public int EmpresaId { get; set; }
        public String archivo;
        public int EmpleadoId { get; set; }
        public clsTemplateMemorandumcs memorandum { get; set; }
        public String Motivo { get; set; }
        public String Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoMemorandum { get; set; }
        public String MemorandumContenido { get; set; }
        public int TareoTardanzaId { get; set; }
        public int TareoInasistenciaId { get; set; }
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
        public string MemoTag { get; set; } //Variable creada para el tag
    }
}
