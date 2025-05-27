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

        public DashBoard VerDashBoard()
        {
            return objCapaDato.VerDashBoard();
        }
        public DashboardResumen ObtenerDatosDashboardResumen()
        {
            return objCapaDato.ObtenerDatosDashboardResumen();
        }
        // Nuevo método de negocio para obtener el historial detallado de ventas
        public List<ReporteVentaDetalle> ObtenerHistorialVentas(string fechaInicio, string fechaFin, string idVenta)
        {
            // Aquí podrías añadir validaciones de negocio adicionales si fueran necesarias
            return objCapaDato.ObtenerHistorialVentas(fechaInicio, fechaFin, idVenta);
        }
    }
}
