using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    //CREATE TABLE COMERCIAL.CLIENTE 
    //(
     //id_cliente INTEGER NOT NULL IDENTITY(1,1), 
     //nombres VARCHAR(150) NOT NULL,
     //apellidos VARCHAR(150) , 
     //telefono VARCHAR(20) , 
     //correo VARCHAR(50) , 
     //estado BIT DEFAULT 1 NOT NULL
    //)
    //GO
    public class Cliente
    {
        public int id_cliente { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public bool estado { get; set; }
    }
}
