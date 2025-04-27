using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
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
        public ActionResult Marcas() {
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















        public ActionResult Productos()
        {
            return View();
        }




        //SERVICIOS
        public ActionResult Servicios()
        {
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
            if (objeto.id_servicio == 0)
            {
                resultado = new CN_Servicios().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Servicios().Editar(objeto, out Mensaje);
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















        public ActionResult Tiendas()
        {
            return View();
        }

        //UNIDAD DE MEDIDA
        public ActionResult UnidadMedida()
        {
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