using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using xAPI.Library.Base;
namespace xAPI.Entity
{
    public class clsSchedule : BaseEntity
    {
        public int EmpresaId { get; set; }
        public String Descripcion { get; set; }
        public int Principal { get; set; }
        public List<clsDetailedSchedule> dts { get; set; }
    }
}
