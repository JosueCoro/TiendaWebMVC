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

        public List<Permisos> Listar()
        {
            return objCapaDatoPermiso.ListarPermisos();
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
