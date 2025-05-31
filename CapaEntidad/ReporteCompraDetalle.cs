using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ReporteCompraDetalle
    {
        public int idCompra { get; set; }
        public DateTime fechaCompra { get; set; }
        public string nombreUsuario { get; set; }
        public string nombreProveedor { get; set; }
        public string tipoPago { get; set; }
        public int cantidad { get; set; }
        public string nombreProducto { get; set; }
        public decimal precioCompraUnitario { get; set; }
        public decimal subTotalItem { get; set; }
        public decimal montoTotalCompra { get; set; }
    }
}
