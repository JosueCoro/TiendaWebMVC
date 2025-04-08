using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    //CREATE TABLE SEGURIDAD.PERMISOS 
    //(
     //id_permiso INTEGER NOT NULL IDENTITY(1,1), 
     //nombre VARCHAR(150) NOT NULL,
     //descripcion VARCHAR(150) , 
     //estado BIT DEFAULT 1 NOT NULL
    //)
    //GO
    public class Permisos
    {
        public int id_permiso { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }
    }
}
