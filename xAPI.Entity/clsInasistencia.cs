using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    [Serializable]
    public class clsInasistencia : BaseEntity
    {
        public Int32 InasistenciaId { get; set; }
        public Int32 EmpleadoId { get; set; }
        public DateTime FechaInasistencia { get; set; }
        public bool EstadoJustificacion { get; set; }
        public Int32 PlazoJustificacion { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int LastUpdateBy { get; set; }
        public Boolean Estado { get; set; }
        public string EstadoEmpleado { get; set; }
    }
}
