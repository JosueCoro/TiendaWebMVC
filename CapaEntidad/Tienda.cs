using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Tienda
    {
        /*CREATE TABLE INVENTARIO.TIENDA 
            (
             id_tienda INTEGER NOT NULL IDENTITY(1,1), 
             nombre VARCHAR (150) NOT NULL , 
             dirrecion VARCHAR (150) NOT NULL , 
             telefono VARCHAR (20) NOT NULL , 
             estado BIT DEFAULT 1 NOT NULL 
            )
        GO*/
        public int id_tienda { get; set; }
        public string nombre { get; set; }
        public string dirrecion { get; set; }
        public string telefono { get; set; }
        public bool estado { get; set; }

    }
}
