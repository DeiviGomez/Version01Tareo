using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    public class clsUserPermissions : BaseEntity 
    {
        public Int32 UsuarioId { get; set; }
        public Int32 PermisosId { get; set; }
    }
}
