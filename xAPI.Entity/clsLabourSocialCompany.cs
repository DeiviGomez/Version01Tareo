using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    public class clsLabourSocialCompany : BaseEntity
    {
        public Int32 EmpresaId { get; set; }
        public bool RegistradaRemype { get; set; }
        public bool RegimenPensionario { get; set; }
        public Int32 EmpresaDedicaId { get; set; }
        public bool PersonalDiscapacidad { get; set; }
        public bool AgenciaEmpleos { get; set; }
        public bool DesplazaPersonal { get; set; }
        public bool EmpleadorDestacaSupersonal { get; set; }
        public string FichaREMYPE { get; set; }
        public String RegREMYPE { get; set; }
        public int TipoEmpresa { get; set; }
    }
}
