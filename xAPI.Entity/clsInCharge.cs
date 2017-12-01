using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    public class clsInCharge : BaseEntity 
    {
        public string NombreApellido { get; set; }
        public string Area  { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
    }
}
