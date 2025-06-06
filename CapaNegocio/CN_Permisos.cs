using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Permisos
    {
        private CD_Permisos objCapaDatoPermiso = new CD_Permisos();
        private CD_RolPermiso objCapaDatoRolPermiso = new CD_RolPermiso();

        public List<Permisos> Listar()
        {
            return objCapaDatoPermiso.ListarPermisos();
        }

        /// <summary>
        /// Obtiene una lista de objetos Permisos que están asignados a un rol específico.
        /// </summary>
        /// <param name="idRol">El ID del rol.</param>
        /// <returns>Una lista de objetos Permisos.</returns>
        public List<Permisos> ObtenerPermisosPorIdRol(int idRol)
        {
            List<int> idsPermisosAsignados = objCapaDatoRolPermiso.ObtenerPermisosPorRol(idRol);

            List<Permisos> todosLosPermisos = objCapaDatoPermiso.ListarPermisos(); 

            List<Permisos> permisosDelRol = todosLosPermisos
                                            .Where(p => idsPermisosAsignados.Contains(p.id_permiso))
                                            .ToList();

            return permisosDelRol;
        }

        public int Registrar(Permisos obj, out string Mensaje)
        {
            Mensaje = string.Empty; 


            if (string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre del permiso no puede estar vacío.";
                return 0; 
            }

            
            return objCapaDatoPermiso.Registrar(obj, out Mensaje);
        }

        public bool Editar(Permisos obj, out string Mensaje)
        {
            Mensaje = string.Empty; 


            

            if (string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre del permiso no puede estar vacío.";
                return false;
            }

            return objCapaDatoPermiso.Editar(obj, out Mensaje);
        }

        public bool Eliminar(int id_permiso, out string Mensaje)
        {
            Mensaje = string.Empty; 


            

            return objCapaDatoPermiso.Eliminar(id_permiso, out Mensaje);
        }
    }
}
