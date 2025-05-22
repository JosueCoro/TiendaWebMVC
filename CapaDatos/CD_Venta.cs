using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace CapaDatos
{
    public class CD_Venta
    {
        public int RegistrarVenta(Venta oVenta, out string mensaje)
        {
            int idVentaGenerada = 0; 
            mensaje = string.Empty; 

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.RegistrarVenta", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure; 

                    cmd.Parameters.AddWithValue("@CLIENTE_id_cliente", oVenta.CLIENTE_id_cliente);
                    cmd.Parameters.AddWithValue("@TIPO_PAGO_id_tipo_pago", oVenta.TIPO_PAGO_id_tipo_pago);
                    cmd.Parameters.AddWithValue("@USUARIO_id_usuario", oVenta.USUARIO_id_usuario);
                    cmd.Parameters.AddWithValue("@monto_total", oVenta.monto_total);
                    cmd.Parameters.AddWithValue("@TIENDA_id_tienda", oVenta.TIENDA_id_tienda); 

                    DataTable dtDetalleVenta = new DataTable();
                    dtDetalleVenta.Columns.Add("PRODUCTO_id_producto", typeof(int));
                    dtDetalleVenta.Columns.Add("SERVICIO_id_servicio", typeof(int));
                    dtDetalleVenta.Columns.Add("cantidad", typeof(int));
                    dtDetalleVenta.Columns.Add("precio", typeof(decimal));
                    dtDetalleVenta.Columns.Add("sub_total", typeof(decimal));
                    dtDetalleVenta.Columns.Add("tipo_item", typeof(string));

                    foreach (Venta.DetalleVenta detalle in oVenta.oDetalleVenta)
                    {
                        dtDetalleVenta.Rows.Add(
                            detalle.PRODUCTO_id_producto.HasValue ? (object)detalle.PRODUCTO_id_producto.Value : DBNull.Value, 
                            detalle.SERVICIO_id_servicio.HasValue ? (object)detalle.SERVICIO_id_servicio.Value : DBNull.Value,
                            detalle.cantidad,
                            detalle.precio,
                            detalle.sub_total,
                            detalle.tipo_item
                        );
                    }

                    SqlParameter paramDetalleVenta = new SqlParameter();
                    paramDetalleVenta.ParameterName = "@DetalleVenta"; 
                    paramDetalleVenta.SqlDbType = SqlDbType.Structured; 
                    paramDetalleVenta.TypeName = "DetalleVentaTipo"; 
                    paramDetalleVenta.Value = dtDetalleVenta; 
                    cmd.Parameters.Add(paramDetalleVenta);

                    SqlParameter parMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500);
                    parMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parMensaje);

                    SqlParameter parResultado = new SqlParameter("@Resultado", SqlDbType.Int);
                    parResultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResultado);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idVentaGenerada = Convert.ToInt32(parResultado.Value);
                    mensaje = parMensaje.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idVentaGenerada = 0;
                mensaje = "Error en la capa de datos al registrar la venta: " + ex.Message;
                Console.WriteLine(mensaje);
            }

            return idVentaGenerada; // Devolvemos el ID de la venta generada o 0 si hubo un error
        }

        public Venta ObtenerVentaCompleta(int idVenta, out string mensaje)
        {
            Venta oVenta = null;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT");
                    query.AppendLine("    V.id_venta, V.fecha, V.monto_total,");
                    query.AppendLine("    C.id_cliente, C.nombres AS ClienteNombres, C.apellidos AS ClienteApellidos, C.telefono AS ClienteTelefono, C.correo AS ClienteCorreo,");
                    query.AppendLine("    TP.id_tipo_pago, TP.descripcion AS TipoPagoDescripcion,");
                    query.AppendLine("    U.id_usuario, U.nombres AS UsuarioNombres, U.apellidos AS UsuarioApellidos,"); 
                    query.AppendLine("    DV.id_detalle_venta, DV.cantidad, DV.precio, DV.sub_total, DV.tipo_item,");
                    query.AppendLine("    P.id_producto AS ProductoId, P.nombre AS ProductoNombre, P.descripcion AS ProductoDescripcion, P.precio AS ProductoPrecio,"); 
                    query.AppendLine("    S.id_servicio AS ServicioId, S.nombre AS ServicioNombre, S.descripcion AS ServicioDescripcion, S.precio AS ServicioPrecio"); 
                    query.AppendLine("FROM TRANSACCIONES.VENTA V");
                    query.AppendLine("INNER JOIN COMERCIAL.CLIENTE C ON V.CLIENTE_id_cliente = C.id_cliente");
                    query.AppendLine("INNER JOIN TRANSACCIONES.TIPO_PAGO TP ON V.TIPO_PAGO_id_tipo_pago = TP.id_tipo_pago");
                    query.AppendLine("INNER JOIN SEGURIDAD.USUARIO U ON V.USUARIO_id_usuario = U.id_usuario"); 
                    query.AppendLine("INNER JOIN TRANSACCIONES.DETALLE_VENTA DV ON V.id_venta = DV.VENTA_id_venta");
                    query.AppendLine("LEFT JOIN INVENTARIO.PRODUCTO P ON DV.PRODUCTO_id_producto = P.id_producto");
                    query.AppendLine("LEFT JOIN TRANSACCIONES.SERVICIO S ON DV.SERVICIO_id_servicio = S.id_servicio");
                    query.AppendLine("WHERE V.id_venta = @idVenta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);
                    cmd.CommandType = CommandType.Text; 

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        bool encabezadoLeido = false;

                        while (dr.Read())
                        {
                            if (!encabezadoLeido)
                            {
                                oVenta = new Venta()
                                {
                                    id_venta = Convert.ToInt32(dr["id_venta"]),
                                    fecha = Convert.ToDateTime(dr["fecha"]),
                                    monto_total = Convert.ToDecimal(dr["monto_total"]),
                                    CLIENTE_id_cliente = Convert.ToInt32(dr["id_cliente"]),
                                    TIPO_PAGO_id_tipo_pago = Convert.ToInt32(dr["id_tipo_pago"]),
                                    USUARIO_id_usuario = Convert.ToInt32(dr["id_usuario"]),

                                    oCliente = new Cliente()
                                    {
                                        id_cliente = Convert.ToInt32(dr["id_cliente"]),
                                        nombres = dr["ClienteNombres"].ToString(),
                                        apellidos = dr["ClienteApellidos"].ToString(),
                                        telefono = dr["ClienteTelefono"].ToString(),
                                        correo = dr["ClienteCorreo"].ToString()
                                    },
                                    oTipoPago = new TipoPago()
                                    {
                                        id_tipo_pago = Convert.ToInt32(dr["id_tipo_pago"]),
                                        descripcion = dr["TipoPagoDescripcion"].ToString()
                                    },
                                    oUsuario = new Usuario()
                                    {
                                        id_usuario = Convert.ToInt32(dr["id_usuario"]),
                                        nombres = dr["UsuarioNombres"].ToString(),
                                        apellidos = dr["UsuarioApellidos"].ToString()
                                    },
                                    oDetalleVenta = new List<Venta.DetalleVenta>() 
                                };
                                encabezadoLeido = true;
                            }

                            Venta.DetalleVenta oDetalle = new Venta.DetalleVenta()
                            {
                                id_detalle_venta = Convert.ToInt32(dr["id_detalle_venta"]),
                                cantidad = Convert.ToInt32(dr["cantidad"]),
                                precio = Convert.ToDecimal(dr["precio"]), 
                                sub_total = Convert.ToDecimal(dr["sub_total"]),
                                tipo_item = dr["tipo_item"].ToString()
                            };

                            if (oDetalle.tipo_item == "PRODUCTO")
                            {
                                oDetalle.PRODUCTO_id_producto = Convert.ToInt32(dr["ProductoId"]);
                                oDetalle.oProducto = new Producto()
                                {
                                    id_producto = Convert.ToInt32(dr["ProductoId"]),
                                    nombre = dr["ProductoNombre"].ToString(),
                                    descripcion = dr["ProductoDescripcion"].ToString(),
                                    precio = Convert.ToDecimal(dr["ProductoPrecio"]) 
                                };
                            }
                            else if (oDetalle.tipo_item == "SERVICIO")
                            {
                                oDetalle.SERVICIO_id_servicio = Convert.ToInt32(dr["ServicioId"]);
                                oDetalle.oServicio = new Servicio()
                                {
                                    id_servicio = Convert.ToInt32(dr["ServicioId"]),
                                    nombre = dr["ServicioNombre"].ToString(),
                                    descripcion = dr["ServicioDescripcion"].ToString(),
                                    precio = Convert.ToDecimal(dr["ServicioPrecio"]) 
                                };
                            }

                            oVenta.oDetalleVenta.Add(oDetalle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oVenta = null;
                mensaje = "Error en la capa de datos al obtener la venta completa: " + ex.Message;
                Console.WriteLine(mensaje);
            }

            return oVenta;
        }
    }
}
