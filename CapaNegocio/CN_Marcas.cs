using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using CapaDatos;
namespace CapaNegocio
{
    public class CN_Marcas
    {
        private CD_Marca objMarca = new CD_Marca();

        public List<Marca> Listar()
        {
            return objMarca.ListarMarcas();
        }

        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción de la marca no puede estar vacía.";
                return 0;
            }

            return objMarca.Registrar(obj, out Mensaje);
        }

        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción de la marca no puede estar vacía.";
                return false;
            }

            return objMarca.Editar(obj, out Mensaje);
        }

        public bool Eliminar(int id_marca, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (!objMarca.Eliminar(id_marca, out Mensaje))
            {
                Mensaje = "La marca está relacionada con uno o más productos. No se puede eliminar.";
                return false;
            }

            return true;
        }
    }
}
