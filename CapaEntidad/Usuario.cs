using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    //CREATE TABLE SEGURIDAD.USUARIO 
    //(
     //id_usuario INTEGER NOT NULL IDENTITY(1,1), 
     //nombres VARCHAR(150) NOT NULL,
     //apellidos VARCHAR(150) NOT NULL,
     //correo VARCHAR(20) NOT NULL,
     //contraseña VARCHAR(150) NOT NULL,
     //telefono VARCHAR(20) NOT NULL,
     //estado BIT DEFAULT 1 NOT NULL,
     //ROLES_id_rol INTEGER NOT NULL 
    //)
    //GO 
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string correo { get; set; }
        public string contraseña { get; set; }
        public string telefono { get; set; }
        public bool estado { get; set; }
        public int id_rol { get; set; }
        public string rol { get; set; }

    }
}
