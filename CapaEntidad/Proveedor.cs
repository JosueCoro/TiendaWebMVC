using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    //CREATE TABLE COMERCIAL.PROVEEDOR 
    //(
     //id_proveedor INTEGER NOT NULL IDENTITY(1,1), 
     //nombre VARCHAR(150) NOT NULL,
     //telefono VARCHAR(20) NOT NULL,
     //correo VARCHAR(50) , 
    // nit VARCHAR(50) , 
    // estado BIT DEFAULT 1 NOT NULL
    //)
    //GO
    public class Proveedor
    {
        public int id_proveedor { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string nit { get; set; }
        public bool estado { get; set; }
    }
}
