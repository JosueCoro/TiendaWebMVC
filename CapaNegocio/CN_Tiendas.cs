using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Tiendas
    {
        private CD_Tienda objTienda = new CD_Tienda();

        public List<Tienda> Listar()
        {
            return objTienda.ListarTiendas();
        }

        public int Registrar(Tienda obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre de la tienda no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.dirrecion) || string.IsNullOrWhiteSpace(obj.dirrecion))
            {
                Mensaje = "La dirección de la tienda no puede ser vacía";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El telefono de la tienda no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objTienda.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Tienda obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre de la tienda no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.dirrecion) || string.IsNullOrWhiteSpace(obj.dirrecion))
            {
                Mensaje = "La dirección de la tienda no puede ser vacía";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El telefono de la tienda no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objTienda.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id_tienda, out string Mensaje)
        {
            Mensaje = string.Empty;

            bool eliminado = objTienda.Eliminar(id_tienda, out Mensaje);
            if (!eliminado)
            {
                Mensaje = "La tienda no se pudo eliminar, porque ya cuenta con productos"; 
            }
            return eliminado;
        }
    }
}
