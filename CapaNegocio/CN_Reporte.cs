using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {

        private CD_Reporte objCapaDato = new CD_Reporte();


        public DashboardResumen ObtenerDatosDashboardResumen()
        {
            return objCapaDato.ObtenerDatosDashboardResumen();
        }



        // Nuevo método de negocio para obtener el historial detallado de ventas
        public List<ReporteVentaDetalle> ObtenerHistorialVentas(string fechaInicio, string fechaFin, string idVenta)
        {
            return objCapaDato.ObtenerHistorialVentas(fechaInicio, fechaFin, idVenta);
        }



        // nuevo método de negocio para obtener el historial detallado de compras
        public List<ReporteCompraDetalle> ObtenerHistorialCompras(string fechaInicio, string fechaFin, string idCompra)
        {
            return objCapaDato.ObtenerHistorialCompras(fechaInicio, fechaFin, idCompra);
        }
    }
}
