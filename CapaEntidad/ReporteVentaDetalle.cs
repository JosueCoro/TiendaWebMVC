using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ReporteVentaDetalle
    {
        public int idVenta { get; set; } // ID de la venta principal
        public DateTime fechaVenta { get; set; }
        public string nombreCliente { get; set; }
        public string tipoItem { get; set; } // "PRODUCTO" o "SERVICIO"
        public string nombreItem { get; set; } // Nombre del producto o servicio
        public decimal precioUnitario { get; set; }
        public int cantidad { get; set; }
        public decimal subTotal { get; set; } // Cantidad * PrecioUnitario
    }
}
