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

            bool eliminado = objRol.Eliminar(id_rol, out Mensaje);
            if (!eliminado)
            {
                Mensaje = "El rol se encuentra relacionado a un usuario. No se puede eliminar.";
            }
            return eliminado;
        }

        /// <summary>
        /// Obtiene un objeto Rol de la base de datos por su ID.
        /// </summary>
        /// <param name="idRol">El ID del rol a obtener.</param>
        /// <returns>Un objeto Rol si se encuentra, null en caso contrario.</returns>
        public Roles ObtenerPorId(int idRol)
        {
            if (idRol <= 0)
            {
                // Manejo de error o log si el ID es inválido
                Console.WriteLine("CN_Rol: ID de Rol inválido para ObtenerPorId.");
                return null;
            }
            return objRol.ObtenerPorId(idRol);
        }


    }
}
