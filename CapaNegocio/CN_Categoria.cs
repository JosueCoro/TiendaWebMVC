using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCategoria = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objCategoria.ListarCategorias();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción de la categoría no puede ser vacía.";
                return 0;
            }
            return objCategoria.Registrar(obj, out Mensaje);
        }
        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción de la categoría no puede ser vacía.";
                return false;
            }
            return objCategoria.Editar(obj, out Mensaje);
        }
        public bool Eliminar(int id_categoria, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (objCategoria.Eliminar(id_categoria, out Mensaje) == false)
            {
                Mensaje = "La categoría se encuentra relacionada con uno o más productos. No se puede eliminar.";
                return false;
            }
            return true;
        }
    }
}
