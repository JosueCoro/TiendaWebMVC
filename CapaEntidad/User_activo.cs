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
        public string contraseña { get; set; }
    }
}
