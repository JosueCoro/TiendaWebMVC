using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CapaEntidad;
using System.Globalization;


namespace CapaDatos
{
    public class CD_Compra
    {
        public int RegistrarCompra(Compra oCompra, out string mensaje)
        {
            int idCompraGenerada = 0; 
            mensaje = string.Empty; 

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.RegistrarCompra", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure; 

                    cmd.Parameters.AddWithValue("@USUARIO_id_usuario", oCompra.USUARIO_id_usuario);
                    cmd.Parameters.AddWithValue("@PROVEEDOR_id_proveedor", oCompra.PROVEEDOR_id_proveedor);
                    cmd.Parameters.AddWithValue("@TIPO_PAGO_id_tipo_pago", oCompra.TIPO_PAGO_id_tipo_pago);
                    cmd.Parameters.AddWithValue("@monto_total", oCompra.monto_total);
                    cmd.Parameters.AddWithValue("@TIENDA_id_tienda", oCompra.TIENDA_id_tienda);

                    DataTable dtDetalleCompra = new DataTable();
                    dtDetalleCompra.Columns.Add("PRODUCTO_id_producto", typeof(int));
                    dtDetalleCompra.Columns.Add("cantidad", typeof(int));
                    dtDetalleCompra.Columns.Add("precio_compra", typeof(decimal));
                    dtDetalleCompra.Columns.Add("sub_total", typeof(decimal));

                    foreach (Compra.DetalleCompra detalle in oCompra.oDetalleCompra)
                    {
                        dtDetalleCompra.Rows.Add(
                            detalle.PRODUCTO_id_producto,
                            detalle.cantidad,
                            detalle.precio_compra,
                            detalle.sub_total
                        );
                    }

                    SqlParameter paramDetalleCompra = new SqlParameter();
                    paramDetalleCompra.ParameterName = "@DetalleCompra"; 
                    paramDetalleCompra.SqlDbType = SqlDbType.Structured; 
                    paramDetalleCompra.TypeName = "DetalleCompraTipo"; 
                    paramDetalleCompra.Value = dtDetalleCompra; 
                    cmd.Parameters.Add(paramDetalleCompra);

                    SqlParameter parMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500);
                    parMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parMensaje);

                    SqlParameter parResultado = new SqlParameter("@Resultado", SqlDbType.Int);
                    parResultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResultado);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idCompraGenerada = Convert.ToInt32(parResultado.Value);
                    mensaje = parMensaje.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idCompraGenerada = 0;
                mensaje = "Error en la capa de datos al registrar la compra: " + ex.Message;
                Console.WriteLine(mensaje);
            }

            return idCompraGenerada; 
        }
    }
}
