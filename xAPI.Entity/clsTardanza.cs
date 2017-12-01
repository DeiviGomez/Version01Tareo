using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Library.Base;


namespace xAPI.Entity
{
    [Serializable]
    public class clsTardanza: BaseEntity
    {
        public Int32 TardanzaId { get; set; }
        public Int32 EmpleadoId { get; set; }
        public DateTime FechaTardanza { get; set; }
        public DateTime? HoraEntradaManiana { get; set; }
        public DateTime? HoraSalidaManiana { get; set; }

        public DateTime? HoraEntradaTarde { get; set; }
        public DateTime? HoraSalidaTarde { get; set; }

        public Int32 MinutosTardanza { get; set; }
        public bool EstadoJustificacion { get; set; }
        public Int32 PlazoJustificacion { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int LastUpdateBy { get; set; }
        public string EstadoEmpleado { get; set; }
        
    }
}
