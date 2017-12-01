using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    [Serializable]
    public class clsTareo: BaseEntity
    {
        public Int32 TareoId { get; set; }
        public Int32 EmpleadoId { get; set; }
        public DateTime FechaTareo { get; set; }
        public DateTime? HoraEntrada { get; set; }
        public DateTime? HoraSalida { get; set; }
        public DateTime? DescansoEntrada { get; set; }
        public DateTime? DescansoSalida { get; set; }
        public DateTime Createdate { get; set; }
        public int Createdby { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Updatedby { get; set; }
        public String Estado { get; set; }
        public String EstadoNoLaboral { get; set; }
        public int FechMesTareoID { get; set; }
        public String FechaMes { get; set; }
        public DateTime DatoMes { get; set; }

    }
}
