using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class User_activo
    {
        public int id_user_activo { get; set; }
        public int id_tienda_user { get; set; }
        public string correo { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; } 
        public string clave { get; set; } 
        public bool activo { get; set; }

        public Roles oRol { get; set; } 
        public List<Permisos> listaPermisosDelRol { get; set; } 
    }
}
