using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CapaEntidad;

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
    }
}
