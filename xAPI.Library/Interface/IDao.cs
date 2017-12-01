using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using xAPI.Library.Base;

namespace xAPI.Library
{
    /// <summary>
    /// Clase Interfaz para las Funciones Basicas de Todas las clases de la Capa de Acceso a Dato
    /// </summary>
    public interface IDao
    {
        bool Insert(ref BaseEntity Base, ref IEntity entity);
        bool Update(ref BaseEntity Base, IEntity entity);
        bool Delete(ref BaseEntity Base, IEntity entity);
        DataTable GetAll(ref BaseEntity Base);
        IEntity GetByID(ref BaseEntity Base, Int32 id);
    }
}
