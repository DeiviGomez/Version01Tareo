using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    public class clsAddressesRisk : BaseEntity
    {
        public Int32 EmpresaId { get; set; }
        public Int32 DepartamentoId { get; set; }
        public Int32 ProvinciaId { get; set; }
        public Int32 DistritoId { get; set; }
        public string Direccion { get; set; }
        public bool Riesgo { get; set; }
        public Int32 Estado { get; set; }
        public override int GetHashCode()
        {
            return this.EmpresaId.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            clsAddressesRisk o = (clsAddressesRisk)obj;
            return (this.EmpresaId == o.EmpresaId);
        }
    }
}
