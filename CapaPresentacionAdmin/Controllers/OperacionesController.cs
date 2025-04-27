using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class OperacionesController : Controller
    {
        // GET: Operaciones
        public ActionResult Ventas()
        {
            return View();
        }
        public ActionResult Compras()
        {
            return View();
        }



        //clientes
        public ActionResult Clientes()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarClientes()
        {
            List<Cliente> olista = new List<Cliente>();
            olista = new CN_Clientes().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GuardarCliente(Cliente objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if (objeto.id_cliente == 0)
            {
                resultado = new CN_Clientes().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Clientes().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarCliente(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta = new CN_Clientes().Eliminar(id, out Mensaje);
            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }




        //proveedores
        public ActionResult Proveedores()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarProveedores()
        {
            List<Proveedor> olista = new List<Proveedor>();
            olista = new CN_Proveedores().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProveedor(Proveedor objeto)
        {
            object resultado;
            string Mensaje = string.Empty;
            if (objeto.id_proveedor == 0)
            {
                resultado = new CN_Proveedores().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Proveedores().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProveedor(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta = new CN_Proveedores().Eliminar(id, out Mensaje);
            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Stock()
        {
            return View();
        }

    }
}