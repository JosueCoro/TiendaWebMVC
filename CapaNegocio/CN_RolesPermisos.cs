using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_RolesPermisos
    {
        // Instancia de la clase de acceso a datos para RolPermiso
        private CD_RolPermiso objCapaDato = new CD_RolPermiso();

        /// <summary>
        /// Obtiene los IDs de los permisos asignados a un rol específico.
        /// </summary>
        /// <param name="idRol">El ID del rol.</param>
        /// <returns>Una lista de IDs de permisos.</returns>
        public List<int> ObtenerPermisosPorRol(int idRol)
        {
            if (idRol <= 0)
            {
                Console.WriteLine("CN_RolesPermisos: ID de Rol inválido.");
                return new List<int>();
            }

            return objCapaDato.ObtenerPermisosPorRol(idRol);
        }

        /// <summary>
        /// Asigna un conjunto de permisos a un rol, gestionando las eliminaciones e inserciones.
        /// </summary>
        /// <param name="idRol">El ID del rol al que se asignarán los permisos.</param>
        /// <param name="idsPermisos">La lista de IDs de permisos a asignar.</param>
        /// <param name="mensaje">Parámetro de salida para mensajes de error o éxito.</param>
        /// <returns>Verdadero si la asignación fue exitosa, falso en caso contrario.</returns>
        public bool AsignarPermisosARol(int idRol, List<int> idsPermisos, out string mensaje)
        {
            mensaje = string.Empty; 

            if (idRol <= 0)
            {
                mensaje = "El ID del rol no es válido.";
                return false;
            }

            if (idsPermisos == null)
            {
                idsPermisos = new List<int>(); 
            }

            bool resultado = objCapaDato.AsignarPermisosARol(idRol, idsPermisos);

            if (resultado)
            {
                mensaje = "Permisos asignados y actualizados correctamente.";
            }
            else
            {
                mensaje = "Ocurrió un error al asignar los permisos al rol.";
            }

            return resultado;
        }
    }
}
