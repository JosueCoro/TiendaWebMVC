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
    public class CN_Compra
    {
        private CD_Compra objCdCompra = new CD_Compra();

        public int Registrar(Compra oCompra, out string Mensaje)
        {
            Mensaje = string.Empty; 


            if (oCompra.PROVEEDOR_id_proveedor == 0)
            {
                Mensaje = "Debe seleccionar un proveedor.";
            }
            else if (oCompra.TIPO_PAGO_id_tipo_pago == 0)
            {
                Mensaje = "Debe seleccionar un tipo de pago.";
            }
            else if (oCompra.TIENDA_id_tienda == 0)
            {
                Mensaje = "Debe seleccionar una tienda para la compra.";
            }
            else if (oCompra.monto_total <= 0)
            {
                Mensaje = "El monto total de la compra debe ser mayor a cero.";
            }
            else if (oCompra.oDetalleCompra == null || oCompra.oDetalleCompra.Count == 0)
            {
                Mensaje = "Debe agregar al menos un producto al detalle de la compra.";
            }
            else if (oCompra.oDetalleCompra.Any(d => d.cantidad <= 0))
            {
                Mensaje = "La cantidad de cada producto debe ser mayor a cero.";
            }
            else if (oCompra.oDetalleCompra.Any(d => d.precio_compra <= 0))
            {
                Mensaje = "El precio de compra de cada producto debe ser mayor a cero.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCdCompra.RegistrarCompra(oCompra, out Mensaje);
            }
            else
            {
                return 0;
            }
        }
    }
}
