using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using xAPI.Library.Base;
using xAPI.Library;

namespace xAPI.Entity
{
    public class clsTypeJustification : BaseEntity
    {
        public String CodSuspensionRelacionLaboral { get; set; }
        public String Descripcion { get; set; }
        public String DescripcionCorta { get; set; }
    }
}
