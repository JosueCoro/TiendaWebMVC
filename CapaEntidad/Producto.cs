using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    //CREATE TABLE INVENTARIO.PRODUCTO 
    //(
     //id_producto INTEGER NOT NULL IDENTITY(1,1), 
     //nombre VARCHAR(150) NOT NULL,
     //descripcion VARCHAR(150) , 
     //precio DECIMAL(30,3) NOT NULL,
     //unidad_medida VARCHAR(50) NOT NULL,
     //ruta_imagen VARCHAR(100) NOT NULL,
     //nombre_imagen VARCHAR(100) NOT NULL,
     //estado BIT DEFAULT 1 NOT NULL,
     //MARCA_id_marca INTEGER , 
     //CATEGORIA_id_categoria INTEGER NOT NULL
    //)
    //GO
    public class Producto
    {
        public int id_producto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public string ruta_imagen { get; set; }
        public string nombre_imagen { get; set; }
        public bool estado { get; set; }
        public string Base64 { get; set; }
        public string Extension { get; set; }

        public Marca oMarca { get; set; }
        public Categoria oCategoria { get; set; }
        public UnidadMedida oUnidadMedida { get; set; }
        public Stock oStock { get; set; }
        public Tienda oTienda { get; set; }

        public int MARCA_id_marca { get; set; }
        public int CATEGORIA_id_categoria { get; set; }
        public int UNIDAD_MEDIDA_id_unidad_medida { get; set; }
    }
}
