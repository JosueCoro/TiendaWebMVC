using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    
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
        public string codigo_barra { get; set; }

        public Marca oMarca { get; set; }
        public Categoria oCategoria { get; set; }
        public UnidadMedida oUnidadMedida { get; set; }
        public Stock oStock { get; set; }
        public int id_stock { get; set; }
        public int cantidad { get; set; }   
        public Tienda oTienda { get; set; }

        public int MARCA_id_marca { get; set; }
        public int CATEGORIA_id_categoria { get; set; }
        public int UNIDAD_MEDIDA_id_unidad_medida { get; set; }
    }
}
