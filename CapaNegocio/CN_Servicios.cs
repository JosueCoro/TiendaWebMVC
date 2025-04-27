using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Servicios
    {
        private CD_Servicios objServicios = new CD_Servicios();

        public List<Servicio> Listar()
        {
            return objServicios.ListarServicios();
        }

        public int Registrar(Servicio obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.precio <= 0)
            {
                Mensaje = "El precio del servicio no puede ser menor o igual a cero.";
                return 0;
            }
            else if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre del servicio no puede estar vacío.";
                return 0;
            }
            return objServicios.Registrar(obj, out Mensaje);
            

        }
        public bool Editar(Servicio obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.precio <= 0)
            {
                Mensaje = "El precio del servicio no puede ser menor o igual a cero.";
                return false;
            }
            else if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre del servicio no puede estar vacío.";
                return false;
            }
            else         
            return objServicios.Editar(obj, out Mensaje);
            
        }
        public bool Eliminar(int id_servicio, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (!objServicios.Eliminar(id_servicio, out Mensaje))
            {
                Mensaje = "El servicio está relacionado con uno o más ventas. No se puede eliminar.";
                return false;
            }
            return true;
        }
    }
}
