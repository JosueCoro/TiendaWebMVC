using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    //CREATE TABLE INVENTARIO.MARCA 
    //(
     //id_marca INTEGER NOT NULL IDENTITY(1,1), 
     //descripcion VARCHAR(100) NOT NULL,
     //estado BIT DEFAULT 1 NOT NULL
    //)
    //GO
    public class Marca
    {
        public int id_marca { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }
    }
}
