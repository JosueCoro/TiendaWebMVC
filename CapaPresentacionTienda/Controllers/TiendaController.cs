using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;
using System.IO;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        //Categorias
        [HttpGet]
        public JsonResult ListaCategoria()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categoria().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        //Marcas

        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> olista = new List<Marca>();
            olista = new CN_Marcas().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }

        
        [HttpGet]
        public JsonResult ListarProductosCatalogo(int idMarca = 0, int idCategoria = 0)
        {
            var olista = new CN_Producto().ListarProductos(idMarca, idCategoria);

            
            var resultado = olista.Select(p => new
            {
                p.id_producto,
                p.nombre,
                p.descripcion,
                p.precio,
                p.ruta_imagen,        
                p.nombre_imagen,   
                p.estado,             

                marca = p.oMarca != null ? new { id = p.oMarca.id_marca, descripcion = p.oMarca.descripcion } : null,
                categoria = p.oCategoria != null ? new { id = p.oCategoria.id_categoria, descripcion = p.oCategoria.descripcion } : null
            }).ToList(); 

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost] 
        public JsonResult ImagenProductoCatalogo(int id)
        {
            bool conversionExitosa = false; 
            string textoBase64 = string.Empty;
            string extension = string.Empty;
            string mensaje = "Producto no encontrado."; 

            var oproductoAnonimo = new CN_Producto().Listar() 
                                                    .Where(p => p.id_producto == id)
                                                    .Select(p => new { p.ruta_imagen, p.nombre_imagen })
                                                    .FirstOrDefault();

            
            if (oproductoAnonimo == null)
            {
                return Json(new { conversion = conversionExitosa, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(oproductoAnonimo.nombre_imagen) && !string.IsNullOrEmpty(oproductoAnonimo.ruta_imagen))
            {
                string rutaCompletaImagen = Path.Combine(oproductoAnonimo.ruta_imagen, oproductoAnonimo.nombre_imagen);

                textoBase64 = CN_Recursos.ConvertirBase64(rutaCompletaImagen, out conversionExitosa);
                extension = Path.GetExtension(oproductoAnonimo.nombre_imagen);
                mensaje = conversionExitosa ? "Imagen cargada correctamente." : "Error al convertir la imagen.";
            }
            else
            {
                mensaje = "El producto no tiene una imagen asociada.";
            }


            return Json(new
            {
                conversion = conversionExitosa, 
                textoBase64 = textoBase64,     
                Extension = extension,         
                mensaje = mensaje              
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}