using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objCdVenta = new CD_Venta();

        public int Registrar(Venta oVenta, int idUsuarioLogeado, int idTienda, out string Mensaje)
        {
            Mensaje = string.Empty; 


            if (oVenta.CLIENTE_id_cliente == 0)
            {
                Mensaje = "Debe seleccionar un cliente.";
            }
            else if (oVenta.TIPO_PAGO_id_tipo_pago == 0)
            {
                Mensaje = "Debe seleccionar un tipo de pago.";
            }
            //else if (oVenta.USUARIO_id_usuario == 0)
            //{
            //    Mensaje = "El ID de usuario que registra la venta no es válido.";
            //}
            //else if (oVenta.TIENDA_id_tienda == 0)
            //{
            //    Mensaje = "Debe seleccionar una tienda para la venta.";
            //}
            else if (oVenta.monto_total <= 0)
            {
                Mensaje = "El monto total de la venta debe ser mayor a cero.";
            }
            else if (oVenta.oDetalleVenta == null || oVenta.oDetalleVenta.Count == 0)
            {
                Mensaje = "Debe agregar al menos un producto o servicio al detalle de la venta.";
            }
            else
            {
                foreach (Venta.DetalleVenta detalle in oVenta.oDetalleVenta)
                {
                    if (detalle.cantidad <= 0)
                    {
                        Mensaje = "La cantidad para cada ítem en el detalle debe ser mayor a cero.";
                        break; 
                    }
                    if (detalle.precio <= 0)
                    {
                        Mensaje = "El precio para cada ítem en el detalle debe ser mayor a cero.";
                        break; 
                    }
                    if (string.IsNullOrEmpty(detalle.tipo_item) || (detalle.tipo_item != "PRODUCTO" && detalle.tipo_item != "SERVICIO"))
                    {
                        Mensaje = "El tipo de ítem para cada detalle debe ser 'PRODUCTO' o 'SERVICIO'.";
                        break;
                    }

                    if (detalle.tipo_item == "PRODUCTO")
                    {
                        if (!detalle.PRODUCTO_id_producto.HasValue || detalle.PRODUCTO_id_producto.Value == 0)
                        {
                            Mensaje = "Un ítem de tipo 'PRODUCTO' debe tener un ID de producto válido.";
                            break;
                        }
                        if (detalle.SERVICIO_id_servicio.HasValue && detalle.SERVICIO_id_servicio.Value != 0)
                        {
                            Mensaje = "Un ítem de tipo 'PRODUCTO' no debe tener un ID de servicio.";
                            break;
                        }
                    }
                    else if (detalle.tipo_item == "SERVICIO")
                    {
                        if (!detalle.SERVICIO_id_servicio.HasValue || detalle.SERVICIO_id_servicio.Value == 0)
                        {
                            Mensaje = "Un ítem de tipo 'SERVICIO' debe tener un ID de servicio válido.";
                            break;
                        }
                        if (detalle.PRODUCTO_id_producto.HasValue && detalle.PRODUCTO_id_producto.Value != 0)
                        {
                            Mensaje = "Un ítem de tipo 'SERVICIO' no debe tener un ID de producto.";
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                oVenta.USUARIO_id_usuario = idUsuarioLogeado;
                oVenta.TIENDA_id_tienda = idTienda;
                return objCdVenta.RegistrarVenta(oVenta, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public Venta ObtenerVenta(int idVenta, out string Mensaje)
        {
            Mensaje = string.Empty; 
            Venta oVenta = null;

            if (idVenta <= 0)
            {
                Mensaje = "ID de venta no válido.";
                return null;
            }

            oVenta = objCdVenta.ObtenerVentaCompleta(idVenta, out Mensaje);

            return oVenta;
        }
    }
}
