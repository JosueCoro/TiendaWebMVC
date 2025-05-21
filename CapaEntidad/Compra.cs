using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Compra
    {
        public int id_compra { get; set; }
        public DateTime fecha { get; set; }
        public decimal monto_total { get; set; }
        public int USUARIO_id_usuario { get; set; }
        public int PROVEEDOR_id_proveedor { get; set; }
        public int TIPO_PAGO_id_tipo_pago { get; set; }
        public int TIENDA_id_tienda { get; set; }

        public Usuario oUsuario { get; set; }
        public Proveedor oProveedor { get; set; }
        public TipoPago oTipoPago { get; set; }
        public Tienda oTienda { get; set; }


        public List<DetalleCompra> oDetalleCompra { get; set; }

        // Constructor para inicializar la lista de detalles
        public Compra()
        {
            oDetalleCompra = new List<DetalleCompra>();
        }

        // Clase anidada para el detalle de cada producto en la compra
        public class DetalleCompra
        {
            public int id_detalle_compra { get; set; }
            public int cantidad { get; set; }
            public decimal precio_compra { get; set; }
            public decimal sub_total { get; set; }

            public int COMPRA_id_compra { get; set; } 
            public int PRODUCTO_id_producto { get; set; }

            // Propiedad para el objeto Producto relacionado
            public Producto oProducto { get; set; } // Para tener acceso a los datos del producto (nombre, etc.)
        }
    }
}
