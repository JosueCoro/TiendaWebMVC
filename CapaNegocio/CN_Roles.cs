using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Roles
    {
        private CD_Roles objRol = new CD_Roles();

        public List<Roles> Listar()
        {
            return objRol.ListarRoles();
        }

        public int Registrar(Roles obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre del rol no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripcion del rol no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objRol.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Roles obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre del rol no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripcion del rol no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objRol.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id_rol, out string Mensaje)
        {
            Mensaje = string.Empty;
            //mostrar el mensaje "El rol se encuentra relacionado a un usuario. No se puede eliminar" si no se pudo elminar
            if (objRol.Eliminar(id_rol, out Mensaje) == false)
            {
                Mensaje = "El rol se encuentra relacionado a un usuario. No se puede eliminar";
                return false;
            }
            else
                return objRol.Eliminar(id_rol, out Mensaje);

        }


    }
}
