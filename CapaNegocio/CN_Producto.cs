using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
using System.Data;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objProducto = new CD_Producto();

        //public List<Producto> Listar()
        //{
        //    return objProducto.Listar();
        //}

        public List<Producto> Listar() => objProducto.Listar();
        public List<Producto> ObtenerProductoPorId(int id_producto = 0)
        {
            return objProducto.ObtenerProductoPorId(id_producto);
        }
        public List<Producto> ListarProductos(int idMarca = 0, int idCategoria = 0)
        {
            return objProducto.ListarProductosFiltro(idMarca, idCategoria);
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombre)) Mensaje = "El nombre del Producto no puede ser vacio";
            else if (string.IsNullOrEmpty(obj.codigo_barra)) Mensaje = "El código de barras es obligatorio"; // Validación opcional
            else if (obj.precio <= 0) Mensaje = "El precio del Producto no puede ser menor o igual a cero";
            else if (obj.MARCA_id_marca == 0) Mensaje = "Debe seleccionar una marca";
            else if (obj.CATEGORIA_id_categoria == 0) Mensaje = "Debe seleccionar una categoría";
            else if (obj.UNIDAD_MEDIDA_id_unidad_medida == 0) Mensaje = "Debe seleccionar una unidad de medida";

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objProducto.Registrar(obj, out Mensaje);
            }
            return 0;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombre)) Mensaje = "El nombre del Producto no puede ser vacio";
            else if (string.IsNullOrEmpty(obj.codigo_barra)) Mensaje = "El código de barras es obligatorio";
            else if (obj.precio <= 0) Mensaje = "El precio del Producto no puede ser menor o igual a cero";
            else if (obj.MARCA_id_marca == 0) Mensaje = "Debe seleccionar una marca";
            else if (obj.CATEGORIA_id_categoria == 0) Mensaje = "Debe seleccionar una categoria";
            else if (obj.UNIDAD_MEDIDA_id_unidad_medida == 0) Mensaje = "Debe seleccionar una unidad de medida";

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objProducto.Editar(obj, out Mensaje);
            }
            return false;
        }

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            return objProducto.GuardarDatosImagen(oProducto, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objProducto.Eliminar(id, out Mensaje);
        }
    }
}
