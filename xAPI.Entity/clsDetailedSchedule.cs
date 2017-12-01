using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using xAPI.Library.Base;
namespace xAPI.Entity
{
    public class clsDetailedSchedule : BaseEntity
    {
        public int iddia { get; set; }
        public clsSchedule Horario { get; set; }
        public String Entradamaniana { get; set; }
        public String Salidamaniana { get; set; }
        public String EntradaTarde { get; set; }
        public String SalidaTarde { get; set; }
        public String BreakEntry { get; set; }
        public String BreakDeparture { get; set; }
       // public int BreakMinutes { get; set; }
        public clsDays Dia { get; set; }
    }
}
