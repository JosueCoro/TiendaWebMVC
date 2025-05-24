using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor

        //CATEGORIA
        public ActionResult Categorias()
        {
            //validar que id_user_activo no sea null osea que el usuario haya iniciado sesion para poder entrar aqui
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                //validar que el usuario tenga un rol asignado
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> olista = new List<Categoria>();
            olista = new CN_Categoria().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if (objeto.id_categoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out Mensaje);



            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }










        //MARCAS
        public ActionResult Marcas() 
        {
            //validar que id_user_activo no sea null osea que el usuario haya iniciado sesion para poder entrar aqui
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                //validar que el usuario tenga un rol asignado
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();

        }

        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> olista = new List<Marca>();
            olista = new CN_Marcas().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if (objeto.id_marca == 0)
            {
                resultado = new CN_Marcas().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Marcas().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarMarcas(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;

            respuesta = new CN_Marcas().Eliminar(id, out Mensaje);



            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }














        //PRODUCTOS
        public ActionResult Productos()
        {
            //validar que id_user_activo no sea null osea que el usuario haya iniciado sesion para poder entrar aqui
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                //validar que el usuario tenga un rol asignado
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }
        [HttpGet]
        public JsonResult ListarProductos()
        {
            var olista = new CN_Producto().Listar();

            var resultado = olista.Select(p => new
            {
                p.id_producto,
                p.nombre,
                p.descripcion,
                p.precio,
                p.estado,
                //retornar ruta imagen y nombre imagen
                p.ruta_imagen,
                p.nombre_imagen,

                oTienda = new { nombre = p.oTienda?.nombre ?? "Sin tienda" },                
                oStock = new { cantidad = p.oStock?.cantidad ?? 0 },
                //oUnidadMedida = new { descripcion = p.oUnidadMedida?.descripcion ?? "" },
                //oCategoria = new { descripcion = p.oCategoria?.descripcion ?? "" },
                //oMarca = new { descripcion = p.oMarca?.descripcion ?? "" }
                oUnidadMedida = p.oUnidadMedida != null ? new { p.oUnidadMedida.descripcion, id_unidad_medida = (int?)p.oUnidadMedida.id_unidad_medida } : new { descripcion = (string)null, id_unidad_medida = (int?)null },
                oCategoria = p.oCategoria != null ? new { p.oCategoria.descripcion, id_categoria = (int?)p.oCategoria.id_categoria } : new { descripcion = (string)null, id_categoria = (int?)null },
                oMarca = p.oMarca != null ? new { p.oMarca.descripcion, id_marca = (int?)p.oMarca.id_marca } : new { descripcion = (string)null, id_marca = (int?)null }
            });

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }


        /*public JsonResult ListarProductos()
        {
            List<Producto> olista = new List<Producto>();
            olista = new CN_Producto().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }*/

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            //object resultado;
            string Mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto oproducto = new Producto();
            oproducto = JsonConvert.DeserializeObject<Producto>(objeto);
            if (oproducto.precio <= 0)
            {
                return Json(new { operacion_exitosa = false, mensaje = "El precio del producto debe ser mayor a cero" }, JsonRequestBehavior.AllowGet);
            }

            if (oproducto.id_producto == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oproducto, out Mensaje);

                if (idProductoGenerado != 0)
                {
                   oproducto.id_producto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }



            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oproducto, out Mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oproducto.id_producto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        //string mensaje_error = ex.Message;
                        guardar_imagen_exito = false;
                        Mensaje = "Se guardó el producto pero hubo problemas con la imagen: " + ex.Message;
                    }

                    if (guardar_imagen_exito)
                    {
                        oproducto.ruta_imagen = ruta_guardar;
                        oproducto.nombre_imagen = nombre_imagen;
                        bool rspta = new CN_Producto().GuardarDatosImagen(oproducto, out Mensaje);
                    }
                    else
                    {
                        Mensaje = "Se guardo el producto pero hubo problemas con la imagen";

                    }

                }
            }



            return Json(new { operacionExitosa = operacion_exitosa, idgenerado = oproducto.id_producto, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            var oproductoAnonimo = new CN_Producto().Listar()
                                                .Where(p => p.id_producto == id)
                                                .Select(p => new { p.ruta_imagen, p.nombre_imagen })
                                                .FirstOrDefault();

            if (oproductoAnonimo == null)
            {
                return Json(new { conversion = false, mensaje = "Producto no encontrado" }, JsonRequestBehavior.AllowGet);
            }

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproductoAnonimo.ruta_imagen, oproductoAnonimo.nombre_imagen), out conversion);
            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                Extension = Path.GetExtension(oproductoAnonimo.nombre_imagen)
            },
            JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ImagenProducto(int id)
        //{
        //    bool conversion;
        //    Producto oproducto = new CN_Producto().Listar().Where(p => p.id_producto == id).FirstOrDefault();
        //    if (oproducto == null)
        //    {
        //        return Json(new { conversion = false, mensaje = "Producto no encontrado" }, JsonRequestBehavior.AllowGet);
        //    }

        //    string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.ruta_imagen, oproducto.nombre_imagen), out conversion);
        //    //Producto oproducto = new CN_Producto().Listar().Where(p => p.id_producto == id).FirstOrDefault();

        //    //string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.ruta_imagen, oproducto.nombre_imagen), out conversion);
        //    return Json(new 
        //    {
        //        conversion = conversion,
        //        textoBase64 = textoBase64,
        //        Extension = Path.GetExtension(oproducto.nombre_imagen)
        //    },
        //    JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta = new CN_Producto().Eliminar(id, out Mensaje);
            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }






        //SERVICIOS
        public ActionResult Servicios()
        {
            //validar que id_user_activo no sea null osea que el usuario haya iniciado sesion para poder entrar aqui
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                //validar que el usuario tenga un rol asignado
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }


        [HttpGet]
        public JsonResult ListarServicios()
        {
            List<Servicio> olista = new List<Servicio>();
            olista = new CN_Servicios().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarServicio(Servicio objeto)
        {
            object resultado;
            string Mensaje = string.Empty;
            
            User_activo usuarioActivo = Session["Usuario"] as User_activo;

            if (usuarioActivo == null || usuarioActivo.id_user_activo == 0)
            {
                Mensaje = "No se ha encontrado el ID del usuario logeado. Por favor, inicie sesión nuevamente.";
                return Json(new { resultado = 0, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }

            int idUsuarioLogeado = usuarioActivo.id_user_activo;

            if (objeto.id_servicio == 0) 
            {
                resultado = new CN_Servicios().Registrar(objeto, idUsuarioLogeado, out Mensaje);
            }
            else
            {
                
                resultado = new CN_Servicios().Editar(objeto, idUsuarioLogeado, out Mensaje);
            }

            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult EliminarServicio(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta = new CN_Servicios().Eliminar(id, out Mensaje);
            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }












        //TIPOS DE PAGO
        public ActionResult TipoPago()
        {
            //validar que id_user_activo no sea null osea que el usuario haya iniciado sesion para poder entrar aqui
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                //validar que el usuario tenga un rol asignado
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }

        [HttpGet]
        public JsonResult ListarTP()
        {
            List<TipoPago> olista = new List<TipoPago>();
            olista = new CN_TipoPago().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GuardarTP(TipoPago objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if (objeto.id_tipo_pago == 0)
            {
                resultado = new CN_TipoPago().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_TipoPago().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarTP(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;

            respuesta = new CN_TipoPago().Eliminar(id, out Mensaje);



            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }














        //TIENDAS
        public ActionResult Tiendas()
        {
            //validar que id_user_activo no sea null osea que el usuario haya iniciado sesion para poder entrar aqui
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                //validar que el usuario tenga un rol asignado
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }
        [HttpGet]
        public JsonResult ListarTiendas()
        {
            List<Tienda> lista = new List<Tienda>();
            lista = new CN_Tiendas().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTienda(Tienda objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if (objeto.id_tienda == 0)
            {
                resultado = new CN_Tiendas().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Tiendas().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarTienda(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta = new CN_Tiendas().Eliminar(id, out Mensaje);
            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }



        //UNIDAD DE MEDIDA
        public ActionResult UnidadMedida()
        {
            //validar que id_user_activo no sea null osea que el usuario haya iniciado sesion para poder entrar aqui
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                //validar que el usuario tenga un rol asignado
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }

        [HttpGet]
        public JsonResult ListarUnidadMedida()
        {
            List<UnidadMedida> olista = new List<UnidadMedida>();
            olista = new CN_UnidadMedida().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GuardarUnidadMedida(UnidadMedida objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if (objeto.id_unidad_medida == 0)
            {
                resultado = new CN_UnidadMedida().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_UnidadMedida().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarUnidadMedida(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;

            respuesta = new CN_UnidadMedida().Eliminar(id, out Mensaje);



            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }



    }
}