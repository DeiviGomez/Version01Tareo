using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Library.Base;

namespace xAPI.Entity
{
    public class clsDashboardShortCut : BaseEntity
    {
  

        public int id { get; set; }
        public String nombre { get; set; }
        public String icono { get; set; }
        public String url { get; set; }
        public String MenuId { get; set; }
        public int pagsecundaria { get; set; }


        public clsUser usuario;
        public clsUserPermissions varPermisosUsuario;
        public clsPermissions varPermisos;
        public clsTypeUser tipoUsuario;


        public clsTypeUser TipoUsuario
        {
            get
            {
                if (tipoUsuario == null) { tipoUsuario = new clsTypeUser(); }
                return tipoUsuario;
            }
            set { tipoUsuario = value; }
        }



        public clsUserPermissions PermisosUsuario
        {
            get
            {
                if (varPermisosUsuario == null) { varPermisosUsuario = new clsUserPermissions(); }
                return varPermisosUsuario;
            }
            set { varPermisosUsuario = value; }
        }




        public clsPermissions Permisos
        {
            get
            {
                if (varPermisos == null) { varPermisos = new clsPermissions(); }
                return varPermisos;
            }
            set { varPermisos = value; }
        }

    }
}
