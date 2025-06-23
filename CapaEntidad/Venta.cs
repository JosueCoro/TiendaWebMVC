using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int id_venta { get; set; }
        public DateTime fecha { get; set; }
        public decimal monto_total { get; set; }

        public decimal monto_pagado { get; set; } 
        public decimal cambio { get; set; }     

        public int CLIENTE_id_cliente { get; set; }
        public int TIPO_PAGO_id_tipo_pago { get; set; }
        public int USUARIO_id_usuario { get; set; }
        public int TIENDA_id_tienda { get; set; } 

        public Cliente oCliente { get; set; }
        public TipoPago oTipoPago { get; set; }
        public Usuario oUsuario { get; set; }
        public Tienda oTienda { get; set; }

        public List<DetalleVenta> oDetalleVenta { get; set; }

        public Venta()
        {
            oDetalleVenta = new List<DetalleVenta>();
        }

        public class DetalleVenta
        {
            public int id_detalle_venta { get; set; }
            public int cantidad { get; set; }
            public decimal precio { get; set; } 
            public decimal sub_total { get; set; }


            public int? PRODUCTO_id_producto { get; set; } 
            public int? SERVICIO_id_servicio { get; set; }  

            public string tipo_item { get; set; } 

            public Producto oProducto { get; set; } 
            public Servicio oServicio { get; set; } 
        }
    }
}
