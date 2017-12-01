using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    [Serializable]
    public class clsUser : BaseEntity
    {
        public int UsuarioId { get; set; }
        public int UsuarioIdv1 { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public Int32 Postulante { get; set; }
        public string NombreCompleto
        {
            get
            {
                return Nombres + " " + Apellidos;
            }
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Imagen { get; set; }
        public bool Estado { get; set; }
        //       public Int32 CompanyId { get; set; }
        public clsTypeUser tipousuario;

        public clsTypeUser TipoUsuario
        {
            get
            {
                if (tipousuario == null)
                {
                    tipousuario = new clsTypeUser();
                } return tipousuario;
            }
            set { tipousuario = value; }
        }

        public clsCompanyUser empresausuario;

        public clsCompanyUser EmpresaUsuario
        {
            get
            {
                if (empresausuario == null)
                {
                    empresausuario = new clsCompanyUser();
                } return empresausuario;
            }
            set
            {
                empresausuario = value;
            }
        }

        public clsCompany empresa;

        public clsCompany Empresa
        {
            get
            {
                if (empresa == null)
                {
                    empresa = new clsCompany();
                }
                return empresa;
            }
            set { empresa = value; }
        }

        public clsEmployee empleado;
        public clsEmployee Empleado
        {
            get
            {
                if (empleado == null)
                {
                    empleado = new clsEmployee();
                }
                return empleado;
            }
            set { empleado = value; }
        }
        /*Usuarios*/
        private clsJustification justificacion;

        public clsJustification Justification
        {
            get { return justificacion; }
            set { justificacion = value; }
        }

        private clsMemorandums memorandum;
        public clsMemorandums Memorandum
        {
            get
            {
                if (memorandum == null)
                {
                    memorandum = new clsMemorandums();
                } return memorandum;

            }
            set { memorandum = value; }
        }
    }
}
