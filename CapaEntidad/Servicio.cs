using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Servicio
    {
        /*CREATE TABLE TRANSACCIONES.SERVICIO 
            (
             id_servicio INTEGER NOT NULL IDENTITY(1,1), 
             nombre VARCHAR (150) NOT NULL , 
             descripcion VARCHAR (150) , 
             precio DECIMAL (30,3) NOT NULL , 
             estado BIT DEFAULT 1 NOT NULL , 
             USUARIO_id_usuario INTEGER NOT NULL 
            )
        GO*/
        public int id_servicio { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public bool estado { get; set; }
        public int USUARIO_id_usuario { get; set; }
    }
}
