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
    public class CD_Reporte
    {
        /*CREATE PROC PA_REPORTEDASHBOARD
        AS

        BEGIN
        SELECT 

        --CANTIDAD DE CLIENTE
        (SELECT COUNT(*) FROM COMERCIAL.CLIENTE) [TotalCliente],
        --CANTIDAD DE COMPRAS
        (SELECT COUNT(*) FROM TRANSACCIONES.DETALLE_COMPRA)[TotalCompra],
        --CANTIDAD DE PRODUCTOS
        (SELECT COUNT(*) FROM INVENTARIO.PRODUCTO) [TotalProducto],
        --CANTIDAD DE VENTAS
        (SELECT COUNT(*) FROM TRANSACCIONES.DETALLE_VENTA) [TotalVenta]

        END*/
        public DashBoard VerDashBoard()
        {
            DashBoard objeto = new DashBoard();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("PA_REPORTEDASHBOARD", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader()) 
                    {
                        while (dr.Read())
                        {
                            objeto = new DashBoard(){
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalCompra = Convert.ToInt32(dr["TotalCompra"]),
                                TotalProducto = Convert.ToInt32(dr["TotalProducto"]),
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"])
                                
                            };

                        }
                    }

                    
                }
            }
            catch
            {
                objeto = new DashBoard();
            }

            return objeto;
        }
        public DashboardResumen ObtenerDatosDashboardResumen()
        {
            DashboardResumen oResumen = new DashboardResumen();
            string mensaje = string.Empty; // Para capturar mensajes de error si los hubiera

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_Reporte_DashboardResumen", oconexion); // Usaremos un SP
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de salida para los resultados
                    cmd.Parameters.Add("@TotalCliente", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalVenta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MontoTotalVentasHoy", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MontoTotalVentasMes", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalCompra", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalProducto", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalServicio", SqlDbType.Int).Direction = ParameterDirection.Output; // Nuevo
                    cmd.Parameters.Add("@TotalProveedor", SqlDbType.Int).Direction = ParameterDirection.Output; // Nuevo

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    // Leer los valores de los parámetros de salida
                    oResumen.TotalCliente = Convert.ToInt32(cmd.Parameters["@TotalCliente"].Value);
                    oResumen.TotalVenta = Convert.ToInt32(cmd.Parameters["@TotalVenta"].Value);
                    oResumen.MontoTotalVentasHoy = Convert.ToDecimal(cmd.Parameters["@MontoTotalVentasHoy"].Value, CultureInfo.InvariantCulture);
                    oResumen.MontoTotalVentasMes = Convert.ToDecimal(cmd.Parameters["@MontoTotalVentasMes"].Value, CultureInfo.InvariantCulture);
                    oResumen.TotalCompra = Convert.ToInt32(cmd.Parameters["@TotalCompra"].Value);
                    oResumen.TotalProducto = Convert.ToInt32(cmd.Parameters["@TotalProducto"].Value);
                    oResumen.TotalServicio = Convert.ToInt32(cmd.Parameters["@TotalServicio"].Value);
                    oResumen.TotalProveedor = Convert.ToInt32(cmd.Parameters["@TotalProveedor"].Value);
                }
            }
            catch (Exception ex)
            {
                // En caso de error, inicializar el objeto con valores predeterminados y capturar el mensaje
                oResumen = new DashboardResumen()
                {
                    TotalCliente = 0,
                    TotalVenta = 0,
                    MontoTotalVentasHoy = 0,
                    MontoTotalVentasMes = 0,
                    TotalCompra = 0,
                    TotalProducto = 0,
                    TotalServicio = 0,
                    TotalProveedor = 0
                };
                mensaje = "Error al obtener datos del dashboard: " + ex.Message;
                Console.WriteLine(mensaje); // Para depuración
            }

            return oResumen;
        }

        // Nuevo método para obtener el historial detallado de ventas
        public List<ReporteVentaDetalle> ObtenerHistorialVentas(string fechaInicio, string fechaFin, string idVenta)
        {
            List<ReporteVentaDetalle> lista = new List<ReporteVentaDetalle>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_Reporte_ObtenerHistorialVentas", oconexion); // Usaremos un nuevo SP
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@IdVenta", string.IsNullOrEmpty(idVenta) ? DBNull.Value : (object)Convert.ToInt32(idVenta));

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteVentaDetalle()
                            {
                                idVenta = Convert.ToInt32(dr["idVenta"]),
                                fechaVenta = Convert.ToDateTime(dr["fechaVenta"]),
                                nombreCliente = dr["nombreCliente"].ToString(),
                                tipoItem = dr["tipoItem"].ToString(),
                                nombreItem = dr["nombreItem"].ToString(),
                                precioUnitario = Convert.ToDecimal(dr["precioUnitario"], CultureInfo.InvariantCulture),
                                cantidad = Convert.ToInt32(dr["cantidad"]),
                                subTotal = Convert.ToDecimal(dr["subTotal"], CultureInfo.InvariantCulture)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<ReporteVentaDetalle>(); // Devolver lista vacía en caso de error
                Console.WriteLine("Error al obtener historial de ventas: " + ex.Message); // Para depuración
            }

            return lista;
        }
    }
}
