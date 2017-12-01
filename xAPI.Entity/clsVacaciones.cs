using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    public class clsVacaciones : BaseEntity
    {
        public int VacacionesId { get; set; }
        public int EmpleadoId { get; set; }
        public String FechaInicio { get; set; }
        public String FechaTermino { get; set; }
        public String Reingreso { get; set; }
        public clsPlantillaVacaciones planvacaciones { get; set; }
        public int Estado { get; set; }
        public clsEmployee empleado { get; set; }
        public clsCompany empresa { get; set; }
        public String FechaActual { get; set; }
        public string JustTag { get; set; } //Variable creada para el tag
    }
}
