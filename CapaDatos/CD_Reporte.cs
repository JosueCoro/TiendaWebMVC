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
        
        public DashboardResumen ObtenerDatosDashboardResumen()
        {
            DashboardResumen oResumen = new DashboardResumen();
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_Reporte_DashboardResumen", oconexion); // Usaremos un SP
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TotalCliente", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalVenta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MontoTotalVentasHoy", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MontoTotalVentasMes", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalCompra", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalProducto", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalServicio", SqlDbType.Int).Direction = ParameterDirection.Output; 
                    cmd.Parameters.Add("@TotalProveedor", SqlDbType.Int).Direction = ParameterDirection.Output; 

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

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
                Console.WriteLine(mensaje); 
            }

            return oResumen;
        }

        public List<ReporteVentaDetalle> ObtenerHistorialVentas(string fechaInicio, string fechaFin, string idVenta)
        {
            List<ReporteVentaDetalle> lista = new List<ReporteVentaDetalle>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_Reporte_ObtenerHistorialVentas", oconexion); 
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
                lista = new List<ReporteVentaDetalle>(); 
                Console.WriteLine("Error al obtener historial de ventas: " + ex.Message); 
            }

            return lista;
        }
        public List<ReporteCompraDetalle> ObtenerHistorialCompras(string fechaInicio, string fechaFin, string idCompra)
        {
            List<ReporteCompraDetalle> lista = new List<ReporteCompraDetalle>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_Reporte_ObtenerHistorialCompras", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio); 
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin); 
                                                                            
                    cmd.Parameters.AddWithValue("@IdCompra", string.IsNullOrEmpty(idCompra) ? (object)DBNull.Value : Convert.ToInt32(idCompra));

                    oconexion.Open(); 

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            
                            lista.Add(new ReporteCompraDetalle()
                            {
                                idCompra = Convert.ToInt32(dr["idCompra"]),
                                fechaCompra = Convert.ToDateTime(dr["fechaCompra"]),
                                nombreUsuario = dr["nombreUsuario"].ToString(),
                                nombreProveedor = dr["nombreProveedor"].ToString(),
                                tipoPago = dr["tipoPago"].ToString(),
                                cantidad = Convert.ToInt32(dr["cantidad"]),
                                nombreProducto = dr["nombreProducto"].ToString(),
                                // Use CultureInfo.InvariantCulture for decimal conversions to avoid locale issues
                                precioCompraUnitario = Convert.ToDecimal(dr["precioCompraUnitario"], CultureInfo.InvariantCulture),
                                subTotalItem = Convert.ToDecimal(dr["subTotalItem"], CultureInfo.InvariantCulture),
                                montoTotalCompra = Convert.ToDecimal(dr["montoTotalCompra"], CultureInfo.InvariantCulture)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<ReporteCompraDetalle>();
                Console.WriteLine("Error al obtener historial de compras: " + ex.Message);
                
            }

            return lista;
        }
    }
}
