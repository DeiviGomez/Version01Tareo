using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using xAPI.Library.Base;
using xAPI.Library;

namespace xAPI.Entity
{
   public class clsTemplateMemorandumcs : BaseEntity
    {
       public int EmpresaId { get; set; }
       public String MemorandumPlantillaTipo { get; set; }
       public String MemorandumContenido { get; set; }
       public int Principal { get; set; }
    
    }
}
