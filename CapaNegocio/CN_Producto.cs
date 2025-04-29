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

        public List<Producto> Listar()
        {
            return objProducto.Listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrEmpty(obj.nombre)) 
            {
                Mensaje = "El nombre del Producto no puede ser vacio";
            }
            if (obj.precio <= 0)
            {
                Mensaje = "El precio del Producto no puede ser menor o igual a cero";
            }
            if (obj.oMarca.id_marca == 0)
            {
                Mensaje = "Debe seleccionar una marca";
            }
            if (obj.oCategoria.id_categoria == 0)
            {
                Mensaje = "Debe seleccionar una categoria";
            }
            if (obj.oUnidadMedida.id_unidad_medida == 0)
            {
                Mensaje = "Debe seleccionar una unidad de medida";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {


                return objProducto.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }

        }
        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrEmpty(obj.nombre))
            {
                Mensaje = "El nombre del Producto no puede ser vacio";
            }
            if (obj.precio <= 0)
            {
                Mensaje = "El precio del Producto no puede ser menor o igual a cero";
            }
            if (obj.oMarca.id_marca == 0)
            {
                Mensaje = "Debe seleccionar una marca";
            }
            if (obj.oCategoria.id_categoria == 0)
            {
                Mensaje = "Debe seleccionar una categoria";
            }
            if (obj.oUnidadMedida.id_unidad_medida == 0)
            {
                Mensaje = "Debe seleccionar una unidad de medida";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objProducto.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
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
