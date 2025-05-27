using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DashboardResumen
    {
        public int TotalCliente { get; set; }
        public int TotalVenta { get; set; } // Esto será el CONTEO de ventas
        public decimal MontoTotalVentasHoy { get; set; } // Nuevo: Suma de montos de ventas de hoy
        public decimal MontoTotalVentasMes { get; set; } // Nuevo: Suma de montos de ventas del mes
        public int TotalCompra { get; set; }
        public int TotalProducto { get; set; }
        public int TotalServicio { get; set; } // Nuevo: Conteo de servicios (si aplica)
        public int TotalProveedor { get; set; } // Nuevo: Conteo de proveedores (si aplica)
    }
}
