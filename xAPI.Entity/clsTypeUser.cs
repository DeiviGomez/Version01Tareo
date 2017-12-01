using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    [Serializable]
    public class clsTypeUser: BaseEntity
    {
        public int TipoUsuarioId { get; set; }
        public string Nombre { get; set; }
        public string DescripcionTipo { get; set; }
        
    }
}
