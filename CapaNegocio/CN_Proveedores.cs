using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Proveedores
    {
        private CD_Proveedor objProveedor = new CD_Proveedor();

        public List<Proveedor> Listar()
        {
            return objProveedor.ListarProveedores();
        }

        public int Registrar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre del proveedor no puede estar vacío.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El teléfono del proveedor no puede estar vacío.";
                return 0;
            }

            return objProveedor.Registrar(obj, out Mensaje);
        }

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre del proveedor no puede estar vacío.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El teléfono del proveedor no puede estar vacío.";
                return false;
            }

            return objProveedor.Editar(obj, out Mensaje);
        }

        public bool Eliminar(int id_proveedor, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (!objProveedor.Eliminar(id_proveedor, out Mensaje))
            {
                Mensaje = "El proveedor está relacionado con una o más compras. No se puede eliminar.";
                return false;
            }

            return true;
        }
    }
}
