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
    public class CD_Stock
    {
        public List<Stock> ListarStock()
        {
            List<Stock> lista = new List<Stock>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT P.id_stock, P.cantidad, P.stock_minimo, P.fecha_actualizacion,");
                    sb.AppendLine("S.id_producto, S.nombre AS producto_nombre,");
                    sb.AppendLine("T.id_tienda, T.nombre AS nombre_tienda");
                    sb.AppendLine("FROM INVENTARIO.STOCK P");
                    sb.AppendLine("LEFT JOIN INVENTARIO.PRODUCTO S ON S.id_producto = P.PRODUCTO_id_producto");
                    sb.AppendLine("LEFT JOIN INVENTARIO.TIENDA T ON T.id_tienda = P.TIENDA_id_tienda");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Stock()
                            {
                                id_stock = Convert.ToInt32(dr["id_stock"]),
                                cantidad = Convert.ToInt32(dr["cantidad"]),
                                stock_minimo = Convert.ToInt32(dr["stock_minimo"]),
                                fecha_actualizacion = Convert.ToDateTime(dr["fecha_actualizacion"]),
                                oProducto = new Producto()
                                {
                                    id_producto = Convert.ToInt32(dr["id_producto"]),
                                    nombre = dr["producto_nombre"].ToString()
                                },
                                oTienda = new Tienda()  
                                {
                                    id_tienda = Convert.ToInt32(dr["id_tienda"]),
                                    nombre = dr["nombre_tienda"].ToString()
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista = new List<Stock>();
            }

            return lista;
        }
        public int Registrar(Stock obj, out string mensaje)
        {
            int resultado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.sp_RegistrarStock", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cantidad", obj.cantidad);
                    cmd.Parameters.AddWithValue("@stock_minimo", obj.stock_minimo);
                    cmd.Parameters.AddWithValue("@fecha_actualizacion", obj.fecha_actualizacion);
                    cmd.Parameters.AddWithValue("@PRODUCTO_id_producto", obj.PRODUCTO_id_producto);
                    cmd.Parameters.AddWithValue("@TIENDA_id_tienda", obj.TIENDA_id_tienda);

                    SqlParameter parMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500);
                    parMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parMensaje);

                    SqlParameter parResultado = new SqlParameter("@Resultado", SqlDbType.Int);
                    parResultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResultado);

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(parResultado.Value);
                    mensaje = parMensaje.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(Stock obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.sp_EditarStock", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_stock", obj.id_stock);
                    cmd.Parameters.AddWithValue("@cantidad", obj.cantidad);
                    cmd.Parameters.AddWithValue("@stock_minimo", obj.stock_minimo);
                    cmd.Parameters.AddWithValue("@fecha_actualizacion", obj.fecha_actualizacion);
                    cmd.Parameters.AddWithValue("@PRODUCTO_id_producto", obj.PRODUCTO_id_producto);
                    cmd.Parameters.AddWithValue("@TIENDA_id_tienda", obj.TIENDA_id_tienda);

                    SqlParameter parMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500);
                    parMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parMensaje);

                    SqlParameter parResultado = new SqlParameter("@Resultado", SqlDbType.Bit);
                    parResultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parResultado);

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(parResultado.Value);
                    mensaje = parMensaje.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

    }
}
