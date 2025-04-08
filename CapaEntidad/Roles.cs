using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    //CREATE TABLE SEGURIDAD.ROLES 
    //(
     //id_rol INTEGER NOT NULL IDENTITY(1,1), 
     //nombre VARCHAR(150) NOT NULL,
     //descripcion VARCHAR(150) , 
     //estado BIT DEFAULT 1 NOT NULL
    //)
    //GO
    public class Roles
    {
        public int id_rol { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }

    }
}
