using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;
namespace CapaNegocio
{
    public class CN_UnidadMedida
    {
        private CD_UnidadMedida objUnidadMedida = new CD_UnidadMedida();
        public List<UnidadMedida> Listar()
        {
            return objUnidadMedida.Listar();
        }
        public int Registrar(UnidadMedida obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción de la unidad de medida no puede ser vacía.";
                return 0;
            }
            return objUnidadMedida.Registrar(obj, out Mensaje);
        }
        public bool Editar(UnidadMedida obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción de la unidad de medida no puede ser vacía.";
                return false;
            }
            return objUnidadMedida.Editar(obj, out Mensaje);
        }
        public bool Eliminar(int id_unidad_medida, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (objUnidadMedida.Eliminar(id_unidad_medida, out Mensaje) == false)
            {
                Mensaje = "La unidad de medida se encuentra relacionada con uno o más productos. No se puede eliminar.";
                return false;
            }
            return true;
        }
    }
}
