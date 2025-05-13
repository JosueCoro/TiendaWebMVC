using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Stock
    {
        /*CREATE TABLE INVENTARIO.STOCK 
            (
             id_stock INTEGER NOT NULL IDENTITY(1,1), 
             cantidad INTEGER NOT NULL , 
             stock_minimo INTEGER NOT NULL , 
             fecha_actualizacion DATE NOT NULL , 
             PRODUCTO_id_producto INTEGER NOT NULL , 
             TIENDA_id_tienda INTEGER NOT NULL 
            )
        GO*/
        public int id_stock { get; set; }
        public int cantidad { get; set; }
        public int stock_minimo { get; set; }
        public DateTime fecha_actualizacion { get; set; }
        public Producto oProducto { get; set; }
        public int PRODUCTO_id_producto { get; set; }
        public Tienda oTienda { get; set; }
        public int TIENDA_id_tienda { get; set; }
        public int stock_actual { get; set; }

    }
}
