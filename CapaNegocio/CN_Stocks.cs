using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;


namespace CapaNegocio
{
    public class CN_Stocks
    {
        private CD_Stock objStock = new CD_Stock();
        public List<Stock> Listar()
        {
            return objStock.ListarStock();
        }
        public int Registrar(Stock obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.cantidad <= 0)
            {
                Mensaje = "La cantidad no puede ser menor o igual a cero";
            }
            if (obj.stock_minimo < 0)
            {
                Mensaje = "El stock mínimo no puede ser negativo";
            }
            if (obj.fecha_actualizacion > DateTime.Now)
            {
                Mensaje = "La fecha de actualización no puede ser futura";
            }
            if (obj.PRODUCTO_id_producto == 0)
            {
                Mensaje = "Debe seleccionar un producto";
            }
            if (obj.TIENDA_id_tienda == 0)
            {
                Mensaje = "Debe seleccionar una tienda";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objStock.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Stock obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.cantidad <= 0)
            {
                Mensaje = "La cantidad no puede ser menor o igual a cero";
            }
            if (obj.stock_minimo < 0)
            {
                Mensaje = "El stock mínimo no puede ser negativo";
            }
            if (obj.fecha_actualizacion > DateTime.Now)
            {
                Mensaje = "La fecha de actualización no puede ser futura";
            }
            if (obj.PRODUCTO_id_producto == 0)
            {
                Mensaje = "Debe seleccionar un producto";
            }
            if (obj.TIENDA_id_tienda == 0)
            {
                Mensaje = "Debe seleccionar una tienda";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objStock.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }
    }
}
